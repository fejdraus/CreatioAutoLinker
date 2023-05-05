using CreatioAutoPackageLinkerBlazor.Data.DbClass;
using CreatioAutoPackageLinkerBlazor.Data.DbClass.Repository;
using CreatioAutoPackageLinkerBlazor.Data.Rest;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace CreatioAutoPackageLinkerBlazor.Services;

public class AutoLinkService
{
    private readonly IDbRepository _dbRepository;
    private readonly ICreatioService _creatioService;
    private static readonly ConcurrentDictionary<Guid, bool> TaskStates = new();
    public AutoLinkService(IDbRepository dbRepository, ICreatioService creatioService)
    {
        _dbRepository = dbRepository;
        _creatioService = creatioService;
    }

    public async Task BackgroundAutoLink(Guid selectedProjectId, Guid productTypeId)
    {
        if (!TaskStates.TryAdd(selectedProjectId, true)) {
            return;
        }
        try
        {
            await AutoLink(selectedProjectId, productTypeId);
        } catch(Exception e) {
            Console.WriteLine(e.Message.ToString());
        }
        finally {
            TaskStates.TryRemove(selectedProjectId, out _);
        }
    }

    public async Task AutoLink(Guid selectedProjectId, Guid productTypeId)
    {
        if (selectedProjectId == Guid.Empty)
            return;
        var packagesList = await _dbRepository.GetPackagesAllNotReadOnlyByProject(selectedProjectId);
        if (packagesList.Count == 0 || productTypeId == Guid.Empty)
            return;
        await _dbRepository.DeletePackageHierarchyByIsModified(true, selectedProjectId);
        await _dbRepository.UpdatePackageHierarchyToIsNotDelete(selectedProjectId);
        var packages = await _dbRepository.GetPackagesAllByProject(selectedProjectId);
        await _dbRepository.CreateOrUpdatePackageRangeAsync(packages.Where(a => a.IsReadOnly.Equals(false)).Select(x => {
            x.Rang = -1;
            return x;
        }));
        var typeOfPackageForProducts = await _dbRepository.GetActivePackageTypesForProductByProductIdAsync(productTypeId);
        var rootPackageId = packages.FirstOrDefault(x => x.IsRootPackage)!.Id;
        var newPackage = packages.Where(a => a.IsReadOnly.Equals(false))
            .Select(x => {
                var clonedPackageHierarchy = ObjectCloneService.Clone(x);
                return clonedPackageHierarchy;
            }).ToList();
        var checkParentNodeConnectionsService = new CheckParentNodeConnectionsService(_creatioService);
        if (newPackage.Any()) {
            string original;
            string modified;
            do {
                original = JsonConvert.SerializeObject(newPackage.Select(x => new
                {
                    item = x.PackageHierarchyDependOnPackages.Select(y => new
                    {
                        y.Id,
                        y.BasePackageId,
                        y.DependOnPackageId
                    })
                }));

                foreach (var package in newPackage) {
                    await checkParentNodeConnectionsService.CheckParentNodeConnections(package, rootPackageId, typeOfPackageForProducts);
                }

                var addDependencyForRootPackageService = new AddDependencyForRootPackageService(_dbRepository);
                await addDependencyForRootPackageService.AddDependencyForRootPackage(typeOfPackageForProducts, rootPackageId, newPackage, selectedProjectId);
                modified = JsonConvert.SerializeObject(newPackage.Select(x => new
                {
                    item = x.PackageHierarchyDependOnPackages.Select(y => new
                    {
                        y.Id,
                        y.BasePackageId,
                        y.DependOnPackageId
                    })
                }));
            } while (!original.Equals(modified));
            
            foreach (var package in newPackage.Where(package => package.PackageHierarchyDependOnPackages is { Count: > 0 })) {
                await _dbRepository.CreatePackageHierarchyRange(package.PackageHierarchyDependOnPackages.Select(x => {
                    x.Id = Guid.NewGuid();
                    x.IsModified = true;
                    return x;
                }));
            }
            await _dbRepository.CreateOrUpdatePackageRangeAsync(newPackage);
            var rangForPackagesService = new SetRangForPackagesService(_dbRepository);
            await rangForPackagesService.SetRangForPackages(selectedProjectId);
            var isCanBeRootService = new IsCanBeRootService(_dbRepository);
            await isCanBeRootService.IsCanBeRoot(true, selectedProjectId);
        }
    }
}
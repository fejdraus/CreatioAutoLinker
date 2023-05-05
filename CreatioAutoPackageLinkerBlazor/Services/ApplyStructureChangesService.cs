using CreatioAutoPackageLinkerBlazor.Data.DbClass.Repository;
using CreatioAutoPackageLinkerBlazor.Data.Out;
using CreatioAutoPackageLinkerBlazor.Data.Rest;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using CreatioAutoPackageLinkerBlazor.Data.DbClass;

namespace CreatioAutoPackageLinkerBlazor.Services;

public class ApplyStructureChangesService
{
    private readonly IDbRepository _dbRepository;
    private readonly ICreatioService _creatioService;
    private readonly SignalRService _signalRService;
    private static ConcurrentDictionary<Guid, bool> taskStates = new ConcurrentDictionary<Guid, bool>();
    public ApplyStructureChangesService(IDbRepository dbRepository, ICreatioService creatioService, SignalRService signalRService)
    {
        _dbRepository = dbRepository;
        _creatioService = creatioService;
        _signalRService = signalRService;
    }
    
    protected virtual async Task OnPackageChangedAsync(string connectionId, string message)
    {
        await _signalRService.SendPrivateMessageAsync(connectionId, message);
    }

    public async Task BackgroundApplyStructureChanges(Guid selectedProjectId, string url, string login, string password) {
        if (!taskStates.TryAdd(selectedProjectId, true)) {
            return;
        }
        try {
            await ApplyStructureChanges(selectedProjectId, url, login, password);
        } catch (Exception e) {
            Console.WriteLine(e.Message.ToString());
        } finally {
            taskStates.TryRemove(selectedProjectId, out _);
        }
    }

    public async Task ApplyStructureChanges(Guid selectedProjectId, string url, string login, string password)
    {
        if (selectedProjectId == Guid.Empty)
            return;
        var project = await _dbRepository.GetActiveProjectByIdAsync(selectedProjectId);
        if (project == null || project.IsModified)
            return;
        try {
            project.IsModified = true;
            await _dbRepository.CreateOrUpdateProjectAsync(project);
            var packages = await _dbRepository.GetPackagesAllNotReadOnlyByProject(selectedProjectId);
            packages = packages.Where(x => x.IsModule.Equals(false) && x.Successfully.Equals(false)).OrderBy(x => x.Rang).ThenBy(x => x.Name).ToList();
            var packageHierarchies = await _dbRepository.GetPackageHierarchyAllByProjectId(selectedProjectId);
            packageHierarchies = packageHierarchies.Where(x => x.IsModified.Equals(true)).ToList();
            var requestSavePackagePropertiesList = packages.Select(x => new OutPackage {
                Id = x.PackageId,
                UId = x.PackageUId,
                Description = x.Description,
                Name = x.Name,
                Type = x.Type,
                Completed = x.Completed,
                Successfully = x.Successfully,
                DependsOnPackages = packageHierarchies.Where(y => x.Id == y.BasePackageId).Select(p => new OutDependsOnPackage {
                    UId = p.DependOnPackage.PackageUId
                }).ToList()
            });
            foreach (var requestSavePackageProperties in requestSavePackagePropertiesList) {
                int? statusCode = null;
                if (requestSavePackageProperties is { Completed: false, Successfully: false }) {
                    await _dbRepository.SuccessfullyPackageById(requestSavePackageProperties.UId, selectedProjectId, false, "", true);
                    await OnPackageChangedAsync("", "");
                    var response = await _creatioService.SavePackageProperties(url,
                        JsonConvert.SerializeObject(requestSavePackageProperties), login, password);
                    await _dbRepository.SuccessfullyPackageById(requestSavePackageProperties.UId, selectedProjectId, response?.Success ?? false, response?.ErrorInfo == null ? "" : response.ErrorInfo.ToString(), true);
                    statusCode = response?.StatesCode;
                }
                if (requestSavePackageProperties is { Completed: true, Successfully: false } && statusCode is null or 0) {
                    var counter = 100;
                    PackageProperties? packageProperties;
                    bool test3;
                    do {
                        if (counter < 100)
                            await Task.Delay(TimeSpan.FromMinutes(2));
                        counter--;
                        packageProperties = await _creatioService.GetPackageProperties(url, $"\"{requestSavePackageProperties.UId}\"", login, password);
                        if (counter == 0) {
                            throw new Exception("Ошибка " + packageProperties?.ErrorInfo);
                        }
                        var resultPackageProperties = packageProperties?.InPackage?.DependsOnPackages?.Select(x => x.UId).ToHashSet();
                        var resultRequestSavePackageProperties = requestSavePackageProperties.DependsOnPackages?.Select(x => x.UId).ToHashSet();
                        test3 = resultPackageProperties != null && 
                            resultRequestSavePackageProperties != null &&
                            resultPackageProperties.SetEquals(resultRequestSavePackageProperties);
                    } while (!test3);

                    await _dbRepository.SuccessfullyPackageById(requestSavePackageProperties.UId, selectedProjectId, packageProperties?.Success ?? false, packageProperties?.ErrorInfo == null ? "" : packageProperties.ErrorInfo.ToString(), true);
                }
            }
        } catch (Exception ex) {
            Console.WriteLine(ex.Message.ToString());
        } finally {
            project.IsModified = false;
            await _dbRepository.CreateOrUpdateProjectAsync(project);
        }
    }
}
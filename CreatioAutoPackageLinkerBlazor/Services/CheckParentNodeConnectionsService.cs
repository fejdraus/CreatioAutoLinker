using CreatioAutoPackageLinkerBlazor.Data;
using CreatioAutoPackageLinkerBlazor.Data.DbClass;
using CreatioAutoPackageLinkerBlazor.Data.DbClass.Repository;
using CreatioAutoPackageLinkerBlazor.Data.Object;
using CreatioAutoPackageLinkerBlazor.Data.Rest;

namespace CreatioAutoPackageLinkerBlazor.Services;

public class CheckParentNodeConnectionsService
{
    private readonly ICreatioService _creatioService;
    public CheckParentNodeConnectionsService(ICreatioService creatioService)
    {
        _creatioService = creatioService;
    }
    public async Task CheckParentNodeConnections(Package package, Guid rootPackage, List<TypeOfPackageForProduct> typeOfPackageForProducts)
    {
        var hasConnectionService = new HasConnectionService(_creatioService);
        if (package is { IsModule: false,PackageHierarchyDependOnPackages: not null })
        {
            foreach (var parentPackage in package.PackageHierarchyDependOnPackages.Where(x => x is {IsModified: false, IsDelete:false } ))
            {
                foreach (var otherParentPackage in package.PackageHierarchyDependOnPackages.Where(x => x is { IsModified: false, IsDelete:false }))
                {
                    if (await hasConnectionService.HasConnection(otherParentPackage.DependOnPackage.PackageUId, parentPackage.DependOnPackage.PackageUId, ToRootClass.Url, ToRootClass.Login, ToRootClass.Password))
                    {
                        if (otherParentPackage.DependOnPackageId == parentPackage.DependOnPackageId)
                        {
                            if (parentPackage.DependOnPackage.IsReadOnly || parentPackage.DependOnPackage.IsModule)
                            {
                                var isExits = false;
                                foreach (var typeOfPackageForProduct in typeOfPackageForProducts)
                                {
                                    if (typeOfPackageForProduct.Package != null && !await hasConnectionService.HasConnection(parentPackage.DependOnPackage.PackageUId, typeOfPackageForProduct.Package.RecordUId, ToRootClass.Url, ToRootClass.Login, ToRootClass.Password)) continue;
                                    isExits = true;
                                    break;
                                }
                                if (!isExits)
                                {
                                    ToRootClass.ToRoot.Add(new PackageHierarchy
                                    {
                                        IsModified = true,
                                        IsDelete = false,
                                        RecordInactive = false,
                                        BasePackageId = rootPackage,
                                        DependOnPackageId = parentPackage.DependOnPackageId
                                    });
                                }
                                otherParentPackage.IsDelete = true;
                            }
                        }
                        else
                        {
                            otherParentPackage.IsDelete = true;
                        }
                        
                    }
                }
            }
            package.PackageHierarchyDependOnPackages.RemoveAll(y => y.IsDelete);
            if (package.IsModule == false && package.Id != rootPackage && package.PackageHierarchyDependOnPackages.Count == 0)
            {
                package.PackageHierarchyDependOnPackages = new List<PackageHierarchy>
                {
                    new()
                    {
                        IsModified = true,
                        BasePackageId = package.Id,
                        DependOnPackageId = rootPackage,
                        RecordInactive = false
                    }
                };
            }
        }
    }
}
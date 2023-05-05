using CreatioAutoPackageLinkerBlazor.Data;
using CreatioAutoPackageLinkerBlazor.Data.DbClass;
using CreatioAutoPackageLinkerBlazor.Data.DbClass.Repository;
using CreatioAutoPackageLinkerBlazor.Data.Object;

namespace CreatioAutoPackageLinkerBlazor.Services;

public class AddDependencyForRootPackageService
{
    private readonly IDbRepository _dbRepository;
    public AddDependencyForRootPackageService(IDbRepository dbRepository)
    {
        _dbRepository = dbRepository;
    }
    
    public async Task AddDependencyForRootPackage(List<TypeOfPackageForProduct> typeOfPackageForProducts, Guid rootPackageId, List<Package> newPackage, Guid selectedProject)
    {
        foreach (var productTypePackage in typeOfPackageForProducts)
        {
            if (productTypePackage.Package == null) continue;
            var package = await _dbRepository.GetPackageByPackageUIdAndProjectId(productTypePackage.Package.RecordUId, selectedProject);
            if (package.Any())
            {
                ToRootClass.ToRoot.Add(new PackageHierarchy
                {
                    IsModified = true,
                    BasePackageId = rootPackageId,
                    DependOnPackageId = package.FirstOrDefault()!.Id,
                    IsDelete = false,
                    RecordInactive = false
                });
            }
        }
        var dependencyForRoot = ToRootClass.ToRoot.DistinctBy(x => x!.DependOnPackageId)
            .Where(y => !(bool)newPackage.FirstOrDefault(x => x.IsRootPackage)?.PackageHierarchyDependOnPackages
                .Any(v => y != null && v.DependOnPackageId == y.DependOnPackageId)).ToList();
        if (dependencyForRoot.Any())
            newPackage.FirstOrDefault(x => x.IsRootPackage)?.PackageHierarchyDependOnPackages.AddRange(dependencyForRoot!);
    }
}
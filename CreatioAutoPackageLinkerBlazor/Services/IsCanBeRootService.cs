using CreatioAutoPackageLinkerBlazor.Data.DbClass.Repository;

namespace CreatioAutoPackageLinkerBlazor.Services;

public class IsCanBeRootService
{
    private readonly IDbRepository _dbRepository;
    public IsCanBeRootService(IDbRepository dbRepository)
    {
        _dbRepository = dbRepository;
    }
    
    public async Task<bool> IsCanBeRoot(bool? isModified, Guid selectedProject)
    {
        var packagesAllNotReadOnly = await _dbRepository.GetPackagesAllNotReadOnlyByProject(selectedProject);
        if (isModified != null && (bool)isModified)
        {
            packagesAllNotReadOnly = packagesAllNotReadOnly.Where(x => x.PackageHierarchyDependOnPackages.Any(y => y.IsModified.Equals(isModified) && x.IsModule.Equals(false))).ToList();
        }
        foreach (var package in packagesAllNotReadOnly)
        {
            var isExist = package.PackageHierarchyDependOnPackages.Any(x => packagesAllNotReadOnly.Any(y => y.Id == x.DependOnPackageId));
            package.CanBeRoot = !isExist;
        }
        var isCanBeRoot = await _dbRepository.CreateOrUpdatePackageRangeAsync(packagesAllNotReadOnly);
        return isCanBeRoot;
    }
}
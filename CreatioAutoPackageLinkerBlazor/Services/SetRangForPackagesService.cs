using CreatioAutoPackageLinkerBlazor.Data.DbClass.Repository;

namespace CreatioAutoPackageLinkerBlazor.Services;

public class SetRangForPackagesService
{
    private readonly IDbRepository _dbRepository;
    public SetRangForPackagesService(IDbRepository dbRepository)
    {
        _dbRepository = dbRepository;
    }
    
    public async Task SetRangForPackages(Guid selectedProjectId)
    {
        var newPackages = await _dbRepository.GetPackagesAllNotReadOnlyByProject(selectedProjectId);
        newPackages = newPackages.Where(x => x.PackageHierarchyDependOnPackages.Any(y => y.IsModified.Equals(true))).ToList();
        await _dbRepository.CreateOrUpdatePackageRangeAsync(newPackages.Select(x =>
        {
            x.Rang = -1;
            return x;
        }));
        newPackages = newPackages.Where(x => x.IsModule.Equals(false)).OrderBy(o=>o.IsRootPackage).ToList();
        newPackages.FirstOrDefault(x => x.IsRootPackage)!.Rang = 0;
        for (var i = 0; i < newPackages.Count; i++)
        {
            var dependPackages = newPackages.Where(x => x.Rang == i).ToList();
            foreach (var dependPackage in dependPackages)
            {
                foreach (var dependOnPackage in dependPackage.PackageHierarchyBasePackages)
                {
                    foreach (var package in newPackages.Where(x => x.Id == dependOnPackage.BasePackageId))
                    {
                        package.Rang = package.Rang < i + 1 ? i + 1 : package.Rang;
                    }
                }
            }
        }
        await _dbRepository.CreateOrUpdatePackageRangeAsync(newPackages);
    }
}
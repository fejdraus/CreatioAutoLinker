using CreatioAutoPackageLinkerBlazor.Data.DbClass;
using CreatioAutoPackageLinkerBlazor.Data.DbClass.Repository;
using CreatioAutoPackageLinkerBlazor.Data.Object;
using CreatioAutoPackageLinkerBlazor.Data.Rest;

namespace CreatioAutoPackageLinkerBlazor.Services;

public class GetPackagesService
{
    private readonly IDbRepository _dbRepository;
    private readonly ICreatioService _creatioService;
    public GetPackagesService(IDbRepository dbRepository, ICreatioService creatioService)
    {
        _dbRepository = dbRepository;
        _creatioService = creatioService;
    }
    public virtual async Task<List<Package>> GetPackages(string url, string login, string password, Guid selectedProject)
    {
        if (string.IsNullOrWhiteSpace(url) ||
            string.IsNullOrWhiteSpace(login) ||
            string.IsNullOrWhiteSpace(password) ||
            selectedProject == Guid.Empty) return new List<Package>();
        var isDeleted = _dbRepository.DeletePackagesAllByProject(selectedProject);
        Console.WriteLine($"Все записи проекта удалены: {isDeleted}");
        var result = await _creatioService.GetPackages(url.Trim(),login.Trim(), password.Trim());
        var inPackages = result?.InPackages?.Where(x => x.Name != "Custom").ToList();
        if (inPackages == null) return new List<Package>();
        var packageList = inPackages.Select(inPackage => new Package
        {
            Id = Guid.NewGuid(), 
            PackageId = inPackage.Id, 
            PackageUId = inPackage.UId, 
            Description = inPackage.Description, 
            Name = inPackage.Name, 
            Type = inPackage.Type, 
            Maintainer = inPackage.Maintainer, 
            ProjectId = selectedProject,
            Version = inPackage.Version,
            CreatedBy = inPackage.CreatedBy,
            CreatedOn = inPackage.CreatedOn,
            IsLocked = inPackage.IsLocked,
            ModifiedBy = inPackage.ModifiedBy,
            ModifiedOn = inPackage.ModifiedOn,
            IsReadOnly = inPackage.IsReadOnly
        }).ToList();
        var packageListCount = await _dbRepository.CreateOrUpdatePackageRangeAsync(packageList);
        Console.WriteLine($"Созданы пакеты: {packageListCount}");
        var packagesAllNotReadOnly = packageList.Where(x => x.IsReadOnly.Equals(false)).ToList();
        var packageHierarchyList = new List<PackageHierarchy>();
        foreach (var inPackage in packagesAllNotReadOnly)
        {
            
            var packagePropertiesResult = await _creatioService.GetPackageProperties(url, $"\"{inPackage.PackageUId}\"", login, password);
            if (packagePropertiesResult?.InPackage?.DependsOnPackages == null) continue;
            packageHierarchyList.AddRange(from dependsOnPackage in packagePropertiesResult.InPackage.DependsOnPackages
                select packageList.FirstOrDefault(x => x.PackageUId == dependsOnPackage.UId)
                into package
                where package != null
                select new PackageHierarchy
                {
                    Id = Guid.NewGuid(), RecordInactive = false, IsModified = false, BasePackageId = inPackage.Id, DependOnPackageId = package.Id
                });
        }
        var packageHierarchyListModifiedCount = await _dbRepository.CreatePackageHierarchyRange(packageHierarchyList);
        Console.WriteLine($"Созданы связи пакетов: {packageHierarchyListModifiedCount}");
        var isCanBeRootService = new IsCanBeRootService(_dbRepository);
        var isCanBeRoot = await isCanBeRootService.IsCanBeRoot(false, selectedProject);
        Console.WriteLine($"Пакет может быть Root: {isCanBeRoot}");
        return await _dbRepository.GetPackagesAllNotReadOnlyByProject(selectedProject);
    }
}
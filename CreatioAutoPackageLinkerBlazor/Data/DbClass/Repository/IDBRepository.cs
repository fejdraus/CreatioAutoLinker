using System.Runtime.CompilerServices;
using CreatioAutoPackageLinkerBlazor.Data.Object;

namespace CreatioAutoPackageLinkerBlazor.Data.DbClass.Repository;

public interface IDbRepository
{
    Task<List<TypeOfPackageForProduct>> GetActivePackageTypesForProductByProductIdAsync(Guid? id);
    Task<List<TypeOfPackageForProduct>> GetAllActivePackageTypesForProductsAsync();
    Task<List<ProductForType>> GetAllActiveProductsForTypeAsync();
    Task<List<Project>> GetAllActiveProjectsAsync();
    Task<Project?> GetActiveProjectByIdAsync(Guid id);
    Task<bool> CreateOrUpdateProjectAsync(Project project);
    Task<bool> CreatePackageRange(IEnumerable<Package> package);
    Task<bool> UpdatePackageRangeAsync(IEnumerable<Package> dataPackage);
    Task<bool> CreateOrUpdatePackageRangeAsync(IEnumerable<Package> packagesToCreateOrUpdate);
    Task<bool> SuccessfullyPackageById(Guid uId, Guid projectId, bool successfully, string? errorInfo, bool completed);
    Task<List<Package>> GetPackagesAllByProject(Guid projectId);
    Task<bool> DeletePackagesAllByProject(Guid projectId);
    Task<List<Package>> GetPackagesAllNotReadOnlyByProject(Guid projectId);
    Task<List<Package>> GetPackageByPackageUIdAndProjectId(Guid packageUId, Guid projectId);
    Task<Package?> GetPackageById(Guid id);
    Task<bool> CreatePackageHierarchyRange(IEnumerable<PackageHierarchy> packageHierarchy);
    Task<bool> UpdatePackageHierarchy(PackageHierarchy packageHierarchy);
    Task<bool> UpdatePackageHierarchyToIsNotDelete(Guid projectId);
    Task<bool> UpdatePackageHierarchyByBasePackageId(PackageHierarchy packageHierarchy);
    Task<bool> DeletePackageHierarchy(Guid parentPackageId, Guid packageId);
    Task<bool> DeletePackageHierarchyByIsModified(bool isModified, Guid project);
    Task<List<PackageHierarchy>> GetPackageHierarchyAllByProjectId(Guid projectId);
    Task<PackageHierarchy?> GetPackageHierarchyById(Guid id);
    Task<PackageHierarchy?> GetPackageHierarchyByBasePackageId(Guid basePackageId);
    Task<PackageHierarchy?> GetPackageHierarchyByDependOnPackageId(Guid dependOnPackageId);

}
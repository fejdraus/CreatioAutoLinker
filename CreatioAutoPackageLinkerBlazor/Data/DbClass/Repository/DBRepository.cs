using CreatioAutoPackageLinkerBlazor.Data.Object;
using Microsoft.EntityFrameworkCore;

namespace CreatioAutoPackageLinkerBlazor.Data.DbClass.Repository;

public class DbRepository : IDbRepository
{
    private readonly ApplicationDbContext _dbContext;

    public DbRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Получает список всех активных продуктов для типа.
    /// </summary>
    /// <returns>Возвращает список активных продуктов для типа.</returns>
    public async Task<List<ProductForType>> GetAllActiveProductsForTypeAsync()
    {
        var productForTypes = await _dbContext.ProductForTypes
            .Where(x => x.RecordInactive == false)
            .ToListAsync();
        return productForTypes;
    }

    /// <summary>
    /// Получает список типов пакетов для продукта по идентификатору продукта.
    /// </summary>
    /// <param name="id">Идентификатор продукта.</param>
    /// <returns>Возвращает список типов пакетов для продукта с заданным идентификатором продукта.</returns>
    public async Task<List<TypeOfPackageForProduct>> GetActivePackageTypesForProductByProductIdAsync(Guid? id)
    {
        var packageForProducts = await _dbContext.TypeOfPackageForProducts
            .Where(x => x.RecordInactive == false && x.ProductId == id)
            .Include(u => u.Package)
            .ToListAsync();

        return packageForProducts;
    }


    /// <summary>
    /// Получает список всех активных типов пакетов для продуктов.
    /// </summary>
    /// <returns>Возвращает список активных типов пакетов для продуктов.</returns>
    public async Task<List<TypeOfPackageForProduct>> GetAllActivePackageTypesForProductsAsync()
    {
        var packageForProducts = await _dbContext.TypeOfPackageForProducts
            .Where(packageType => packageType.RecordInactive == false)
            .Include(product => product.Product)
            .ToListAsync();

        return packageForProducts;
    }

    
    /// <summary>
    /// Получает список всех активных проектов.
    /// </summary>
    /// <returns>Возвращает список активных проектов.</returns>
    public async Task<List<Project>> GetAllActiveProjectsAsync()
    {
        var projects = await _dbContext.Projects
            .Where(project => project.RecordInactive == false)
            .Include(package => package.Package)
            .ToListAsync();
        return projects;
    }

    /// <summary>
    /// Получает активный проект по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор проекта.</param>
    /// <returns>Возвращает активный проект с указанным идентификатором или null, если не найден.</returns>
    public async Task<Project?> GetActiveProjectByIdAsync(Guid id)
    {
        var project = await _dbContext.Projects
            .Where(project => project.Id == id && project.RecordInactive == false)
            .Include(productForType => productForType.ProductForType)
            .FirstOrDefaultAsync();
        return project;
    }

    /// <summary>
    /// Создает новый проект или обновляет существующий проект в базе данных.
    /// Для нового проекта генерируется новый идентификатор Guid.
    /// </summary>
    /// <param name="project">Объект проекта для создания или обновления.</param>
    /// <returns>Возвращает true, если проект успешно создан или обновлен, иначе false.</returns>
    public async Task<bool> CreateOrUpdateProjectAsync(Project project)
    {
        var existingProject = await _dbContext.Projects.FindAsync(project.Id);
        if (existingProject == null)
        {
            project.Id = Guid.NewGuid();
            await _dbContext.Projects.AddAsync(project);
        }
        else
        {
            project.RecordInactive = existingProject.RecordInactive;
            _dbContext.Entry(existingProject).CurrentValues.SetValues(project);
        }
        var affectedRows = await _dbContext.SaveChangesAsync();
        return affectedRows > 0;
    }
    
    public async Task<bool> CreatePackageRange(IEnumerable<Package> package)
    {
        await _dbContext.Packages.AddRangeAsync(package);
        var count = await _dbContext.SaveChangesAsync();
        return count > 0;
    }
    
    /// <summary>
    /// Обновляет диапазон пакетов в базе данных, используя отслеживание изменений.
    /// </summary>
    /// <param name="packagesToUpdate">Коллекция пакетов для обновления.</param>
    /// <returns>Возвращает true, если успешно обновлено хотя бы одно значение, иначе false.</returns>
    public async Task<bool> UpdatePackageRangeAsync(IEnumerable<Package> packagesToUpdate)
    {
        var packagesList = packagesToUpdate.ToList();
        var packageIds = packagesList.Select(package => package.Id).ToList();
        var existingPackages = await _dbContext.Packages
            .Where(package => packageIds.Contains(package.Id))
            .ToListAsync();
        foreach (var packageToUpdate in packagesList)
        {
            var existingPackage = existingPackages.FirstOrDefault(package => package.Id == packageToUpdate.Id);
            if (existingPackage == null) continue;
            _dbContext.Entry(existingPackage).CurrentValues.SetValues(packageToUpdate);
            _dbContext.Entry(existingPackage).State = EntityState.Modified;
        }
        var affectedRows = await _dbContext.SaveChangesAsync();
        return affectedRows > 0;
    }
    
    /// <summary>
    /// Сохраняет диапазон пакетов в базе данных, используя отслеживание изменений.
    /// </summary>
    /// <param name="packagesToCreateOrUpdate">Коллекция пакетов для сохранения или обновления.</param>
    /// <returns>Возвращает true, если успешно сохранено или обновлено хотя бы одно значение, иначе false.</returns>
    public async Task<bool> CreateOrUpdatePackageRangeAsync(IEnumerable<Package> packagesToCreateOrUpdate)
    {
        var packageList = packagesToCreateOrUpdate.ToList();
        foreach (var package in packageList)
        {
            var existingPackage = await _dbContext.Packages
                .Where(p => p.Id == package.Id)
                .FirstOrDefaultAsync();
            if (existingPackage == null)
            {
                await _dbContext.Packages.AddAsync(package);
            }
            else
            {
                _dbContext.Entry(existingPackage).CurrentValues.SetValues(package);
                _dbContext.Entry(existingPackage).State = EntityState.Modified;
            }
        }
        var affectedRows = await _dbContext.SaveChangesAsync();
        return affectedRows > 0;
    }

    public async Task<bool> SuccessfullyPackageById(Guid uId, Guid projectId, bool successfully, string? errorInfo, bool completed)
    {
        var existingSuccessfullyPackage = await _dbContext.Packages.Where(x => x.PackageUId == uId && x.ProjectId == projectId).FirstOrDefaultAsync();
        if (existingSuccessfullyPackage == null) return false;
        existingSuccessfullyPackage.Successfully = successfully;
        existingSuccessfullyPackage.ErrorInfo = errorInfo;
        existingSuccessfullyPackage.Completed = completed;
        _dbContext.Entry(existingSuccessfullyPackage).CurrentValues
            .SetValues(existingSuccessfullyPackage);
        var count = await _dbContext.SaveChangesAsync();
        return count > 0;
    }

    public async Task<List<Package>> GetPackagesAllByProject(Guid projectId)
    {
        var packages = await _dbContext.Packages
            .Where(x => x.RecordInactive.Equals(false) && x.ProjectId == projectId)
            .Include(e => e.PackageHierarchyBasePackages)
            .Include(e => e.PackageHierarchyDependOnPackages)
            .ToListAsync();
        return packages;
    }

    public async Task<bool> DeletePackagesAllByProject(Guid projectId)
    {
        var packages = await _dbContext.Packages.Where(x => x.ProjectId == projectId).ToListAsync();
        _dbContext.Packages.RemoveRange(packages);
        var count = await _dbContext.SaveChangesAsync();
        return count > 0;
    }

    public async Task<List<Package>> GetPackagesAllNotReadOnlyByProject(Guid projectId)
    {
        var packages = await _dbContext.Packages
            .Where(x => x.ProjectId == projectId &&
                        x.IsReadOnly.Equals(false) &&
                        x.RecordInactive.Equals(false))
            .Include(x => x.PackageHierarchyBasePackages)
            .Include(x => x.PackageHierarchyDependOnPackages)
            .ToListAsync();
        return packages;
    }

    public async Task<List<Package>> GetPackageByPackageUIdAndProjectId(Guid packageUId, Guid project)
    {
        var packages = await _dbContext.Packages
            .Where(x => x.PackageUId == packageUId && x.RecordInactive.Equals(false) && x.ProjectId == project)
            .ToListAsync();
        return packages;
    }

    public async Task<Package?> GetPackageById(Guid id)
    {
        var packages = await _dbContext.Packages.Where(x => x.Id == id).FirstOrDefaultAsync();
        return packages;
    }

    public async Task<bool> CreatePackageHierarchyRange(IEnumerable<PackageHierarchy> packageHierarchy)
    {
        var count = 0;
        foreach (var item in packageHierarchy)
        {
            var existingPackageHierarchy = await _dbContext.PackageHierarchies.FindAsync(item.Id);
            if (existingPackageHierarchy == null) {
                await _dbContext.PackageHierarchies.AddAsync(item);
            } else {
                _dbContext.Entry(existingPackageHierarchy).CurrentValues.SetValues(item);
            }
            count += await _dbContext.SaveChangesAsync();
        }
        
        return count > 0;
    }
    
    public async Task<bool> UpdatePackageHierarchy(PackageHierarchy packageHierarchy)
    {
        var existingPackageHierarchy = await _dbContext.PackageHierarchies.FindAsync(packageHierarchy.Id);
        if (existingPackageHierarchy == null)
        {
            return false;
        }
        _dbContext.Entry(existingPackageHierarchy).CurrentValues.SetValues(packageHierarchy);
        var count = await _dbContext.SaveChangesAsync();
        return count > 0;
    }

    public async Task<bool> UpdatePackageHierarchyToIsNotDelete(Guid projectId)
    {
        var existingPackages = await _dbContext.PackageHierarchies
            .Where(p => projectId == p.BasePackage.ProjectId && p.RecordInactive.Equals(false))
            .ToListAsync();
        foreach (var existingPackage in existingPackages)
        {
            _dbContext.Entry(existingPackage).CurrentValues.SetValues(existingPackage.IsDelete = false);
        }
        var count = await _dbContext.SaveChangesAsync();
        return count > 0;
    }

    public async Task<bool> UpdatePackageHierarchyByBasePackageId(PackageHierarchy packageHierarchy)
    {
        var existingPackageHierarchy = await _dbContext.PackageHierarchies.FirstOrDefaultAsync(x => x.BasePackageId == packageHierarchy.BasePackageId);
        if (existingPackageHierarchy == null)
        {
            return false;
        }
        _dbContext.Entry(existingPackageHierarchy).CurrentValues.SetValues(packageHierarchy);
        var count = await _dbContext.SaveChangesAsync();
        return count > 0;
    }

    public async Task<bool> DeletePackageHierarchy(Guid parentPackageId, Guid packageId)
    {
        var packageHierarchies = await _dbContext.PackageHierarchies
            .Where(x => x.BasePackageId == packageId && x.DependOnPackageId == parentPackageId).ToListAsync();
        _dbContext.PackageHierarchies.RemoveRange(packageHierarchies);
        var count = await _dbContext.SaveChangesAsync();
        return count > 0;
    }

    public async Task<bool> DeletePackageHierarchyByIsModified(bool isModified, Guid project)
    {
        var packageHierarchiesToRemove = await _dbContext.PackageHierarchies
            .Where(x => x.IsModified == isModified && x.BasePackage.ProjectId == project)
            .ToListAsync();
        _dbContext.PackageHierarchies.RemoveRange(packageHierarchiesToRemove);
        var count = await _dbContext.SaveChangesAsync();
        return count > 0;
    }

    public async Task<List<PackageHierarchy>> GetPackageHierarchyAllByProjectId(Guid projectId)
    {
        var packageHierarchies = await _dbContext.PackageHierarchies
            .Where(x => x.RecordInactive.Equals(false) && x.BasePackage.ProjectId == projectId)
            .Include(x => x.BasePackage)
            .Include(x => x.DependOnPackage)
            .ToListAsync();
        return packageHierarchies;
    }
    
    public async Task<PackageHierarchy?> GetPackageHierarchyById(Guid id)
    {
        var packageHierarchies = await _dbContext.PackageHierarchies
            .FirstOrDefaultAsync(x => x.Id == id && x.RecordInactive.Equals(false));
        return packageHierarchies;
    }
    
    public async Task<PackageHierarchy?> GetPackageHierarchyByBasePackageId(Guid basePackageUId)
    {
        var packageHierarchies = await _dbContext.PackageHierarchies
            .Include(x => x.BasePackage)
            .FirstOrDefaultAsync(y => y.BasePackage.PackageUId == basePackageUId && y.BasePackage.RecordInactive.Equals(false));
        return packageHierarchies;
    }
    
    public async Task<PackageHierarchy?> GetPackageHierarchyByDependOnPackageId(Guid dependOnPackageUId)
    {
        var packageHierarchies = await _dbContext.PackageHierarchies
            .Include(x => x.BasePackage)
            .FirstOrDefaultAsync(y => y.DependOnPackage.PackageUId == dependOnPackageUId && y.RecordInactive.Equals(false));
        return packageHierarchies;
    }
}
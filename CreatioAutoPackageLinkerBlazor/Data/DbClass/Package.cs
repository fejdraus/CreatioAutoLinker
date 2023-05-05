using CreatioAutoPackageLinkerBlazor.Data.Object;

namespace CreatioAutoPackageLinkerBlazor.Data.DbClass;

public class Package: BaseObjectWithName
{
    public string? CreatedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public string? Description { get; set; }
    public bool IsLocked { get; set; }
    public string? Maintainer { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public int? Position { get; set; }
    public bool IsReadOnly { get; set; }
    public int? Type { get; set; }
    public Guid PackageId { get; set; }
    public Guid PackageUId { get; set; }
    public string? Version { get; set; }

    public bool IsModule { get; set; }
    public bool IsRootPackage { get; set; }
    public int Rang { get; set; }
    public bool Completed { get; set; }
    public string? ErrorInfo { get; set; }
    public bool Successfully { get; set; }
    public string? ResultDescription { get; set; }
    public bool CanBeRoot { get; set; }
    public virtual Package DeepCopy()
    {
        var other = (Package) this.MemberwiseClone();
        other.PackageHierarchyBasePackages = new List<PackageHierarchy>(other.PackageHierarchyBasePackages);
        other.PackageHierarchyDependOnPackages = new List<PackageHierarchy>(other.PackageHierarchyDependOnPackages);
        return other;
    }

    public List<PackageHierarchy> PackageHierarchyBasePackages { get; set; } = null!;
    public List<PackageHierarchy> PackageHierarchyDependOnPackages { get; set; } = null!;
    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;
}
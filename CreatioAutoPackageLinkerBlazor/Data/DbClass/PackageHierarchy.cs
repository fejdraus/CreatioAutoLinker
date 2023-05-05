using CreatioAutoPackageLinkerBlazor.Data.DbClass;

namespace CreatioAutoPackageLinkerBlazor.Data.Object;

public class PackageHierarchy : BaseObject
{
    public Guid BasePackageId { get; set; }
    public Package BasePackage { get; set; } = null!;
    public Guid DependOnPackageId { get; set; }
    public Package DependOnPackage { get; set; } = null!;
    public bool IsModified { get; set; }
    public bool IsDelete { get; set; }
}
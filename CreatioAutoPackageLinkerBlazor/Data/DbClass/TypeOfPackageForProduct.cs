using CreatioAutoPackageLinkerBlazor.Data.Object;

namespace CreatioAutoPackageLinkerBlazor.Data.DbClass;

public class TypeOfPackageForProduct : BaseObject
{
    public Guid PackageId { get; set; }
    public PackageForType? Package { get; set; }
    public Guid ProductId { get; set; }
    public ProductForType? Product { get; set; }
}
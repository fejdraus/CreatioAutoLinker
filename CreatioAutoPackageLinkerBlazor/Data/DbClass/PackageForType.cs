using CreatioAutoPackageLinkerBlazor.Data.Object;

namespace CreatioAutoPackageLinkerBlazor.Data.DbClass;

public class PackageForType : BaseObjectWithName
{
    public Guid RecordUId { get; set; }
    public List<TypeOfPackageForProduct>? TypeOfPackageForProducts { get; set; }
}
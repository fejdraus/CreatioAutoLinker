using CreatioAutoPackageLinkerBlazor.Data.Object;

namespace CreatioAutoPackageLinkerBlazor.Data.DbClass;

public class ProductForType : BaseObjectWithName
{
    public List<TypeOfPackageForProduct>? TypeOfPackageForProducts { get; set; }
    public List<Project>? Projects { get; set; }
}
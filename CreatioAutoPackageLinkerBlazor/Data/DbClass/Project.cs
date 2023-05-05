using CreatioAutoPackageLinkerBlazor.Data.Object;

namespace CreatioAutoPackageLinkerBlazor.Data.DbClass;

public class Project : BaseObjectWithName
{
    public List<Package>? Package { get; set; }
    public string Url { get; set; } = "";
    public string Login { get; set; } = "";
    public string Password { get; set; } = "";
    public Guid ProductForTypeId { get; set; }
    public ProductForType? ProductForType { get; set; }
    public bool IsModified { get; set; }
}
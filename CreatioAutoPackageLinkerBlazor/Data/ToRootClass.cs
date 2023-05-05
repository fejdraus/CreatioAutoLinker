using CreatioAutoPackageLinkerBlazor.Data.Object;

namespace CreatioAutoPackageLinkerBlazor.Data;

public class ToRootClass
{
    public static List<PackageHierarchy?> ToRoot { get; set; } = new();
    public static string Url { get; set; }
    public static string Login { get; set; }
    public static string Password { get; set; }
}
using CreatioAutoPackageLinkerBlazor.Data.In;
using CreatioAutoPackageLinkerBlazor.Data.Object;
using Newtonsoft.Json;

namespace CreatioAutoPackageLinkerBlazor.Data.Rest;

public class PackageProperties : BasePackageProperties
{
    [JsonProperty("package", NullValueHandling = NullValueHandling.Ignore)]
    public InPackage? InPackage { get; set; }
}
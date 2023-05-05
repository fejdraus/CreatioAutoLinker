using CreatioAutoPackageLinkerBlazor.Data.In;
using CreatioAutoPackageLinkerBlazor.Data.Object;
using Newtonsoft.Json;

namespace CreatioAutoPackageLinkerBlazor.Data;

public class ListInPackages : BasePackageProperties
{
    [JsonProperty("packages", NullValueHandling = NullValueHandling.Ignore)]
    public List<InPackage>? InPackages { get; set; }
}
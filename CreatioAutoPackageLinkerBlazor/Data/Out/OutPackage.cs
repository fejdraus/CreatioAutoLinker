using CreatioAutoPackageLinkerBlazor.Data.Object;
using Newtonsoft.Json;

namespace CreatioAutoPackageLinkerBlazor.Data.Out;

public class OutPackage : BaseObjectWithName
{
    [JsonProperty("dependsOnPackages", NullValueHandling = NullValueHandling.Ignore)]
    public List<OutDependsOnPackage>? DependsOnPackages { get; set; }

    [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
    public string? Description { get; set; }

    [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
    public int? Type { get; set; }

    [JsonProperty("uId", NullValueHandling = NullValueHandling.Ignore)]
    public Guid UId { get; set; }
    public bool Completed { get; set; }
    public bool Successfully { get; set; }
}
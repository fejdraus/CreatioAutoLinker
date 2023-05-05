using Newtonsoft.Json;

namespace CreatioAutoPackageLinkerBlazor.Data.Out;

public class OutDependsOnPackage
{
    [JsonProperty("uId", NullValueHandling = NullValueHandling.Ignore)]
    public Guid UId { get; set; }
}
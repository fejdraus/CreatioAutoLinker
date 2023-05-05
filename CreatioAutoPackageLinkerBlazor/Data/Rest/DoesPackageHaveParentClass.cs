using Newtonsoft.Json;

namespace CreatioAutoPackageLinkerBlazor.Data.Rest;

public class DoesPackageHaveParentResult
{
    [JsonProperty("DoesPackageHaveParentByIdResult", NullValueHandling = NullValueHandling.Ignore)]
    public bool DoesPackageHaveParentByIdResult { get; set; }
}
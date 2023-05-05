using Newtonsoft.Json;

namespace CreatioAutoPackageLinkerBlazor.Data.Object;

public class BasePackageProperties
{
    [JsonProperty("errorInfo", NullValueHandling = NullValueHandling.Ignore)]
    public object? ErrorInfo { get; set; }

    [JsonProperty("success", NullValueHandling = NullValueHandling.Ignore)]
    public bool Success { get; set; }
}
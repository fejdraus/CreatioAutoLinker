using Newtonsoft.Json;

namespace CreatioAutoPackageLinkerBlazor.Data.Rest;

public class ValidationWorkspaceMessage
{
    [JsonProperty("itemName", NullValueHandling = NullValueHandling.Ignore)]
    public string? ItemName { get; set; }

    [JsonProperty("itemType", NullValueHandling = NullValueHandling.Ignore)]
    public int? ItemType { get; set; }

    [JsonProperty("itemTypeCaption", NullValueHandling = NullValueHandling.Ignore)]
    public string? ItemTypeCaption { get; set; }

    [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
    public string? Message { get; set; }

    [JsonProperty("messageType", NullValueHandling = NullValueHandling.Ignore)]
    public int? MessageType { get; set; }

    [JsonProperty("packageName", NullValueHandling = NullValueHandling.Ignore)]
    public string? PackageName { get; set; }

    [JsonProperty("packageUId", NullValueHandling = NullValueHandling.Ignore)]
    public string? PackageUId { get; set; }
}
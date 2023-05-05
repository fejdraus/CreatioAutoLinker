using CreatioAutoPackageLinkerBlazor.Data.Object;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;

namespace CreatioAutoPackageLinkerBlazor.Data.Rest;

public class ValidateWorkspaceRoot : BasePackageProperties
{
    [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
    public bool Value { get; set; }

    [JsonProperty("validationError", NullValueHandling = NullValueHandling.Ignore)]
    public string? ValidationError { get; set; }

    [JsonProperty("validationMessages", NullValueHandling = NullValueHandling.Ignore)]
    public List<ValidationWorkspaceMessage>? ValidationMessages { get; set; }
}
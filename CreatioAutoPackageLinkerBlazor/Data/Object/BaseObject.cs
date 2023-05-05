using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace CreatioAutoPackageLinkerBlazor.Data.Object;

public class BaseObject
{
    [Required]
    [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
    public Guid Id { get; set; }
    public bool RecordInactive { get; set; }
}
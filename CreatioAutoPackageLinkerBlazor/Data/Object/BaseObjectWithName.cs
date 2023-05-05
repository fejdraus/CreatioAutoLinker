using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace CreatioAutoPackageLinkerBlazor.Data.Object;

public class BaseObjectWithName : BaseObject
{
        
    [Required]
    [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
    public string? Name { get; set; }
}
using CreatioAutoPackageLinkerBlazor.Data.Object;
using Newtonsoft.Json;

namespace CreatioAutoPackageLinkerBlazor.Data.In;

public class InPackage : BaseObjectWithName
{
    [JsonProperty("createdBy", NullValueHandling = NullValueHandling.Ignore)]
    public string? CreatedBy { get; set; }

    [JsonProperty("createdOn", NullValueHandling = NullValueHandling.Ignore)]
    public DateTime? CreatedOn { get; set; }

    [JsonProperty("dependentPackages", NullValueHandling = NullValueHandling.Ignore)]
    public List<InPackage>? DependentPackages { get; set; }

    [JsonProperty("dependsOnPackages", NullValueHandling = NullValueHandling.Ignore)]
    public List<InPackage>? DependsOnPackages { get; set; }

    [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
    public string? Description { get; set; }

    [JsonProperty("isLocked", NullValueHandling = NullValueHandling.Ignore)]
    public bool IsLocked { get; set; }

    [JsonProperty("maintainer", NullValueHandling = NullValueHandling.Ignore)]
    public string? Maintainer { get; set; }

    [JsonProperty("modifiedBy", NullValueHandling = NullValueHandling.Ignore)]
    public string? ModifiedBy { get; set; }

    [JsonProperty("modifiedOn", NullValueHandling = NullValueHandling.Ignore)]
    public DateTime? ModifiedOn { get; set; }

    [JsonProperty("position", NullValueHandling = NullValueHandling.Ignore)]
    public int? Position { get; set; }
    
    [JsonProperty("isReadOnly", NullValueHandling = NullValueHandling.Ignore)]
    public bool IsReadOnly { get; set; }

    [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
    public int? Type { get; set; }

    [JsonProperty("uId", NullValueHandling = NullValueHandling.Ignore)]
    public Guid UId { get; set; }

    [JsonProperty("version", NullValueHandling = NullValueHandling.Ignore)]
    public string? Version { get; set; }
}
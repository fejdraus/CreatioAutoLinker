using CreatioAutoPackageLinkerBlazor.Data.Object;
using Newtonsoft.Json;

namespace CreatioAutoPackageLinkerBlazor.Data.Rest;

public class ResponseSavePackageProperties : BasePackageProperties
{
    [JsonProperty("compilationRequired", NullValueHandling = NullValueHandling.Ignore)]
    public bool CompilationRequired { get; set; }

    public int StatesCode { get; set; }
}
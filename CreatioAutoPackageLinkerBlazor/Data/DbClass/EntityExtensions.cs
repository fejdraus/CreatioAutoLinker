using Newtonsoft.Json;

namespace CreatioAutoPackageLinkerBlazor.Data.DbClass;

public static class EntityExtensions
{
    public static T? Clone<T>(this T entity) where T : class, new()
    {
        var jsonString = JsonConvert.SerializeObject(entity);
        return JsonConvert.DeserializeObject<T>(jsonString);
    }
}
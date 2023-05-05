using CreatioAutoPackageLinkerBlazor.Data.Rest;

namespace CreatioAutoPackageLinkerBlazor.Services;

public class HasConnectionService
{
    private readonly ICreatioService _creatioService;
    public HasConnectionService(ICreatioService creatioService)
    {
        _creatioService = creatioService;
    }
    public virtual async Task<bool> HasConnection(Guid parentPackageUId, Guid packageUId, string url, string login, string password)
    {
        return await _creatioService.DoesPackageHaveParentById(url,
            $"{{\"packageUId\":\"{packageUId.ToString()}\", \"parentPackageUId\":\"{parentPackageUId.ToString()}\"}}",login, password);
    }
}
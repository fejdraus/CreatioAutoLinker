using System;
using System.Threading;
using System.Threading.Tasks;
using CreatioAutoPackageLinkerBlazor.Data.DbClass.Repository;
using CreatioAutoPackageLinkerBlazor.Data.Object;
using CreatioAutoPackageLinkerBlazor.Data.Out;
using CreatioAutoPackageLinkerBlazor.Data.Rest;
using Microsoft.AspNetCore.SignalR;

namespace CreatioAutoPackageLinkerBlazor.Services;

public class LinkerService
{
    private readonly ICreatioService _creatioRest;
    private readonly IDbRepository _dbRepository;
    private readonly IHubContext<Hub> _hubContext;

    public LinkerService(ICreatioService creatioRest, IDbRepository dbRepository, IHubContext<Hub> hubContext)
    {
        _creatioRest = creatioRest;
        _dbRepository = dbRepository;
        _hubContext = hubContext;
    }

    public async Task AutoLink(CancellationToken cancellationToken)
    {

    }

    public async Task GetPackages(CancellationToken cancellationToken)
    {
        
    }

    public async Task ApplyStructureChanges(CancellationToken cancellationToken)
    {
        
    }
    
    public void Stop()
    {
        //_hubContext.Clients.All.SendAsync("UpdateProgressBar", profileId, progress);
        // Ваша логика для остановки задач
    }
}
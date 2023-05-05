using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace CreatioAutoPackageLinkerBlazor.Services;

public class SignalRService
{
    private readonly IHubContext<SignalRHub> _hubContext;
    public event EventHandler<string>? OnMessageReceived;

    public SignalRService(IHubContext<SignalRHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendPublicMessageAsync(string message)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
    }

    public async Task SendPrivateMessageAsync(string connectionId, string message)
    {
        await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
    }

    public async Task SendMessage(string message)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
        OnMessageReceived?.Invoke(this, message);
    }
}
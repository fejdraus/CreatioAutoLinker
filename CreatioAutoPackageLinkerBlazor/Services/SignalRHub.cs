using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace CreatioAutoPackageLinkerBlazor.Services;

public class SignalRHub : Hub
{
    public async Task SendMessage(string message)
    {
        // Обработка сообщения от клиента
        // Например, отправка сообщения другим клиентам через Clients
        await Clients.Others.SendAsync("ReceiveMessage", message);
    }
}
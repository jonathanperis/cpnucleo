using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace dotnet_cpnucleo_pages.Hubs
{
    public class FluxoTrabalhoHub : Hub
    {
        public Task Send(string message)
        {
            return Clients.All.InvokeAsync("Send", message);
        }
    }
}
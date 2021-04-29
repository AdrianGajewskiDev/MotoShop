using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace MotoShop.WebAPI.SignalR.Hubs
{
    public class MessagesHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            var connectionID = Context.ConnectionId;

            return base.OnConnectedAsync();
        }
    }
}

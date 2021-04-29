using Microsoft.AspNetCore.SignalR;
using MotoShop.Services.Services;
using System.Threading.Tasks;

namespace MotoShop.WebAPI.SignalR.Hubs
{
    public class MessagesHub : Hub
    {
        private readonly IWebSocketProviderService _webSocketProvider;

        public MessagesHub(IWebSocketProviderService webSocketProvider)
        {
            _webSocketProvider = webSocketProvider;
        }

        public override  async Task OnConnectedAsync()
        {
            var connectionID = Context.ConnectionId;
            var userID = Context.User.FindFirst(x => x.Type == "UserID").Value;

            if(await _webSocketProvider.HasActivConnection(userID))
                await _webSocketProvider.AddConnectionAsync(connectionID, userID);

            await base.OnConnectedAsync();
        }
    }
}

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MotoShop.Services.Services;
using MotoShop.WebAPI.Helpers;
using System.Text;
using System.Threading.Tasks;

namespace MotoShop.WebAPI.SignalR.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
            var userID = Context.User.FindFirst(x => x.Type == "UserID")?.Value;


            if (!string.IsNullOrEmpty(userID))
            {
                var strBuilder = new StringBuilder();
                var key = strBuilder.Append(userID).Append(StaticMessages.ConnectionID);

                await _webSocketProvider.UpdateConnectionIDAsync(key.ToString(), connectionID);
            }


            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(System.Exception exception)
        {
            var connectionID = Context.ConnectionId;

            _webSocketProvider.RemoveConnection(connectionID);

            return base.OnDisconnectedAsync(exception);
        }
    }
}

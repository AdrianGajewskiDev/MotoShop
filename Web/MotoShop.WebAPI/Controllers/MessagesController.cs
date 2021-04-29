using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MotoShop.WebAPI.SignalR.Hubs;
using System.Threading.Tasks;

namespace MotoShop.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IHubContext<MessagesHub> _messagesHub;

        public MessagesController(IHubContext<MessagesHub> messagesHub)
        {
            _messagesHub = messagesHub;
        }

        [HttpGet()]
        public async Task<IActionResult> SendMessage()
        {
            while(true)
            {
                await _messagesHub.Clients.All.SendAsync("message", "Message from server");
                await Task.Delay(2000);
            }

            return Ok();
        }
    }
}

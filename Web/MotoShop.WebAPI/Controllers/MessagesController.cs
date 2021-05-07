using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MotoShop.Data.Models.Messages;
using MotoShop.Services.Services;
using MotoShop.WebAPI.Extensions;
using MotoShop.WebAPI.Helpers;
using MotoShop.WebAPI.Models.Requests.Conversations;
using MotoShop.WebAPI.Models.Response;
using MotoShop.WebAPI.Models.Response.Conversations;
using MotoShop.WebAPI.SignalR.Hubs;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MotoShop.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IConversationService _conversationService;
        private readonly IWebSocketProviderService _webSocketProviderService;
        private readonly IHubContext<MessagesHub> _hub;

        public MessagesController(IConversationService conversationService, IWebSocketProviderService webSocketProviderService, IHubContext<MessagesHub> hub)
        {
            _conversationService = conversationService;
            _webSocketProviderService = webSocketProviderService;
            _hub = hub;
        }

        [HttpGet()]
        [Authorize]
        public ActionResult<ConversationResponseModel> GetConversation([FromQuery] string senderID, [FromQuery]string recipientID, [FromQuery]string topic)
        {
            if(_conversationService.HasConversation(senderID, recipientID))
            {
                Conversation conversation = _conversationService.GetConversationBySenderAndRecipient(senderID, recipientID);

                if(conversation is not null)
                {
                    var responseModel = new ConversationResponseModel
                    {
                        ReceiverID = recipientID,
                        SenderID = senderID,
                        Topic = topic,
                        Messages = conversation.Messages,
                        Id = conversation.Id,
                        ReceiverName = conversation.Receiver.UserName
                    };

                    return Ok(responseModel);
                }

                return BadRequest(StaticMessages.SomethingWentWrong);
            }

            var newConversation = new Conversation
            {
                ReceiverID = recipientID,
                SenderID = senderID,
                Topic = topic
            };

            var result = _conversationService.AddConversation(newConversation);

            if(result)
            {
                var responseModel = new ConversationResponseModel
                {
                    ReceiverID = recipientID,
                    SenderID = senderID,
                    Topic = topic,
                };

                return Ok(responseModel);
            }

            return BadRequest(StaticMessages.SomethingWentWrong);
        }


        [HttpGet("conversations")]
        [Authorize]
        public IActionResult GetConversations()
        {
            var userID = User.GetUserID();

            var conversationsList = _conversationService.GetUserConversations(userID);

            if (conversationsList.Conversations.Any())
            {
                var responseModel = new ConversationsListResponseModel
                {
                    ConversationsList = conversationsList
                };

                return Ok(responseModel);
            }

            return NotFound(StaticMessages.NotFound(nameof(Conversation), "UserID", userID));
        }

        [HttpPost()]
        public async Task<IActionResult> SendMessage(NewMessageRequestModel model)
        {
            if (model is null)
                return BadRequest(StaticMessages.WasNull(nameof(NewMessageRequestModel)));

            var newMessage = new Message
            {
                Content = model.Content,
                Read = false,
                Sent = DateTime.UtcNow,
                ConversationID = model.ConversationId
            };

            var result = _conversationService.AddMessage(newMessage);

            if(result)
            {
                var connectionID = await _webSocketProviderService.GetConnectionIDAsync(model.ReceiverID);

                if(!string.IsNullOrEmpty(connectionID))
                {
                    await _hub.Clients.Client(connectionID).SendAsync("message", newMessage);

                    return Ok();
                }
            }

            return BadRequest();
        }

    }
}

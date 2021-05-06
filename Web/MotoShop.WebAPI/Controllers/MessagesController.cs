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
using System.Linq;
using System.Threading.Tasks;

namespace MotoShop.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IHubContext<MessagesHub> _messagesHub;
        private readonly IConversationService _conversationService;

        public MessagesController(IHubContext<MessagesHub> messagesHub, IConversationService conversationService)
        {
            _messagesHub = messagesHub;
            _conversationService = conversationService;
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

    }
}

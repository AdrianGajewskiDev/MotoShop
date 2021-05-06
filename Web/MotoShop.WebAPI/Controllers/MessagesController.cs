using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MotoShop.Data.Models.Messages;
using MotoShop.Services.Services;
using MotoShop.WebAPI.Helpers;
using MotoShop.WebAPI.Models.Requests.Conversations;
using MotoShop.WebAPI.Models.Response;
using MotoShop.WebAPI.SignalR.Hubs;
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
                        Id = conversation.Id
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

    }
}

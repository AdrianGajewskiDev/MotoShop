using System;

namespace MotoShop.WebAPI.Models.Requests.Conversations
{
    public class NewMessageRequestModel
    {
        public int ConversationId { get; set; }
        public string ReceiverID { get; set; }
        public string Content { get; set; }
    }
}

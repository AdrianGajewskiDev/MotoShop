using MotoShop.Data.Models.Messages;
using System.Collections.Generic;

namespace MotoShop.WebAPI.Models.Response
{
    public class ConversationResponseModel
    {
        public int Id { get; set; }
        public string Topic { get; set; }
        public string SenderID { get; set; }
        public string ReceiverID { get; set; }
        public string ReceiverName { get; set; }
        public string SenderName { get; set; }

        public IEnumerable<Message> Messages { get; set; }
    }
}

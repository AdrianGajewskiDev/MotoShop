using System;

namespace MotoShop.Services.HelperModels
{
    public record ConversationListItemModel
    {
        public int Id { get; set;}
        public string Topic { get; set; }
        public DateTime LastMsgSentTime { get; set; }
        public string LastMsgContent { get; set; }
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
        public string SenderID { get; set; }
    }
}

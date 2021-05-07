namespace MotoShop.WebAPI.Models.Requests.Conversations
{
    public record GetConversationRequestModel
    {
        public string SenderID { get; set; }
        public string RecipientID { get; set; }
        public string Topic { get; set; }
    }
}

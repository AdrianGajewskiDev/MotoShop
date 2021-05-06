using MotoShop.Data.Models.Messages;

namespace MotoShop.Services.Services
{
    public interface IConversationService
    {
        bool HasConversation(string senderID, string recipientID);
        bool AddConversation(Conversation conversation);
        Conversation GetConversationBySenderAndRecipient(string senderID, string recipientID);
    }
}

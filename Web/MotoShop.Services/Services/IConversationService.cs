using MotoShop.Data.Models.Messages;
using MotoShop.Services.HelperModels;
using System.Collections.Generic;

namespace MotoShop.Services.Services
{
    public interface IConversationService
    {
        bool HasConversation(string senderID, string recipientID);
        bool AddConversation(Conversation conversation);
        Conversation GetConversationBySenderAndRecipient(string senderID, string recipientID);
        ConversationsListModel GetUserConversations(string userID);
    }
}

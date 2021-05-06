using Microsoft.EntityFrameworkCore;
using MotoShop.Data.Database_Context;
using MotoShop.Data.Models.Messages;
using MotoShop.Services.Services;
using System.Linq;

namespace MotoShop.Services.Implementation
{
    public class ConversationService : IConversationService
    {

        private readonly ApplicationDatabaseContext _dbContext;

        public ConversationService(ApplicationDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool AddConversation(Conversation conversation)
        {
            _dbContext.Conversations.Add(conversation);

            return _dbContext.SaveChanges() > 0;
        }

        public Conversation GetConversationBySenderAndRecipient(string senderID, string recipientID)
        {
            var conversation = _dbContext.Conversations.Where(x => x.SenderID == senderID && x.ReceiverID == recipientID).Include(x => x.Messages).FirstOrDefault();

            if(conversation is null)
            {
                conversation = _dbContext.Conversations.Where(x => x.SenderID == recipientID && x.ReceiverID == senderID).Include(x => x.Messages).FirstOrDefault();
            }

            return conversation;
        }

        public bool HasConversation(string senderID, string recipientID)
        {
            var senderConversations = _dbContext.Conversations.Where(x => x.SenderID == senderID).Select(x => x.ReceiverID);

            var flag = senderConversations.Any(x => x == recipientID);

            if (flag == false)
            {
                var receiverConversations = _dbContext.Conversations.Where(x => x.SenderID == recipientID).Select(x => x.ReceiverID);

                flag = receiverConversations.Any(x => x == senderID);
            }

            return flag;
        }
    }
}

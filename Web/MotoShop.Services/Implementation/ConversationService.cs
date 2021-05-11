using Microsoft.EntityFrameworkCore;
using MotoShop.Data.Database_Context;
using MotoShop.Data.Models.Messages;
using MotoShop.Services.HelperModels;
using MotoShop.Services.Services;
using System;
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
            var conversation = _dbContext.Conversations
                .Where(x => x.SenderID == senderID && x.ReceiverID == recipientID)
                .Include(x => x.Messages)
                .Include(x => x.Receiver)
                .Include(x => x.Sender)
                .FirstOrDefault();

            if (conversation is null)
            {
                conversation = _dbContext.Conversations
                    .Where(x => x.SenderID == recipientID && x.ReceiverID == senderID)
                    .Include(x => x.Messages)
                    .Include(x => x.Receiver)
                    .Include(x => x.Sender)
                    .FirstOrDefault();
            }

            return conversation;
        }

        public ConversationsListModel GetUserConversations(string userID)
        {
            var conversations = _dbContext.Conversations
                .Include(x => x.Messages)
                .Include(x => x.Sender)
                .Include(x => x.Receiver)
                .Where(x => x.SenderID == userID && x.Messages.Count() != 0)
                .Select(x => new ConversationListItemModel
                {
                    Id = x.Id,
                    LastMsgContent = x.Messages.OrderBy(x => x.Sent).Last().Content,
                    LastMsgSentTime = x.Messages.OrderBy(x => x.Sent).Last().Sent,
                    Topic = x.Topic,
                    ReceiverName = x.Receiver.UserName,
                    SenderID = x.SenderID,
                    SenderName = x.Sender.UserName,
                    ReceiverID = x.ReceiverID
                });

            if (conversations.Count() == 0)
            {
                conversations = _dbContext.Conversations
                   .Include(x => x.Messages)
                   .Include(x => x.Sender)
                   .Include(x => x.Receiver)
                   .Where(x => x.ReceiverID == userID && x.Messages.Count() != 0)
                   .Select(x => new ConversationListItemModel
                   {
                       Id = x.Id,
                       LastMsgContent = x.Messages.OrderBy(x => x.Sent).Last().Content,
                       LastMsgSentTime = x.Messages.OrderBy(x => x.Sent).Last().Sent,
                       Topic = x.Topic,
                       ReceiverName = x.Receiver.UserName,
                       SenderName = x.Sender.UserName,
                       SenderID = x.SenderID,
                       ReceiverID = x.ReceiverID
                   });
            }

            return new ConversationsListModel
            {
                Conversations = conversations
            };
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

        public bool AddMessage(Message newMessage)
        {
            _dbContext.Messages.Add(newMessage);

            return _dbContext.SaveChanges() > 0;
        }

        public int GetUnreadMessagesCountForUser(string userID)
        {
            var messages = _dbContext.Conversations.Where(x => x.SenderID == userID || x.ReceiverID == userID).Include(x => x.Messages.Where(x => x.SenderID != userID)).Select(x => x.Messages);

            int count = 0;

            foreach(var msg in messages)
            {
                var userMsg = msg.Where(x => x.SenderID != userID).ToList();

                count += userMsg.Count;
            }

            return count;
        }

        public bool HasAnyConversation(string v)
        {
            return _dbContext.Conversations.Any(x => x.SenderID == v || x.ReceiverID == v);
        }
    }
}

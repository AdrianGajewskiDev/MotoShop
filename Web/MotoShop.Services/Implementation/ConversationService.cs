﻿using Microsoft.EntityFrameworkCore;
using MotoShop.Data.Database_Context;
using MotoShop.Data.Models.Messages;
using MotoShop.Services.HelperModels;
using MotoShop.Services.Services;
using System;
using System.Collections.Generic;
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
            var conversation = _dbContext.Conversations.Where(x => x.SenderID == senderID && x.ReceiverID == recipientID).Include(x => x.Messages).Include(x => x.Receiver).FirstOrDefault();

            if (conversation is null)
            {
                conversation = _dbContext.Conversations.Where(x => x.SenderID == recipientID && x.ReceiverID == senderID).Include(x => x.Messages).Include(x => x.Receiver).FirstOrDefault();
            }

            return conversation;
        }

        public ConversationsListModel GetUserConversations(string userID)
        {
            var conversations = _dbContext.Conversations.Include(x => x.Messages).Where(x => x.SenderID == userID && x.Messages.Count() != 0).Select(x => new ConversationListItemModel
            {
                Id = x.Id,
                LastMsgContent = x.Messages.OrderBy(x => x.Sent).Last().Content,
                LastMsgSentTime = x.Messages.OrderBy(x => x.Sent).Last().Sent,
                Topic = x.Topic
            });

            if (conversations.Count() == 0)
            {
                conversations = _dbContext.Conversations.Include(x => x.Messages).Where(x => x.ReceiverID == userID && x.Messages.Count() != 0).Select(x => new ConversationListItemModel
                {
                    Id = x.Id,
                    LastMsgContent = x.Messages.OrderBy(x => x.Sent).Last().Content,
                    LastMsgSentTime = x.Messages.OrderBy(x => x.Sent).Last().Sent,
                    Topic = x.Topic
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
    }
}

using System;

namespace MotoShop.Data.Models.Messages
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Sent { get; set; }
        public bool Read { get; set; }
        public Conversation Conversation { get; set; }
    }
}

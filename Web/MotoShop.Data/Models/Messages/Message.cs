using MotoShop.Data.Models.User;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoShop.Data.Models.Messages
{
    public class Message
    {
        public int Id { get; set; }
        public string SenderID { get; set; }

        [ForeignKey(nameof(SenderID))]
        public ApplicationUser Sender { get; set; }

        public string ReceiverID { get; set; }

        [ForeignKey(nameof(ReceiverID))]
        public ApplicationUser Receiver { get; set; }

        public string Content { get; set; }
        public DateTime Sent { get; set; }
        public bool Read { get; set; }

    }
}

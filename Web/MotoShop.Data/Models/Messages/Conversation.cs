using MotoShop.Data.Models.User;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoShop.Data.Models.Messages
{
    public class Conversation
    {
        public int Id { get; set; }
        public string SenderID { get; set; }

        [ForeignKey(nameof(SenderID))]
        public ApplicationUser Sender { get; set; }

        public string ReceiverID { get; set; }

        [ForeignKey(nameof(ReceiverID))]
        public ApplicationUser Receiver { get; set; }

        public string Topic { get; set; }

        public ICollection<Message> Messages { get; set; }
    }
}

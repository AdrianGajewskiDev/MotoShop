using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoShop.Data.Models.User
{
    public class RefreshToken
    {
        [Key]
        public string Token { get; set; }
        public bool Used { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime CreationDate { get; set; }

        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }
    }
}


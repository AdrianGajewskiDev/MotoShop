using MotoShop.Data.Models.User;
using System;
using System.ComponentModel.DataAnnotations;

namespace MotoShop.Data.Models.Store
{
    public class Advertisement
    {
        [Required]
        public int ID { get; set; }
        [Required]
        [MaxLength(length: 50)]
        public string Title { get; set; }
        [Required]
        [MaxLength(length: 200)]
        public string Description { get; set; }
        public DateTime Placed { get; set; }

        public string AuthorID { get; set; }
        public virtual ApplicationUser Author { get; set; }

        public virtual ShopItem ShopItem { get; set; }

    }
}

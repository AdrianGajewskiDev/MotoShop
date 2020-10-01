using MotoShop.Data.Models.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoShop.Data.Models.Store
{
    public class ShopItem
    {
        public int ID { get; set; }

        [Required]
        [MaxLength(length: 20)]
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }

        public string OwnerID { get; set; }
        public string ItemType { get; set; }

    }
}

using MotoShop.Data.Models.Store;
using MotoShop.Data.Models.User;
using System;

namespace MotoShop.WebAPI.Models.Response.Advertisements
{
    public class AdvertisementDetailsResponseModel<T> where T: ShopItem
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Placed { get; set; }
        public string AuthorID { get; set; }
        public ApplicationUser Author { get; set; }
        public T ShopItem { get; set; }
    }
}

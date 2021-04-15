using MotoShop.Data.Models.Store;
using MotoShop.Data.Models.User;
using MotoShop.WebAPI.Models.Response.UserAccount;
using System;
using System.Collections.Generic;

namespace MotoShop.WebAPI.Models.Response.Advertisements
{
    public class AdvertisementDetailsResponseModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Placed { get; set; }
        public string AuthorID { get; set; }
        public UserAccountDetailsResponseModel Author { get; set; }
        public object ShopItem { get; set; }

        public IEnumerable<string> ImageUrls { get; set; }
    }
}

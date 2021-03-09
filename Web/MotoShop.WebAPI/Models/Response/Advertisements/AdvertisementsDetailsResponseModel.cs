using System.Collections.Generic;

namespace MotoShop.WebAPI.Models.Response.Advertisements
{
    public class AdvertisementsDetailsResponseModel
    {
        public IEnumerable<AdvertisementDetailsResponseModel> Advertisements { get; set; }
    }
}

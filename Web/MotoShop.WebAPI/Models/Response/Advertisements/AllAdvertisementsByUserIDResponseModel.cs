using MotoShop.Data.Models.Store;
using System.Collections.Generic;

namespace MotoShop.WebAPI.Models.Response.Advertisements
{
    public class AllAdvertisementsByUserIDResponseModel
    {
        public IEnumerable<Advertisement> Advertisements { get; set; }
    }
}

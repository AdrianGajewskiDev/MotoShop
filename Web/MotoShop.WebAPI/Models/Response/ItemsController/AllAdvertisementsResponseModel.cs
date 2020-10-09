using MotoShop.Data.Models.Store;
using System.Collections.Generic;

namespace MotoShop.WebAPI.Models.Response.ItemsController
{
    public class AllAdvertisementsResponseModel
    {
        public IEnumerable<Advertisement> Advertisements { get; set; }
    } 
}

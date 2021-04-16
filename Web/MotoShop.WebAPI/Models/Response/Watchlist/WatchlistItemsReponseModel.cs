using MotoShop.Data.Models.Store;
using MotoShop.Services.HelperModels;
using System.Collections.Generic;

namespace MotoShop.WebAPI.Models.Response.Watchlist
{
    public class WatchlistItemsReponseModel
    {
        public IEnumerable<AdvertisementOveralInfoModel> Items { get; set; }
    }
}

using MotoShop.Data.Models.Store;
using MotoShop.Services.HelperModels;
using System;
using System.Collections.Generic;

namespace MotoShop.WebAPI.Models.Response.Watchlist
{
    public class WatchlistItemsReponseModel
    {
        public IEnumerable<WatchListItemModel> Items { get; set; }
    }
}

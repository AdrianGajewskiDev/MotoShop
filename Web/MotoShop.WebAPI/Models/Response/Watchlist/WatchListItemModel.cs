using System;
using System.Collections.Generic;

namespace MotoShop.WebAPI.Models.Response.Watchlist
{
    public class WatchListItemModel
    {
        public int Id { get; set; }
        public IEnumerable<string> ImageUrls { get; set; }
        public DateTime PinDate { get; set; }
        public string Title { get; set; }
    }
}

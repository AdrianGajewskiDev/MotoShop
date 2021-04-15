using System;

namespace MotoShop.Data.Models.Store
{
    public class WatchlistItem
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public DateTime PinDate { get; set; }

        public int WatchlistId { get; set; }
        public Watchlist Watchlist { get; set; }
    }
}

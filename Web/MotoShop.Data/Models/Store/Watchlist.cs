using MotoShop.Data.Models.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MotoShop.Data.Models.Store
{
    public class Watchlist
    {
        [Key]
        public int Id { get; set; }
        public IEnumerable<WatchlistItem> Items { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}

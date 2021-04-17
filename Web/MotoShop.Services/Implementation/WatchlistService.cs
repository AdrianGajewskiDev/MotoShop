using Microsoft.EntityFrameworkCore;
using MotoShop.Data.Database_Context;
using MotoShop.Data.Models.Store;
using MotoShop.Services.Services;
using System.Collections.Generic;
using System.Linq;

namespace MotoShop.Services.Implementation
{
    public class WatchlistService : IWatchlistService
    {
        private readonly ApplicationDatabaseContext _context;

        public WatchlistService(ApplicationDatabaseContext context)
        {
            _context = context;
        }

        public bool AddItemToWatchlist( WatchlistItem watchlistItem)
        {
            _context.WatchlistItems.Add(watchlistItem);

            if (_context.SaveChanges() > 0)
                return true;

            return false;
        }

        public bool DeleteWatchlistItem(int id)
        {
            var itemToRemove = new WatchlistItem { Id = id };

            _context.WatchlistItems.Remove(itemToRemove);

            if (_context.SaveChanges() > 0)
                return true;

            return false;
        }

        public Watchlist GetWatchlistById(int id)
        {
            return _context.Watchlists.Where(x => x.Id == id).Include(x => x.Items).FirstOrDefault();
        }

        public Watchlist GetWatchlistByUserId(string id)
        {
            return _context.Watchlists.Where(x => x.UserId == id).Include(x => x.Items).FirstOrDefault();
        }

        public int GetWatchlistId(string userID)
        {
            return _context.Watchlists.Where(x => x.UserId == userID).Select(x => x.Id).FirstOrDefault();
        }

        public IEnumerable<WatchlistItem> GetWatchlistItems(string userID)
        {
            var watchlistId = GetWatchlistId(userID);
            
            return _context.WatchlistItems.Where(x => x.WatchlistId == watchlistId);
        }
    }
}

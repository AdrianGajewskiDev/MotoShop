using MotoShop.Data.Models.Store;
using System.Collections.Generic;

namespace MotoShop.Services.Services
{
    public interface IWatchlistService
    {
        IEnumerable<WatchlistItem> GetWatchlistItems(string userID);
        Watchlist GetWatchlistById(int id);
        Watchlist GetWatchlistByUserId(string id);
        int GetWatchlistId(string userID);
        bool AddItemToWatchlist( WatchlistItem watchlistItem);
        bool DeleteWatchlistItem(int id);
    }
}

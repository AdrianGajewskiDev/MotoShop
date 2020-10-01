using Microsoft.EntityFrameworkCore;
using MotoShop.Data.Database_Context;
using MotoShop.Data.Models.Store;
using MotoShop.Services.Services;
using System.Collections.Generic;
using System.Linq;

namespace MotoShop.Services.Implementation
{
    public class AdvertisementService : IAdvertisementService
    {
        private readonly ApplicationDatabaseContext _context;

        public AdvertisementService(ApplicationDatabaseContext context)
        {
            _context = context;
        }

        public Advertisement GetAdvertisementById(int id, bool includeAuthorAndItem = true)
        {
            if (includeAuthorAndItem == true)
                return _context.Advertisements
                    .Where(x => x.ID == id)
                    .Include(x => x.Author)
                    .Include(x => x.ShopItem)
                    .FirstOrDefault();

            return _context.Advertisements
                    .Where(x => x.ID == id)
                    .FirstOrDefault();
        }

        public IEnumerable<Advertisement> GetAll()
        {
            return _context.Advertisements;
        }

        public IEnumerable<Advertisement> GetAllAdvertisementsByAuthorId(string authorID)
        {
            return GetAll().Where(x => x.AuthorID == authorID);
        }
    }
}

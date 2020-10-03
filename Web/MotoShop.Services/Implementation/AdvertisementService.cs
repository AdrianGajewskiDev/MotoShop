using Microsoft.EntityFrameworkCore;
using MotoShop.Data.Database_Context;
using MotoShop.Data.Models.Constants;
using MotoShop.Data.Models.Store;
using MotoShop.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotoShop.Services.Implementation
{
    public class AdvertisementService : IAdvertisementService
    {
        private readonly ApplicationDatabaseContext _context;
        private readonly IShopItemsService _service;

        public AdvertisementService(ApplicationDatabaseContext context, IShopItemsService service)
        {
            _context = context;
            _service = service;
        }

        public async Task AddAdvertisementAsync(Advertisement advertisement)
        {
            if (advertisement == null)
                throw new ArgumentNullException();

            if(advertisement.ShopItem != null)
            {
                await _service.AddItemAsync(advertisement.ID, advertisement.ShopItem);
            }
            await _context.Advertisements.AddAsync(advertisement);
            await _context.SaveChangesAsync();
        }

        public void DeleteAdvertisement(int advertisementID)
        {
            var ad = GetAdvertisementById(advertisementID);

            switch (ad.ShopItem.ItemType)
            {
                case "Car":
                    _context.Cars.Remove(ad.ShopItem as Car);
                    break;
                case "Motocycle":
                    _context.Motocycles.Remove(ad.ShopItem as Motocycle);
                    break;

            }

            _context.Advertisements.Remove(ad);

            _context.SaveChanges();
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

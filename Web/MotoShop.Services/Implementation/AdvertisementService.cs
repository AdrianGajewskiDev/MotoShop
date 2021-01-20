using MotoShop.Data.Database_Context;
using MotoShop.Data.Models.Store;
using MotoShop.Services.EntityFramework.CompiledQueries;
using MotoShop.Services.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MotoShop.Services.Implementation
{
    public class AdvertisementService : IAdvertisementService
    {
        private readonly ApplicationDatabaseContext _context;

        public AdvertisementService(ApplicationDatabaseContext context)
        {
            _context = context;
        }

        public async Task<bool> AddAdvertisementAsync(Advertisement advertisement)
        {
            if (advertisement == null)
                throw new ArgumentNullException();


            await _context.Advertisements.AddAsync(advertisement);

            if (await _context.SaveChangesAsync() > 0)
                return true;

            return false;
        }

        public void DeleteAdvertisement(int advertisementID)
        {
            var ad = GetAdvertisementById(advertisementID);

            if (ad == null)
                return;

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
            return AdvertisementQueries.GetByID(_context, id);
        }


        public IEnumerable<Advertisement> GetAll()
        {
            return AdvertisementQueries.GetAll(_context);
        }

        public IEnumerable<Advertisement> GetAllAdvertisementsByAuthorId(string authorID)
        {
            return AdvertisementQueries.GetAllAdvertisementsByAuthorId(_context, authorID);
        }
    }
}

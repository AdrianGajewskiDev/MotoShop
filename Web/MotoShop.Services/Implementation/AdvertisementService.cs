using Microsoft.EntityFrameworkCore;
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
        private readonly IShopItemsService _shopItemsService;
        public AdvertisementService(ApplicationDatabaseContext context, IShopItemsService shopItemsService)
        {
            _context = context;
            _shopItemsService = shopItemsService;
        }
        public async Task<bool> AddAdvertisementAsync(Advertisement advertisement)
        {
            if (advertisement == null)
                throw new ArgumentNullException();


            _context.Advertisements.Add(advertisement);

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
        public IEnumerable<Advertisement> GetByTitle(string title)
        {
            return AdvertisementQueries.GetByTitle(_context, title.ToLower());
        }
        public async Task<bool> UpdateAdvertisementAsync(int id, Advertisement newAdvertisement, Advertisement oldAdvertisement)
        {
            if (oldAdvertisement.ID != newAdvertisement.ID)
                return false;

            oldAdvertisement.Description = newAdvertisement.Description;
            oldAdvertisement.Title = newAdvertisement.Title;
            oldAdvertisement.Placed= newAdvertisement.Placed;

            var shopItemUpdateResult = await _shopItemsService.UpdateItemAsync(oldAdvertisement.ShopItem.ID, newAdvertisement.ShopItem);

            if (shopItemUpdateResult == false)
                return false;

            _context.Entry(oldAdvertisement).State = EntityState.Modified;
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }
    }
}

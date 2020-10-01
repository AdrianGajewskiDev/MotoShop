using MotoShop.Data.Database_Context;
using MotoShop.Data.Models.Constants;
using MotoShop.Data.Models.Store;
using MotoShop.Services.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MotoShop.Services.Implementation
{
    public class ShopItemsService : IShopItemsService
    {
        private readonly ApplicationDatabaseContext _context;
        private readonly IAdvertisementService _advertisementService;

        public ShopItemsService(ApplicationDatabaseContext context, IAdvertisementService advertisementService)
        {
            _context = context;
            _advertisementService = advertisementService;
        }

        public async Task<bool> AddItemAsync(int advertisementID, ShopItem item)
        {
            Advertisement advertisement = _advertisementService.GetAdvertisementById(advertisementID);
            advertisement.ShopItem = item;

            if (await _context.SaveChangesAsync() > 0)
                return true;

            return false;
        }

        public void DeleteItem(ShopItem item)
        {
            _context.Remove(item);
            _context.SaveChanges();
        }


        public ShopItem GetItemByAdvertisement(int advertisementID)
        {
            var items = _context.Advertisements.Where(x => x.ID == advertisementID).FirstOrDefault().ShopItem;

            switch (items.ItemType)
            {
                case "Car":
                    return GetItemByID(items.ID,ItemType.Car );
                case "Motocycle":
                    return GetItemByID(items.ID, ItemType.Motocycle);
            }

            return null;
        }

        public ShopItem GetItemByID(int id, ItemType type)
        {
            switch (type)
            {
                case ItemType.Car:
                    return _context.Cars.Where(x => x.ID == id).FirstOrDefault();
                case ItemType.Motocycle:
                    return _context.Motocycles.Where(x => x.ID == id).FirstOrDefault();
                case ItemType.CarParts:
                    break;
            }

            return _context.Cars.Where(x => x.ID == id).FirstOrDefault();
        }

        public async Task<bool> UpdateItemAsync(int itemID, ShopItem updatedItem)
        {
            var item = GetItemByID(itemID, Enum.Parse<ItemType>(updatedItem.ItemType));

            _context.Update<ShopItem>(updatedItem);

            if (await _context.SaveChangesAsync() > 0)
                return true;

            return false;
        }
    }
}

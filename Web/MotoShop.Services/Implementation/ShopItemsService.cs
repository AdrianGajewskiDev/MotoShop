using Microsoft.EntityFrameworkCore;
using MotoShop.Data.Database_Context;
using MotoShop.Data.Models.Constants;
using MotoShop.Data.Models.Store;
using MotoShop.Services.EntityFramework.CompiledQueries;
using MotoShop.Services.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MotoShop.Services.Implementation
{
    public class ShopItemsService : IShopItemsService
    {
        private readonly ApplicationDatabaseContext _context;
        public ShopItemsService(ApplicationDatabaseContext context)
        {
            _context = context;
        }
        public async Task<bool> AddItemAsync(int advertisementID, ShopItem item)
        {
            Advertisement advertisement = AdvertisementQueries.GetByID(_context, advertisementID);
            advertisement.ShopItem = item;

            switch (item.ItemType)
            {
                case "Car":
                    {
                        await _context.Cars.AddAsync(advertisement.ShopItem as Car);
                    }break;
                case "Motocycle":
                    {
                        await _context.Motocycles.AddAsync(advertisement.ShopItem as Motocycle);
                    }
                    break;
            }


            if (await _context.SaveChangesAsync() > 0)
                return true;

            return false;
        }
        public void DeleteItem(ShopItem item)
        {
            _context.Remove(item);
            _context.SaveChanges();
        }
        public Car GetCarItem(int id)
        {
            return _context.Cars.FirstOrDefault(x => x.ID == id);
        }
        public ShopItem GetItemByAdvertisement(int advertisementID)
        {
            var item = AdvertisementQueries.GetByID(_context,advertisementID).ShopItem;

            if (item == null)
                return null;

            switch (item.ItemType)
            {
                case "Car":
                    return GetItemByID(item.ID,ItemType.Car );
                case "Motocycle":
                    return GetItemByID(item.ID, ItemType.Motocycle);
            }

            return null;
        }
        public ShopItem GetItemByID(int id, ItemType type)
        {
            switch (type)
            {
                case ItemType.Car:
                    return ShopItemQueries.GetByID<Car>(_context, id);
                case ItemType.Motocycle:
                    return ShopItemQueries.GetByID<Motocycle>(_context, id);
                case ItemType.CarParts:
                    break;
            }

            return _context.Cars.Where(x => x.ID == id).FirstOrDefault();
        }
        public Motocycle GetMotocycleItem(int id)
        {
            return _context.Motocycles.FirstOrDefault(x => x.ID == id);
        }
        public async Task<bool> UpdateItemAsync(int itemID, ShopItem updatedItem)
        {
            var item = GetItemByID(itemID, Enum.Parse<ItemType>(updatedItem.ItemType));

            item.ImageUrl = updatedItem.ImageUrl;
            item.ItemType = updatedItem.ItemType;
            item.Price = updatedItem.Price;


            _context.Entry(item).State = EntityState.Modified;

            if (await _context.SaveChangesAsync() > 0)
                return true;

            return false;
        }
    }
}

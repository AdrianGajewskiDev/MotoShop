using Microsoft.EntityFrameworkCore;
using MotoShop.Data.Database_Context;
using MotoShop.Data.Models.Constants;
using MotoShop.Data.Models.Store;
using MotoShop.Services.EntityFramework.CompiledQueries;
using MotoShop.Services.HelperModels;
using MotoShop.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            return AdvertisementQueries.GetAllWithAuthorAndShopItem(_context);
        }
        public IEnumerable<Advertisement> GetAllAdvertisementsByAuthorId(string authorID)
        {
            return AdvertisementQueries.GetAllAdvertisementsByAuthorId(_context, authorID);
        }
        public IEnumerable<Advertisement> GetByTitle(string title, ICollection<Advertisement> all)
        {
            title = title.ToLower();

            //split, then search for all keywords and return item if contains any of the given keyword

            var result = all.Where(ad => ad.Title.ToLower().Contains(title));

            if (result.Any())
                return result;

            string[] keywords = title.Split(" ");

            result = SearchByKeywords(ad => ad.Title, keywords, all)
                .ToList();

            return result;
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
        public IQueryable<Advertisement> SearchByKeywords(Func<Advertisement, string>  selector, string[] keywords, ICollection<Advertisement> all) 
        {
            List<Advertisement> result = new List<Advertisement>();

            foreach (var key in keywords)
            {
                foreach (var ad in all)
                {
                    if (selector(ad).ToLower().Contains(key.ToLower()))
                        result.Add(ad);
                }
            }

            return result.AsQueryable();
        }
        public TopThreeAdvertisementsResult GetTopThree()
        {
            var advertisements = _context.Cars.Select(x => new AdvertisementOveralInfoModel 
            {
                BodyType = x.CarType,
                Gearbox = x.Gearbox,
                HP = x.HorsePower,
                Id = _context.Advertisements.Where(ad => ad.ShopItem.ID == x.ID).Select(id => id.ID).FirstOrDefault(),
                Name = $"{x.CarBrand} {x.CarModel}",
                ProductionYear = x.YearOfProduction.Year,
                CubicCapacity = x.CubicCapacity,
                Price = x.Price,
                Mileage = x.Mileage
            }).ToArray();

            foreach (var ad in advertisements)
            {
                ad.ImageUrl = _context.Images.Where(x => x.AdvertisementID == ad.Id).Select(x => x.FilePath);
            }

            var sportCars = advertisements.Where(x => x.BodyType == CarType.Coupe.ToString()).OrderByDescending(x => x.HP).Take(3).ToArray();
            var suvCars  = advertisements.Where(x => x.BodyType == CarType.MUV_SUV.ToString().Replace('_', '/').ToUpper()).OrderByDescending(x => x.HP).Take(3).ToArray();
            var sedanCars  = advertisements.Where(x => x.BodyType == CarType.Sedan.ToString()).OrderByDescending(x => x.HP).Take(3).ToArray();

            return new TopThreeAdvertisementsResult
            {
                SedanCars = sedanCars,
                SportCars = sportCars,
                SuvCars = suvCars
            };
        }
        public void AddImage(int adID, string path)
        {
            var ad = GetAdvertisementById(adID);

            ad.ShopItem.ImageUrl = path;

            _context.Entry(ad).State = EntityState.Modified;

            _context.SaveChanges();
        }
    }
}

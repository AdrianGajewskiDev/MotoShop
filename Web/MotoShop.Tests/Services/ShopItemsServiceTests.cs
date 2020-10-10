using Microsoft.EntityFrameworkCore;
using Moq;
using MotoShop.Data.Database_Context;
using MotoShop.Data.Models.Constants;
using MotoShop.Data.Models.Store;
using MotoShop.Services.Implementation;
using MotoShop.Services.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MotoShop.Tests.Services
{
    public class ShopItemsServiceTests
    {
        private const string _connectionString = "Server=(localdb)\\mssqllocaldb;Database=MotoShop;Trusted_Connection=True;MultipleActiveResultSets=true";
        private const string _userID = "e0d6c11b-5f16-4194-99be-cf698eda3820";

        [Fact]
        public void Shop_Items_Service_Should_Return_Shop_Item_By_Advertisement_ID()
        {
            DbContextOptionsBuilder<ApplicationDatabaseContext> dbContextOptions = new DbContextOptionsBuilder<ApplicationDatabaseContext>().UseSqlServer(_connectionString);
            ApplicationDatabaseContext context = new ApplicationDatabaseContext(dbContextOptions.Options);
            IAdvertisementService service = new Mock<IAdvertisementService>().Object;
            IShopItemsService shopItemService = new ShopItemsService(context);

            ShopItem item = shopItemService.GetItemByAdvertisement(18);

            Assert.NotNull(item);
            Assert.IsType<Car>(item);
        }

        [Fact]
        public async Task Shop_Items_Service_Should_Add_New_Item_To_Advertisement()
        {
            DbContextOptionsBuilder<ApplicationDatabaseContext> dbContextOptions = new DbContextOptionsBuilder<ApplicationDatabaseContext>().UseSqlServer(_connectionString);
            ApplicationDatabaseContext context = new ApplicationDatabaseContext(dbContextOptions.Options);
            IAdvertisementService service = new AdvertisementService(context);
            IShopItemsService shopItemService = new ShopItemsService(context);


            var advertisement = new Advertisement
            {
                AuthorID = _userID,
                Description = "Selling a Romet",
                Placed = DateTime.UtcNow,
                Title = "Romet"
            };

            await service.AddAdvertisementAsync(advertisement);


            ShopItem moto = new Motocycle
            {
                Acceleration = 10,
                CubicCapacity = 50,
                Fuel = Fuel.Petrol.ToString(),
                FuelConsumption = 2,
                HorsePower = 2,
                ImageUrl = "ImageURl",
                ItemType = ItemType.Motocycle.ToString(),
                Lenght = 2,
                MotocycleBrand = "Romet",
                MotocycleModel = "727",
                OwnerID = _userID,
                Price = 500,
                Width = 1,
                YearOfProduction = new DateTime(2015, 5,15)
            };

            var result = await shopItemService.AddItemAsync(advertisement.ID, moto);

            var advert = new AdvertisementService(context).GetAdvertisementById(advertisement.ID);


            Assert.True(result);
            Assert.NotNull(advert);
            Assert.NotNull(advert.Author);
            Assert.NotNull(advert.ShopItem);
            Assert.IsType<Motocycle>(advert.ShopItem);
        }

        [Fact]
        public async Task Shop_Items_Service_Should_Update_Item()
        {
            DbContextOptionsBuilder<ApplicationDatabaseContext> dbContextOptions = new DbContextOptionsBuilder<ApplicationDatabaseContext>().UseSqlServer(_connectionString);
            ApplicationDatabaseContext context = new ApplicationDatabaseContext(dbContextOptions.Options);
            IShopItemsService shopItemService = new ShopItemsService(context);
            IAdvertisementService service = new AdvertisementService(context);

            var item = shopItemService.GetItemByID(16, ItemType.Car);

            item.Price = 100;

            var result = await shopItemService.UpdateItemAsync(16, item);

            Assert.True(result);
            Assert.Equal(100, shopItemService.GetItemByID(16, ItemType.Car).Price);
        }


    }
}

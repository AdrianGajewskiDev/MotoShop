using Microsoft.EntityFrameworkCore;
using Moq;
using MotoShop.Data.Database_Context;
using MotoShop.Data.Models.Store;
using MotoShop.Services.Implementation;
using MotoShop.Services.Services;
using System.Linq;
using Xunit;

namespace MotoShop.Tests.Services
{
    public class AdvertisementServiceTests
    {
        private const string _connectionString = "Server=(localdb)\\mssqllocaldb;Database=MotoShop;Trusted_Connection=True;MultipleActiveResultSets=true";
        private const string _userID = "e0d6c11b-5f16-4194-99be-cf698eda3820";

        [Fact]
        public void Advertisement_Service_Should_Return_Advertisement_By_Id_With_Author_And_ShopItem()
        {
            DbContextOptionsBuilder<ApplicationDatabaseContext> dbContextOptions = new DbContextOptionsBuilder<ApplicationDatabaseContext>().UseSqlServer(_connectionString);
            ApplicationDatabaseContext context = new ApplicationDatabaseContext(dbContextOptions.Options);
            IShopItemsService shopItemsService = new Mock<IShopItemsService>().Object;
            IAdvertisementService service = new AdvertisementService(context, null);


            var actual = service.GetAdvertisementById(18);

            Assert.NotNull(actual.Author);
            Assert.NotNull(actual.ShopItem);
            Assert.IsType<Car>(actual.ShopItem);
        }

        [Fact]
        public void Advertisement_Service_Should_Return_All_Advertisement_By_AuthorID()
        {
            DbContextOptionsBuilder<ApplicationDatabaseContext> dbContextOptions = new DbContextOptionsBuilder<ApplicationDatabaseContext>().UseSqlServer(_connectionString);
            ApplicationDatabaseContext context = new ApplicationDatabaseContext(dbContextOptions.Options);
            IShopItemsService shopItemsService = new Mock<IShopItemsService>().Object;
            IAdvertisementService service = new AdvertisementService(context,null);

            var actual = service.GetAllAdvertisementsByAuthorId(_userID);
            var expected = 2;

            Assert.Equal(expected, actual.Count());
        }

        [Fact]
        public void Advertisement_Service_Should_Return_All_Advertisements()
        {
            DbContextOptionsBuilder<ApplicationDatabaseContext> dbContextOptions = new DbContextOptionsBuilder<ApplicationDatabaseContext>().UseSqlServer(_connectionString);
            ApplicationDatabaseContext context = new ApplicationDatabaseContext(dbContextOptions.Options);
            IShopItemsService shopItemsService = new Mock<IShopItemsService>().Object;
            IAdvertisementService service = new AdvertisementService(context, null);

            var actual = service.GetAll().Count();
            var expected = 3;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Advertisement_Service_Should_Delete_Advertisement_With_Corresponding_Shop_Item()
        {
            DbContextOptionsBuilder<ApplicationDatabaseContext> dbContextOptions = new DbContextOptionsBuilder<ApplicationDatabaseContext>().UseSqlServer(_connectionString);
            ApplicationDatabaseContext context = new ApplicationDatabaseContext(dbContextOptions.Options);
            IShopItemsService shopItemsService = new ShopItemsService(context);
            IAdvertisementService service = new AdvertisementService(context, null);

            service.DeleteAdvertisement(23);

            Assert.Null(service.GetAdvertisementById(23));
            Assert.Null(shopItemsService.GetItemByID(20, Data.Models.Constants.ItemType.Motocycle));
        }

    }
}

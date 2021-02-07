using MotoShop.Data.Database_Context;
using MotoShop.Data.Models.Store;
using MotoShop.Services.Services;
using System;

namespace MotoShop.WebAPI.Helpers.Database
{
    public class DatabaseSeeder
    {
        private readonly ApplicationDatabaseContext _dbContext;
        private readonly IApplicationUserService _userService;
        private readonly IAdvertisementService _adService;
        private readonly IShopItemsService _shopItemService;

        public DatabaseSeeder(ApplicationDatabaseContext dbContext, IApplicationUserService userService,
            IAdvertisementService adService, IShopItemsService shopItemService)
        {
            _dbContext = dbContext;
            _userService = userService;
            _adService = adService;
            _shopItemService = shopItemService;
        }

        private string[] _carBrands = 
        {
            "BMW",
            "Audi",
            "Mercedes",
            "Seat",
            "Volkswagen",
            "Fiat",
            "Ford",
            "Toyota",
            "Renault",
            "KIA",
            "Mitsubishi"
        };

        private string[] _motoBrands =
        {
            "Romet",
            "BMW",
            "Suzuki",
            "Yamaha",
            "Kawasaki"
        };

        private string[] _fuels =
        {
            "Petrol",
            "Diesel",
            "Gas",
            "Electric"
        };

        private string[] _Gearbox = { "Manual", "Automatic", "SemiAuto"}; 
        private string[] _carBodyTypes = { "Sedan", "Coupe", "SUV", "Combi"};

        public async void AddAdvertisements(string userID, int count) 
        {
            for(int i = 0; i <= count; i++)
            {

                var rnd = new Random();

                var carNameIndex = rnd.Next(0, _carBrands.Length);
                var carBodyTypeIndex = rnd.Next(0, _carBodyTypes.Length);
                var carFuelIndex = rnd.Next(0, _fuels.Length);
                var carGearboxIndex = rnd.Next(0, _Gearbox.Length);

                var carName = _carBrands[carNameIndex];
                var carBody = _carBodyTypes[carBodyTypeIndex];
                var carFuel = _fuels[carFuelIndex];
                var carGearbox = _fuels[carGearboxIndex];

                var item = new Car
                {
                    Acceleration = rnd.Next(0, 10),
                    CarBrand = carName,
                    CarModel = "model",
                    CarType = carBody,
                    CubicCapacity = rnd.Next(500, 5000),
                    Fuel = carFuel,
                    FuelConsumption = rnd.Next(5, 20),
                    Gearbox = carGearbox,
                    HorsePower = rnd.Next(100, 1000),
                    ImageUrl = @"wwwroot/resources/images/CarImage.jpg",
                    ItemType = "Car",
                    Lenght = rnd.Next(3, 6),
                    NumberOfDoors = carBody == "Coupe" ? 3 : 5,
                    NumberOfSeats = 4,
                    OwnerID = userID,
                    Price = rnd.Next(0, 10000),
                    Width = 2,
                    YearOfProduction = new DateTime(rnd.Next(2000, 2021), rnd.Next(1, 12), rnd.Next(1, 31))
                };

                var descriptionTemplate = $"I'm selling a {carName}, with {item.HorsePower} HP. It's a {item.CarType} with {item.NumberOfDoors} doors. The price is {item.Price}";

                var advertisement = new Advertisement
                {
                    AuthorID = userID,
                    Author = await _userService.GetUserByID(userID),
                    Description = descriptionTemplate,
                    Placed = DateTime.Now,
                    Title = $"Selling a {carName}",
                    ShopItem = item
                };

                _dbContext.Add(advertisement);
            }
            _dbContext.SaveChanges();
        }
    }
}

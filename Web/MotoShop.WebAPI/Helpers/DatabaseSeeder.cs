using Microsoft.AspNetCore.Identity;
using MotoShop.Data.Database_Context;
using MotoShop.Data.Models.Constants;
using MotoShop.Data.Models.Store;
using MotoShop.Data.Models.User;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MotoShop.WebAPI.Helpers
{
    public class DatabaseSeeder
    {
        private readonly ApplicationDatabaseContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DatabaseSeeder(ApplicationDatabaseContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task AddAdvertisementToDatabase()
        {
         

                Car car = new Car 
                {
                    Acceleration = 4,
                    CarBrand = "BMW",
                    CarModel = "m4",
                    CarType = "Coupe",
                    CubicCapacity = 2500,
                    Fuel = Fuel.Petrol.ToString(),
                    FuelConsumption = 15,
                    Gearbox = Gearbox.Manual.ToString(),
                    HorsePower = 200,
                    ImageUrl = "ImageUrl",
                    Lenght = 4.5f,
                    NumberOfDoors = 3, 
                    NumberOfSeats = 4,
                    OwnerID = "e0d6c11b-5f16-4194-99be-cf698eda3820",
                    Price = 1600,
                    Width = 1.5f,
                    YearOfProduction = new DateTime(2001, 1,10),
                    ItemType = ItemType.Car.ToString()
                };

            _context.Cars.Add(car);

            Advertisement advertisement = new Advertisement()
            {
                AuthorID = "e0d6c11b-5f16-4194-99be-cf698eda3820",
                Description = "Selling a BMW m4",
                Placed = DateTime.UtcNow,
                Title = "Selling a BMW ",
                ShopItem = car
            };

            _context.Advertisements.Add(advertisement);




            Car cr = new Car
            {
                Acceleration = 4,
                CarBrand = "BMW",
                CarModel = "x4",
                CarType = "SUV",
                CubicCapacity = 3900,
                Fuel = Fuel.Petrol.ToString(),
                FuelConsumption = 25,
                Gearbox = Gearbox.Auto.ToString(),
                HorsePower = 200,
                ImageUrl = "ImageUrl",
                Lenght = 4.5f,
                NumberOfDoors = 5,
                NumberOfSeats = 4,
                OwnerID = "e0d6c11b-5f16-4194-99be-cf698eda3820",
                Price = 1600,
                Width = 1.5f,
                YearOfProduction = new DateTime(2001, 1, 10),
                ItemType = ItemType.Car.ToString()

            };

            _context.Cars.Add(cr);


            Advertisement adverti = new Advertisement()
            {
                AuthorID = "e0d6c11b-5f16-4194-99be-cf698eda3820",
                Description = "Selling a BMW x4",
                Placed = DateTime.UtcNow,
                Title = "Selling a BMW ",
                ShopItem = cr 
            };

            _context.Advertisements.Add(adverti);
            await _context.SaveChangesAsync();
        }
    }
}

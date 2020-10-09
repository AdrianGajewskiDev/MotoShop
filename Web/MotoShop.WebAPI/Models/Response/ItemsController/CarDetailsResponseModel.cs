using MotoShop.Data.Models.Store;
using System;

namespace MotoShop.WebAPI.Models.Response.ItemsController
{
    public class CarDetailsResponseModel : ItemDetailsResponseModel
    {
        public int ID { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public string OwnerID { get; set; }
        public string ItemType { get; set; }
        public int HorsePower { get; set; }
        public int FuelConsumption { get; set; }
        public float Acceleration { get; set; }
        public float Lenght { get; set; }
        public float Width { get; set; }
        public float CubicCapacity { get; set; }
        public string Fuel { get; set; }
        public DateTime YearOfProduction { get; set; }
        public string CarBrand { get; set; }
        public string CarModel { get; set; }
        public string CarType { get; set; }
        public int NumberOfDoors { get; set; }
        public int NumberOfSeats { get; set; }
        public string Gearbox { get; set; }
    }
}

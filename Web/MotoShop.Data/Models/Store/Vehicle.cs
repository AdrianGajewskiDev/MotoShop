using System;
using System.ComponentModel.DataAnnotations;

namespace MotoShop.Data.Models.Store
{
    public class Vehicle : ShopItem
    {
  
        [Required]
        public int HorsePower { get; set; }
        [Required]
        public int FuelConsumption { get; set; }
        [Required]
        public float Acceleration { get; set; }
        [Required]
        public float Lenght { get; set; }
        [Required]
        public float Width { get; set; }
        [Required]
        public float CubicCapacity { get; set; }
        [Required]
        [MaxLength(length: 20)]
        public string Fuel { get; set; }
        public int Mileage { get; set; }
        public DateTime YearOfProduction { get; set; }
    }
}

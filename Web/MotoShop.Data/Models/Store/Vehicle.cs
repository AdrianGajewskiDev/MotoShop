using System;
using System.ComponentModel.DataAnnotations;

namespace MotoShop.Data.Models.Store
{
    public class Vehicle : ShopItem
    {
  
        [Required]
        [MaxLength(length: 10)]
        public int HorsePower { get; set; }
        [Required]
        [MaxLength(length: 10)]
        public int FuelConsumption { get; set; }
        [Required]
        [MaxLength(length: 10)]
        public float Acceleration { get; set; }
        [Required]
        [MaxLength(length: 10)]
        public float Lenght { get; set; }
        [Required]
        [MaxLength(length: 10)]
        public float Width { get; set; }
        [Required]
        [MaxLength(length: 10)]
        public float CubicCapacity { get; set; }
        [Required]
        [MaxLength(length: 20)]
        public string Fuel { get; set; }

        public DateTime YearOfProduction { get; set; }
    }
}

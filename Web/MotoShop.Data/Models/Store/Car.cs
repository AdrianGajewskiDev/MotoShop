using System.ComponentModel.DataAnnotations;

namespace MotoShop.Data.Models.Store
{
    public class Car : Vehicle
    {
        [Required]
        [MaxLength(length: 30)]
        public string CarBrand { get; set; }
        [Required]
        [MaxLength(length: 50)]
        public string CarModel { get; set; }
        [Required]
        [MaxLength(length: 50)]
        public string CarType { get; set; }
        [Required]
        [MaxLength(length: 10)]
        public int NumberOfDoors { get; set; }
        [Required]
        [MaxLength(length: 10)]
        public int NumberOfSeats { get; set; }
        public string Gearbox { get; set; }
    }
}

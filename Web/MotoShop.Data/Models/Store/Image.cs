using System.ComponentModel.DataAnnotations;

namespace MotoShop.Data.Models.Store
{
    public class Image
    {
        [Key]
        public int ID { get; set; }
        [MaxLength(length: 100)]
        public string FilePath { get; set; }
        public bool Deleted { get; set; }
        public Advertisement Advertisement { get; set; }

        [MaxLength(length: 100)]
        public int AdvertisementID { get; set; }
    }
}

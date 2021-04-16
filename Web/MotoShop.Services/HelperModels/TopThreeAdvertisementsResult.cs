using System.Collections.Generic;

namespace MotoShop.Services.HelperModels
{
    public class TopThreeAdvertisementsResult
    {
        public IEnumerable<AdvertisementOveralInfoModel> SportCars { get; set; }
        public IEnumerable<AdvertisementOveralInfoModel> SuvCars { get; set; }
        public IEnumerable<AdvertisementOveralInfoModel> SedanCars { get; set; }
    }

    public class AdvertisementOveralInfoModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> ImageUrl { get; set; }
        public string Gearbox { get; set; }
        public string BodyType { get; set; }
        public int HP { get; set; }
        public int Mileage { get; set; }
        public int ProductionYear { get; set; }
        public float CubicCapacity { get; set; }
        public float Price { get; set; }
    }
}

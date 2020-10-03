using Microsoft.AspNetCore.Mvc;
using MotoShop.Data.Models.Store;
using MotoShop.Services.Services;
using MotoShop.WebAPI.Helpers;
using System.Collections.Generic;

namespace MotoShop.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly DatabaseSeeder databaseSeeder;
        private readonly IAdvertisementService advertisementService;
        private readonly IShopItemsService shopItemsService;

        public ItemsController(DatabaseSeeder databaseSeeder, IAdvertisementService advertisementService, IShopItemsService shopItemsService)
        {
            this.databaseSeeder = databaseSeeder;
            this.advertisementService = advertisementService;
            this.shopItemsService = shopItemsService;
        }

        [HttpGet("All")]
        public IActionResult GetAllAdvertisements()
        {

            List<Advertisement> ads = new List<Advertisement>();

            var adverts = advertisementService.GetAll();

            foreach (var ad in adverts)
            {
                ads.Add(advertisementService.GetAdvertisementById(ad.ID));
            }

            return Ok(ads);
        }
    }
}

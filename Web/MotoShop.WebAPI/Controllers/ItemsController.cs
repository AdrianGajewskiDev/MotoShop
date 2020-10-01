using Microsoft.AspNetCore.Mvc;
using MotoShop.Services.Services;
using MotoShop.WebAPI.Helpers;
using System.Threading.Tasks;

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

        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            var ad = advertisementService.GetAdvertisementById(18);
            var item = shopItemsService.GetItemByAdvertisement(ad.ID);

            return Ok(item);
        }
    }
}

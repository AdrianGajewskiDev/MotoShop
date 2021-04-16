using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotoShop.Data.Models.Constants;
using MotoShop.Data.Models.Store;
using MotoShop.Services.HelperModels;
using MotoShop.Services.Services;
using MotoShop.WebAPI.Attributes;
using MotoShop.WebAPI.Extensions;
using MotoShop.WebAPI.Helpers;
using MotoShop.WebAPI.Models.Response.Watchlist;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MotoShop.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WatchlistController : ControllerBase
    {
        private readonly IWatchlistService _watchlistService;
        private readonly IShopItemsService _shopItemsService;
        private readonly IImageUploadService _imageUploadService;

        public WatchlistController(IWatchlistService watchlistService, IShopItemsService shopItemsService, IImageUploadService imageUploadService)
        {
            _watchlistService = watchlistService;
            _shopItemsService = shopItemsService;
            _imageUploadService = imageUploadService;
        }

        [HttpPost("{itemId}")]
        [ClearCache]
        public IActionResult AddToWatchlist(int itemId)
        {
            var watchlistId = _watchlistService.GetWatchlistId(User.GetUserID());

            var watchlistItem = new WatchlistItem 
            {
                ItemId = itemId,
                PinDate = DateTime.UtcNow,
                WatchlistId = watchlistId
            };

            var result = _watchlistService.AddItemToWatchlist(watchlistItem);

            if (result)
                return Ok(StaticMessages.Created(nameof(Watchlist)));

            return BadRequest(StaticMessages.SomethingWentWrong);
        }

        [HttpGet()]
        [Authorize]
        [Cache(10)]
        public IActionResult GetUserWatchlistItems()
        {
            var userID = User.GetUserID();

            var watchlist = _watchlistService.GetWatchlistItems(userID);

            if(watchlist.Any())
            {
                List<AdvertisementOveralInfoModel> models = new List<AdvertisementOveralInfoModel>();

                foreach (var item in watchlist)
                {
                    var model = BuildAdvertisementOveralInfoModel(item);

                    models.Add(model);
                }

                var responseModel = new WatchlistItemsReponseModel 
                {
                    Items = models
                };

                return Ok(responseModel);
            }

            return NotFound(StaticMessages.NotFound("Watchlist items", "User", userID)); 
        }

        private AdvertisementOveralInfoModel BuildAdvertisementOveralInfoModel(WatchlistItem item)
        {
            var advertisement = _shopItemsService.GetItemByAdvertisement(item.ItemId) as Car;

            var result = new AdvertisementOveralInfoModel
            {
                BodyType = advertisement.CarType,
                Gearbox = advertisement.Gearbox,
                HP = advertisement.HorsePower,
                Id = item.ItemId,
                Name = $"{advertisement.CarBrand} {advertisement.CarModel}",
                ProductionYear = advertisement.YearOfProduction.Year,
                CubicCapacity = advertisement.CubicCapacity,
                Price = advertisement.Price,
                Mileage = advertisement.Mileage,
                ImageUrl = _imageUploadService.GetImagePathsForItem(item.ItemId)
            };

            return result;
        }
    }
}

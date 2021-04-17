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
        private readonly IAdvertisementService _advertisementsService;
        private readonly IImageUploadService _imageUploadService;

        public WatchlistController(IWatchlistService watchlistService, IAdvertisementService shopItemsService, IImageUploadService imageUploadService)
        {
            _watchlistService = watchlistService;
            _advertisementsService = shopItemsService;
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

            var watchlist = _watchlistService.GetWatchlistByUserId(userID);

            if(watchlist.Items.Any())
            {
                List<WatchListItemModel> models = new List<WatchListItemModel>();

                foreach (var item in watchlist.Items)
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

        [HttpDelete("{watchlistItemId}")]
        [ClearCache]
        public IActionResult DeleteWatchlistItem(int watchlistItemId)
        {
            var result = _watchlistService.DeleteWatchlistItem(watchlistItemId);

            if (result)
                return Ok();

            return BadRequest(StaticMessages.SomethingWentWrong);
        }

        private WatchListItemModel BuildAdvertisementOveralInfoModel(WatchlistItem item)
        {
            //TODO: add functionality to handle motocycles
            var advertisementTitle = _advertisementsService.GetAdvertisementTitle(item.ItemId);
            var authorName = _advertisementsService.GetAdvertisementData(item.ItemId, x => x.Author.UserName);
            var price = _advertisementsService.GetAdvertisementData(item.ItemId, x => x.ShopItem.Price);

            var result = new WatchListItemModel
            {
                Id = item.ItemId,
                ImageUrls = _imageUploadService.GetImagePathsForItem(item.ItemId),
                PinDate = item.PinDate,
                Title = advertisementTitle,
                Price = price,
                AuthorName = authorName
            };

            return result;
        }
    }
}

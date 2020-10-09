using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotoShop.Data.Models.Constants;
using MotoShop.Data.Models.Store;
using MotoShop.Services.Services;
using MotoShop.WebAPI.Models.Requests;
using MotoShop.WebAPI.Models.Response;
using MotoShop.WebAPI.Models.Response.ItemsController;

namespace MotoShop.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertisementsController : ControllerBase
    {
        private readonly IAdvertisementService _advertisementService;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IShopItemsService _shopItemsService;

        public AdvertisementsController(IAdvertisementService advertisementService, IApplicationUserService applicationUserService, IShopItemsService shopItemsService)
        {
            _advertisementService = advertisementService;
            _applicationUserService = applicationUserService;
            _shopItemsService = shopItemsService;
        }

        [HttpGet("All")]
        public ActionResult<ApiResponse<AllAdvertisementsResponseModel>> GetAllAdvertisements()
        {
            IEnumerable<Advertisement> advertisements = _advertisementService.GetAll();

            if (advertisements == null)
                return NotFound("Cannot find any available advertisements");

            var model = new AllAdvertisementsResponseModel
            {
                Advertisements = advertisements
            };

            ApiResponse<AllAdvertisementsResponseModel> responseModel = new ApiResponse<AllAdvertisementsResponseModel>
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                ResponseContent = model
            };

            return Ok(responseModel);
        }

        [HttpPost("Add")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddAdvertisement([ FromBody]NewAdvertisementRequestModel newAdvertisementRequestModel)
        {
            if (newAdvertisementRequestModel == null)
                return BadRequest($"{nameof(newAdvertisementRequestModel)} was null");

            string userID = User.FindFirst(x => x.Type == "UserID").Value;

            Advertisement advertisement = new Advertisement
            {
                Author = await _applicationUserService.GetUserByID(userID),
                AuthorID =userID,
                Description = newAdvertisementRequestModel.Description,
                Placed = DateTime.Now,
                Title = newAdvertisementRequestModel.Title
            };

            if (newAdvertisementRequestModel.Car == null && newAdvertisementRequestModel.Motocycle == null)
                return BadRequest("Advertisement item was null");


            if (newAdvertisementRequestModel.Car != null)
                advertisement.ShopItem = newAdvertisementRequestModel.Car;
            else
                advertisement.ShopItem = newAdvertisementRequestModel.Motocycle;

            advertisement.ShopItem.OwnerID = userID;

            if (await _advertisementService.AddAdvertisementAsync(advertisement))
                return Ok();

            return BadRequest("Something bad has happened while trying to add new advertisement");
        }
    }
}

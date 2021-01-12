using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotoShop.Data.Models.Store;
using MotoShop.Services.Services;
using MotoShop.WebAPI.Attributes;
using MotoShop.WebAPI.Helpers;
using MotoShop.WebAPI.Models.Requests;
using MotoShop.WebAPI.Models.Response;
using MotoShop.WebAPI.Models.Response.Advertisements;
using MotoShop.WebAPI.Models.Response.ItemsController;

namespace MotoShop.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertisementsController : ControllerBase
    {
        private readonly IAdvertisementService _advertisementService;
        private readonly IApplicationUserService _applicationUserService;

        public AdvertisementsController(IAdvertisementService advertisementService, IApplicationUserService applicationUserService)
        {
            _advertisementService = advertisementService;
            _applicationUserService = applicationUserService;
        }

        [HttpGet()]
        [Cache(5)]
        public ActionResult<ApiResponse<AllAdvertisementsResponseModel>> GetAllAdvertisements()
        {
            IEnumerable<Advertisement> advertisements = _advertisementService.GetAll();

            if (advertisements == null)
                return NotFound(StaticMessages.NotFound("advertisements"));

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

        [HttpPost()]
        [ClearCache()]
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
                return BadRequest(StaticMessages.WasNull("Advertisement item"));


            if (newAdvertisementRequestModel.Car != null)
                advertisement.ShopItem = newAdvertisementRequestModel.Car;
            else
                advertisement.ShopItem = newAdvertisementRequestModel.Motocycle;

            advertisement.ShopItem.OwnerID = userID;

            if (await _advertisementService.AddAdvertisementAsync(advertisement))
                return Ok();

            return BadRequest(StaticMessages.SomethingWentWrong);
        }

        [HttpGet("byUser/{userID}")]
        public IActionResult GetAllByUserID(string userID)
        {
            var advertisements = _advertisementService.GetAllAdvertisementsByAuthorId(userID);

            if (advertisements == null)
                return NotFound(StaticMessages.NotFound("Advertisements", "user id", userID));


            var responseModel = new AllAdvertisementsByUserIDResponseModel
            {
                Advertisements = advertisements
            };

            return Ok(responseModel);
        }
    }
}

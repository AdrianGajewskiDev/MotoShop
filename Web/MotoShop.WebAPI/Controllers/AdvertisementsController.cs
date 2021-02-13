using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotoShop.Data.Models.Store;
using MotoShop.Services.Services;
using MotoShop.WebAPI.Attributes;
using MotoShop.WebAPI.Extensions;
using MotoShop.WebAPI.Helpers;
using MotoShop.WebAPI.Models.Requests;
using MotoShop.WebAPI.Models.Response;
using MotoShop.WebAPI.Models.Response.Advertisements;
using MotoShop.WebAPI.Models.Response.ItemsController;
using MotoShop.WebAPI.Models.Response.UserAccount;

namespace MotoShop.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertisementsController : ControllerBase
    {
        private readonly IAdvertisementService _advertisementService;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IShopItemsService _shopItemService;
        private readonly IMapper _mapper;

        public AdvertisementsController(IAdvertisementService advertisementService, IApplicationUserService applicationUserService, 
            IShopItemsService shopItemService, IMapper mapper)
        {
            _advertisementService = advertisementService;
            _applicationUserService = applicationUserService;
            _shopItemService = shopItemService;
            _mapper = mapper;
        }

        [HttpGet()]
        [Cache(5)]
        public ActionResult<AllAdvertisementsResponseModel> GetAllAdvertisements()
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
                AuthorID = userID,
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

        [HttpGet("{id}")]
        [Cache(5)]
        public IActionResult GetById(int id)
        {
            var advertisement = _advertisementService.GetAdvertisementById(id);

            if (advertisement == null)
                return NotFound(StaticMessages.NotFound("Advertisement", "ID", id));

            switch (advertisement.ShopItem.ItemType)
            {
                case "Car":
                    {
                        var model = new AdvertisementDetailsResponseModel<Car>
                        {
                            Author = _mapper.Map<UserAccountDetailsResponseModel>(advertisement.Author),
                            AuthorID = advertisement.AuthorID,
                            Description = advertisement.Description,
                            ID = advertisement.ID,
                            Placed = advertisement.Placed,
                            ShopItem = _shopItemService.GetCarItem(advertisement.ShopItem.ID),
                            Title = advertisement.Title
                        };

                        return Ok(model);

                    }
                case "Motocycle":
                    {
                        var model = new AdvertisementDetailsResponseModel<Motocycle>
                        {
                            Author = _mapper.Map<UserAccountDetailsResponseModel>(advertisement.Author),
                            AuthorID = advertisement.AuthorID,
                            Description = advertisement.Description,
                            ID = advertisement.ID,
                            Placed = advertisement.Placed,
                            ShopItem = _shopItemService.GetMotocycleItem(advertisement.ShopItem.ID),
                            Title = advertisement.Title
                        };

                        return Ok(model);

                    }
            }

            return BadRequest(StaticMessages.SomethingWentWrong);
        }

        [HttpDelete("{id}")]
        [ClearCache]
        public IActionResult DeleteAdvertisement(int id)
        {
            _advertisementService.DeleteAdvertisement(id);

            return Ok(StaticMessages.Deleted("Advertisement"));

        }

        [HttpPut("{id}")]
        [ClearCache]
        public async Task<IActionResult> UpdateAdvertisement(int id, [FromBody] UpdateAdvertisementRequestModel model)
        {
            if (model.DataModels.Length <= 0)
                return BadRequest(StaticMessages.Empty("There is no any data to update"));

            var originalAdvertisement = _advertisementService.GetAdvertisementById(id);
            var advertisement = _mapper.MapFromCollection<Advertisement>(model.DataModels, originalAdvertisement);
            advertisement.ShopItem = _mapper.MapFromCollection<ShopItem>(model.DataModels, originalAdvertisement.ShopItem);

            var result = await _advertisementService.UpdateAdvertisementAsync(id, advertisement, originalAdvertisement);

            //TODO: Inform owner about the changes 

            if (result == true)
                return Ok(StaticMessages.Updated("Advertisement"));
                
            return BadRequest(StaticMessages.SomethingWentWrong);
        }
    }
}

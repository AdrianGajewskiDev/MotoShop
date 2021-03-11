using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
using SendGrid.Helpers.Mail;

namespace MotoShop.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertisementsController : ControllerBase
    {
        private readonly IAdvertisementService _advertisementService;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IShopItemsService _shopItemService;
        private readonly IEmailSenderService _emailSender;
        private readonly IMapper _mapper;

        public AdvertisementsController(IAdvertisementService advertisementService, IApplicationUserService applicationUserService,
            IShopItemsService shopItemService, IMapper mapper, IEmailSenderService emailSender)
        {
            _advertisementService = advertisementService;
            _applicationUserService = applicationUserService;
            _shopItemService = shopItemService;
            _mapper = mapper;
            _emailSender = emailSender;
        }

        [HttpGet()]
        [Cache(5)]
        public ActionResult<PaginatedResponse<IEnumerable<Advertisement>>> GetAllAdvertisements([FromQuery] int? page, [FromQuery] int? pageSize)
        {
            IEnumerable<Advertisement> advertisements = _advertisementService.GetAll();

            if (advertisements == null)
                return NotFound(StaticMessages.NotFound("advertisements"));

            if( page is not null && pageSize is not null )
            {
                var paginatedResult = PaginatedResult.Create(advertisements, (int)pageSize, (int)page);

                float itemsCount = advertisements.Count();
                float size = (int)pageSize;

                float totalPages = itemsCount / size;

                var responseModel = new PaginatedResponse<IEnumerable<Advertisement>>
                {
                    Content = paginatedResult,
                    TotalPages = (int)Math.Round(totalPages)
                };

                return Ok(responseModel);
            }

            var response = new AllAdvertisementsResponseModel
            {
                Advertisements = advertisements
            };

            return Ok(response);      
        }

        [HttpPost()]
        [ClearCache()]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddAdvertisement([FromBody] NewAdvertisementRequestModel newAdvertisementRequestModel)
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

            var responseModel = BuildAdDetailsResponseModel(advertisement);

            if (responseModel != null)
                return Ok(responseModel);

            return BadRequest(StaticMessages.SomethingWentWrong);
        }

        [HttpGet("query")]
        public IActionResult GetByQuery([FromQuery, Required] string searchQuery)
        {

            //do we need to get All advertisements everytime ?
            var ad = _advertisementService.GetByTitle(searchQuery, _advertisementService.GetAll().ToList());

            if (ad.Any())
            {
                var responseModel = new AdvertisementsDetailsResponseModel 
                {
                    Advertisements = ad.Select(advert => BuildAdDetailsResponseModel(advert))
                };

                return Ok(responseModel);
            }

            return NotFound(StaticMessages.NotFound("Advertisement", "query", searchQuery));
        }

        [HttpDelete("{id}")]
        [ClearCache]
        public IActionResult DeleteAdvertisement(int id)
        {
            _advertisementService.DeleteAdvertisement(id);

            return Ok(StaticMessages.Deleted("Advertisement"));

        }

        [Authorize]
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

            if (result == true)
            {
                var userID = User.FindFirst(x => x.Type == "UserID").Value;

                if (!_advertisementService.IsOwner(userID, originalAdvertisement))
                {
                    var userEmail = await _applicationUserService.GetUserData(advertisement.AuthorID, x => x.Email);

                    string messageTemplate = $"Administrator changed some part of your advertisement: {advertisement.Title}. Changes: ";

                    foreach (var change in model.DataModels)
                    {
                        messageTemplate += change.Key;
                    }

                    await _emailSender.SendStandardMessageAsync(new EmailAddress(userEmail), "Advertisement Update", messageTemplate);
                }

                return Ok(StaticMessages.Updated("Advertisement"));
            }

            return BadRequest(StaticMessages.SomethingWentWrong);
        }

        #region Helpers
        private AdvertisementDetailsResponseModel BuildAdDetailsResponseModel(Advertisement advertisement)
        {
            switch (advertisement.ShopItem.ItemType)
            {
                case "Car":
                    {
                        var model = new AdvertisementDetailsResponseModel
                        {
                            Author = _mapper.Map<UserAccountDetailsResponseModel>(advertisement.Author),
                            AuthorID = advertisement.AuthorID,
                            Description = advertisement.Description,
                            ID = advertisement.ID,
                            Placed = advertisement.Placed,
                            ShopItem = _shopItemService.GetCarItem(advertisement.ShopItem.ID),
                            Title = advertisement.Title
                        };

                        return model;

                    }
                case "Motocycle":
                    {
                        var model = new AdvertisementDetailsResponseModel
                        {
                            Author = _mapper.Map<UserAccountDetailsResponseModel>(advertisement.Author),
                            AuthorID = advertisement.AuthorID,
                            Description = advertisement.Description,
                            ID = advertisement.ID,
                            Placed = advertisement.Placed,
                            ShopItem = _shopItemService.GetMotocycleItem(advertisement.ShopItem.ID),
                            Title = advertisement.Title
                        };

                        return model;

                    }
            }
            return null;
        }
        #endregion
    }
}

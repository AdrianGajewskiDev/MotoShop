using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MotoShop.Data.Models.Store;
using MotoShop.Services.HelperModels;
using MotoShop.Services.Services;
using MotoShop.WebAPI.Attributes;
using MotoShop.WebAPI.Extensions;
using MotoShop.WebAPI.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotoShop.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly IImageUploadService _imageUploadService;
        private readonly IApplicationUserService _userService;
        private readonly IShopItemsService _shopItemsService;
        private readonly IAdvertisementService _advertisementService;

        public UploadController(IImageUploadService imageUploadService, IApplicationUserService userService,
            IAdvertisementService advertisementService, IShopItemsService shopItemsService)
        {
            _imageUploadService = imageUploadService;
            _userService = userService;
            _advertisementService = advertisementService;
            _shopItemsService = shopItemsService;
        }

        [Authorize]
        [HttpPost("userProfile")]
        [DisableRequestSizeLimit]
        [ClearCache]
        public async Task<IActionResult> UploadUserProfileImage()
        {
            string userID = User.GetUserID();
            string dbPath = string.Empty;
            IFormFile image = Request.Form.Files[0];

            ImageUploadResult result = await _imageUploadService.UploadImageAsync(image);


            if (result.Success)
            {
                await _userService.AddUserProfileImageAsync(userID, result.Path);
                return Ok(new { path = result.Path });
            }

            return BadRequest(StaticMessages.SomethingWentWrong);
        }

        [Authorize]
        [HttpPost("adImages/{id}")]
        [DisableRequestSizeLimit]
        [ClearCache]
        public async Task<IActionResult> UploadAdvertisementImage(int id)
        {
            IFormFile[] images = Request.Form.Files.ToArray();


            MultipleImageUploadResult result =  await _imageUploadService.UploadMultipleImagesAsync(images, id);

            if (result.Success)
            {
                return Ok();
            }

            return BadRequest(StaticMessages.SomethingWentWrong);
        }

    }
}

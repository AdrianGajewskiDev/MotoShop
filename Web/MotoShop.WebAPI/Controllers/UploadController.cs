using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MotoShop.Data.Models.User;
using MotoShop.Services.HelperModels;
using MotoShop.Services.Services;
using MotoShop.WebAPI.Attributes;
using MotoShop.WebAPI.Extensions;
using System.Threading.Tasks;

namespace MotoShop.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly IImageUploadService _imageUploadService;
        private readonly IApplicationUserService _userService;

        public UploadController(IImageUploadService imageUploadService, IApplicationUserService userService)
        {
            _imageUploadService = imageUploadService;
            _userService = userService;
        }

        [Authorize]
        [HttpPost("single")]
        [DisableRequestSizeLimit]
        [ClearCache]
        public async Task<IActionResult> UploadSingleImage()
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

            return BadRequest("Something went wrong while trying to upload an image");

        }
    }
}

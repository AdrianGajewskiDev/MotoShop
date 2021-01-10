using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MotoShop.Data.Models.User;
using MotoShop.Services.HelperModels;
using MotoShop.Services.Services;
using MotoShop.WebAPI.Attributes;
using MotoShop.WebAPI.Helpers;
using MotoShop.WebAPI.Models.Requests;
using MotoShop.WebAPI.Models.Response;
using MotoShop.WebAPI.Models.Response.UserAccount;
using System;
using System.Threading.Tasks;

namespace MotoShop.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly IApplicationUserService _userService;
        private readonly ICachingService _cachingService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserAccountController(IApplicationUserService userService, ICachingService cachingService, IMapper mapper, IConfiguration configuration)
        {
            _userService = userService;
            _cachingService = cachingService;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpGet("details")]
        [Authorize]
        [IdentityCache(5)]
        public async Task<ActionResult<ApiResponse<UserAccountDetailsResponseModel>>> GetUserProfileDetails()
        {
            string userID = User.FindFirst(c => c.Type == "UserID").Value;

            if (string.IsNullOrEmpty(userID))
                return NotFound(StaticMessages.NotFound("User", "id", userID));

            var user = await _userService.GetUserByID(userID);

            var model = _mapper.Map<UserAccountDetailsResponseModel>(user);

            model.Roles = await  _userService.GetUserRolesAsync(userID);
            var responseModel = new ApiResponse<UserAccountDetailsResponseModel>
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                ResponseContent = model
            };

            return Ok(responseModel);
        }

        [HttpPost("updateUserData")]
        [Authorize]
        [ClearCache]
        public async Task<IActionResult> UpdateUserData(UpdateUserDataRequestModel model)
        {
            if (model == null)
                return BadRequest(new { message = StaticMessages.WasNull(nameof(model))});

            string userID = User.FindFirst(c => c.Type == "UserID").Value;
            ApplicationUser applicationUser = _mapper.Map<ApplicationUser>(model);

            var result = await _userService.UpdateUserDataAsync(userID,applicationUser);

            if(result.Result == false)
            {
                if(result.ErrorIdentificator != null)
                {
                    string dataType = (result.ErrorIdentificator == 1) ? "Email" : "Username";

                    return BadRequest(new { message = StaticMessages.Taken(dataType)});
                }

                return BadRequest(new { message = StaticMessages.SomethingWentWrong });
            }

            return Ok();
        }

        [HttpPost("changePassword")]
        [Authorize]

        public async Task<IActionResult> ChangePassword(NewPasswordRequestModel model)
        {
            if (model == null)
                return BadRequest(new { message = StaticMessages.WasNull(nameof(model))});

            ApplicationUser user = await _userService.GetUserByID(User.FindFirst(x => x.Type == "UserID").Value);

            var result = await _userService.SendPasswordChangingConfirmationMessageAsync(user, model.NewPassword);

            if(result == true)
                return Ok();

            return BadRequest(new { message = StaticMessages.SomethingWentWrong });
        }

        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequestModel model)
        {
            if (model == null)
                return BadRequest(new { message = $"{nameof(model)} was null" });

            ApplicationUser user = await _userService.GetUserByEmail(model.Email);

            if (user == null)
                return NotFound(new { message = StaticMessages.NotExist("User", "Email", model.Email)});


            var result = await _userService.SendPasswordChangingConfirmationMessageAsync(user, model.NewPassword);

            if (result == true)
                return Ok();

            return BadRequest(new { message = StaticMessages.SomethingWentWrong });
        }

        [HttpGet("verificationCallback")]
        public async Task<IActionResult> VerificationCallback([FromQuery]string userID,[FromQuery]string token, [FromQuery]string? dataType, [FromQuery] string? newData)
        {
            var user = await _userService.GetUserByID(userID);

            UpdateDataType updateDataType = UpdateDataType.None;

            if (!string.IsNullOrEmpty(dataType))
                updateDataType = Enum.Parse<UpdateDataType>(dataType);

            bool result = false;
            switch (updateDataType)
            {
                case UpdateDataType.None: 
                    {
                       result = await _userService.ConfirmUserEmailAsync(user, token);
                    }break;
                case UpdateDataType.Email:
                    {
                        result = await _userService.UpdateEmailAsync(user, token, newData);
                    }
                    break;
                case UpdateDataType.Password:
                    result = await _userService.UpdatePasswordAsync(user, token, newData);
                    break;
                default:
                    break;
            }

            string message = (updateDataType == UpdateDataType.None) ? "Your account is now confirmed!!" : $"{dataType} was successfully updated";

            if(result == true)
            {
                BrowserLauncher.Launch(_configuration["ApplicationUrls:Client"] + $"confirmation/{dataType.ToLower()}");
                await _cachingService.ClearCache(new string[1] {userID});
                return Ok(new { message =  message});
            }

            return BadRequest(new { message = StaticMessages.FailedToUpdate(dataType)});
        }
    }
}

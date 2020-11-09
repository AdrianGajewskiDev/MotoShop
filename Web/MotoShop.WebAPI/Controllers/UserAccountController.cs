using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using MotoShop.Data.Models.User;
using MotoShop.Services.HelperModels;
using MotoShop.Services.Services;
using MotoShop.WebAPI.Attributes;
using MotoShop.WebAPI.Models.Requests;
using MotoShop.WebAPI.Models.Response;
using MotoShop.WebAPI.Models.Response.UserAccount;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace MotoShop.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly IApplicationUserService _userService;
        private readonly IMapper _mapper;

        public UserAccountController(IApplicationUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("details")]
        [Authorize]
        [IdentityCache(5)]
        public async Task<ActionResult<ApiResponse<UserAccountDetailsResponseModel>>> GetUserProfileDetails()
        {
            string userID = User.FindFirst(c => c.Type == "UserID").Value;

            if (string.IsNullOrEmpty(userID))
                return NotFound($"Cannot find user with id of { userID}");

            var user = await _userService.GetUserByID(userID);

            var model = _mapper.Map<UserAccountDetailsResponseModel>(user);

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
                return BadRequest(new { message = "Request model was null" });

            string userID = User.FindFirst(c => c.Type == "UserID").Value;
            ApplicationUser applicationUser = _mapper.Map<ApplicationUser>(model);

            var result = await _userService.UpdateUserDataAsync(userID,applicationUser);

            if(result.Result == false)
            {
                if(result.ErrorIdentificator != null)
                {
                    string dataType = (result.ErrorIdentificator == 1) ? "Email" : "Username";

                    return BadRequest(new { message = $"{dataType} is already taken" });
                }

                return BadRequest(new { message = "Something went wrong while trying to update user profile" });
            }

            return Ok();
        }

        [HttpGet("verificationCallback")]
        public async Task<IActionResult> VerificationCallback([FromQuery]string userID,[FromQuery]string token, [FromQuery]string dataType, [FromQuery] string newData)
        {
            var user = await _userService.GetUserByID(userID);

            UpdateDataType updateDataType = Enum.Parse<UpdateDataType>(dataType);
            bool result = false;
            switch (updateDataType)
            {
                case UpdateDataType.UserName:
                    break;
                case UpdateDataType.Email:
                    {
                        result = await _userService.UpdateEmailAsync(user, token, newData);
                    }
                    break;
                case UpdateDataType.PhoneNumber:
                    break;
                case UpdateDataType.Name:
                    break;
                case UpdateDataType.Lastname:
                    break;
                case UpdateDataType.Password:
                    break;
                default:
                    break;
            }

            if(result == true)
                return Ok(new { message = $"{dataType} was successfully updated"});

            return BadRequest(new { message = $"Something went wrong while trying to update the {dataType}" });
        }
    }
}

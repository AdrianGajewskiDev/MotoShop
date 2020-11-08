using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotoShop.Data.Models.User;
using MotoShop.Services.Services;
using MotoShop.WebAPI.Attributes;
using MotoShop.WebAPI.Models.Requests;
using MotoShop.WebAPI.Models.Response;
using MotoShop.WebAPI.Models.Response.UserAccount;
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
    }
}

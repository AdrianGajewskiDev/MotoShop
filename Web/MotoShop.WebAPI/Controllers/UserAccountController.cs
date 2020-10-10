using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotoShop.Services.Services;
using MotoShop.WebAPI.Attributes;
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
        [Cache(5)]
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
    }
}

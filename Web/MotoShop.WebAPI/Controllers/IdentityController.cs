using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using MotoShop.Data.Helpers;
using MotoShop.Data.Models.User;
using MotoShop.Services.Services;
using MotoShop.WebAPI.Models;
using MotoShop.WebAPI.Token_Providers;
using Serilog;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MotoShop.WebAPI.Controllers
{
    [ApiController]
    [Route(template: "api/[controller]")]
    public class IdentityController : ControllerBase
    {

        private readonly IApplicationUserService _applicationUserService;
        private readonly LinkGenerator _linkGenerator;
        private readonly JsonWebTokenWriter _jsonWebTokenWriter;

        public IdentityController(IApplicationUserService applicationUserService, LinkGenerator linkGenerator, JsonWebTokenWriter jsonWebTokenWriter)
        {
            _applicationUserService = applicationUserService;
            _linkGenerator = linkGenerator;
            _jsonWebTokenWriter = jsonWebTokenWriter;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterNewUser([FromBody] UserRegisterRequestModel userRegisterRequestModel)
        {
            if (userRegisterRequestModel == null)
                return BadRequest(new { message = $"{nameof(userRegisterRequestModel)} was null" });

            var user = new ApplicationUser
            {
                Email = userRegisterRequestModel.Email,
                LastName = userRegisterRequestModel.LastName,
                Name = userRegisterRequestModel.Name,
                UserName = userRegisterRequestModel.UserName
            };

            var check = _applicationUserService.UserExists(user);

            if(check > 0)
            {
                if (check == 1)
                    return BadRequest(new { message = "Email is already taken"});
                else
                    return BadRequest(new { message = "Username is already taken" });
            }

            var result = await _applicationUserService.RegisterNewUserAsync(user, userRegisterRequestModel.Password);

            if (result == true)
            {
                Log.Information($"Registered new user with id of { user.Id}");
                return Created(_linkGenerator.GetPathByAction("Register", "Identity"), "User was succesfully created");
            }

            return BadRequest(new { message = "Something bad has happened while trying to register new user" });
        }

        [HttpPost("signIn")]
        public async Task<IActionResult> SignIn([FromBody] UserSignInRequestModel userSignInRequestModel)
        {
            if (userSignInRequestModel == null)
                return BadRequest(new { message = $"{nameof(userSignInRequestModel)} was null" });

            UserSignInVariant userLogInVariant = userSignInRequestModel.Data.Contains("@") ? UserSignInVariant.Email : UserSignInVariant.UserName;

            string userID = await _applicationUserService.SignInAsync(userSignInRequestModel.Data, userSignInRequestModel.Password, userLogInVariant);

            if(string.IsNullOrEmpty(userID))
            {
                string prefix = userLogInVariant == UserSignInVariant.Email ? "email" : "username";
                return NotFound(new { message = $"User with {prefix} of {userSignInRequestModel.Data} does not exists !!" });
            }

            Log.Information($"The { userSignInRequestModel.Data} logged in");

            Claim[] claims = { new Claim("UserID", userID) };

            var token = _jsonWebTokenWriter.GenerateToken(claims, 5000);

            return Ok(new { token = token });
        }
    }
}

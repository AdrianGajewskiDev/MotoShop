using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using MotoShop.Data.Helpers;
using MotoShop.Data.Models.User;
using MotoShop.Services.Services;
using MotoShop.WebAPI.Models.Request;
using MotoShop.WebAPI.Token_Providers;
using Serilog;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MotoShop.WebAPI.Controllers
{
    [ApiController]
    [Route(template: "api/[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly IExternalLoginProviderService _externalLoginProviderService;
        private readonly IApplicationUserService _applicationUserService;
        private readonly JsonWebTokenWriter _jsonWebTokenWriter;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IdentityController(IExternalLoginProviderService externalLoginProviderService, 
            IApplicationUserService applicationUserService, 
            SignInManager<ApplicationUser> signInManager,
            JsonWebTokenWriter jsonWebTokenWriter)
        {
            _externalLoginProviderService = externalLoginProviderService;
            _applicationUserService = applicationUserService;
            _signInManager = signInManager;
            _jsonWebTokenWriter = jsonWebTokenWriter;
        }

        [AllowAnonymous]
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
                await _applicationUserService.SendAccountConfirmationMessageAsync(await _applicationUserService.GetUserByUserName(user.UserName));
                Log.Information($"Registered new user with id of { user.Id}");
                return Ok(new { message = "User created successfully"});
            }

            return BadRequest(new { message = "Something bad has happened while trying to register new user" });
        }

        [AllowAnonymous]
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
                return NotFound(new { message = $"Invalid {prefix} or password " });
            }

            Log.Information($"The { userSignInRequestModel.Data} signed in");

            Claim[] claims = { new Claim("UserID", userID) };

            var token = _jsonWebTokenWriter.GenerateToken(claims, 5);

            return Ok(new { token = token });
        }

        [AllowAnonymous]
        [HttpGet("externalSignIn/{provider}")]
        public async Task<IActionResult> ExternalSignIn(string provider)
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("ExternalSignInCallback") };
            var externalLoginProviders = await _signInManager.GetExternalAuthenticationSchemesAsync();

            if (!externalLoginProviders.Any() || !externalLoginProviders.Any(x => x.Name == provider))
                return BadRequest("Didn't find any configured external login providers!!");

            return Challenge(properties, provider);
        }

        public async Task<IActionResult> ExternalSignInCallback()
        {
            var info = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            
            if (info == null)
                return NotFound();

            ApplicationUser user = new ApplicationUser
            {
                Name = info.Principal.FindFirstValue(ClaimTypes.Name).Split(" ")[0],
                LastName = info.Principal.FindFirstValue(ClaimTypes.Surname),
                Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                Id = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier)
            };

            if (string.IsNullOrEmpty(user.UserName))
                user.UserName = _externalLoginProviderService.BuildUsername(user);
                    

            if(_applicationUserService.UserExists(user) == 0)
            {
                var result = await _externalLoginProviderService.CreateAsync(user);

                if(result.Succeeded)
                {
                    var addLoginResult = await _externalLoginProviderService.AddLoginAsync(user, new UserLoginInfo(info.Properties.Items.First().Value
                        ,user.Id, info.Properties.Items.First().Value));

                    if(addLoginResult.Succeeded)
                    {
                        var token = _jsonWebTokenWriter.GenerateToken(new Claim[]
                        {
                            new Claim("UserID", user.Id)
                        }, 5);

                        return Ok(new { token = token });
                    }
                }
            }

            return BadRequest($"Something went wrong while trying to sign in: {info}");
        }
    }
}

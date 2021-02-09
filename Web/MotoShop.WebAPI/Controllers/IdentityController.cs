using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using MotoShop.Data.Helpers;
using MotoShop.Data.Models.User;
using MotoShop.Services.HelperModels;
using MotoShop.Services.Services;
using MotoShop.WebAPI.Helpers;
using MotoShop.WebAPI.Models.Request;
using MotoShop.WebAPI.Models.Requests;
using MotoShop.WebAPI.Token_Providers;
using Serilog;
using System;
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
        private readonly IMapper _mapper;

        public IdentityController(IExternalLoginProviderService externalLoginProviderService, IApplicationUserService applicationUserService, 
            JsonWebTokenWriter jsonWebTokenWriter, IMapper mapper)
        {
            _externalLoginProviderService = externalLoginProviderService;
            _applicationUserService = applicationUserService;
            _jsonWebTokenWriter = jsonWebTokenWriter;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterNewUser([FromBody] UserRegisterRequestModel userRegisterRequestModel)
        {
            if (userRegisterRequestModel == null)
                return BadRequest(new { message = StaticMessages.WasNull(nameof(userRegisterRequestModel)) });

            var user = _mapper.Map<ApplicationUser>(userRegisterRequestModel);

            var check = _applicationUserService.UserExists(user);

            if (check > 0)
            {
                if (check == 1)
                    return BadRequest(new { message = StaticMessages.Taken("Email") });
                else
                    return BadRequest(new { message = StaticMessages.Taken("Username") });
            }

            var result = await _applicationUserService.RegisterNewUserAsync(user, userRegisterRequestModel.Password);

            if (result != null)
            {
                await _applicationUserService.SendAccountConfirmationMessageAsync(await _applicationUserService.GetUserByUserName(user.UserName));
                Log.Information($"Registered new user with id of { user.Id}");
                return Ok(new { message = StaticMessages.Created("User")});
            }

            return BadRequest(new { message = StaticMessages.SomethingWentWrong });
        }

        [AllowAnonymous]
        [HttpPost("signIn")]
        public async Task<IActionResult> SignIn([FromBody] UserSignInRequestModel userSignInRequestModel)
        {
            if (userSignInRequestModel == null)
                return BadRequest(new { message = StaticMessages.WasNull(nameof(userSignInRequestModel)) });

            UserSignInVariant userLogInVariant = userSignInRequestModel.Data.Contains("@") ? UserSignInVariant.Email : UserSignInVariant.UserName;

            string userID = await _applicationUserService.SignInAsync(userSignInRequestModel.Data, userSignInRequestModel.Password, userLogInVariant);

            if (string.IsNullOrEmpty(userID))
            {
                string prefix = userLogInVariant == UserSignInVariant.Email ? "email" : "username";
                return NotFound(new { message = StaticMessages.InvalidSignInData(prefix) });
            }

            Log.Information($"The { userSignInRequestModel.Data} signed in");

            var role = await _applicationUserService.IsAdmin(userID) ? ApplicationRoles.Administrator : ApplicationRoles.NormalUser;
            var token = _jsonWebTokenWriter.GenerateToken(_jsonWebTokenWriter.AddStandardClaims(userID, role), 5);

            return Ok(new { token = token });
        }

        [AllowAnonymous]
        [HttpPost("externalSignIn")]
        public async Task<IActionResult> ExternalSignIn([FromBody] ExternalSignInRequestModel model)
        {
            if (!_externalLoginProviderService.CheckIfValidLoginProvider(model.Provider))
                return BadRequest(StaticMessages.InvalidLoginProvider);

            ExternalSignInProvider provider = Enum.Parse<ExternalSignInProvider>(model.Provider);

            var result = await _externalLoginProviderService.ValidateGoogleAccessTokenAsync(model.AccessToken);

            var user = await _applicationUserService.GetUserByEmail(result.Email);

            if(user != null)
            {
                var token = _jsonWebTokenWriter.GenerateToken("UserID", user.Id, 5);

                return Ok(new { token = token });
            }

            user = new ApplicationUser
            {
                Email = result.Email,
                ImageUrl = result.Picture,
                LastName = result.FamilyName,
                Name = result.GivenName,
                EmailConfirmed = result.EmailVerified,
                UserName = string.Concat(result.GivenName, result.FamilyName, provider)
            };

            var succeeded = await _externalLoginProviderService.Create(user, provider, model.ProviderID);

            if (succeeded)
            {
                var role = await _applicationUserService.IsAdmin(user.Id) ? ApplicationRoles.Administrator : ApplicationRoles.NormalUser;
                var token = _jsonWebTokenWriter.GenerateToken(_jsonWebTokenWriter.AddStandardClaims(user.Id, role), 5);

                return Ok(new { token = token });
            }

            return BadRequest(StaticMessages.SomethingWentWrong);
        }
    }
}

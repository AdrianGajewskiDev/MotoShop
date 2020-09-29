using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using MotoShop.Data.Models.User;
using MotoShop.Services.Services;
using MotoShop.WebAPI.Models;
using System.Threading.Tasks;

namespace MotoShop.WebAPI.Controllers
{
    [ApiController]
    [Route(template: "api/[controller]")]
    public class IdentityController : ControllerBase
    {

        private readonly IApplicationUserService _applicationUserService;
        private readonly LinkGenerator _linkGenerator;

        public IdentityController(IApplicationUserService applicationUserService, LinkGenerator linkGenerator)
        {
            _applicationUserService = applicationUserService;
            _linkGenerator = linkGenerator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterNewUser([FromBody] UserRegisterRequestModel userRegisterRequestModel)
        {
            if (userRegisterRequestModel == null)
                return BadRequest($"{nameof(userRegisterRequestModel)} was null");

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
                    return BadRequest("Email is already taken");
                else
                    return BadRequest("Username is already taken");
            }

            var result = await _applicationUserService.RegisterNewUserAsync(user, userRegisterRequestModel.Password);

            if (result == true)
                return Created(_linkGenerator.GetPathByAction("Register", "Identity"), "User was succesfully created");

            return BadRequest("Something bad has happened while trying to register new user");
        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotoShop.Data.Models.User;
using MotoShop.Services.HelperModels;
using MotoShop.Services.Services;
using MotoShop.WebAPI.Models.Request;
using System.Threading.Tasks;

namespace MotoShop.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdministrationController : ControllerBase
    {
        private readonly IAdministrationService _service;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IMapper _mapper;

        public AdministrationController(IAdministrationService service, IApplicationUserService applicationUserService, IMapper mapper)
        {
            _service = service;
            _applicationUserService = applicationUserService;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = ApplicationRoles.Administrator)]
        public async Task<IActionResult> CreateAdmin(UserRegisterRequestModel model)
        {
            if((await _applicationUserService.UserExists(model.Email, model.UserName)))
            {
                ApplicationUser user = await _applicationUserService.GetUserByEmail(model.Email);

                if (await _service.IsInRole(user, ApplicationRoles.Administrator))
                    return BadRequest("User already is a administrator");

                if (await _service.AddRoleToUser(user, ApplicationRoles.Administrator))
                    return Ok("Administrator successfully created");

                return BadRequest("Something went wrong while trying to complete the action. Try again");
            }

            var newUser = await _applicationUserService.RegisterNewUserAsync(_mapper.Map<ApplicationUser>(model), model.Password);

            if(newUser != null)
            {
                await _service.CreateAdminAsync(newUser);

                return Ok("Administrator successfully created");
            }

            return BadRequest("Something went wrong while trying to complete the action. Try again");
        }
    }
}

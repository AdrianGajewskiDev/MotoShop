using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotoShop.Data.Models.User;
using MotoShop.Services.HelperModels;
using MotoShop.Services.Services;
using MotoShop.WebAPI.Attributes;
using MotoShop.WebAPI.Helpers;
using MotoShop.WebAPI.Models.Request;
using MotoShop.WebAPI.Models.Response.Administration;
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
                    return BadRequest(StaticMessages.AlreadyIs(ApplicationRoles.Administrator));

                if (await _service.AddRoleToUser(user, ApplicationRoles.Administrator))
                    return Ok(StaticMessages.Created(ApplicationRoles.Administrator));

                return BadRequest(StaticMessages.SomethingWentWrong);
            }

            var newUser = await _applicationUserService.RegisterNewUserAsync(_mapper.Map<ApplicationUser>(model), model.Password);

            if(newUser != null)
            {
                await _service.CreateAdminAsync(newUser);

                return Ok(StaticMessages.Created(ApplicationRoles.Administrator));
            }

            return BadRequest(StaticMessages.SomethingWentWrong);
        }

        [HttpGet("{filter?}")]
        [Authorize(Roles = ApplicationRoles.Administrator)]
        [Cache(5)]
        public IActionResult GetAllUsers(string filter = null)
        {
            if(filter == null)
            {
                var users = _service.GetAllUsers();

                if (users == null)
                    return NotFound(StaticMessages.NotFound("Users"));

                var model = new GetAllUsersResponseModel<ApplicationUser>
                {
                    Users = users
                };

                return Ok(new { data = model });
            }

            var usersData = _service.GetUsersData<string>(filter);

            var dataModel = new GetAllUsersResponseModel<string>
            {
                Users = usersData
            };

            return Ok(new { data = dataModel });

        }
    }
}

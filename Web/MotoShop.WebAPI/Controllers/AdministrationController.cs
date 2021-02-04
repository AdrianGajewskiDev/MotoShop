using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotoShop.Data.Models.User;
using MotoShop.Services.HelperModels;
using MotoShop.Services.Services;
using MotoShop.WebAPI.Attributes;
using MotoShop.WebAPI.Helpers;
using MotoShop.WebAPI.Models.Request;
using MotoShop.WebAPI.Models.Requests.Administration;
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

        public AdministrationController(IAdministrationService service, IApplicationUserService applicationUserService, 
            IMapper mapper)
        {
            _service = service;
            _applicationUserService = applicationUserService;
            _mapper = mapper;
        }

        [HttpPost]
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
                await _applicationUserService.SendAccountConfirmationMessageAsync(newUser);

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

                return Ok(model);
            }

            var usersData = _service.GetUsersData<string>(filter);

            var dataModel = new GetAllUsersResponseModel<string>
            {
                Users = usersData
            };

            return Ok(dataModel );

        }

        [HttpPost("addRole")]
        [Authorize(Roles = ApplicationRoles.Administrator)]
        [ClearCache]
        public async Task<IActionResult> AddRole(AddRoleRequestModel model)
        {
            if (model == null)
                return BadRequest(StaticMessages.WasNull(nameof(model)));

            if (!await _service.RoleExists(model.Role))
                return NotFound(StaticMessages.NotFound("Role", "Role", model.Role));

            var user = await _applicationUserService.GetUserByID(model.UserID);

            if(user == null)
                return NotFound(StaticMessages.NotFound("User", "ID", model.UserID));

            var result = await _service.AddRoleToUser(user, model.Role);

            if (result == true)
                return Ok();

            return BadRequest(StaticMessages.SomethingWentWrong);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = ApplicationRoles.Administrator)]
        [ClearCache]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest(StaticMessages.WasNull(id));

            var result = await _applicationUserService.DeleteUser(id);

            if(result == true)
                return Ok();

            return BadRequest(StaticMessages.SomethingWentWrong);
        }
    }
}

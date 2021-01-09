using Microsoft.AspNetCore.Authorization;
using MotoShop.Services.HelperModels;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MotoShop.WebAPI.Authorization
{
    public class AdministratorAuthorizationRequirement : IAuthorizationRequirement
    { }

    public class AdministratorAuthorizationRequirementHandler : AuthorizationHandler<AdministratorAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdministratorAuthorizationRequirement requirement)
        {
            if (!context.User.HasClaim(x => x.Type == ClaimTypes.Role))
                return Task.CompletedTask;

            var role = context.User.FindFirst(x => x.Type == ClaimTypes.Role).Value;

            if (role == ApplicationRoles.Administrator)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }

}

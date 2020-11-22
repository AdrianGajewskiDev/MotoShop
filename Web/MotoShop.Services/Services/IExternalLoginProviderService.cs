using Microsoft.AspNetCore.Identity;
using MotoShop.Data.Models.User;
using System.Threading.Tasks;

namespace MotoShop.Services.Services
{
    public interface IExternalLoginProviderService
    {
        Task<IdentityResult> CreateAsync(ApplicationUser user);
        Task<IdentityResult> AddLoginAsync(ApplicationUser user, UserLoginInfo info);
        string BuildUsername(ApplicationUser user);
    }
}

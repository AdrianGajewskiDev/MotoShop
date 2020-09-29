using MotoShop.Data.Helpers;
using MotoShop.Data.Models.User;
using System.Threading.Tasks;

namespace MotoShop.Services.Services
{
    public interface IApplicationUserService
    {
        Task<bool> RegisterNewUserAsync(ApplicationUser user, string password);
        Task<string> SignInAsync(string data, string password, UserSignInVariant variant);
        int UserExists(ApplicationUser user);
    }
}

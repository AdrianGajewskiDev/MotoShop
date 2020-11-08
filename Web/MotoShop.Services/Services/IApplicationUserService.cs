using MotoShop.Data.Helpers;
using MotoShop.Data.Models.User;
using MotoShop.Services.HelperModels;
using System.Threading.Tasks;

namespace MotoShop.Services.Services
{
    public interface IApplicationUserService
    {
        Task<bool> RegisterNewUserAsync(ApplicationUser user, string password);
        Task<string> SignInAsync(string data, string password, UserSignInVariant variant);
        Task<ApplicationUser> GetUserByID(string id);
        Task<ApplicationUser> GetUserByEmail(string email);
        Task<ApplicationUser> GetUserByUserName(string username);
        int UserExists(ApplicationUser user);
        Task<UpdateResult> UpdateUserDataAsync(string userID, ApplicationUser model);
    }
}

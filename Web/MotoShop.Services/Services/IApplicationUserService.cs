using MotoShop.Data.Helpers;
using MotoShop.Data.Models.User;
using MotoShop.Services.HelperModels;
using System.Threading.Tasks;

namespace MotoShop.Services.Services
{
    public interface IApplicationUserService
    {
        int UserExists(ApplicationUser user);
        Task<bool> UserExists(string email, string username);
        Task<ApplicationUser> GetUserByID(string id);
        Task<ApplicationUser> GetUserByEmail(string email);
        Task<ApplicationUser> GetUserByUserName(string username);
        Task<UpdateResult> UpdateUserDataAsync(string userID, ApplicationUser model);
        Task<ApplicationUser> RegisterNewUserAsync(ApplicationUser user, string password);
        Task<bool> UpdateEmailAsync(ApplicationUser user, string token, string newEmail);
        Task<bool> SendAccountConfirmationMessageAsync(ApplicationUser user);
        Task<bool> SendPasswordChangingConfirmationMessageAsync(ApplicationUser user, string newPassword);
        Task<bool> ConfirmUserEmailAsync(ApplicationUser user, string token);
        Task<bool> UpdatePasswordAsync(ApplicationUser user,string token, string newPassword);
        Task<bool> AddUserProfileImageAsync(string userID, string path);
        Task<string> SignInAsync(string data, string password, UserSignInVariant variant);
        string GenerateConfirmationLink(string token, string userID, string newData, UpdateDataType updateDataType);
        string EncodeToken(string token);
        string DecodeToken(string token);

        Task<bool> IsAdmin(string userID);
    }
}

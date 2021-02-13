using MotoShop.Data.Helpers;
using MotoShop.Data.Models.User;
using MotoShop.Services.HelperModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MotoShop.Services.Services
{
    public interface IApplicationUserService
    {
        int UserExists(ApplicationUser user);
        Task<ApplicationUser> GetUserByID(string id);
        Task<ApplicationUser> GetUserByEmail(string email);
        Task<ApplicationUser> GetUserByUserName(string username);
        Task<ApplicationUser> RegisterNewUserAsync(ApplicationUser user, string password);
        Task<UpdateResult> UpdateUserDataAsync(string userID, ApplicationUser model);
        Task<bool> IsAdmin(string userID);
        Task<bool> UserExists(string email, string username);
        Task<bool> UpdateEmailAsync(ApplicationUser user, string token, string newEmail);
        Task<bool> SendAccountConfirmationMessageAsync(ApplicationUser user);
        Task<bool> SendPasswordChangingConfirmationMessageAsync(ApplicationUser user, string newPassword);
        Task<bool> ConfirmUserEmailAsync(ApplicationUser user, string token);
        Task<bool> UpdatePasswordAsync(ApplicationUser user,string token, string newPassword);
        Task<bool> AddUserProfileImageAsync(string userID, string path);
        Task<bool> DeleteUser(string userID);
        Task<string> SignInAsync(string data, string password, UserSignInVariant variant);
        Task<TResult> GetUserData<TResult>(string userID, Func<ApplicationUser, TResult> selectExpression);
        Task<IEnumerable<string>> GetUserRolesAsync(string userID);
        string GenerateConfirmationLink(string token, string userID, string newData, UpdateDataType updateDataType);
        string EncodeToken(string token);
        string DecodeToken(string token);


    }
}

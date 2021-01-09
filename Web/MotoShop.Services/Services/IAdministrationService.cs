using Microsoft.AspNetCore.Identity;
using MotoShop.Data.Models.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MotoShop.Services.Services
{
    public interface IAdministrationService
    {
        IEnumerable<ApplicationUser> GetAllUsers();
        IEnumerable<TResult> GetUsersData<TResult>(Func<ApplicationUser, TResult> selectExpression);
        IEnumerable<TResult> GetUsersData<TResult>(string data);
        Task<IdentityResult> CreateAdminAsync(ApplicationUser user);
        Task<IdentityResult> DeleteAdmin(ApplicationUser user);

        Task<bool> AddRoleToUser(ApplicationUser user, IdentityRole role);
        Task<bool> AddRoleToUser(ApplicationUser user, string role);
        Task<bool> IsInRole(ApplicationUser user, string role);
    }
}

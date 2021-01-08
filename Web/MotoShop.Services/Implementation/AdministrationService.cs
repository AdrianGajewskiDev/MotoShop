using Microsoft.AspNetCore.Identity;
using MotoShop.Data.Database_Context;
using MotoShop.Data.Models.User;
using MotoShop.Services.EntityFramework.CompiledQueries;
using MotoShop.Services.HelperModels;
using MotoShop.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotoShop.Services.Implementation
{
    public class AdministrationService : IAdministrationService
    {
        private readonly ApplicationDatabaseContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdministrationService(ApplicationDatabaseContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> AddRoleToUser(ApplicationUser user, IdentityRole role)
        {
            return await AddRoleToUser(user, role.Name);
        }

        public async Task<bool> AddRoleToUser(ApplicationUser user, string role)
        {
            if (!(await _roleManager.RoleExistsAsync(role)))
                return false;

            var result = await _userManager.AddToRoleAsync(user, role);

            return result.Succeeded;
        }

        public async Task<IdentityResult> CreateAdminAsync(ApplicationUser user)
        {
            if(!(await _roleManager.RoleExistsAsync(ApplicationRoles.Administrator)))
                await _roleManager.CreateAsync(new IdentityRole(ApplicationRoles.Administrator));

            return await _userManager.AddToRoleAsync(user, ApplicationRoles.Administrator);
        }

        public async Task<IdentityResult> DeleteAdmin(ApplicationUser user)
        {
            return await _userManager.RemoveFromRoleAsync(user, ApplicationRoles.Administrator);
        }

        public IEnumerable<ApplicationUser> GetAllUsers()
        {
            return AdministrationQueries.GetAllUsers(_dbContext);
        }

        public IEnumerable<TResult> GetUsersData<TResult>(Func<ApplicationUser, TResult> selectExpression)
        {
            return GetAllUsers().Select(selectExpression);
        }
    }
}

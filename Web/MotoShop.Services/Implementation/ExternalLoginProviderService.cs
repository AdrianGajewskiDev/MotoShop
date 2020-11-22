using Microsoft.AspNetCore.Identity;
using MotoShop.Data.Database_Context;
using MotoShop.Data.Models.User;
using MotoShop.Services.Services;
using System.Linq;
using System.Threading.Tasks;

namespace MotoShop.Services.Implementation
{
    public class ExternalLoginProviderService : IExternalLoginProviderService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDatabaseContext _dbContext;

        public ExternalLoginProviderService(UserManager<ApplicationUser> userManager, ApplicationDatabaseContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<IdentityResult> AddLoginAsync(ApplicationUser user, UserLoginInfo info) => await _userManager.AddLoginAsync(user, info);

        public string BuildUsername(ApplicationUser user)
        {
            if (!_dbContext.Users.Any(x => x.UserName == user.Name))
                return user.Name;
            else if (!_dbContext.Users.Any(x => x.UserName == user.LastName))
                return user.LastName;

            return user.Id.Substring(1, 5);
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user) => await _userManager.CreateAsync(user);
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MotoShop.Data.Database_Context;
using MotoShop.Data.Models.User;
using MotoShop.Services.HelperModels;
using MotoShop.Services.Services;
using System.Threading.Tasks;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace MotoShop.Services.Implementation
{
    public class ExternalLoginProviderService : IExternalLoginProviderService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDatabaseContext _dbContext;
        private readonly GoogleAuthOptions _googleAuthOptions;

        public ExternalLoginProviderService(UserManager<ApplicationUser> userManager, 
            ApplicationDatabaseContext dbContext, IOptions<GoogleAuthOptions> options)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _googleAuthOptions = options.Value;
        }

        public Task<bool> Create(ApplicationUser model, ExternalSignInProvider provider, string providerID)
        {
            return Task.FromResult(false);
        }

        public async Task<Payload> ValidateGoogleAccessTokenAsync(string token)
        {
            ValidationSettings settings = new ValidationSettings
            {
                Audience = new string[] { _googleAuthOptions.ClientID }
            };

            return await ValidateAsync(token, settings);
        }
    }
}

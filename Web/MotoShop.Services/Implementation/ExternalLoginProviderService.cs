using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MotoShop.Data.Database_Context;
using MotoShop.Data.Models.User;
using MotoShop.Services.HelperModels;
using MotoShop.Services.Services;
using System;
using System.Threading.Tasks;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace MotoShop.Services.Implementation
{
    public class ExternalLoginProviderService : IExternalLoginProviderService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly GoogleAuthOptions _googleAuthOptions;

        public ExternalLoginProviderService(UserManager<ApplicationUser> userManager, ApplicationDatabaseContext dbContext, 
            IOptions<GoogleAuthOptions> options, IApplicationUserService applicationUserService)
        {
            _userManager = userManager;
            _googleAuthOptions = options.Value;
        }

        public bool CheckIfValidLoginProvider(string provider)
        {
            return Enum.TryParse(typeof(ExternalSignInProvider), provider, true, out var enumProvider);
        }

        public async Task<bool> Create(ApplicationUser model, ExternalSignInProvider provider, string providerID)
        {
            model.IsExternal = true;
            var result = await _userManager.CreateAsync(model);

            if(result.Succeeded)
            {
                var providerStr = provider.ToString();
                UserLoginInfo loginInfo = new UserLoginInfo(providerStr, providerID, providerStr);
                var success = await _userManager.AddLoginAsync(model, loginInfo);

                if (success.Succeeded)
                    return true;
            }

            return false;
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

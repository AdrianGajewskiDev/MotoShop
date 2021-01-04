using Google.Apis.Auth;
using MotoShop.Data.Models.User;
using MotoShop.Services.HelperModels;
using System.Threading.Tasks;

namespace MotoShop.Services.Services
{
    public interface IExternalLoginProviderService
    {
        Task<bool> Create(ApplicationUser model, ExternalSignInProvider provider, string providerID);
        Task<GoogleJsonWebSignature.Payload> ValidateGoogleAccessTokenAsync(string token);
    }
}

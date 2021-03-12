using MotoShop.Data.Models.User;
using MotoShop.Services.HelperModels;
using System.Threading.Tasks;

namespace MotoShop.Services.Services
{
    public interface ITokenProviderService
    {
        bool CheckIfRefreshTokenExists(string userID);
        RefreshToken GetRefreshTokenByUserID(string userID);
        RefreshToken GetRefreshToken(string token);
        Task<RefreshTokenResult> RefreshToken(string token, string userID, ITokenWriter tokenWriter);
        bool TokenExpired(string token);
    }
}

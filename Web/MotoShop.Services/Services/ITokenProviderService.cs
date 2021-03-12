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
        RefreshTokenResult RefreshToken(string token, string userID, ITokenWriter tokenWriter);
        bool RefreshTokenExpired(RefreshToken token);
        bool TemporaryTokenExpired(string token);
        bool IsValidRefreshTokenForUser(RefreshToken refreshToken, string token);
    }
}

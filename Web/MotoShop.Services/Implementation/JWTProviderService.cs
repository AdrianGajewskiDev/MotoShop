using MotoShop.Data.Database_Context;
using MotoShop.Data.Models.User;
using MotoShop.Services.HelperModels;
using MotoShop.Services.Services;
using System.Linq;
using System.Threading.Tasks;

namespace MotoShop.Services.Implementation
{
    public class JWTProviderService : ITokenProviderService
    {
        private readonly ApplicationDatabaseContext _dbContext;

        public JWTProviderService(ApplicationDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool CheckIfRefreshTokenExists(string userID)
        {
            return _dbContext.RefreshTokens.Any(x => x.UserId == userID);
        }

        public RefreshToken GetRefreshToken(string token)
        {
            return _dbContext.RefreshTokens.FirstOrDefault(x => x.Token == token);
        }

        public RefreshToken GetRefreshTokenByUserID(string userID)
        {
            return _dbContext.RefreshTokens.FirstOrDefault(x => x.UserId == userID);
        }

        public Task<RefreshTokenResult> RefreshToken(string token, string userID, ITokenWriter tokenWriter)
        {
            var tk = GetRefreshToken(token);

            var result = new RefreshTokenResult();

            if (tk == null)
            {
                result.Errors.Add("Cannot find a refresh token");
                return Task.FromResult(result);
            }

            if (TokenExpired(tk.Token))
            {
                result.Errors.Add("Refresh token has already expired");
                return Task.FromResult(result);
            }


               
        }

        public bool TokenExpired(string token)
        {

        }
    }
}

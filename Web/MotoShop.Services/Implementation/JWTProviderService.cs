using Microsoft.IdentityModel.Tokens;
using MotoShop.Data.Database_Context;
using MotoShop.Data.Models.User;
using MotoShop.Services.HelperModels;
using MotoShop.Services.Services;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace MotoShop.Services.Implementation
{
    public class JWTProviderService : ITokenProviderService
    {
        private readonly ApplicationDatabaseContext _dbContext;
        private readonly ITokenWriter _tokenWriter;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public JWTProviderService(ApplicationDatabaseContext dbContext, ITokenWriter tokenWriter, TokenValidationParameters tokenValidationParameters)
        {
            _dbContext = dbContext;
            _tokenWriter = tokenWriter;
            _tokenValidationParameters = tokenValidationParameters;
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

        public void AssertValidToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
        }

        public bool IsValidRefreshTokenForUser(RefreshToken refreshToken, string token)
        {
            var userID = _tokenWriter.DecodeToken(token).First(x => x.Type == "UserID").Value;

            return refreshToken.UserId == userID;
        }

        public RefreshTokenResult RefreshToken(string token, string tempToken, string userID)
        {
            var tk = GetRefreshToken(token);

            var result = new RefreshTokenResult();

            if (tk == null)
            {
                result.Errors.Add("Cannot find a refresh token");
                return result;
            }

            if (RefreshTokenExpired(tk))
            {
                result.Errors.Add("Refresh token has already expired");
                return result;
            }

            try
            {
                AssertValidToken(tempToken);
            }
            catch(Exception ex)
            {
                result.Errors.Add(ex.Message);
                return result;
            }

            if(!IsValidRefreshTokenForUser(tk, tempToken))
            {
                result.Errors.Add("Refresh token and temporary token are not valid for specified user");
                return result;
            }

            var newToken = _tokenWriter.GenerateToken(_tokenWriter.AddStandardClaims(userID, ApplicationRoles.NormalUser));

            return new RefreshTokenResult
            {
                RefreshToken = tk.Token,
                Token = newToken
            };
        }

   
        public bool RefreshTokenExpired(RefreshToken token) =>  token.ExpiryDate < DateTime.UtcNow;

        public bool TemporaryTokenExpired(string token)
        {
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

            DateTime expiryDate = jwtToken.ValidTo;

            if (expiryDate == DateTime.MinValue)
                throw new InvalidOperationException("Cannot find the expiration claim in JWT token");

            return expiryDate < DateTime.UtcNow;
        }
    }
}

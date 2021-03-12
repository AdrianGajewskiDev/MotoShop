using MotoShop.Data.Database_Context;
using MotoShop.Data.Models.User;
using MotoShop.Services.Services;
using System;

namespace MotoShop.WebAPI.Token_Providers
{
    public class RefreshTokenGenerator : IRefreshTokenGenerator<RefreshToken>
    {
        private readonly ApplicationDatabaseContext _ctx;

        public RefreshTokenGenerator(ApplicationDatabaseContext ctx)
        {
            _ctx = ctx;
        }
        public RefreshToken Generate(string userID)
        {
            var newRefreshToken = new RefreshToken
            {
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6),
                Token = Guid.NewGuid().ToString(),
                Used = false,
                UserId = userID
            };

            _ctx.RefreshTokens.Add(newRefreshToken);
            _ctx.SaveChanges();

            return newRefreshToken;
        }
    }
}

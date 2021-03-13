using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MotoShop.Data.Database_Context;
using MotoShop.Services.Implementation;
using MotoShop.Services.Services;
using MotoShop.WebAPI.Token_Providers;
using System;
using System.Text;
using Xunit;

namespace MotoShop.Tests.Services
{
    public class TokenProviderServiceTests
    {
        private const string _connectionString = "Server=(localdb)\\mssqllocaldb;Database=MotoShop;Trusted_Connection=True;MultipleActiveResultSets=true";

        [Theory]
        [InlineData("108be340-04a6-4c22-8b90-e3f9ad7a8f39", true)]
        [InlineData("8a7eeeb4-a890-4048-8738-a7b19d3db070", true)]
        [InlineData("d5bcb6f8-181e-45c2-831e-d321042f5cd3", false)]
        public void Should_Check_If_Refresh_Token_Exists_For_Specified_User(string userID, bool expectedResult)
        {
            DbContextOptionsBuilder<ApplicationDatabaseContext> dbContextOptions = new DbContextOptionsBuilder<ApplicationDatabaseContext>().UseSqlServer(_connectionString);
            ApplicationDatabaseContext context = new ApplicationDatabaseContext(dbContextOptions.Options);

            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("./Data/data.json")
                .Build();

            var jwtKey = Encoding.UTF8.GetBytes(configuration["JWT:Key"]);

            ITokenWriter tokenWriter = new JsonWebTokenWriter(configuration);
            ITokenProviderService service = new JWTProviderService(context, tokenWriter, new TokenValidationParameters 
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(jwtKey),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                RequireExpirationTime = true,
                ClockSkew = TimeSpan.Zero,
            });

            var result = service.CheckIfRefreshTokenExists(userID);

            Assert.Equal(result, expectedResult);
        }
    }
}

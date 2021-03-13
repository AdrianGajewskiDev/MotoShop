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

        [Theory]
        [InlineData("8e194e96-2a5e-49be-b2ee-ad4684761bb3", true)]
        [InlineData("dfebd7ec-313f-411d-9ced-2dca5ffebcc3", true)]
        [InlineData("8e194e96-2a5e-49be-b2ee-ad4684761basdasdb3", false)]
        public void Should_Return_Refresh_Token_Based_On_The_Token_Itself(string token, bool expectedResult)
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

            var result = service.GetRefreshToken(token);

            if (expectedResult == true)
                Assert.NotNull(result);

            Assert.Equal(result != null, expectedResult);
        }

        [Theory]
        [InlineData("108be340-04a6-4c22-8b90-e3f9ad7a8f39", true)]
        [InlineData("8a7eeeb4-a890-4048-8738-a7b19d3db070", true)]
        [InlineData("8e194e96-2a5e-49be-b2ee-ad4684761basdasdb3", false)]
        public void Should_Return_Refresh_Token_Based_On_The_UserID(string userID, bool expectedResult)
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

            var result = service.GetRefreshTokenByUserID(userID);

            if (expectedResult == true)
                Assert.NotNull(result);

            Assert.Equal(result != null, expectedResult);
        }

        [Theory]
        [InlineData("108be340-04a6-4c22-8b90-e3f9ad7a8f39", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySUQiOiIxMDhiZTM0MC0wNGE2LTRjMjItOGI5MC1lM2Y5YWQ3YThmMzkiLCJyb2xlIjoiTm9ybWFsVXNlciIsIm5iZiI6MTYxNTY2MDY2MSwiZXhwIjoxNjE1NjYwNzA2LCJpYXQiOjE2MTU2NjA2NjF9.cXGlRGjYQebclTkqE2IdP_WfSK4F23_MbKvQtTYapzQ")]
        public void Should_Compare_And_Return_If_The_RefreshToken_And_JWTToken_Points_To_The_Same_User(string userID, string token)
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

            var refreshToken = service.GetRefreshTokenByUserID(userID);
            var result = service.IsValidRefreshTokenForUser(refreshToken, token);

            Assert.NotNull(refreshToken);
            Assert.True(result);
        }

        [Fact]
        public void Should_Return_If_The_Refresh_Token_Has_Expired()
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

            var refToken = service.GetRefreshTokenByUserID("108be340-04a6-4c22-8b90-e3f9ad7a8f39");
            var result = service.RefreshTokenExpired(refToken);

            Assert.False(result);
        }

        [Fact]
        public void Should_Return_If_The_JWT_Token_Has_Expired()
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

            var result = service.TemporaryTokenExpired("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySUQiOiIxMDhiZTM0MC0wNGE2LTRjMjItOGI5MC1lM2Y5YWQ3YThmMzkiLCJyb2xlIjoiTm9ybWFsVXNlciIsIm5iZiI6MTYxNTY2MDY2MSwiZXhwIjoxNjE1NjYwNzA2LCJpYXQiOjE2MTU2NjA2NjF9.cXGlRGjYQebclTkqE2IdP_WfSK4F23_MbKvQtTYapzQ");

            Assert.True(result);
        }
    }
}

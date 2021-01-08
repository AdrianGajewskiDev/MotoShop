using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MotoShop.Data.Database_Context;
using MotoShop.Services.HelperModels;
using MotoShop.Services.Implementation;
using MotoShop.Services.Services;
using System.Threading.Tasks;
using Xunit;

namespace MotoShop.Tests.Services
{
    public class ExternalLoginProviderServiceTests
    {
        private const string _connectionString = "Server=(localdb)\\mssqllocaldb;Database=MotoShop;Trusted_Connection=True;MultipleActiveResultSets=true";

        private const string validToken = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjI1MmZjYjk3ZGY1YjZiNGY2ZDFhODg1ZjFlNjNkYzRhOWNkMjMwYzUiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJhY2NvdW50cy5nb29nbGUuY29tIiwiYXpwIjoiNzgyMjAyOTI2MTgtZWFuODl1cmhjbnQxNXUzODNxOThnZ2tlOXNhc3VvZnYuYXBwcy5nb29nbGV1c2VyY29udGVudC5jb20iLCJhdWQiOiI3ODIyMDI5MjYxOC1lYW44OXVyaGNudDE1dTM4M3E5OGdna2U5c2FzdW9mdi5hcHBzLmdvb2dsZXVzZXJjb250ZW50LmNvbSIsInN1YiI6IjExNzAxMjU5MzcyNTU1NzMwNDIyNCIsImVtYWlsIjoiYWRyaWFuLmdhamV3c2tpMDAxQGdtYWlsLmNvbSIsImVtYWlsX3ZlcmlmaWVkIjp0cnVlLCJhdF9oYXNoIjoiWTFMVnliR3k0NTJFMi03UEYyOVlTQSIsIm5hbWUiOiJBZHJpYW4gRGV2IiwicGljdHVyZSI6Imh0dHBzOi8vbGg2Lmdvb2dsZXVzZXJjb250ZW50LmNvbS8tZkhrU0xUeDdKOUUvQUFBQUFBQUFBQUkvQUFBQUFBQUFBQUEvQU1adXVja2pnSmlMR2dvSWZ6dWhNWlhsTGs5aEh2Zi1Udy9zOTYtYy9waG90by5qcGciLCJnaXZlbl9uYW1lIjoiQWRyaWFuIiwiZmFtaWx5X25hbWUiOiJEZXYiLCJsb2NhbGUiOiJwbCIsImlhdCI6MTYxMDExMTY3MCwiZXhwIjoxNjEwMTE1MjcwLCJqdGkiOiI4MzIzYzZkZThmNDgzNTA4ZTZhM2Q5NGY3ODFhNzMzNTRhMzZiZjMxIn0.X-XRufi-cjuIb0DekI1IgEIuPJWh0rnNcu88GJGwgL80qN1aM5OfcUsDB-iQNACGq-Gg7aQmkyNuxZzmfeNJcHPbJxvICILys_4oAHnKPu6rrXrjgtBFpVZ0TFbS2qe7HTy380xDhCKxRxVnVbH3-4gIWuvW--L1aSS2fzSbyVcrFXoeBHcw2xxSTkahnzBRCLoygkGAJsjtUSYUU5U0wo6Uz_6tKk0wA0AO3m-kcPVD5VtGbe1KSO4hhvcsPDxvDjr7cGoOBIwq9uUAHbyJzslf1TCNdVVkata05YYN6AB8WCbBQ50zsXA2ZpOdozLfa4kjvNd3pDzMMfBWOD_nFg";
        private const string invalidToken = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjI1MmZjYjk3ZGY1YjZiNGY2ZDFhODg1ZjFlNjNkYzRhOW";

        [Theory]
        [InlineData(true, "Google")]
        [InlineData(true, "Facebook")]
        [InlineData(false, "Microsoft")]
        [InlineData(true, "Github")]
        public void External_Login_Provider_Service_Should_Check_If_LP_is_valid(bool expected, string provider)
        {
            DbContextOptionsBuilder<ApplicationDatabaseContext> dbContextOptions = new DbContextOptionsBuilder<ApplicationDatabaseContext>().UseSqlServer(_connectionString);
            ApplicationDatabaseContext dbContext = new ApplicationDatabaseContext(dbContextOptions.Options);
            var options = Options.Create(new GoogleAuthOptions
            {
                ClientID = "78220292618-ean89urhcnt15u383q98ggke9sasuofv.apps.googleusercontent.com",
                ClientSecret = "mx3sA-pkKFJmPjLv7Zfo1lCY"
            });

            IApplicationUserService userService = new ApplicationUserService(dbContext, null, null, null, null);
            IExternalLoginProviderService service = new ExternalLoginProviderService(null, dbContext, options, userService);

            var result = service.CheckIfValidLoginProvider(provider);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(validToken, true)]
        [InlineData(invalidToken, false)]
        public async Task External_Login_Provider_Service_Should_Validate_Access_Token(string token, bool expected)
        {
            DbContextOptionsBuilder<ApplicationDatabaseContext> dbContextOptions = new DbContextOptionsBuilder<ApplicationDatabaseContext>().UseSqlServer(_connectionString);
            ApplicationDatabaseContext dbContext = new ApplicationDatabaseContext(dbContextOptions.Options);
            var options = Options.Create(new GoogleAuthOptions
            {
                ClientID = "78220292618-ean89urhcnt15u383q98ggke9sasuofv.apps.googleusercontent.com",
                ClientSecret = "mx3sA-pkKFJmPjLv7Zfo1lCY"
            });

            IApplicationUserService userService = new ApplicationUserService(dbContext, null, null, null, null);
            IExternalLoginProviderService service = new ExternalLoginProviderService(null, dbContext, options, userService);

            if (expected)
            {
                var result = await service.ValidateGoogleAccessTokenAsync(token);
                Assert.NotNull(result);
            }
            else
            {
                await Assert.ThrowsAsync<InvalidJwtException>( async () => 
                {
                    await service.ValidateGoogleAccessTokenAsync(token);
                });
            }
        }
    }
}


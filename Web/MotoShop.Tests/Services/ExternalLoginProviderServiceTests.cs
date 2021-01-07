using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using MotoShop.Data.Database_Context;
using MotoShop.Services.HelperModels;
using MotoShop.Services.Implementation;
using MotoShop.Services.Services;
using Newtonsoft.Json;
using System.IO;
using Xunit;

namespace MotoShop.Tests.Services
{
    public class ExternalLoginProviderServiceTests
    {
        private const string _connectionString = "Server=(localdb)\\mssqllocaldb;Database=MotoShop;Trusted_Connection=True;MultipleActiveResultSets=true";

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

    }
}


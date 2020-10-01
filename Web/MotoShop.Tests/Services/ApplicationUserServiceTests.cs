using Microsoft.EntityFrameworkCore;
using MotoShop.Data.Database_Context;
using MotoShop.Data.Models.User;
using MotoShop.Services.Implementation;
using Xunit;

namespace MotoShop.Tests.Services
{
    public class ApplicationUserServiceTests
    {
        private const string _connectionString = "Server=(localdb)\\mssqllocaldb;Database=MotoShop;Trusted_Connection=True;MultipleActiveResultSets=true";

        [Theory]
        [InlineData("adrian.gajewski001@gmail.com", 1)]
        [InlineData("AdrianDev", 2)]
        [InlineData("Adrian", 0)]
        public void Application_User_Service_Should_Not_Register_Two_Users_With_The_Same_Email(string data, int expected)
        {
            DbContextOptionsBuilder<ApplicationDatabaseContext> dbContextOptions = new DbContextOptionsBuilder<ApplicationDatabaseContext>().UseSqlServer(_connectionString);
            ApplicationDatabaseContext dbContext = new ApplicationDatabaseContext(dbContextOptions.Options);
            ApplicationUserService service = new ApplicationUserService(dbContext, null);

            ApplicationUser user = new ApplicationUser
            {
                Email = data,
                UserName = data
            };

            var result = service.UserExists(user);

            Assert.Equal(result, expected);

        }

    }
}

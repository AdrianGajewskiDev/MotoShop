using Microsoft.EntityFrameworkCore;
using MotoShop.Data.Database_Context;
using MotoShop.Data.Models.User;
using MotoShop.Services.HelperModels;
using MotoShop.Services.Implementation;
using System.Threading.Tasks;
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
        public void Application_User_Service_Should_Not_Register_Two_Users_With_The_Same_Email_Or_Username(string data, int expected)
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


        public async Task Application_User_Service_Should_Update_User_Profile_Data(UpdateDataType updateDataType, string data)
        {
            DbContextOptionsBuilder<ApplicationDatabaseContext> dbContextOptions = new DbContextOptionsBuilder<ApplicationDatabaseContext>().UseSqlServer(_connectionString);
            ApplicationDatabaseContext dbContext = new ApplicationDatabaseContext(dbContextOptions.Options);
            ApplicationUserService service = new ApplicationUserService(dbContext, null);
            string userID = "e0d6c11b-5f16-4194-99be-cf698eda3820";

            ApplicationUser userUpdate = new ApplicationUser();

            switch(updateDataType)
            {
                case UpdateDataType.Username: userUpdate.UserName = data; break;
                case UpdateDataType.Email: userUpdate.Email = data; break;
                case UpdateDataType.Name: userUpdate.Name = data; break;
                case UpdateDataType.Lastname: userUpdate.LastName = data; break;
                case UpdateDataType.PhoneNumber: userUpdate.PhoneNumber= data; break;

            };

             await service.UpdateUserDataAsync(userID, userUpdate);
            var user = await service.GetUserByID(userID);

            string actual = DataToCheck(updateDataType, user);
            Assert.Equal(data, actual);

        }

        private string DataToCheck(UpdateDataType updateDataType, ApplicationUser user)
        {
            switch (updateDataType)
            {
                case UpdateDataType.Username:
                    break;
                case UpdateDataType.Email:
                    break;
                case UpdateDataType.PhoneNumber:
                    break;
                case UpdateDataType.Name:
                    break;
                case UpdateDataType.Lastname:
                    break;
                case UpdateDataType.Password:
                    break;
                default:
                    break;
            }
        }
    }
}

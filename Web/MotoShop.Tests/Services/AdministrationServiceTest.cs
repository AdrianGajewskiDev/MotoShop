using Microsoft.EntityFrameworkCore;
using MotoShop.Data.Database_Context;
using MotoShop.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MotoShop.Tests.Services
{
    public class AdministrationServiceTest
    {
        private const string connectionString = "Server=(localdb)\\mssqllocaldb;Database=MotoShop;Trusted_Connection=True;MultipleActiveResultSets=true";

        [Fact]
        public void Administration_Service_Should_Return_All_Users()
        {
            DbContextOptionsBuilder<ApplicationDatabaseContext> dbContextOptions = new DbContextOptionsBuilder<ApplicationDatabaseContext>().UseSqlServer(connectionString);
            ApplicationDatabaseContext context = new ApplicationDatabaseContext(dbContextOptions.Options);


            var service = new AdministrationService(context, null, null);

            var result = service.GetAllUsers();

            Assert.Equal(3, result.Count());
        }

        [Theory]
        [InlineData(DataType.String)]
        [InlineData(DataType.Bool)]
        [InlineData(DataType.Int)]
        public void Administration_Service_Should_Return_All_User_Data(DataType dataType)
        {
            DbContextOptionsBuilder<ApplicationDatabaseContext> dbContextOptions = new DbContextOptionsBuilder<ApplicationDatabaseContext>().UseSqlServer(connectionString);
            ApplicationDatabaseContext context = new ApplicationDatabaseContext(dbContextOptions.Options);


            var service = new AdministrationService(context, null, null);

            switch (dataType)
            {
                case DataType.String:
                    {
                        var result = service.GetUsersData<string>(x => x.UserName);

                        Assert.NotNull(result);
                        Assert.IsAssignableFrom<IEnumerable<string>>(result);
                        foreach (var res in result)
                        {
                            Assert.IsType<string>(res);
                        }
                    }
                    break;
                case DataType.Bool:
                    {
                        var result = service.GetUsersData<bool>(x => x.EmailConfirmed);

                        Assert.NotNull(result);
                        Assert.IsAssignableFrom<IEnumerable<bool>>(result);
                        foreach (var res in result)
                        {
                            Assert.IsType<bool>(res);
                        }
                    }
                    break;
                case DataType.Int:
                    {
                        var result = service.GetUsersData<int>(x => x.AccessFailedCount);

                        Assert.NotNull(result);
                        Assert.IsAssignableFrom<IEnumerable<int>>(result);
                        foreach (var res in result)
                        {
                            Assert.IsType<int>(res);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        public enum DataType
        {
            String,
            Bool,
            Int
        }

    }
}

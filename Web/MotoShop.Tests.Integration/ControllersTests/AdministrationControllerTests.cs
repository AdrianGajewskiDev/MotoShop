using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MotoShop.Services.HelperModels;
using MotoShop.Services.Services;
using MotoShop.Web;
using MotoShop.WebAPI.FileProviders;
using MotoShop.WebAPI.Models.Request;
using MotoShop.WebAPI.Models.Requests.Administration;
using MotoShop.WebAPI.Models.Response;
using MotoShop.WebAPI.Models.Response.Administration;
using MotoShop.WebAPI.Token_Providers;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace MotoShop.Tests.Integration.ControllersTests
{
    public class AdministrationControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public AdministrationControllerTests(WebApplicationFactory<Startup> webApplicationFactory)
        {
            _factory = webApplicationFactory;
        }

        [Fact]
        public async Task ShouldReturnAllUsers()
        {
            var client = _factory.CreateClient();
            var credentials = new UserSignInRequestModel 
            {
                Data = "Admin",
                Password = "Lakiernik2345"
            };

            var token = await client.PostAsync("api/identity/signIn", JsonContent.Create<UserSignInRequestModel>(credentials));


            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JsonConvert.DeserializeObject<AuthenticatedResponse>(await token.Content.ReadAsStringAsync()).Token);

            var response = await client.GetAsync("api/Administration");

            Assert.NotNull(response);
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("AdrianDev", "Lakiernik2345")]
        [InlineData("JohnDoe1234", "Lakiernik2345")]

        public async Task ShouldReturnForbiddenWhenUserIsNotaAdmin(string data, string password)
        {
            var client = _factory.CreateClient();
            var credentials = new UserSignInRequestModel
            {
                Data = data,
                Password = password
            };

            var token = await client.PostAsync("api/identity/signIn", JsonContent.Create<UserSignInRequestModel>(credentials));

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JsonConvert.DeserializeObject<AuthenticatedResponse>(await token.Content.ReadAsStringAsync()).Token);
            
            var response = await client.GetAsync("api/Administration");

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task ShouldSuccessfullyAddRoleToUser()
        {
            var client = _factory.CreateClient();
            var credentials = new UserSignInRequestModel
            {
                Data = "Admin",
                Password = "Lakiernik2345"
            };

            var request = await client.PostAsync("api/identity/signIn", JsonContent.Create<UserSignInRequestModel>(credentials));

            var token = JsonConvert.DeserializeObject<AuthenticatedResponse>(await request.Content.ReadAsStringAsync());

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token.Token);

            var reponse = await client.PostAsJsonAsync<AddRoleRequestModel>("api/administration/addRole", new AddRoleRequestModel
            {
                Role = ApplicationRoles.NormalUser,
                UserID = "108be340-04a6-4c22-8b90-e3f9ad7a8f39"
            });

            Assert.Equal(HttpStatusCode.OK, reponse.StatusCode);
        }
    }
}

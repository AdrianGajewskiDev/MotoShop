using Microsoft.AspNetCore.Mvc.Testing;
using MotoShop.Web;
using MotoShop.WebAPI.Models.Request;
using MotoShop.WebAPI.Models.Response;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace MotoShop.Tests.Integration.ControllersTests
{
    public class IdentityControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public IdentityControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ShouldSignInSuccessfully()
        {
            var client = _factory.CreateClient();

            UserSignInRequestModel signInCredentials = new()
            {
                Data = "AdrianDev",
                Password = "Lakiernik2345"
            };

            var response = await client.PostAsync("api/identity/signIn", JsonContent.Create<UserSignInRequestModel>(signInCredentials));

            var content = await response.Content.ReadAsStringAsync();

            var convertedResponse = JsonConvert.DeserializeObject<AuthenticatedResponse>(content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.False(string.IsNullOrEmpty(convertedResponse.Token));
            Assert.False(string.IsNullOrEmpty(convertedResponse.RefreshToken));
        }

        [Fact]
        public async Task ShouldReturn404NotFoundWithInvalidSignInCredentials()
        {
            var client = _factory.CreateClient();

            UserSignInRequestModel signInCredentials = new()
            {
                Data = "Adrian",
                Password = "Lakiern"
            };

            var response = await client.PostAsync("api/identity/signIn", JsonContent.Create<UserSignInRequestModel>(signInCredentials));

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task ShouldntAllowToRegisterUserWithWithTakenEmailOrUsername()
        {
            var client = _factory.CreateClient();

            var credentials = new UserRegisterRequestModel 
            { 
                Email = "adrian.gajewski001@gmail.com",
                LastName = "Gajewski",
                Name = "Adrian",
                Password = "Lakiernik2345",
                UserName = "AdrianDev"
            };

            var response = await client.PostAsync("api/identity/register", JsonContent.Create<UserRegisterRequestModel>(credentials));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}

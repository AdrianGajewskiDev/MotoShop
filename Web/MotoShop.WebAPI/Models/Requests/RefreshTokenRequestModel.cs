namespace MotoShop.WebAPI.Models.Requests
{
    public class RefreshTokenRequestModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}

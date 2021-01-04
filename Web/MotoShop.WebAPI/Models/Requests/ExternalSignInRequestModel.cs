namespace MotoShop.WebAPI.Models.Requests
{
    public class ExternalSignInRequestModel 
    {
        public string AccessToken { get; set; }
        public string Provider { get; set; }
    }
}

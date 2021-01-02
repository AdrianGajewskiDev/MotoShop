namespace MotoShop.WebAPI.Models.Requests
{
    public class ExternalSignInRequestModel 
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public string ProviderDisplayName { get; set; }
        public string UserId { get; set; }
    }
}

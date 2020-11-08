namespace MotoShop.WebAPI.Models.Requests
{
    public class UpdateUserDataRequestModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }
}

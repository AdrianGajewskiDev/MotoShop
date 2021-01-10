using System.Collections.Generic;

namespace MotoShop.WebAPI.Models.Response.UserAccount
{
    public class UserAccountDetailsResponseModel
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool IsExternal { get; set; }
        public string ImageUrl { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}

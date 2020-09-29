
using Microsoft.AspNetCore.Identity;

namespace MotoShop.Data.Models.User
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string LastName { get; set; }
    }
}

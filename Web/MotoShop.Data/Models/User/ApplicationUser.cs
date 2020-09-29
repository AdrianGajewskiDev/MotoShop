
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MotoShop.Data.Models.User
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}

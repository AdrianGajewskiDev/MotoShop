using Microsoft.AspNetCore.Identity;
using MotoShop.Data.Models.Messages;
using MotoShop.Data.Models.Store;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MotoShop.Data.Models.User
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        public string ImageUrl { get; set; }
        public bool IsExternal { get; set; }

        public virtual Watchlist Watchlist { get; set; }

        public virtual ICollection<Conversation> Conversations { get; set; }

    }
}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MotoShop.Data.Models.Store;
using MotoShop.Data.Models.User;

namespace MotoShop.Data.Database_Context
{
    public class ApplicationDatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDatabaseContext(DbContextOptions<ApplicationDatabaseContext> options) : base(options) { }

        public DbSet<Advertisement> Advertisements { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Motocycle> Motocycles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
        }
    }
}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MotoShop.Data.Models.Messages;
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
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Image> Images { get; set; } 
        public DbSet<Watchlist> Watchlists { get; set; }
        public DbSet<WatchlistItem> WatchlistItems { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Conversation> Conversations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Conversation>()
                .HasOne(x => x.Sender)
                .WithMany(x => x.Conversations)
                .HasForeignKey(x => x.SenderID);

            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}

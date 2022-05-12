using API.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class StoreContext : IdentityDbContext<AppUser>
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }

       public DbSet<FriendshipRequest> FriendshipRequests { get; set; }
       public DbSet<Friendship> Friendships { get; set; }

         protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<FriendshipRequest>()
                .HasKey(k => new { k.SourceUserId, k.DestinationUserId});

            builder.Entity<FriendshipRequest>()
                .HasOne(s => s.SourceUser)
                .WithMany(d => d.FriendshipRequestsSended)
                .HasForeignKey(s => s.SourceUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<FriendshipRequest>()
                .HasOne(s => s.DestinationUser)
                .WithMany(d => d.FriendshipRequestsReceived)
                .HasForeignKey(s => s.DestinationUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Friendship>()
                .HasKey(k => new {k.CurrentUserId, k.FriendUserId});
        }
    }
}
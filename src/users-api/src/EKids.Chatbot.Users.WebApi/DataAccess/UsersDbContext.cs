using EKids.Chatbot.Users.WebApi.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EKids.Chatbot.Users.DataAccess;
public sealed class UsersDbContext(DbContextOptions<UsersDbContext> options) : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<Child> Children { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // TODO: set User
        builder.Entity<Child>(e =>
        {
            e.ToTable("Children");
            e.HasKey(x => x.Id);
            e.HasOne(x => x.ParentUser)
                .WithMany()
                .HasForeignKey(x => x.ParentUserId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
            e.HasOne(x => x.ChildUser)
                .WithOne()
                .HasForeignKey<Child>(x => x.ChildUserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });
    }
}

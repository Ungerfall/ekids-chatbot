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

        builder.Entity<Child>(e =>
        {
            e.ToTable("Children");
            e.HasKey(x => x.Id);
            e.HasOne(x => x.ParentUser)
                .WithMany()
                .HasForeignKey(x => x.ParentUserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
            e.HasOne(x => x.ChildUser)
                .WithOne()
                .HasForeignKey<Child>(x => x.ChildUserId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
        });
    }
}

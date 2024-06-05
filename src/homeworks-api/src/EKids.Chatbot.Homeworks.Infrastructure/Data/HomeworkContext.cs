using EKids.Chatbot.Homeworks.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EKids.Chatbot.Homeworks.Infrastructure.Data;

public class HomeworkContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Homework> Homeworks { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Homework>(ConfigureHomework);
    }

    private void ConfigureHomework(EntityTypeBuilder<Homework> builder)
    {
        builder.ToTable("Homework");

        builder.HasKey(ci => ci.Id);

        builder.Property(cb => cb.CourseId)
            .IsRequired();

        builder.Property(cb => cb.Title)
            .IsRequired();

        builder.Property(cb => cb.StartDate)
            .IsRequired();

        builder.Property(cb => cb.Description);
        builder.Property(cb => cb.Url);
        builder.Property(cb => cb.EndDate);
    }
}

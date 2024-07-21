using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace EKids.Chatbot.Homeworks.Infrastructure.Data;
public class DesignTimeHomeworkContextFactory : IDesignTimeDbContextFactory<HomeworkContext>
{
    public HomeworkContext CreateDbContext(string[] args)
    {
        string? connectionString = null;
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "--connection-string")
            {
                connectionString = args[i + 1];
            }
        }

        if (connectionString is null)
        {
            throw new ArgumentException("Connection string (--connection-string) arg is missing");
        }

        var optionsBuilder = new DbContextOptionsBuilder<HomeworkContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new HomeworkContext(optionsBuilder.Options);
    }
}

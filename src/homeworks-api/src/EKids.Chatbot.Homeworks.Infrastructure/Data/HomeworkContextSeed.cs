using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EKids.Chatbot.Homeworks.Infrastructure.Data;

public class HomeworkContextSeed
{
    public static async Task SeedAsync(HomeworkContext homeworkContext, ILoggerFactory loggerFactory, int? retry = 0)
    {
        int retryForAvailability = retry ?? 0;

        try
        {
            // TODO: Only run this if using a real database
            homeworkContext.Database.Migrate();
            homeworkContext.Database.EnsureCreated();
        }
        catch (Exception exception)
        {
            if (retryForAvailability < 10)
            {
                retryForAvailability++;
                var log = loggerFactory.CreateLogger<HomeworkContextSeed>();
                log.LogError("Exception {Message}", exception.Message);
                await SeedAsync(homeworkContext, loggerFactory, retryForAvailability);
            }

            throw;
        }
    }
}

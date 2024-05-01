using EKids.Chatbot.Tasks.BusinessLayer;
using EKids.Chatbot.Tasks.DataAccessLayer;
using EKids.Chatbot.Tasks.PresentationLayer;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace EKids.Chatbot.Tasks;

internal static class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateSlimBuilder(args);

        builder.Services.ConfigureHttpJsonOptions(options
            => options.SerializerOptions.TypeInfoResolverChain.Insert(0, TasksJsonSerializerContext.Default));
        builder.Services.AddOptions<CosmosDbOptions>()
            .Bind(builder.Configuration.GetSection(CosmosDbOptions.ConfigSection));
        builder.Services.AddSingleton(sp =>
        {
            var opt = sp.GetRequiredService<IOptions<CosmosDbOptions>>().Value;
            return new CosmosClient(
                opt.CosmosDbConnectionString,
                clientOptions: new CosmosClientOptions
                {
                    MaxRetryAttemptsOnRateLimitedRequests = 3,
                    Serializer = new CosmosSystemTextJsonSerializer(),
                });
        });
        builder.Services.AddScoped<ILearningTaskRepository, CosmosDbLearningTaskRepository>();
        builder.Services.AddScoped<ILearningTaskService, LearningTaskService>();

        var app = builder.Build();
        app.MapLearningTasks();

        app.Run();
    }
}

using EKids.Chatbot.Tasks.DataAccessLayer;
using EKids.Chatbot.Tasks.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Testcontainers.CosmosDb;

namespace EKids.Chatbot.Tasks.WebApi.Tests;
public class TasksWebApplicationFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly CosmosDbContainer _container;

    public HttpClient? Client { get; private set; }

    public string CourseId { get; } = Guid.NewGuid().ToString();

    public string TaskId { get; } = Guid.NewGuid().ToString();

    public TasksWebApplicationFixture()
    {
        _container = new CosmosDbBuilder()
            .WithImage(CosmosDbBuilder.CosmosDbImage)
            .WithExposedPort(8081)
            .WithExposedPort(10251)
            .WithExposedPort(10252)
            .WithExposedPort(10253)
            .WithExposedPort(10254)
            .WithPortBinding(8081, true)
            .WithPortBinding(10251, true)
            .WithPortBinding(10252, true)
            .WithPortBinding(10253, true)
            .WithPortBinding(10254, true)
            .WithEnvironment("AZURE_COSMOS_EMULATOR_IP_ADDRESS_OVERRIDE", "127.0.0.1")
            .WithEnvironment("AZURE_COSMOS_EMULATOR_ENABLE_DATA_PERSISTENCE", "false")
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        Client = WithWebHostBuilder(c =>
        {
            c.ConfigureServices(sc =>
            {
                sc.PostConfigure<CosmosDbOptions>(action =>
                {
                    action.CosmosDbConnectionString = ConnectionString().Value;
                });
                sc.AddSingleton(_ => CosmosClient().Value);
                sc.AddLogging(cfg => cfg.AddDebug().AddConsole());
            });
        })
        .CreateClient();
        await SeedData();
    }

    private Lazy<CosmosClient> CosmosClient()
    {
        var cosmosClientOptions = new CosmosClientOptions
        {
            ConnectionMode = ConnectionMode.Gateway,
            Serializer = new CosmosSystemTextJsonSerializer(),
            ServerCertificateCustomValidationCallback = (_, __, ___) => true,
            HttpClientFactory = () => _container.HttpClient,
        };

        return new Lazy<CosmosClient>(new CosmosClient(ConnectionString().Value, cosmosClientOptions));
    }

    private Lazy<string> ConnectionString()
    {
        return new Lazy<string>(_container.GetConnectionString() + ";DisableServerCertificateValidation=True;");
    }

    private async Task SeedData()
    {
        var cosmosClient = CosmosClient().Value;
        var cosmosOptions = Services.GetRequiredService<IOptions<CosmosDbOptions>>().Value;
        var db = (await cosmosClient
            .CreateDatabaseIfNotExistsAsync(cosmosOptions.DatabaseId, ThroughputProperties.CreateManualThroughput(400)))
            .Database;
        var container = (await db
            .CreateContainerIfNotExistsAsync(new ContainerProperties(cosmosOptions.TasksContainer, "/courseId")))
            .Container;

        // tasks
        var learningTask = new LearningTask
        {
            CourseId = CourseId,
            Description = "testing",
            Id = TaskId,
            Title = "testing",
            Year = 2024,
        };
        await container.UpsertItemAsync(learningTask, new PartitionKey(CourseId));
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _container.DisposeAsync();
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        Client?.Dispose();
    }
}

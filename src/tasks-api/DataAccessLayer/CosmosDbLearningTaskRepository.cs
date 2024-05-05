using EKids.Chatbot.Tasks.DataAccessLayer.Entities;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace EKids.Chatbot.Tasks.DataAccessLayer;
public class CosmosDbLearningTaskRepository(
    CosmosClient cosmosClient,
    IOptions<CosmosDbOptions> cosmosDbOptions,
    ILogger<CosmosDbLearningTaskRepository> logger)
    : ILearningTaskRepository
{
    private readonly CosmosDbOptions _cosmosDbOptions = cosmosDbOptions.Value;

    public async Task DeleteById(Guid courseId, Guid taskId, CancellationToken cancellation)
    {
        var container = cosmosClient.GetContainer(_cosmosDbOptions.DatabaseId, _cosmosDbOptions.TasksContainer);
        await container.DeleteItemAsync<LearningTask>(
            taskId.ToString(),
            new PartitionKey(courseId.ToString()),
            new ItemRequestOptions { EnableContentResponseOnWrite = false },
            cancellation);
    }

    public async IAsyncEnumerable<LearningTask> FindAll(Guid? courseId, [EnumeratorCancellation] CancellationToken cancellation)
    {
        var container = cosmosClient.GetContainer(_cosmosDbOptions.DatabaseId, _cosmosDbOptions.TasksContainer);
        var query = courseId is null
            ? new QueryDefinition("SELECT * FROM c")
            : new QueryDefinition("SELECT * FROM c WHERE c.courseId = @courseId")
                .WithParameter("@courseId", courseId.Value.ToString());
        using var it = container.GetItemQueryIterator<LearningTask>(
            query,
            requestOptions: new QueryRequestOptions
            {
                MaxConcurrency = 1,
                PartitionKey = courseId is null ? null : new PartitionKey(courseId.Value.ToString()),
            });
        while (it.HasMoreResults)
        {
            FeedResponse<LearningTask> response = await it.ReadNextAsync(cancellation);
            foreach (LearningTask item in response)
            {
                cancellation.ThrowIfCancellationRequested();
                yield return item;
            }

            if (response.Diagnostics != null)
            {
                logger.LogWarning("Diagnostics: {diagnostics}", response.Diagnostics.ToString());
            }
        }
    }

    public async Task<LearningTask?> Find(Guid courseId, Guid taskId, CancellationToken cancellation)
    {
        var container = cosmosClient.GetContainer(_cosmosDbOptions.DatabaseId, _cosmosDbOptions.TasksContainer);
        try
        {
            return await container.ReadItemAsync<LearningTask>(
                taskId.ToString(),
                new PartitionKey(courseId.ToString()),
                cancellationToken: cancellation);
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            logger.LogWarning(ex, "Task has not been found. Returning null");
            return null;
        }
    }

    public async Task Save(IEnumerable<LearningTask> tasks, CancellationToken cancellation)
    {
        var container = cosmosClient.GetContainer(_cosmosDbOptions.DatabaseId, _cosmosDbOptions.TasksContainer);
        foreach (var task in tasks)
        {
            await container.UpsertItemAsync(
                task,
                new PartitionKey(task.CourseId),
                new ItemRequestOptions { EnableContentResponseOnWrite = true },
                cancellation);
        }
    }
}

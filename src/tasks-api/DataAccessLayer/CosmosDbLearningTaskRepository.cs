using System.Runtime.CompilerServices;

using EKids.Chatbot.Tasks.DataAccessLayer.Entities;

using Microsoft.Azure.Cosmos;

namespace EKids.Chatbot.Tasks.DataAccessLayer;
internal class CosmosDbLearningTaskRepository(CosmosClient cosmosClient, CosmosDbOptions cosmosDbOptions)
    : ILearningTaskRepository
{
    public async Task DeleteById(Guid courseId, Guid taskId, CancellationToken cancellation)
    {
        var container = cosmosClient.GetContainer(cosmosDbOptions.DatabaseId, cosmosDbOptions.TasksContainer);
        await container.DeleteItemAsync<LearningTask>(
            taskId.ToString(),
            new PartitionKey(courseId.ToString()),
            new ItemRequestOptions { EnableContentResponseOnWrite = false },
            cancellation);
    }

    public async IAsyncEnumerable<LearningTask> FindAll([EnumeratorCancellation] CancellationToken cancellation)
    {
        var container = cosmosClient.GetContainer(cosmosDbOptions.DatabaseId, cosmosDbOptions.TasksContainer);
        var query = new QueryDefinition("SELECT * FROM c");
        using var it = container.GetItemQueryIterator<LearningTask>(
            query,
            requestOptions: new QueryRequestOptions
            {
                MaxConcurrency = 1,
                PartitionKey = PartitionKey.None,
            });
        while (it.HasMoreResults)
        {
            FeedResponse<LearningTask> response = await it.ReadNextAsync(cancellation);
            foreach (LearningTask item in response)
            {
                cancellation.ThrowIfCancellationRequested();
                yield return item;
            }
        }
    }

    public async Task<LearningTask> Find(Guid courseId, Guid taskId, CancellationToken cancellation)
    {
        var container = cosmosClient.GetContainer(cosmosDbOptions.DatabaseId, cosmosDbOptions.TasksContainer);
        return await container.ReadItemAsync<LearningTask>(
            taskId.ToString(),
            new PartitionKey(courseId.ToString()),
            cancellationToken: cancellation);
    }

    public async IAsyncEnumerable<LearningTask> Save(IEnumerable<LearningTask> tasks, [EnumeratorCancellation] CancellationToken cancellation)
    {
        var container = cosmosClient.GetContainer(cosmosDbOptions.DatabaseId, cosmosDbOptions.TasksContainer);
        foreach (var task in tasks)
        {
            yield return await container.UpsertItemAsync(
                task,
                new PartitionKey(task.CourseId),
                new ItemRequestOptions { EnableContentResponseOnWrite = true },
                cancellation);
        }
    }
}

using System.Runtime.CompilerServices;

using Microsoft.Azure.Cosmos;

namespace EKids.Chatbot.Tasks.DataAccessLayer;
internal class CosmosDbLearningTaskRepository(CosmosClient cosmosClient, CosmosDbOptions cosmosDbOptions)
    : ILearningTaskRepository<string>
{
    public async Task DeleteById(string id, CancellationToken cancellation)
    {
        var container = cosmosClient.GetContainer(cosmosDbOptions.DatabaseId, cosmosDbOptions.TasksContainer);
        await container.DeleteItemAsync<LearningTask>(
            id,
            PartitionKey.None,
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

    public async Task<LearningTask> FindById(string id, CancellationToken cancellation)
    {
        var container = cosmosClient.GetContainer(cosmosDbOptions.DatabaseId, cosmosDbOptions.TasksContainer);
        return await container.ReadItemAsync<LearningTask>(id, PartitionKey.None, cancellationToken: cancellation);
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

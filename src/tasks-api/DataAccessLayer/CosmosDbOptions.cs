namespace EKids.Chatbot.Tasks.DataAccessLayer;

internal class CosmosDbOptions
{
    public required string DatabaseId { get; init; }
    public required string TasksContainer { get; init; }
}
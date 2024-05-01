namespace EKids.Chatbot.Tasks.DataAccessLayer;

internal class CosmosDbOptions
{
    public const string ConfigSection = "Tasks:CosmosDb";

    public required string CosmosDbConnectionString { get; set; }
    public required string DatabaseId { get; set; }
    public required string TasksContainer { get; set; }
}
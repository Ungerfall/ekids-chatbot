namespace EKids.Chatbot.Tasks.DataAccessLayer.Entities;
public class LearningTask
{
    public required string Id { get; init; }

    public required string Title { get; init; }

    public required string CourseId { get; init; }

    public required string Description { get; init; }

    public required int Year { get; init; }

    public string? Url { get; init; }

    public TaskComplexity? Complexity { get; init; }
}

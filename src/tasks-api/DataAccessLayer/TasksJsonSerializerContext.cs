using EKids.Chatbot.Tasks.DataAccessLayer.Entities;
using System.Text.Json.Serialization;

namespace EKids.Chatbot.Tasks.DataAccessLayer;
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(LearningTask))]
[JsonSerializable(typeof(IAsyncEnumerable<LearningTask>))]
[JsonSerializable(typeof(LearningTask[]))]
[JsonSerializable(typeof(TaskComplexity))]
internal partial class TasksJsonSerializerContext : JsonSerializerContext;

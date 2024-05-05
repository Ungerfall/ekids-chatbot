using EKids.Chatbot.Tasks.DataAccessLayer.Entities;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EKids.Chatbot.Tasks.DataAccessLayer;
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(LearningTask))]
[JsonSerializable(typeof(IAsyncEnumerable<LearningTask>))]
[JsonSerializable(typeof(LearningTask[]))]
[JsonSerializable(typeof(TaskComplexity))]
public partial class TasksJsonSerializerContext : JsonSerializerContext;

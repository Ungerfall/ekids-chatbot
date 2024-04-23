using System.Text.Json.Serialization;

namespace EKids.Chatbot.Tasks.DataAccessLayer;
[JsonSerializable(typeof(LearningTask))]
[JsonSerializable(typeof(LearningTask[]))]
[JsonSerializable(typeof(TaskComplexity))]
internal partial class TasksJsonSerializerContext : JsonSerializerContext;

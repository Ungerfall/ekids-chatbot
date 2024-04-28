using System.Text.Json.Serialization;

using EKids.Chatbot.Tasks.DataAccessLayer.Entities;

namespace EKids.Chatbot.Tasks.DataAccessLayer;
[JsonSerializable(typeof(LearningTask))]
[JsonSerializable(typeof(LearningTask[]))]
[JsonSerializable(typeof(TaskComplexity))]
internal partial class TasksJsonSerializerContext : JsonSerializerContext;

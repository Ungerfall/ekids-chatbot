using System.Text.Json.Serialization;

namespace EKids.Chatbot.Tasks.DataAccessLayer;

[Flags]
[JsonConverter(typeof(JsonStringEnumConverter<TaskComplexity>))]
public enum TaskComplexity
{
    None = 0,

    Novice = 1,
    Intermediate = 1 << 8,
    Advanced = 1 << 16,
}

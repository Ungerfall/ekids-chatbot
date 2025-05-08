using System.Diagnostics.CodeAnalysis;

namespace EKids.Chatbot.Telegram.Worker;
public class Configuration
{
    [DisallowNull]
    public string TelegramBotToken { get; set; } = null!;
}

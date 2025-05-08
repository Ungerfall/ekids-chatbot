using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace EKids.Chatbot.Telegram.Core.Commands;
public class StartCommand(
    ITelegramBotClient botClient,
    ILogger<StartCommand> logger)
{
    public async Task Handle(long chatId)
    {
        logger.LogInformation("Handling /start command");
        await botClient.SendTextMessageAsync(chatId, "/start command has been handled");
    }
}

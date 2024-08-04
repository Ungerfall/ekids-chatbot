using EKids.Chatbot.Telegram.Worker.Abstractions;
using Telegram.Bot;

namespace EKids.Chatbot.Telegram.Worker.Services;

// Compose Receiver and UpdateHandler implementation
public class ReceiverService : ReceiverServiceBase<PollingUpdateHandler>
{
    public ReceiverService(
        ITelegramBotClient botClient,
        PollingUpdateHandler updateHandler,
        ILogger<ReceiverServiceBase<PollingUpdateHandler>> logger)
        : base(botClient, updateHandler, logger)
    {
    }
}
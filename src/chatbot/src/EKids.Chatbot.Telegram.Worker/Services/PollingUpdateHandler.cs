using EKids.Chatbot.Telegram.Core;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace EKids.Chatbot.Telegram.Worker.Services;

public class PollingUpdateHandler(UpdateHandler updateHandler) : IUpdateHandler
{
    private readonly UpdateHandler _updateHandler = updateHandler;

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        return _updateHandler.HandlePollingErrorAsync(exception, cancellationToken);
    }

    public async Task HandleUpdateAsync(ITelegramBotClient _, Update update, CancellationToken cancellationToken)
    {
        await _updateHandler.Handle(update, cancellationToken);
    }
}
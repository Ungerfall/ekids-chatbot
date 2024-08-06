using EKids.Chatbot.Telegram.Core.Commands;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace EKids.Chatbot.Telegram.Core;

public class UpdateHandler(
    ITelegramBotClient botClient,
    StartCommand startCommand,
    GetUsersCommand getUsersCommand,
    ILogger<UpdateHandler> logger)
{
    private const string BotUsername = "@ekids-bot";

    public async Task Handle(Update update, CancellationToken cancellation)
    {
        logger.LogInformation("Invoke telegram update function");

        var handler = update switch
        {
            /*
             { EditedMessage: { } }      => UpdateType.EditedMessage,
             { InlineQuery: { } }        => UpdateType.InlineQuery,
             { ChosenInlineResult: { } } => UpdateType.ChosenInlineResult,
             { CallbackQuery: { } }      => UpdateType.CallbackQuery,
             { ChannelPost: { } }        => UpdateType.ChannelPost,
             { EditedChannelPost: { } }  => UpdateType.EditedChannelPost,
             { ShippingQuery: { } }      => UpdateType.ShippingQuery,
             { PreCheckoutQuery: { } }   => UpdateType.PreCheckoutQuery,
             { Poll: { } }               => UpdateType.Poll,
             { PollAnswer: { } }         => UpdateType.PollAnswer,
             { MyChatMember: { } }       => UpdateType.MyChatMember,
             { ChatMember: { } }         => UpdateType.ChatMember,
             { ChatJoinRequest: { } }    => UpdateType.ChatJoinRequest,
            */
            { Message: { } message } => BotOnMessageReceived(message, cancellation),
            _ => UnknownUpdateHandlerAsync(update, cancellation)
        };

        await handler;
    }

    private async Task BotOnMessageReceived(Message message, CancellationToken cancellation)
    {
        logger.LogInformation("Receive message {@message}", new { message.Text, message.Type, message.Chat.Id });
        if (message.Text is not { } messageText)
        {
            return;
        }

        string command = messageText; // TODO: add logic to extract command
        var action = command switch
        {
            "/start" => startCommand.Handle(message.Chat.Id),
            "/getusers" => getUsersCommand.Handle(message.Chat.Id),
            _ => OnMessageReceived(message, messageText, cancellation),
        };

        await action;
    }

    private Task OnMessageReceived(Message msg, string messageText, CancellationToken cancellation)
    {
        return Task.CompletedTask;
    }

    private Task UnknownUpdateHandlerAsync(Update update, CancellationToken _)
    {
        logger.LogInformation("Unknown update type: {UpdateType}", update.Type);
        return Task.CompletedTask;
    }

    public async Task HandlePollingErrorAsync(Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        logger.LogInformation("HandleError: {ErrorMessage}", errorMessage);

        // Cooldown in case of network connection error
        if (exception is RequestException)
        {
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
        }
    }
}

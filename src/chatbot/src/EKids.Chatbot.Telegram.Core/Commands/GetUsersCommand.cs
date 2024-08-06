using EKids.Chatbot.Telegram.Core.ApiClients;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using UsersApiUser = EKids.Chatbot.Telegram.Core.ApiClients.User;

namespace EKids.Chatbot.Telegram.Core.Commands;
public class GetUsersCommand(
    ITelegramBotClient botClient,
    UsersApiClient usersApiClient,
    ILogger<StartCommand> logger)
{
    public async Task Handle(long chatId)
    {
        logger.LogInformation("Handling /getusers command");

        IEnumerable<UsersApiUser?> users = await usersApiClient.GetUsers();
        await botClient.SendTextMessageAsync(chatId, System.Text.Json.JsonSerializer.Serialize(users));
    }
}

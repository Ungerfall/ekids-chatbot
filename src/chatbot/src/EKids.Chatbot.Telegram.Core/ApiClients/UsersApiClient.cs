using System.Net.Http.Json;

namespace EKids.Chatbot.Telegram.Core.ApiClients;
public record class User(Guid Id, string? UserName, string? Email);

public class UsersApiClient(HttpClient client)
{
    public async Task<IEnumerable<User?>> GetUsers(CancellationToken cancellation = default)
    {
        return await client.GetFromJsonAsync<IEnumerable<User?>>("/users?api-version=1.0", cancellation) ?? [];
    }
}

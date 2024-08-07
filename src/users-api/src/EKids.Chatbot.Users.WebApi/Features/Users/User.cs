namespace EKids.Chatbot.Users.WebApi.Features.Users;

public record class User(Guid Id, string? UserName, string? Email, ICollection<string> Roles);

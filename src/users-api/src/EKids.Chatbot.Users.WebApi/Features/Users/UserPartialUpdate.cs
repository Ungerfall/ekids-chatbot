namespace EKids.Chatbot.Users.WebApi.Features.Users;
public record class UserPartialUpdate(string? UserName, string? Email, string[]? Roles);

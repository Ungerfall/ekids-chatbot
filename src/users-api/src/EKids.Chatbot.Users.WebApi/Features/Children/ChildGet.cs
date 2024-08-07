namespace EKids.Chatbot.Users.WebApi.Features.Children;
public record class ChildGet(Guid Id, string Name, string Email, ParentGet Parent);

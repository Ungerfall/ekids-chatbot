using Microsoft.AspNetCore.Identity;

namespace EKids.Chatbot.Users.WebApi.DataAccess;
public class Child
{
    public Guid Id { get; set; }

    public Guid ChildUserId { get; set; }
    public required IdentityUser<Guid> ChildUser { get; set; }

    public Guid ParentUserId { get; set; }
    public required IdentityUser<Guid> ParentUser { get; set; }
}

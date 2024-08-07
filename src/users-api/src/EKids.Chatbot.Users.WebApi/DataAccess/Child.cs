using Microsoft.AspNetCore.Identity;

namespace EKids.Chatbot.Users.WebApi.DataAccess;
public class Child
{
    public Guid Id { get; set; }

    public Guid ChildUserId { get; set; }
    public IdentityUser<Guid> ChildUser { get; set; } = null!;

    public Guid ParentUserId { get; set; }
    public IdentityUser<Guid> ParentUser { get; set; } = null!;
}

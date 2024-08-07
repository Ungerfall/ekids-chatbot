using Asp.Versioning.Builder;
using EKids.Chatbot.Users.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace EKids.Chatbot.Users.WebApi.Features.Children;
public static class ChildrenEndpoint
{
    public static IVersionedEndpointRouteBuilder MapChildrenApi(this IEndpointRouteBuilder routeBuilder)
    {
        var coursesApi = routeBuilder.NewVersionedApi();
        var v1 = coursesApi.MapGroup("/courses").HasApiVersion(1.0);
        v1.MapGet("/{courseId}/children", async (Guid courseId, UsersDbContext db) =>
        {
            courseId = Guid.Empty; // TODO: implement course id
            return await db.Children.Select(x =>
                new ChildGet(
                    x.ChildUser.Id,
                    x.ChildUser.UserName,
                    x.ChildUser.Email,
                    new(x.ParentUser.UserName, x.ParentUser.Email)))
                .ToArrayAsync();
        })
            .Produces<ChildrenList>();

        return coursesApi;
    }
}

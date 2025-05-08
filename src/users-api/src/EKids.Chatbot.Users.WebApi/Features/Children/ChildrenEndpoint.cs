using Asp.Versioning.Builder;
using EKids.Chatbot.Users.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EKids.Chatbot.Users.WebApi.Features.Children;
public static class ChildrenEndpoint
{
    public static IVersionedEndpointRouteBuilder MapChildrenApi(this IEndpointRouteBuilder routeBuilder)
    {
        var coursesApi = routeBuilder.NewVersionedApi();
        var v1 = coursesApi.MapGroup("/courses").HasApiVersion(1.0);
        var v2 = coursesApi.MapGroup("/courses").HasApiVersion(2.0);
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

        v2.MapGet("/{courseId}/children", async (
            Guid courseId,
            [FromQuery(Name = "parent")] string? parent,
            [FromQuery(Name = "userName")] string? userName,
            UsersDbContext db) =>
        {
            courseId = Guid.Empty; // TODO: implement course id
            var query = db.Children.AsQueryable();
            if (parent is not null)
            {
                query = query.Where(x => x.ParentUser.UserName == parent);
            }

            if (userName is not null)
            {
                query = query.Where(x => x.ChildUser.UserName == userName);
            }

            return await query
                .Select(x =>
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

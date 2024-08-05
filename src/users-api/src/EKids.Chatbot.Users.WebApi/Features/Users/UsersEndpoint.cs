using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EKids.Chatbot.Users.WebApi.Features.Users;
public static class UsersEndpoint
{
    public static IVersionedEndpointRouteBuilder MapUsersApi(this IEndpointRouteBuilder routeBuilder)
    {
        var usersApi = routeBuilder.NewVersionedApi();
        var v1 = usersApi.MapGroup("/users").HasApiVersion(1.0);
        v1.MapGet("/", (UserManager<IdentityUser<Guid>> userManger, CancellationToken cancellation) =>
                userManger.Users
                    .Select(x => new User(x.Id, x.UserName, x.Email))
                    .ToArrayAsync(cancellationToken: cancellation))
            .Produces<UsersList>();
        v1.MapPost("/", async (UserCreate user, UserManager<IdentityUser<Guid>> userManager) =>
        {
            var result = await userManager.CreateAsync(new IdentityUser<Guid>(user.UserName) { Email = user.Email });

            return result.Succeeded
                ? Results.Created()
                : Results.ValidationProblem(
                    result.Errors.ToDictionary(
                        x => x.Code,
                        elementSelector: ie => new string[] { ie.Description }),
                    statusCode: StatusCodes.Status409Conflict);
        });

        return usersApi;
    }
}

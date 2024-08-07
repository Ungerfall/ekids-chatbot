using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace EKids.Chatbot.Users.WebApi.Features.Users;
public static class UsersEndpoint
{
    public static IVersionedEndpointRouteBuilder MapUsersApi(this IEndpointRouteBuilder routeBuilder)
    {
        var usersApi = routeBuilder.NewVersionedApi();
        var v1 = usersApi.MapGroup("/users").HasApiVersion(1.0);
        v1.MapGet("/", async (UserManager<IdentityUser<Guid>> userManger, CancellationToken cancellation) =>
        {
            var users = await userManger.Users.ToArrayAsync(cancellationToken: cancellation);
            return await Task.WhenAll(
                users
                    .Select(async x =>
                    {
                        var roles = await userManger.GetRolesAsync(x);
                        return new User(x.Id, x.UserName, x.Email, roles);
                    }));
        })
            .Produces<UsersList>();

        v1.MapPost("/", async (
            UserCreate user,
            UserManager<IdentityUser<Guid>> userManager,
            RoleManager<IdentityRole<Guid>> roleManager) =>
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var identityUser = new IdentityUser<Guid>(user.UserName) { Email = user.Email };
                    var result = await userManager.CreateAsync(identityUser);

                    if (!result.Succeeded)
                    {
                        scope.Dispose();
                        return Results.ValidationProblem(
                                            result.Errors.ToDictionary(
                                                x => x.Code,
                                                elementSelector: ie => new string[] { ie.Description }),
                                            statusCode: StatusCodes.Status409Conflict);
                    }

                    foreach (var role in user.Roles)
                    {
                        IdentityRole<Guid>? foundRole = await roleManager.FindByNameAsync(role);
                        if (foundRole is null)
                        {
                            IdentityRole<Guid> newRole = new(role);
                            var roleCreateResult = await roleManager.CreateAsync(newRole);
                            if (!roleCreateResult.Succeeded)
                            {
                                scope.Dispose();
                                return Results.Problem(
                                    detail: string.Join(';', roleCreateResult.Errors.Select(x =>
                                        $"{x.Code}: {x.Description}")),
                                    statusCode: StatusCodes.Status409Conflict);
                            }
                        }

                        await userManager.AddToRoleAsync(identityUser, role);
                    }

                    scope.Complete();
                }
                catch (Exception)
                {
                    scope.Dispose();
                    throw;
                }
            }

            return Results.Created();
        });

        v1.MapDelete("/{userId}", async (Guid userId, UserManager<IdentityUser<Guid>> userManager) =>
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user is null)
            {
                return Results.NotFound();
            }

            var result = await userManager.DeleteAsync(user);

            return result.Succeeded
                ? Results.Ok()
                : Results.ValidationProblem(
                    result.Errors.ToDictionary(
                        x => x.Code,
                        elementSelector: ie => new string[] { ie.Description }),
                    statusCode: StatusCodes.Status409Conflict);
        });

        v1.MapPatch("/{userId}", async (
            Guid userId,
            UserPartialUpdate partialUpdate,
            UserManager<IdentityUser<Guid>> userManager,
            RoleManager<IdentityRole<Guid>> roleManager) =>
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user is null)
            {
                return Results.NotFound();
            }

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    user.UserName = partialUpdate.UserName ?? user.UserName;
                    user.Email = partialUpdate.Email ?? user.Email;
                    var result = await userManager.UpdateAsync(user);
                    if (!result.Succeeded)
                    {
                        scope.Dispose();
                        return Results.ValidationProblem(
                                            result.Errors.ToDictionary(
                                                x => x.Code,
                                                elementSelector: ie => new string[] { ie.Description }),
                                            statusCode: StatusCodes.Status409Conflict);
                    }

                    // remove existing roles
                    if (partialUpdate.Roles is not null)
                    {
                        var existingRoles = await userManager.GetRolesAsync(user);
                        result = await userManager.RemoveFromRolesAsync(user, existingRoles);
                        if (!result.Succeeded)
                        {
                            scope.Dispose();
                            return Results.Problem(
                                detail: string.Join(';', result.Errors.Select(x =>
                                    $"{x.Code}: {x.Description}")),
                                statusCode: StatusCodes.Status409Conflict);
                        }
                    }

                    foreach (var role in partialUpdate.Roles ?? [])
                    {
                        IdentityRole<Guid>? foundRole = await roleManager.FindByNameAsync(role);
                        if (foundRole is null)
                        {
                            IdentityRole<Guid> newRole = new(role);
                            result = await roleManager.CreateAsync(newRole);
                            if (!result.Succeeded)
                            {
                                scope.Dispose();
                                return Results.Problem(
                                    detail: string.Join(';', result.Errors.Select(x =>
                                        $"{x.Code}: {x.Description}")),
                                    statusCode: StatusCodes.Status409Conflict);
                            }
                        }

                        await userManager.AddToRoleAsync(user, role);
                    }

                    scope.Complete();
                }
                catch (Exception)
                {
                    scope.Dispose();
                    throw;
                }
            }

            return Results.Ok();
        });

        return usersApi;
    }
}

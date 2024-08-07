using Asp.Versioning.Builder;
using EKids.Chatbot.Users.DataAccess;
using EKids.Chatbot.Users.WebApi.Features.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace EKids.Chatbot.Users.WebApi.Features.Parents;
public static class ParentsEndpoint
{
    public static IVersionedEndpointRouteBuilder MapParentsApi(this IEndpointRouteBuilder routeBuilder)
    {
        var parentsApi = routeBuilder.NewVersionedApi();
        var v1 = parentsApi.MapGroup("/parents").HasApiVersion(1.0);
        v1.MapPost("/", async (
            ParentCreate parent,
            UserManager<IdentityUser<Guid>> userManager,
            RoleManager<IdentityRole<Guid>> roleManager) =>
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var parentUser = new IdentityUser<Guid>(parent.UserName) { Email = parent.Email };
                    var result = await userManager.CreateAsync(parentUser);
                    if (!result.Succeeded)
                    {
                        scope.Dispose();
                        return Results.ValidationProblem(
                                            result.Errors.ToDictionary(
                                                x => x.Code,
                                                elementSelector: ie => new string[] { ie.Description }),
                                            statusCode: StatusCodes.Status409Conflict);
                    }

                    var parentRole = await roleManager.FindByNameAsync(RolesConst.Parent);
                    if (parentRole is null)
                    {
                        result = await roleManager.CreateAsync(new IdentityRole<Guid>(RolesConst.Parent));
                        if (!result.Succeeded)
                        {
                            scope.Dispose();
                            return Results.Problem(
                                detail: string.Join(';', result.Errors.Select(x =>
                                    $"{x.Code}: {x.Description}")),
                                statusCode: StatusCodes.Status409Conflict);
                        }
                    }

                    result = await userManager.AddToRoleAsync(parentUser, RolesConst.Parent);
                    if (!result.Succeeded)
                    {
                        scope.Dispose();
                        return Results.Problem(
                            detail: string.Join(';', result.Errors.Select(x =>
                                $"{x.Code}: {x.Description}")),
                            statusCode: StatusCodes.Status409Conflict);
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

        v1.MapPost("/{parentId}/children", async (
            Guid parentId,
            ChildrenCreate children,
            UserManager<IdentityUser<Guid>> userManager,
            RoleManager<IdentityRole<Guid>> roleManager,
            UsersDbContext db) =>
        {
            var parent = await userManager.FindByIdAsync(parentId.ToString());
            if (parent is null)
            {
                return Results.NotFound("Parent is not found");
            }

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    foreach (var child in children.Children)
                    {
                        var childUser = new IdentityUser<Guid>(child.UserName);
                        var result = await userManager.CreateAsync(childUser);
                        if (!result.Succeeded)
                        {
                            scope.Dispose();
                            return Results.ValidationProblem(
                                                result.Errors.ToDictionary(
                                                    x => x.Code,
                                                    elementSelector: ie => new string[] { ie.Description }),
                                                statusCode: StatusCodes.Status409Conflict);
                        }

                        var childRole = await roleManager.FindByNameAsync(RolesConst.Child);
                        if (childRole is null)
                        {
                            result = await roleManager.CreateAsync(new IdentityRole<Guid>(RolesConst.Child));
                            if (!result.Succeeded)
                            {
                                scope.Dispose();
                                return Results.Problem(
                                    detail: string.Join(';', result.Errors.Select(x =>
                                        $"{x.Code}: {x.Description}")),
                                    statusCode: StatusCodes.Status409Conflict);
                            }
                        }

                        result = await userManager.AddToRoleAsync(childUser, RolesConst.Child);
                        if (!result.Succeeded)
                        {
                            scope.Dispose();
                            return Results.Problem(
                                detail: string.Join(';', result.Errors.Select(x =>
                                    $"{x.Code}: {x.Description}")),
                                statusCode: StatusCodes.Status409Conflict);
                        }

                        await db.Children.AddAsync(new DataAccess.Child { ChildUser = childUser, ParentUser = parent });
                    }

                    await db.SaveChangesAsync();
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

        v1.MapGet("/", async (UsersDbContext db) =>
        {
            var query = from u in db.Users
                        join ur in db.UserRoles on u.Id equals ur.UserId
                        join r in db.Roles on ur.RoleId equals r.Id
                        join c in db.Children on u.Id equals c.ParentUserId into children
                        where r.Name == RolesConst.Parent
                        select new { Parent = u, Children = children };

            return await query.ToArrayAsync();
        });

        return parentsApi;
    }
}

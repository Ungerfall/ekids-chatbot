using EKids.Chatbot.Users.DataAccess;
using EKids.Chatbot.Users.WebApi.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddDbContext<UsersDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UsersConnection")));

builder.Services.AddIdentity<IdentityUser<Guid>, IdentityRole<Guid>>()
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<UsersDbContext>();

builder.Services.AddApiVersioning();

var app = builder.Build();
app.UseDeveloperExceptionPage();

var usersApi = app.NewVersionedApi("/users");
usersApi
    .MapGet("/", async (UsersDbContext db, CancellationToken cancellation) =>
        new UsersList(await db.Users
            .Select(x => new User(x.Id, x.UserName, x.Email))
            .ToArrayAsync(cancellationToken: cancellation)))
    .Produces<UsersList>()
    .HasApiVersion(1.0);
usersApi
    .MapPost("/", async (UserCreate user, UsersDbContext db, CancellationToken cancellation) =>
    {
        await db.Users.AddAsync(new IdentityUser<Guid>(user.UserName) { Email = user.Email }, cancellation);
        await db.SaveChangesAsync(cancellation);

        return Results.Created();
    })
    .HasApiVersion(1.0);

app.Run();

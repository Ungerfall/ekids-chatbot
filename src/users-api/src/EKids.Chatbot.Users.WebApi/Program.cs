using EKids.Chatbot.Users.DataAccess;
using EKids.Chatbot.Users.WebApi.Features.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateSlimBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddDbContext<UsersDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UsersConnection")));

builder.Services.AddIdentity<IdentityUser<Guid>, IdentityRole<Guid>>()
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<UsersDbContext>();

builder.Services.AddApiVersioning();

var app = builder.Build();
app.UseDeveloperExceptionPage();
app.MapUsersApi();

app.Run();

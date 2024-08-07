
var builder = DistributedApplication.CreateBuilder(args);

var usersService = builder.AddProject<Projects.EKids_Chatbot_Users_WebApi>("users-api");

builder.AddProject<Projects.EKids_Chatbot_Telegram_Worker>("chatbot-worker")
    .WithExternalHttpEndpoints()
    .WithReference(usersService);

builder.Build().Run();

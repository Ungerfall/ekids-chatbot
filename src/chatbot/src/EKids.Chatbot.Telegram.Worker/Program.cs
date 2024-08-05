using EKids.Chatbot.Telegram.Core;
using EKids.Chatbot.Telegram.Core.ApiClients;
using EKids.Chatbot.Telegram.Worker;
using EKids.Chatbot.Telegram.Worker.Services;
using Microsoft.Extensions.Options;
using Telegram.Bot;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
// Aspire defaults
builder.AddServiceDefaults();
// Register Bot configuration
builder.Services.Configure<Configuration>(opt =>
{
    opt.TelegramBotToken = builder.Configuration["Bot:TELEGRAM_BOT_TOKEN"]
        ?? throw new ArgumentNullException(nameof(opt), "TELEGRAM_BOT_TOKEN is missing");
});
builder.Services.AddHttpClient("telegram_bot_client")
    .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
    {
        Configuration? botConfig = sp.GetRequiredService<IOptions<Configuration>>().Value;
        TelegramBotClientOptions options = new(botConfig.TelegramBotToken);
        return new TelegramBotClient(options, httpClient);
    });
builder.Services.AddHttpClient<UsersApiClient>("users_api_client", client =>
{
    // This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
    // Learn more about service discovery scheme resolution at https://aka.ms/dotnet/sdschemes.
    client.BaseAddress = new("https+http://users-api");
});
builder.Services.AddScoped<PollingUpdateHandler>();
builder.Services.AddScoped<ReceiverService>();
builder.Services.AddScoped<UpdateHandler>();
builder.Services.AddHostedService<PollingService>();

IHost host = builder.Build();

await host.RunAsync();
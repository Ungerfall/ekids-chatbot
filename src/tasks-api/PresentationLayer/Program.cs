using EKids.Chatbot.Tasks.BusinessLayer;
using EKids.Chatbot.Tasks.DataAccessLayer;
using EKids.Chatbot.Tasks.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading;

namespace EKids.Chatbot.Tasks.WebApi;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateSlimBuilder(args);

        builder.Services.ConfigureHttpJsonOptions(options
            => options.SerializerOptions.TypeInfoResolverChain.Insert(0, TasksJsonSerializerContext.Default));
        builder.Services.AddOptions<CosmosDbOptions>()
            .Bind(builder.Configuration.GetSection(CosmosDbOptions.ConfigSection));
        builder.Services.AddSingleton(sp =>
        {
            var opt = sp.GetRequiredService<IOptions<CosmosDbOptions>>().Value;
            return new CosmosClient(
                opt.CosmosDbConnectionString,
                clientOptions: new CosmosClientOptions
                {
                    MaxRetryAttemptsOnRateLimitedRequests = 3,
                    Serializer = new CosmosSystemTextJsonSerializer(),
                });
        });
        builder.Services.AddScoped<ILearningTaskRepository, CosmosDbLearningTaskRepository>();
        builder.Services.AddScoped<ILearningTaskService, LearningTaskService>();

        var app = builder.Build();
        app.UseDeveloperExceptionPage();
        var tasksApi = app.MapGroup("/tasks");
        tasksApi.MapGet("/", (ILearningTaskService service, CancellationToken cancellation)
            => service.ListTasks(courseId: null, cancellation: cancellation));

        var coursesApi = app.MapGroup("/courses");
        coursesApi.MapGet("/{courseId}/tasks", (
            Guid courseId,
            ILearningTaskService service,
            CancellationToken cancellation)
                => service.ListTasks(courseId, cancellation));
        coursesApi.MapGet("/{courseId}/tasks/{taskId}", async (
            Guid courseId,
            Guid taskId,
            ILearningTaskService service,
            CancellationToken cancellation)
                => (await service.FindTask(courseId, taskId, cancellation)) is { } task
                    ? Results.Ok(task)
                    : Results.NotFound());
        coursesApi.MapPost("/{courseId}/tasks", async (
            Guid courseId,
            LearningTask task,
            ILearningTaskService service,
            CancellationToken cancellation) =>
        {
            await service.AddTask(task, courseId, cancellation);
            return Results.Accepted();
        });
        coursesApi.MapDelete("/{courseId}/tasks/{taskId}", async (
            Guid courseId,
            Guid taskId,
            ILearningTaskService service,
            CancellationToken cancellation)
                => await service.DeleteTask(courseId, taskId, cancellation));

        app.Run();
    }
}

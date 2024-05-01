// Ignore Spelling: app

using EKids.Chatbot.Tasks.BusinessLayer;
using EKids.Chatbot.Tasks.DataAccessLayer.Entities;

namespace EKids.Chatbot.Tasks.PresentationLayer;
/// <summary>
/// Extends minimal API <see cref="WebApplication"/> with learning tasks endpoints
/// </summary>
public static class LearningTasksController
{
    public static void MapLearningTasks(this WebApplication app)
    {
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
    }
}

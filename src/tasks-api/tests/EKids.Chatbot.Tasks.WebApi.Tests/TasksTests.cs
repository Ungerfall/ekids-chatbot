using EKids.Chatbot.Tasks.DataAccessLayer.Entities;
using System.Net.Http.Json;

namespace EKids.Chatbot.Tasks.WebApi.Tests;

public class TasksTests(TasksWebApplicationFixture fixture) : IClassFixture<TasksWebApplicationFixture>
{
    [Fact]
    public async Task GetTasks_ReturnsTasks()
    {
        // Arrange
        Assert.NotNull(fixture.Client);

        // Act
        var response = await fixture.Client.GetAsync("/tasks");

        // Assert
        response.EnsureSuccessStatusCode();
        var tasks = await response.Content.ReadFromJsonAsync<LearningTask[]>();
        Assert.NotNull(tasks);
        Assert.NotEmpty(tasks);
    }

    [Fact]
    public async Task GetCourseTasks_ReturnsTasks()
    {
        // Arrange
        Assert.NotNull(fixture.Client);

        // Act
        var response = await fixture.Client.GetAsync($"/courses/{fixture.CourseId}/tasks");

        // Assert
        response.EnsureSuccessStatusCode();
        var tasks = await response.Content.ReadFromJsonAsync<LearningTask[]>();
        Assert.NotNull(tasks);
        Assert.NotEmpty(tasks);
        Assert.Single(tasks.Where(x => x.CourseId == fixture.CourseId));
    }

    [Fact]
    public async Task GetCourseTasksById_ReturnTasks()
    {
        // Arrange
        Assert.NotNull(fixture.Client);

        // Act
        var response = await fixture.Client.GetAsync($"/courses/{fixture.CourseId}/tasks/{fixture.TaskId}");

        // Assert
        response.EnsureSuccessStatusCode();
        var tasks = await response.Content.ReadFromJsonAsync<LearningTask>();
        Assert.NotNull(tasks);
        Assert.Equal(fixture.CourseId, tasks.CourseId);
        Assert.Equal(fixture.TaskId, tasks.Id);
    }

    [Fact]
    public async Task DeleteTask_ReturnsOk()
    {
        // Arrange
        Assert.NotNull(fixture.Client);
        string courseId = Guid.NewGuid().ToString();
        string taskId = Guid.NewGuid().ToString();
        await fixture.Client.PostAsJsonAsync(
            $"/courses/{courseId}/tasks",
            new LearningTask
            {
                CourseId = courseId,
                Description = "testing delete",
                Id = taskId,
                Title = "title delete",
                Year = 2024,
            });

        // Act
        var response = await fixture.Client.DeleteAsync($"/courses/{courseId}/tasks/{taskId}");

        // Assert
        response.EnsureSuccessStatusCode();
    }
}
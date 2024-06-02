using System;

namespace EKids.Chatbot.Homeworks.WebApi.ViewModels;

public sealed class HomeworkViewModel : BaseViewModel
{
    public required Guid CourseId { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Url { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
}

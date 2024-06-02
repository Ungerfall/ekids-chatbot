using EKids.Chatbot.Homeworks.Application.Models.Base;
using System;

namespace EKids.Chatbot.Homeworks.Application.Models;

public class HomeworkModel : BaseModel
{
    public required Guid CourseId { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Url { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
}

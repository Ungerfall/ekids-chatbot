using EKids.Chatbot.Homeworks.Core.Entities.Base;
using System;

namespace EKids.Chatbot.Homeworks.Core.Entities;

public class Homework(
    Guid id,
    Guid courseId,
    string title,
    string description,
    string url,
    DateTime startDate,
    DateTime endDate) : Entity(id)
{
    public Guid CourseId { get; } = courseId;
    public string Title { get; } = title;
    public string Description { get; } = description;
    public string Url { get; } = url;
    public DateTime StartDate { get; } = startDate;
    public DateTime EndDate { get; } = endDate;
}

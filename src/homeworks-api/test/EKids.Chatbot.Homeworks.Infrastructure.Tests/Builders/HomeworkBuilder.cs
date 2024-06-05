using EKids.Chatbot.Homeworks.Core.Entities;
using System;

namespace EKids.Chatbot.Homeworks.Infrastructure.Tests.Builders;

public class HomeworkBuilder
{
    private readonly Homework _homework;

    public Guid TestCourseId { get; } = Guid.NewGuid();
    public string TestTitle { get; } = "title";
    public string TestDescription { get; } = "description";
    public string TestUrl { get; } = "url";
    public DateTime TestSd { get; } = new(2023, 1, 1);
    public DateTime TestEd { get; } = new(2024, 1, 1);

    public HomeworkBuilder()
    {
        _homework = WithDefaultValues();
    }

    public Homework Build()
    {
        return _homework;
    }

    public Homework WithDefaultValues()
    {
        return new Homework(Guid.NewGuid(), TestCourseId, TestTitle, TestDescription, TestUrl, TestSd, TestEd);
    }
}

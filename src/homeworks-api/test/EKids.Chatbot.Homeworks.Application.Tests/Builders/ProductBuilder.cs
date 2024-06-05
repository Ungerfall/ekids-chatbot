using System;

namespace EKids.Chatbot.Homeworks.Application.Tests.Builders;

public class ProductBuilder
{
    public Guid HomeworkId_1 { get; } = Guid.NewGuid();
    public Guid HomeworkId_2 { get; } = Guid.NewGuid();
    public Guid HomeworkId_3 { get; } = Guid.NewGuid();
    public string HomeworkTitle_1 { get; } = "HomeworkTitleX";
    public string HomeworkTitle_2 { get; } = "HomeworkTitleY";
    public string HomeworkTitle_3 { get; } = "HomeworkTitleZ";
}

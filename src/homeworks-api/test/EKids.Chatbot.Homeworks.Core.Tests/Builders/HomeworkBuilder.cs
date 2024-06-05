using EKids.Chatbot.Homeworks.Core.Entities;
using System;
using System.Collections.Generic;

namespace EKids.Chatbot.Homeworks.Core.Tests.Builders;

public class HomeworkBuilder
{
    public Guid HomeworkId_1 { get; } = Guid.NewGuid();
    public Guid HomeworkId_2 { get; } = Guid.NewGuid();
    public Guid HomeworkId_3 { get; } = Guid.NewGuid();
    public string HomeworkTitle_1 { get; } = "Homework_X";
    public string HomeworkTitle_2 { get; } = "Homework_Y";
    public string HomeworkTitle_3 { get; } = "Homework_Z";

    public List<Homework> GetProductCollection()
    {
        Guid courseId = Guid.NewGuid();
        DateTime sd = new(2023, 1, 2);
        DateTime ed = new(2024, 1, 1);
        return
        [
            new (HomeworkId_1, courseId, HomeworkTitle_1, "desc", "url", sd, ed),
            new (HomeworkId_2, courseId, HomeworkTitle_2, "desc", "url", sd, ed),
            new (HomeworkId_3, courseId, HomeworkTitle_3, "desc", "url", sd, ed),
        ];
    }
}

using EKids.Chatbot.Homeworks.Core.Entities;
using System;
using Xunit;

namespace EKids.Chatbot.Homeworks.Core.Tests.Entities;

public class ProductTests
{
    private readonly Guid _testCourseId = Guid.NewGuid();
    private readonly string _testTitle = "title";
    private readonly string _testDescription = "description";
    private readonly string _testUrl = "url";
    private readonly DateTime _testSd = new(2023, 1, 1);
    private readonly DateTime _testEd = new(2024, 1, 1);

    [Fact]
    public void Create_Homework()
    {
        Homework hw = new(Guid.NewGuid(), _testCourseId, _testTitle, _testDescription, _testUrl, _testSd, _testEd);

        Assert.NotEqual(Guid.Empty, hw.Id);
        Assert.Equal(_testCourseId, hw.CourseId);
        Assert.Equal(_testTitle, hw.Title);
        Assert.Equal(_testDescription, hw.Description);
        Assert.Equal(_testUrl, hw.Url);
        Assert.Equal(_testSd, hw.StartDate);
        Assert.Equal(_testEd, hw.EndDate);
    }
}

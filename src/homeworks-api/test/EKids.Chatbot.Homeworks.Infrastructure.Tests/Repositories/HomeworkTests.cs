using EKids.Chatbot.Homeworks.Core.Entities;
using EKids.Chatbot.Homeworks.Core.Specifications.Base;
using EKids.Chatbot.Homeworks.Infrastructure.Data;
using EKids.Chatbot.Homeworks.Infrastructure.Repository;
using EKids.Chatbot.Homeworks.Infrastructure.Tests.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EKids.Chatbot.Homeworks.Infrastructure.Tests.Repositories;

public class HomeworkTests
{
    private readonly HomeworkContext _homeworkContext;
    private readonly HomeworkRepository _homeworkRepository;
    private readonly ITestOutputHelper _output;
    private HomeworkBuilder HomeworkBuilder { get; } = new HomeworkBuilder();

    public HomeworkTests(ITestOutputHelper output)
    {
        _output = output;
        var dbOptions = new DbContextOptionsBuilder<HomeworkContext>()
            .UseInMemoryDatabase(databaseName: "Homework")
            .Options;
        _homeworkContext = new HomeworkContext(dbOptions);
        _homeworkRepository = new HomeworkRepository(_homeworkContext);
    }

    [Fact]
    public async Task Get_Existing_Homework()
    {
        var existingHomework = HomeworkBuilder.WithDefaultValues();
        _homeworkContext.Homeworks.Add(existingHomework);
        _homeworkContext.SaveChanges();

        var id = existingHomework.Id;
        _output.WriteLine("Homework id: {0}", id);

        var savedHomework = await _homeworkRepository.GetByIdAsync(id);
        Assert.NotEqual(Guid.Empty, savedHomework.Id);
        Assert.Equal(HomeworkBuilder.TestCourseId, savedHomework.CourseId);
        Assert.Equal(HomeworkBuilder.TestTitle, savedHomework.Title);
        Assert.Equal(HomeworkBuilder.TestDescription, savedHomework.Description);
        Assert.Equal(HomeworkBuilder.TestUrl, savedHomework.Url);
        Assert.Equal(HomeworkBuilder.TestSd, savedHomework.StartDate);
        Assert.Equal(HomeworkBuilder.TestEd, savedHomework.EndDate);
    }

    [Fact]
    public async Task Count_Homeworks()
    {
        var existingHomework = HomeworkBuilder.WithDefaultValues();
        _homeworkContext.Homeworks.Add(existingHomework);
        _homeworkContext.SaveChanges();

        _output.WriteLine("Homework id: {0}", existingHomework.Id);

        int count = await _homeworkRepository.CountAsync(new EmptySpec());
        Assert.Equal(_homeworkContext.Homeworks.Count(), count);
    }

    private sealed class EmptySpec : BaseSpecification<Homework>
    {
        public EmptySpec() : base((_) => true)
        {
        }
    }
}

using EKids.Chatbot.Homeworks.Application.Services;
using EKids.Chatbot.Homeworks.Core.Entities;
using EKids.Chatbot.Homeworks.Core.Repositories.Base;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EKids.Chatbot.Homeworks.Application.Tests.Services;

public class HomeworkTests
{
    // NOTE : This layer we are not loaded database objects, test functionality of application layer

    private readonly IRepository<Homework> _mockHomeworkRepository;
    private readonly ILogger<HomeworkService> _mockLogger;

    public HomeworkTests()
    {
        _mockHomeworkRepository = Substitute.For<IRepository<Homework>>();
        _mockLogger = Substitute.For<ILogger<HomeworkService>>();
    }

    [Fact]
    public async Task Get_Homework_List()
    {
        var hw_1 = new Homework(
            id: Guid.NewGuid(),
            courseId: Guid.NewGuid(),
            title: string.Empty,
            description: string.Empty,
            url: string.Empty,
            startDate: new DateTime(2023, 1, 1),
            endDate: new DateTime(2024, 1, 1));
        var hw_2 = new Homework(
            id: Guid.NewGuid(),
            courseId: Guid.NewGuid(),
            title: string.Empty,
            description: string.Empty,
            url: string.Empty,
            startDate: new DateTime(2021, 1, 1),
            endDate: new DateTime(2022, 1, 1));

        _mockHomeworkRepository.GetAllAsync().Returns([hw_1, hw_2]);

        var homeworkService = new HomeworkService(_mockHomeworkRepository, _mockLogger);
        await homeworkService.GetHomeworkList();

        await _mockHomeworkRepository.Received(1).GetAllAsync();
    }

    [Fact]
    public async Task Create_New_Homework()
    {
        var hw = new Homework(
            id: Guid.NewGuid(),
            courseId: Guid.NewGuid(),
            title: string.Empty,
            description: string.Empty,
            url: string.Empty,
            startDate: new DateTime(2021, 1, 1),
            endDate: new DateTime(2022, 1, 1));
        Homework nullHomework = null; // we gave null homework in order to create new one, if you give real homework it returns already existing error

        _mockHomeworkRepository.GetByIdAsync(Arg.Any<Guid>()).Returns(nullHomework);
        _mockHomeworkRepository.AddAsync(hw).Returns(hw);

        var homeworkService = new HomeworkService(_mockHomeworkRepository, _mockLogger);
        await homeworkService.Create(new Models.HomeworkModel
        {
            CourseId = hw.CourseId,
            Description = hw.Description,
            EndDate = hw.EndDate,
            StartDate = hw.StartDate,
            Title = hw.Title,
            Url = hw.Url,
            Id = hw.Id
        });

        await _mockHomeworkRepository.Received(1).GetByIdAsync(Arg.Any<Guid>());
        await _mockHomeworkRepository.Received(1).AddAsync(hw);
    }
}

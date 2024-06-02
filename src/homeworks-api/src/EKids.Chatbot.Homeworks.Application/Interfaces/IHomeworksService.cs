using EKids.Chatbot.Homeworks.Application.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EKids.Chatbot.Homeworks.Application.Interfaces;

public interface IHomeworksService
{
    Task<IEnumerable<HomeworkModel>> GetHomeworkList();
    Task<HomeworkModel?> GetHomeworkById(Guid homeworkId);
    Task<HomeworkModel> Create(HomeworkModel homeworkModel);
    Task Update(HomeworkModel homeworkModel);
    Task Delete(HomeworkModel homeworkModel);
}

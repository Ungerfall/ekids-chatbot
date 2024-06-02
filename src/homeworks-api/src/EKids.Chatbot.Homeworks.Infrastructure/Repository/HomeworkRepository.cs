using EKids.Chatbot.Homeworks.Core.Entities;
using EKids.Chatbot.Homeworks.Infrastructure.Data;
using EKids.Chatbot.Homeworks.Infrastructure.Repository.Base;

namespace EKids.Chatbot.Homeworks.Infrastructure.Repository;

public class HomeworkRepository(HomeworkContext dbContext) : Repository<Homework>(dbContext)
{
}

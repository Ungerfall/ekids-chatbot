using EKids.Chatbot.Tasks.DataAccessLayer;
using EKids.Chatbot.Tasks.DataAccessLayer.Entities;

namespace EKids.Chatbot.Tasks.BusinessLayer;
public class LearningTaskService(ILearningTaskRepository repo) : ILearningTaskService
{
    public Task AddTask(LearningTask task, Guid courseId, CancellationToken cancellation)
    {
        return repo.Save([task], cancellation);
    }

    public Task DeleteTask(Guid courseId, Guid taskId, CancellationToken cancellation)
    {
        return repo.DeleteById(courseId, taskId, cancellation);
    }

    public Task<LearningTask?> FindTask(Guid courseId, Guid taskId, CancellationToken cancellation)
    {
        return repo.Find(courseId, taskId, cancellation);
    }

    public IAsyncEnumerable<LearningTask> ListTasks(Guid? courseId, CancellationToken cancellation)
    {
        return repo.FindAll(courseId, cancellation);
    }
}

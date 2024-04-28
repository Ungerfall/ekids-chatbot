using EKids.Chatbot.Tasks.DataAccessLayer.Entities;

namespace EKids.Chatbot.Tasks.BusinessLayer;
public interface ILearningTaskService
{
    Task AddTask(LearningTask task, Guid courseId, CancellationToken cancellation);
    Task DeleteTask(Guid courseId, Guid taskId, CancellationToken cancellation);
    Task<LearningTask> FindTask(Guid courseId, Guid taskId, CancellationToken cancellation);
    IAsyncEnumerable<LearningTask> ListTasks(Guid? courseId, CancellationToken cancellation);
}

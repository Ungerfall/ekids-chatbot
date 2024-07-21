using EKids.Chatbot.Tasks.DataAccessLayer.Entities;

namespace EKids.Chatbot.Tasks.BusinessLayer;
/// <summary>
/// Defines business layer of CRUD methods for managing learning tasks associated with courses.
/// </summary>
public interface ILearningTaskService
{
    Task AddTask(LearningTask task, Guid courseId, CancellationToken cancellation);
    Task DeleteTask(Guid courseId, Guid taskId, CancellationToken cancellation);
    Task<LearningTask?> FindTask(Guid courseId, Guid taskId, CancellationToken cancellation);
    IAsyncEnumerable<LearningTask> ListTasks(Guid? courseId, CancellationToken cancellation);
}

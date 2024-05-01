using EKids.Chatbot.Tasks.DataAccessLayer.Entities;

namespace EKids.Chatbot.Tasks.DataAccessLayer;
public interface ILearningTaskRepository
{
    Task Save(IEnumerable<LearningTask> tasks, CancellationToken cancellation);
    Task<LearningTask?> Find(Guid courseId, Guid taskId, CancellationToken cancellation);
    Task DeleteById(Guid courseId, Guid taskId, CancellationToken cancellation);
    IAsyncEnumerable<LearningTask> FindAll(Guid? courseId, CancellationToken cancellation);
}

namespace EKids.Chatbot.Tasks.DataAccessLayer;
public interface ILearningTaskRepository<ID>
{
    IAsyncEnumerable<LearningTask> Save(IEnumerable<LearningTask> tasks, CancellationToken cancellation);
    Task<LearningTask> FindById(ID id, CancellationToken cancellation);
    Task DeleteById(ID id, CancellationToken cancellation);
    IAsyncEnumerable<LearningTask> FindAll(CancellationToken cancellation);
}

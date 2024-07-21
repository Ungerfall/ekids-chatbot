using EKids.Chatbot.Tasks.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EKids.Chatbot.Tasks.DataAccessLayer;
/// <summary>
/// Defines data access layer of CRUD methods for managing learning tasks associated with courses.
/// </summary>
public interface ILearningTaskRepository
{
    Task Save(IEnumerable<LearningTask> tasks, CancellationToken cancellation);
    Task<LearningTask?> Find(Guid courseId, Guid taskId, CancellationToken cancellation);
    Task DeleteById(Guid courseId, Guid taskId, CancellationToken cancellation);
    IAsyncEnumerable<LearningTask> FindAll(Guid? courseId, CancellationToken cancellation);
}

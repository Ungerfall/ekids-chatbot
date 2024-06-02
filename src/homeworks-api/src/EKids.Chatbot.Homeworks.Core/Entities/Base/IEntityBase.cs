namespace EKids.Chatbot.Homeworks.Core.Entities.Base;

public interface IEntityBase<TId>
{
    TId Id { get; }
}

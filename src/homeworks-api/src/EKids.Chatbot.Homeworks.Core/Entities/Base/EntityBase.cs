namespace EKids.Chatbot.Homeworks.Core.Entities.Base;

public abstract class EntityBase<TId>(TId id) : IEntityBase<TId> where TId : notnull
{
    public virtual TId Id { get; protected set; } = id;

    int? _requestedHashCode;

    public bool IsTransient()
    {
        return Id.Equals(default(TId));
    }

    public override bool Equals(object? obj)
    {
        if (obj is null || obj is not EntityBase<TId>)
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        if (GetType() != obj.GetType())
            return false;

        var item = (EntityBase<TId>)obj;

        return !item.IsTransient() && !IsTransient() && item == this;
    }

    public override int GetHashCode()
    {
        if (!IsTransient())
        {
            if (!_requestedHashCode.HasValue)
                _requestedHashCode = Id.GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

            return _requestedHashCode.Value;
        }
        else
        {
            return base.GetHashCode();
        }
    }

    public static bool operator ==(EntityBase<TId> left, EntityBase<TId> right)
    {
        return Equals(left, null) ? Equals(right, null) : left.Equals(right);
    }

    public static bool operator !=(EntityBase<TId> left, EntityBase<TId> right)
    {
        return !(left == right);
    }
}

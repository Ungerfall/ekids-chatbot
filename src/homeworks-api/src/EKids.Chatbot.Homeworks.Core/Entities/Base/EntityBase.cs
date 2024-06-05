using System;

namespace EKids.Chatbot.Homeworks.Core.Entities.Base;

public abstract class EntityBase<TId>(TId id) : IEquatable<EntityBase<TId>>, IEntityBase<TId> where TId : notnull, IEquatable<TId>
{
    public virtual TId Id { get; protected set; } = id;

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (GetType() != obj.GetType())
        {
            return false;
        }

        if (obj is not EntityBase<TId> entity)
        {
            return false;
        }

        return Id.Equals(entity.Id);
    }

    public bool Equals(EntityBase<TId>? other)
    {
        if (other is null)
        {
            return false;
        }

        if (other.GetType() != GetType())
        {
            return false;
        }

        return Id.Equals(other.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)
    }

    public static bool operator ==(EntityBase<TId> left, EntityBase<TId> right)
    {
        return left is not null && right is not null && left.Equals(right);
    }

    public static bool operator !=(EntityBase<TId> left, EntityBase<TId> right)
    {
        return !(left == right);
    }
}

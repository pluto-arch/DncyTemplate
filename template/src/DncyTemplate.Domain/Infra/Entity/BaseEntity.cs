using System.Diagnostics.CodeAnalysis;

namespace DncyTemplate.Domain.Infra;

public abstract class BaseEntity : IEntity
{
    public abstract object[] GetKeys();

    /// <inheritdoc />
    public override string ToString()
    {
        return $"[ENTITY: {GetType().Name}] Keys = {string.Join(",", GetKeys())}";
    }

    public virtual bool EntityEquals(IEntity other)
    {
        return EntityHelper.EntityEquals(this, other);
    }
}

public abstract class BaseEntity<TKey> : BaseEntity, IEntity<TKey>
{
    [AllowNull] public TKey Id { get; set; }

    public override object[] GetKeys()
    {
        return [Id];
    }
}
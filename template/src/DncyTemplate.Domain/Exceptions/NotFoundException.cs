﻿namespace DncyTemplate.Domain.Exceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(Type entityType)
    {
        EntityType = entityType;
    }

    public Type EntityType { get; set; }

    public override string ToString()
    {
        return $"There is no such an entity given given id. Entity type: {EntityType.FullName}";
    }
}
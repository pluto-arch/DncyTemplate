namespace DncyTemplate.Domain.Exceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string id)
    {
        Id = id;
    }

    public string Id { get; set; }

    public override string ToString()
    {
        return $"There is no such an entity given given id: `{Id}`";
    }
}
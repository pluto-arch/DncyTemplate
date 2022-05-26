namespace DncyTemplate.Domain.UnitOfWork;

public class UnitOfWorkCollectionOptions
{
    public Dictionary<string, Type> DbContexts { get; set; } = new();
}
namespace DncyTemplate.Domain.Infra;

public interface ISoftDelete
{
    bool Deleted { get; set; }
}
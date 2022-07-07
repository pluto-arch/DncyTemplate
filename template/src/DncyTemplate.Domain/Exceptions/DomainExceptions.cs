namespace DncyTemplate.Domain.Exceptions;

public class DomainExceptions : Exception
{
    public DomainExceptions()
    {
    }

    public DomainExceptions(string message)
        : base(message)
    {
    }

    public DomainExceptions(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}


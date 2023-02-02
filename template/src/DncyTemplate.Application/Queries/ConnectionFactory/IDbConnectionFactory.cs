using System.Data;

namespace DncyTemplate.Application.AppServices.Queries.ConnectionFactory
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}

using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace DncyTemplate.Application.AppServices.Queries.ConnectionFactory
{
    [Injectable(InjectLifeTime.Singleton)]
    public class DbConnectionFactory : IDbConnectionFactory
    {
        static DbConnectionFactory() => DbProviderFactories.RegisterFactory("System.Data.SqlClient", SqlClientFactory.Instance);

        public IDbConnection CreateConnection()
        {
            DbConnection dbConnection = DbProviderFactories.GetFactory("System.Data.SqlClient").CreateConnection();
            if (dbConnection is null)
            {
                throw new ArgumentException("Unable to find the requested database provider. It may not be installed.");
            }
            return dbConnection;
        }
    }
}

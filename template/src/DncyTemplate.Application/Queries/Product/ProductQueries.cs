#if Tenant
using Dotnetydd.MultiTenancy;
#endif
using DncyTemplate.Constants;
using DncyTemplate.Application.AppServices.Queries.ConnectionFactory;
using DncyTemplate.Application.Models.Product;
using System.Data;
using DncyTemplate.Domain.Infra;

namespace DncyTemplate.Application.Queries.Product
{

    [Injectable(InjectLifeTime.Transient)]
    public class ProductQueries : IProductQueries
    {
        private readonly IDbConnectionFactory _factory;
        private readonly IConfiguration _configuration;

#if Tenant

        private readonly IConnectionStringResolver _connectionStringResolver;
#endif

        public ProductQueries(IDbConnectionFactory factory,IConfiguration configuration
#if Tenant
            , IConnectionStringResolver connectionStringResolver
#endif
            )
        {
            _factory = factory;
            _configuration = configuration;
#if Tenant
            _connectionStringResolver = connectionStringResolver;
#endif
        }

        public async Task<ProductDto> GetAsync(string id)
        {
            using IDbConnection connection = _factory.CreateConnection();
#if Tenant
            connection.ConnectionString = await _connectionStringResolver.GetAsync(InfraConstantValue.DEFAULT_CONNECTIONSTRING_NAME);
#else
            connection.ConnectionString = _configuration.GetConnectionString(InfraConstantValue.DEFAULT_CONNECTIONSTRING_NAME);
#endif
            connection.Open();
            await Task.Delay(1000);
            // TODO use dapper 等轻量查询工具
            return null;

        }
    }
}

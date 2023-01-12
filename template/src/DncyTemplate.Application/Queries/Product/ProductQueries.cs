using Dncy.MultiTenancy.ConnectionStrings;
using DncyTemplate.Application.AppServices.Queries.ConnectionFactory;
using DncyTemplate.Application.Models.Product;
using DncyTemplate.Infra.Constants;
using System.Data;

namespace DncyTemplate.Application.Queries.Product
{

    [Injectable(InjectLifeTime.Transient)]
    public class ProductQueries : IProductQueries
    {
        private readonly IDbConnectionFactory _factory;
        private readonly IConnectionStringResolver _connectionStringResolver;

        public ProductQueries(IDbConnectionFactory factory, IConnectionStringResolver connectionStringResolver)
        {
            _factory = factory;
            _connectionStringResolver = connectionStringResolver;
        }

        public async Task<ProductDto> GetAsync(string id)
        {
            using IDbConnection connection = _factory.CreateConnection();
            connection.ConnectionString = await _connectionStringResolver.GetAsync(DbConstants.DEFAULT_CONNECTIONSTRING_NAME);
            connection.Open();
            await Task.Delay(1000);
            // TODO use dapper 等轻量查询工具
            return null;

        }
    }
}

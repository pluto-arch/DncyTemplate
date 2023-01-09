using DncyTemplate.Infra.EntityFrameworkCore.ConnectionStringResolve;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace DncyTemplate.Infra.EntityFrameworkCore.Interceptor;

public class TenantDbConnectionInterceptor : DbConnectionInterceptor
{
    private readonly string _connectionStringName;
    private readonly IConnectionStringResolve _connectionStringProvider;

    public TenantDbConnectionInterceptor(IConnectionStringResolve connectionStringProvider, string connName)
    {
        _connectionStringProvider = connectionStringProvider;
        _connectionStringName = connName;
    }

    public override InterceptionResult ConnectionOpening(DbConnection connection, ConnectionEventData eventData,
        InterceptionResult result)
    {
        connection.ConnectionString = _connectionStringProvider.GetAsync(_connectionStringName).GetAwaiter().GetResult();
        return base.ConnectionOpening(connection, eventData, result);
    }

    public override async ValueTask<InterceptionResult> ConnectionOpeningAsync(DbConnection connection,
        ConnectionEventData eventData, InterceptionResult result, CancellationToken cancellationToken = default)
    {
        connection.ConnectionString = await _connectionStringProvider.GetAsync(_connectionStringName);
        return await base.ConnectionOpeningAsync(connection, eventData, result, cancellationToken);
    }
}
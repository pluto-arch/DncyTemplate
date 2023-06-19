using Dapper;
using DncyTemplate.Infra;
using System.Data;

namespace DncyTemplate.Uow;

public static class DbContextExtension
{

    /// <summary>
    /// dapper查询，返回结果集
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="context"></param>
    /// <param name="sql"></param>
    /// <param name="parameters"></param>
    /// <param name="timeout"></param>
    /// <param name="commandType"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<IEnumerable<T>> QueryFromSqlAsync<T>(
        this IDataContext context,
        string sql,
        DynamicParameters parameters,
        int? timeout = null,
        CommandType commandType = CommandType.Text,
        CancellationToken cancellationToken = default)
    {

        var cmd = CreateCommandDefinition(context, sql, parameters, timeout, commandType, cancellationToken);
        var connection = context.GetDbConnection();
        return await connection.QueryAsync<T>(cmd);
    }

    /// <summary>
    /// dapper查询，返回第一行
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="context"></param>
    /// <param name="sql"></param>
    /// <param name="parameters"></param>
    /// <param name="timeout"></param>
    /// <param name="commandType"></param>
    /// <param name="cancellationToken"></param>
    /// <see href="https://www.learndapper.com/dapper-query/selecting-single-rowsv"/>
    /// <exception cref="InvalidOperationException"/> , 当查询返回多个元素时
    /// <returns></returns>
    public static async Task<T> SingleOrDefaultFromSqlAsync<T>(
        this IDataContext context,
        string sql,
        DynamicParameters parameters,
        int? timeout = null,
        CommandType commandType = CommandType.Text,
        CancellationToken cancellationToken = default)
    {

        var cmd = CreateCommandDefinition(context, sql, parameters, timeout, commandType, cancellationToken);
        var connection = context.GetDbConnection();
        return await connection.QuerySingleOrDefaultAsync<T>(cmd);
    }


    /// <summary>
    /// dapper查询，返回第一行
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="context"></param>
    /// <param name="sql"></param>
    /// <param name="parameters"></param>
    /// <param name="timeout"></param>
    /// <param name="commandType"></param>
    /// <param name="cancellationToken"></param>
    /// <see href="https://www.learndapper.com/dapper-query/selecting-single-rowsv"/>
    /// <returns></returns>
    public static async Task<T> FirstOrDefaultFromSqlAsync<T>(
        this IDataContext context,
        string sql,
        DynamicParameters parameters,
        int? timeout = null,
        CommandType commandType = CommandType.Text,
        CancellationToken cancellationToken = default)
    {
        var cmd = CreateCommandDefinition(context, sql, parameters, timeout, commandType, cancellationToken);
        var connection = context.GetDbConnection();
        return await connection.QueryFirstOrDefaultAsync<T>(cmd);
    }




    /// <summary>
    /// Dapper执行，返回受影响行数
    /// </summary>
    /// <param name="context"></param>
    /// <param name="sql"></param>
    /// <param name="parameters"></param>
    /// <param name="timeout"></param>
    /// <param name="commandType"></param>
    /// <param name="cancellationToken"></param>
    /// <remarks>建议配合dbcontext begin transaction来调用</remarks>
    /// <returns></returns>
    public static async Task<int> ExecuteFromSqlAsync(
        this IDataContext context,
        string sql,
        DynamicParameters parameters,
        int? timeout = null,
        CommandType commandType = CommandType.Text,
        CancellationToken cancellationToken = default)
    {
        var cmd = CreateCommandDefinition(context, sql, parameters, timeout, commandType, cancellationToken);
        var connection = context.GetDbConnection();
        return await connection.ExecuteAsync(cmd);
    }



    /// <summary>
    /// Dapper执行，返回第一行第一列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="context"></param>
    /// <param name="sql"></param>
    /// <param name="parameters"></param>
    /// <param name="timeout"></param>
    /// <param name="commandType"></param>
    /// <param name="cancellationToken"></param>
    /// <remarks>建议配合dbcontext begin transaction来调用</remarks>
    /// <returns></returns>
    public static async Task<T> ExecuteScalarFromSqlAsync<T>(
        this IDataContext context,
        string sql,
        DynamicParameters parameters,
        int? timeout = null,
        CommandType commandType = CommandType.Text,
        CancellationToken cancellationToken = default)
    {
        var cmd = CreateCommandDefinition(context, sql, parameters, timeout, commandType, cancellationToken);
        var connection = context.GetDbConnection();
        return await connection.ExecuteScalarAsync<T>(cmd);
    }


    /// <summary>
    /// dapper插入实体
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="context"></param>
    /// <param name="entity"></param>
    /// <param name="timeout"></param>
    /// <returns></returns>
    public static async Task<int?> DapperInsertAsync<TEntity>(
        this IDataContext context,
        TEntity entity,
        int? timeout = null)
    {
        var connection = context.GetDbConnection();
        var tran = context.GetDbTransaction();
        var commandTimeout = timeout ?? context.GetCommandTimeout() ?? 30;
        return await connection.InsertAsync(entity, tran, commandTimeout);
    }


    /// <summary>
    /// dapper插入实体
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="context"></param>
    /// <param name="entity"></param>
    /// <param name="timeout"></param>
    /// <returns></returns>
    public static async Task<int?> DapperUpdateAsync<TEntity>(
        this IDataContext context,
        TEntity entity,
        int? timeout = null)
    {
        var connection = context.GetDbConnection();
        var tran = context.GetDbTransaction();
        var commandTimeout = timeout ?? context.GetCommandTimeout() ?? 30;
        return await connection.UpdateAsync(entity, tran, commandTimeout);
    }


    /// <summary>
    /// dapper插入实体
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="context"></param>
    /// <param name="entity"></param>
    /// <param name="timeout"></param>
    /// <returns></returns>
    public static async Task<int?> DapperDeleteAsync<TEntity>(
        this IDataContext context,
        TEntity entity,
        int? timeout = null)
    {
        var connection = context.GetDbConnection();
        var tran = context.GetDbTransaction();
        var commandTimeout = timeout ?? context.GetCommandTimeout() ?? 30;
        return await connection.DeleteAsync(entity, tran, commandTimeout);
    }

    private static CommandDefinition CreateCommandDefinition(
        IDataContext context,
        string sql,
        DynamicParameters parameters,
        int? timeout = null,
        CommandType commandType = CommandType.Text,
        CancellationToken cancellationToken = default)
    {
        var transaction = context.GetDbTransaction();
        var commandTimeout = timeout ?? context.GetCommandTimeout() ?? 30;
        var _logger = context.GetLogger<CommandDefinition>();
        var cmd = new CommandDefinition(
            sql,
            parameters,
            transaction,
            commandTimeout,
            commandType,
            cancellationToken: cancellationToken
        );
        _logger.LogDapperDbCommand(cmd.CommandType, cmd.CommandTimeout, cmd.CommandText);
        return cmd;
    }



    public static IDbTransaction GetDbTransaction(this IDataContext context)
    {
        return context.DbTransaction;
    }

    public static IDbConnection GetDbConnection(this IDataContext context)
    {
        return context.DbConnection;
    }


    public static int? GetCommandTimeout(this IDataContext context)
    {
        return context.CommandTimeOut;
    }
}
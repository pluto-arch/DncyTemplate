namespace DncyTemplate.Infra.EntityFrameworkCore.ConnectionStringResolve;

public interface IConnectionStringResolve
{
    /// <summary>
    ///     获取对应名称的连接字符串
    /// </summary>
    /// <param name="connectionStringName"></param>
    /// <returns></returns>
    Task<string> GetAsync(string connectionStringName = null);
}
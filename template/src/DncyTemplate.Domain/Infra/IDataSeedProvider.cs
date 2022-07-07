namespace DncyTemplate.Domain.Infra;


/// <summary>
/// 种子数据提供者契约类
/// </summary>
public interface IDataSeedProvider
{
    /// <summary>
    /// 种子数据的初始化顺序
    /// </summary>
    int Sorts { get; }

    Task SeedAsync(IServiceProvider serviceProvider);
}
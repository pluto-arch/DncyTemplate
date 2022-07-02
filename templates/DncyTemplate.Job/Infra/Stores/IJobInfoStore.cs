using DncyTemplate.Job.Models;

using Quartz;

namespace DncyTemplate.Job.Infra.Stores;

public interface IJobInfoStore
{
    /// <summary>
    ///     总数
    /// </summary>
    /// <returns></returns>
    public Task<int> CountAsync();

    /// <summary>
    ///     获取job 列表
    /// </summary>
    /// <returns></returns>
    public Task<List<JobInfoModel>> GetListAsync();


    /// <summary>
    ///     获取job 列表
    /// </summary>
    /// <returns></returns>
    public Task<JobInfoModel> GetAsync(string id);


    /// <summary>
    ///     获取job 列表
    /// </summary>
    /// <returns></returns>
    public Task<JobInfoModel> GetAsync(JobKey job);


    /// <summary>
    ///     添加job
    /// </summary>
    /// <returns></returns>
    public Task AddAsync(JobInfoModel job);


    /// <summary>
    ///     添加job
    /// </summary>
    /// <returns></returns>
    public Task UpdateAsync(JobInfoModel job);


    /// <summary>
    ///     移除job
    /// </summary>
    /// <returns></returns>
    public Task RemoveAsync(string groupName, string jobName);


    /// <summary>
    ///     暂停
    /// </summary>
    /// <returns></returns>
    public Task PauseAsync(string groupName, string jobName);
}
using DncyTemplate.Job.Models;

using Quartz;

namespace DncyTemplate.Job.Infra.Stores;

public interface IJobLogStore
{
    /// <summary>
    ///     记录日志
    /// </summary>
    /// <returns></returns>
    Task RecordAsync(JobKey job, JobLogModel model);

    /// <summary>
    ///     获取日志
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    Task<List<JobLogModel>> GetListAsync(JobKey job, int count = 20);
}
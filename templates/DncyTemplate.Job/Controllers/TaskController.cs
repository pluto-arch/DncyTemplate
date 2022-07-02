using DncyTemplate.Job.Infra.Stores;
using DncyTemplate.Job.Jobs;
using DncyTemplate.Job.Models;

using Quartz;
using Quartz.Impl.Triggers;
using Quartz.Spi;
namespace DncyTemplate.Job.Controllers;

/// <summary>
///     后台作业
/// </summary>
[AutoResolveDependency]
public partial class TaskController : Controller
{
    [AutoInject]
    private readonly IJobInfoStore _jobInfoStore;
    [AutoInject]
    private readonly IJobLogStore _jobLogStore;
    [AutoInject]
    private readonly ISchedulerFactory _jobSchedularFactory;
    [AutoInject]
    private readonly ILogger<TaskController> _logger;
    [AutoInject]
    private readonly IJobFactory _jobFactory;


    public IActionResult Index()
    {
        return View();
    }


    /// <summary>
    ///     获取所有job
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> TasksAsync()
    {
        var jobs = await _jobInfoStore.GetListAsync();
        jobs = jobs?.OrderBy(x => x.Id)?.ToList();
        IScheduler scheduler = await _jobSchedularFactory.GetScheduler();
        foreach (JobInfoModel job in jobs ?? new List<JobInfoModel>())
        {
            IReadOnlyCollection<ITrigger> triggers =
                await scheduler.GetTriggersOfJob(JobKey.Create(job.TaskName, job.GroupName));
            foreach (ITrigger trigger in triggers)
            {
                DateTimeOffset? dateTimeOffset = trigger.GetPreviousFireTimeUtc();
                if (!dateTimeOffset.HasValue)
                {
                    continue;
                }
                job.TriggerName = trigger.Key.Name;
                job.LastRunTime = $"{dateTimeOffset.Value.LocalDateTime:yyyy-MM-dd HH:mm:ss}";
                job.TriggerStatus = await scheduler.GetTriggerState(trigger.Key);
            }
        }

        return Json(new { code = 0, data = jobs, msg = "加载成功", count = jobs?.Count ?? 0 });
    }

    /// <summary>
    ///     暂停任务
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> PauseTaskAsync(PauseTaskRequest request)
    {
        JobInfoModel job = await _jobInfoStore.GetAsync(request.Id);
        if (job == null)
        {
            return Json(new { code = -1, msg = "job不存在" });
        }

        IScheduler scheduler = await _jobSchedularFactory.GetScheduler();
        JobKey jk = JobKey.Create(job.TaskName, job.GroupName);
        await scheduler.PauseJob(jk);
        job.Status = EnumJobStates.Pause;
        await _jobInfoStore.UpdateAsync(job);
        await _jobLogStore.RecordAsync(jk,
            new JobLogModel
            {
                Time = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                RunSeconds = 0,
                State = EnumJobStates.Pause,
                Message = "暂停指令发送成功"
            });
        return Json(new { code = 0, msg = "操作成功" });
    }

    /// <summary>
    ///     重启任务
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> RefireAsync(PauseTaskRequest request)
    {
        JobInfoModel job = await _jobInfoStore.GetAsync(request.Id);
        if (job == null)
        {
            return Json(new { code = -1, msg = "job不存在" });
        }

        IScheduler scheduler = await _jobSchedularFactory.GetScheduler();
        JobKey jk = JobKey.Create(job.TaskName, job.GroupName);
        await _jobLogStore.RecordAsync(jk,
            new JobLogModel
            {
                Time = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                RunSeconds = 0,
                State = EnumJobStates.Normal,
                Message = "重启指令发送成功"
            });
        await scheduler.ResumeJob(jk);
        job.Status = EnumJobStates.Normal;
        await _jobInfoStore.UpdateAsync(job);
        return Json(new { code = 0, msg = "操作成功" });
    }


    /// <summary>
    ///     立即执行
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> ExecuteAsync(string id)
    {
        JobInfoModel job = await _jobInfoStore.GetAsync(id);
        if (job == null)
        {
            return Json(new { code = -1, msg = "job不存在" });
        }

        JobKey jobKey = JobKey.Create(job.TaskName, job.GroupName);
        IScheduler scheduler = await _jobSchedularFactory.GetScheduler();
        await scheduler.TriggerJob(jobKey);
        await _jobLogStore.RecordAsync(jobKey,
            new JobLogModel
            {
                Time = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                RunSeconds = 0,
                State = EnumJobStates.Normal,
                Message = "发送立即执行指令成功"
            });
        return Json(new { code = 0, msg = "操作成功" });
    }

    /// <summary>
    ///     作业日志
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> JobLogsAsync(string id)
    {
        JobInfoModel job = await _jobInfoStore.GetAsync(id);
        if (job == null)
        {
            return Json(new { code = -1, msg = "job不存在" });
        }

        JobKey jk = JobKey.Create(job.TaskName, job.GroupName);
        List<JobLogModel> logs = await _jobLogStore.GetListAsync(jk);
        return Json(new { code = 0, msg = "操作成功", data = logs });
    }


    /// <summary>
    ///     作业日志
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> AddAsync(CreateJobModel request)
    {
        var job = new JobInfoModel
        {
            Id = Guid.NewGuid().ToString("N"),
            TaskType = EnumTaskType.DynamicExecute,
            TaskName = request.Name,
            DisplayName = request.DisplayName,
            GroupName = request.GroupName,
            Interval = request.Interval,
            Describe = request.Desc,
            Status = EnumJobStates.Stopped,
            ApiUrl = request.CallUrl
        };
        await _jobInfoStore.AddAsync(job);
        var res = await AddNewJob(request);
        if (!res)
        {
            return Json(new { code = -1, msg = "创建失败" });
        }

        job.Status = EnumJobStates.Normal;
        await _jobInfoStore.UpdateAsync(job);
        return Json(new { code = 0, msg = "操作成功", data = job.Id });
    }


    #region private

    private async Task<bool> AddNewJob(CreateJobModel model)
    {
        try
        {
            var (success, _) = IsValidExpression(model.Interval);
            if (!success)
            {
                return false;
            }
            IJobDetail job = JobBuilder.Create<HttpResultfulJob>()
                .WithIdentity(model.Name, model.GroupName)
                .Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity(model.Name, model.GroupName)
                .StartNow()
                .WithDescription(model.Desc)
                .WithCronSchedule(model.Interval)
                .Build();
            IScheduler scheduler = await _jobSchedularFactory.GetScheduler();

            scheduler.JobFactory = _jobFactory;

            await scheduler.ScheduleJob(job, trigger);
            await scheduler.Start();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "message: {msg}", e.Message);
            return false;
        }
    }


    public static (bool, string) IsValidExpression(string cronExpression)
    {
        try
        {
            CronTriggerImpl trigger = new()
            {
                CronExpressionString = cronExpression
            };
            DateTimeOffset? date = trigger.ComputeFirstFireTimeUtc(null);
            return (date != null, date == null ? $"请确认表达式{cronExpression}是否正确!" : "");
        }
        catch (Exception e)
        {
            return (false, $"请确认表达式{cronExpression}是否正确!{e.Message}");
        }
    }

    #endregion

}
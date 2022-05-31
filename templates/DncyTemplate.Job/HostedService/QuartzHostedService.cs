using DncyTemplate.Job.Infra;
using DncyTemplate.Job.Infra.Listenings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl.Matchers;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DncyTemplate.Job.Infra.Stores;
using DncyTemplate.Job.Jobs;
using DncyTemplate.Job.Models;

namespace DncyTemplate.Job.HostedService;

public class QuartzHostedService : IHostedService
{
    private readonly JobDefined _jobDefined;
    private readonly IJobFactory _jobFactory;
    private readonly IJobInfoStore _jobStore;

    private readonly ISchedulerFactory _schedulerFactory;
    private readonly IServiceProvider _serviceProvider;

    public QuartzHostedService(ISchedulerFactory schedulerFactory, IJobInfoStore jobStore,
        IJobFactory jobFactory, JobDefined jobDefined, IServiceProvider serviceProvider)
    {
        _schedulerFactory = schedulerFactory;
        _jobFactory = jobFactory;
        _jobDefined = jobDefined;
        _serviceProvider = serviceProvider;
        _jobStore = jobStore;
    }

    public IScheduler Scheduler { get; set; }

    /// <inheritdoc />
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
        Scheduler.JobFactory = _jobFactory;
        IJobListener jobListener = _serviceProvider.GetRequiredService<IJobListener>();
        ITriggerListener triggerListener = _serviceProvider.GetRequiredService<ITriggerListener>();
        Scheduler.ListenerManager.AddJobListener(jobListener ?? new NullJobListener(),
            GroupMatcher<JobKey>.AnyGroup());
        //Scheduler.ListenerManager.AddTriggerListener(triggerListener ?? new NullTriggerListener(), GroupMatcher<TriggerKey>.AnyGroup());
        Dictionary<string, Type> jobDic = _jobDefined.JobDictionary;


        var jobs = await _jobStore.GetListAsync();
        if (jobs==null||!jobs.Any())
        {
            return;
        }

        foreach (var jobInfo in jobs)
        {
            if (jobInfo.TaskType==EnumTaskType.StaticExecute)
            {
                var type = jobDic.FirstOrDefault(x => x.Key == jobInfo.TaskName).Value;
                if (type == null)
                    continue;
                IJobDetail job = JobBuilder.Create(type)
                    .WithIdentity(jobInfo.TaskName, jobInfo.GroupName)
                    .WithDescription(jobInfo.Describe)
                    .Build();
                var triggerBuilder = TriggerBuilder.Create()
                    .WithIdentity(jobInfo.TaskName, jobInfo.GroupName)
                    .WithCronSchedule(jobInfo.Interval);
                if (jobInfo.Status==EnumJobStates.Normal)
                {
                    triggerBuilder.StartNow();
                }
                await Scheduler.ScheduleJob(job, triggerBuilder.Build(), cancellationToken);
            }
            else
            {
                IJobDetail job = JobBuilder.Create<HttpResultfulJob>()
                    .WithIdentity(jobInfo.TaskName, jobInfo.GroupName)
                    .Build();
                var triggerBuilder = TriggerBuilder.Create()
                    .WithIdentity(jobInfo.TaskName, jobInfo.GroupName)
                    .WithDescription(jobInfo.Describe)
                    .WithCronSchedule(jobInfo.Interval);
                if (jobInfo.Status==EnumJobStates.Normal)
                {
                    triggerBuilder.StartNow();
                }
                await Scheduler.ScheduleJob(job, triggerBuilder.Build(), cancellationToken);
            }

        }
        await Scheduler.Start(cancellationToken);
    }

    /// <inheritdoc />
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (Scheduler != null)
        {
            await Scheduler.Shutdown(cancellationToken);
        }
    }
}



public class SingletonJobFactory : IJobFactory
{
    private readonly IServiceProvider _serviceProvider;

    public SingletonJobFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
        return _serviceProvider.GetRequiredService<QuartzJobRunner>();
    }

    public void ReturnJob(IJob job)
    {
        (job as IDisposable)?.Dispose();
    }
}
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

namespace DncyTemplate.Job.HostedService;

public class QuartzHostedService : IHostedService
{
    private readonly JobDefined _jobDefined;
    private readonly IJobFactory _jobFactory;
    private readonly IEnumerable<JobSetting> _jobs;

    private readonly ISchedulerFactory _schedulerFactory;
    private readonly IServiceProvider _serviceProvider;

    public QuartzHostedService(ISchedulerFactory schedulerFactory, IConfiguration configuration,
        IJobFactory jobFactory, JobDefined jobDefined, IServiceProvider serviceProvider)
    {
        _schedulerFactory = schedulerFactory;
        _jobFactory = jobFactory;
        _jobDefined = jobDefined;
        _serviceProvider = serviceProvider;
        _jobs = configuration.GetSection("JobSettings").Get<List<JobSetting>>();
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
        foreach (JobSetting jobInfo in _jobs)
        {
            Type type = jobDic.FirstOrDefault(x => x.Key == jobInfo.Name).Value;
            if (type == null)
            {
                continue;
            }

            if (!jobInfo.IsOpen)
            {
                continue;
            }

            IJobDetail job = JobBuilder.Create(type)
                .WithIdentity(jobInfo.Name, jobInfo.GroupName)
                .WithDescription(jobInfo.Description)
                .Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity($"{jobInfo.Name}.trigger")
                .WithCronSchedule(jobInfo.Cron)
                .StartNow()
                .Build();
            await Scheduler.ScheduleJob(job, trigger, cancellationToken);
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
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public SingletonJobFactory(IServiceScopeFactory serviceProvider)
    {
        _serviceScopeFactory = serviceProvider;
    }

    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            return scope.ServiceProvider.GetRequiredService<QuartzJobRunner>();
        }
    }

    public void ReturnJob(IJob job)
    {
        (job as IDisposable)?.Dispose();
    }
}
using DncyTemplate.Job.Infra.Stores;
using DncyTemplate.Job.Models;

using Quartz;

namespace DncyTemplate.Job.Infra.Listenings;

public class NullJobListener : IJobListener
{
    /// <inheritdoc />
    public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = new())
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = new())
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException,
        CancellationToken cancellationToken = new())
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public string Name => "CustomerJobListener";
}


public class CustomJobListener : IJobListener
{
    private readonly IJobLogStore _jobLogStore;

    public CustomJobListener(IJobLogStore jobLogStore)
    {
        _jobLogStore = jobLogStore;
    }


    /// <inheritdoc />
    public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = new())
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = new())
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = new())
    {
        JobKey job = context.JobDetail.Key;
        bool hasException = jobException != null;
        _jobLogStore.RecordAsync(job,
            new JobLogModel
            {
                Time = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                RunSeconds = context.JobRunTime.Seconds,
                State = hasException ? EnumJobStates.Exception : EnumJobStates.Normal,
                Message = jobException?.Message ?? context.Result?.ToString() ?? ""
            });
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public string Name => "CustomerJobListener";
}
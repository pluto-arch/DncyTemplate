using Quartz;

namespace DncyTemplate.Job.Infra.Listenings;

public class NullTriggerListener : ITriggerListener
{
    /// <inheritdoc />
    public Task TriggerFired(ITrigger trigger, IJobExecutionContext context,
        CancellationToken cancellationToken = new())
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task<bool> VetoJobExecution(ITrigger trigger, IJobExecutionContext context,
        CancellationToken cancellationToken = new())
    {
        return Task.FromResult(false);
    }

    /// <inheritdoc />
    public Task TriggerMisfired(ITrigger trigger, CancellationToken cancellationToken = new())
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task TriggerComplete(ITrigger trigger, IJobExecutionContext context,
        SchedulerInstruction triggerInstructionCode,
        CancellationToken cancellationToken = new())
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public string Name => "CustomTriggerListener";
}


public class CustomTriggerListener : ITriggerListener
{
    /// <summary>
    ///     job执行时调用
    /// </summary>
    /// <param name="trigger"></param>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task TriggerFired(ITrigger trigger, IJobExecutionContext context,
        CancellationToken cancellationToken = new())
    {
        return Task.CompletedTask;
    }

    /// <summary>
    ///     Trigger触发后，job执行时调用本方法。true即否决，job后面不执行。
    /// </summary>
    /// <param name="trigger"></param>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<bool> VetoJobExecution(ITrigger trigger, IJobExecutionContext context,
        CancellationToken cancellationToken = new())
    {
        return Task.FromResult(false);
    }

    /// <summary>
    ///     错过触发时调用(例：线程不够用的情况下)
    /// </summary>
    /// <param name="trigger"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task TriggerMisfired(ITrigger trigger, CancellationToken cancellationToken = new())
    {
        return Task.CompletedTask;
    }

    /// <summary>
    ///     job完成时调用
    /// </summary>
    /// <param name="trigger"></param>
    /// <param name="context"></param>
    /// <param name="triggerInstructionCode"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task TriggerComplete(ITrigger trigger, IJobExecutionContext context,
        SchedulerInstruction triggerInstructionCode,
        CancellationToken cancellationToken = new())
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public string Name => "CustomTriggerListener";
}
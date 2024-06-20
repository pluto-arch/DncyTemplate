using System.Transactions;
using DncyTemplate.Domain.Aggregates.EventLogs;
using DncyTemplate.Domain.Infra.Repository;
using DncyTemplate.Infra.EntityFrameworkCore.DbContexts;
using DncyTemplate.Uow;

namespace DncyTemplate.Application.IntegrationEvents.IntegrationEventbox;



/// <summary>
/// 发件箱
/// </summary>
[Injectable(InjectLifeTime.Transient)]
public class IntegrationEventBoxService
{
    private readonly IUnitOfWork<DncyTemplateDbContext> _uow;
    private readonly ILogger<IntegrationEventBoxService> _logger;

    public IntegrationEventBoxService(IUnitOfWork<DncyTemplateDbContext> uow, ILogger<IntegrationEventBoxService> logger)
    {
        _uow = uow;
        _logger = logger;
    }


    /// <summary>
    /// 添加集成事件
    /// </summary>
    /// <param name="evt"></param>
    /// <param name="transactionId"></param>
    /// <returns></returns>
    public async ValueTask Add(IntegrationEvent evt, string transactionId = null)
    {
        var eventRep = _uow.Resolve<IEfRepository<IntegrationEventLogEntry>>();
        await eventRep.InsertAsync(new IntegrationEventLogEntry(evt, transactionId ?? Transaction.Current?.TransactionInformation?.LocalIdentifier));
    }


    /// <summary>
    /// 保存集成事件并保存业务变更
    /// </summary>
    /// <returns></returns>
    public async Task SaveEventAndChangesAsync(IntegrationEvent evt, string transactionId = null)
    {
        await Add(evt, transactionId);
        await _uow.CompleteAsync();
    }

    /// <summary>
    /// 通过事件总线发布事件
    /// </summary>
    /// <param name="evt"></param>
    /// <returns></returns>
    public async Task PublishThroughEventBusAsync(IntegrationEvent evt)
    {
        try
        {
            _logger.LogInformation("Publishing integration event: {IntegrationEventId_published} - ({@IntegrationEvent})", evt.Id, evt);
            await MarkEventAsInProgressAsync(evt.Id);
            // TODO call eventbus publish
            await MarkEventAsPublishedAsync(evt.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error Publishing integration event: {IntegrationEventId} - ({@IntegrationEvent})", evt.Id, evt);
            await MarkEventAsFailedAsync(evt.Id);
        }
    }



    public Task MarkEventAsInProgressAsync(string eventId)
    {
        return UpdateEventStatus(eventId, EventStateEnum.InProgress);
    }


    public Task MarkEventAsPublishedAsync(string eventId)
    {
        return UpdateEventStatus(eventId, EventStateEnum.Published);
    }

    public Task MarkEventAsFailedAsync(string eventId)
    {
        return UpdateEventStatus(eventId, EventStateEnum.PublishedFailed);
    }


    private Task UpdateEventStatus(string eventId, EventStateEnum status)
    {
        var eventRep = _uow.Resolve<IEfRepository<IntegrationEventLogEntry>>();
        var eventLogEntry = eventRep.FirstOrDefault(ie => ie.EventId == eventId);
        if (eventLogEntry != null)
        {
            eventLogEntry.State = status;

            if (status == EventStateEnum.InProgress)
                eventLogEntry.TimesSent++;

            return eventRep.UnitOfWork.CompleteAsync();
        }
        return Task.CompletedTask;
    }

}
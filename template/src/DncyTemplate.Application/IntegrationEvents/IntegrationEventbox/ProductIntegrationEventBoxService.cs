using System.Transactions;

namespace DncyTemplate.Application.IntegrationEvents.IntegrationEventbox;



[Injectable(InjectLifeTime.Transient)]
public partial class ProductIntegrationEventBoxService
{
    public static readonly ConcurrentBag<string> inMemoryProductBox = new ConcurrentBag<string>();

    public void AddAndSaveEvent(string @event)
    {
        Console.WriteLine(Transaction.Current?.TransactionInformation?.LocalIdentifier);
        inMemoryProductBox.Add(@event);
    }


    public ConcurrentBag<string> MemoryBox => inMemoryProductBox;
}
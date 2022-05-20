using ImportScheduledJobs.Repositories;
using MassTransit;
using TransportException = Elastic.Transport.TransportException;

namespace ImportScheduledJobs.Consumers;

public class ImportMessageConsumer : IConsumer<Batch<ImportMessage>>
{
    private readonly ILogger<ImportMessageConsumer> _logger;
    private readonly IOrderedItemIndexingRepository orderedItemIndexingRepository;

    public ImportMessageConsumer(
        ILogger<ImportMessageConsumer> logger,
        IOrderedItemIndexingRepository orderedItemIndexingRepository)
    {
        this._logger = logger;
        this.orderedItemIndexingRepository = orderedItemIndexingRepository;
    }

    public async Task Consume(ConsumeContext<Batch<ImportMessage>> context)
    {
        try
        {
            var orderedItems = context.Message
                    .Select(message => message.Message.OrderedItem)
                    .Where(item => item is not null);

            if (orderedItems.Any())
                await orderedItemIndexingRepository.IndexDocuments(orderedItems);
        }
        catch (TransportException exception)
        {
            _logger.LogError(exception, "Error occurred during indexing documents.");
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Unexpected error.");
        }
    }
}

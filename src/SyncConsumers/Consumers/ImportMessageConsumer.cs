using MassTransit;
using SharedKernel.Consumers;
using SyncConsumers.Repositories;
using TransportException = Elastic.Transport.TransportException;

namespace SyncConsumers.Consumers;

public class ImportMessageConsumer : IConsumer<Batch<ImportMessage>>
{
    private readonly ILogger<ImportMessageConsumer> _logger;
    private readonly IOrderedItemIndexingRepository _orderedItemIndexingRepository;

    public ImportMessageConsumer(
        ILogger<ImportMessageConsumer> logger,
        IOrderedItemIndexingRepository orderedItemIndexingRepository)
    {
        _logger = logger;
        _orderedItemIndexingRepository = orderedItemIndexingRepository;
    }

    public async Task Consume(ConsumeContext<Batch<ImportMessage>> context)
    {
        try
        {
            var orderedItems = context.Message
                    .Select(message => message.Message.OrderedItem)
                    .Where(item => item is not null);

            if (orderedItems.Any())
                await _orderedItemIndexingRepository.IndexDocuments(orderedItems);
        }
        catch (TransportException exception)
        {
            _logger.LogError(exception, "Error occurred while indexing documents.");
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Unexpected error.");
        }
    }
}

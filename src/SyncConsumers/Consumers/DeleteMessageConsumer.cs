using MassTransit;
using SharedKernel.Consumers;
using SyncConsumers.Repositories;

namespace SyncConsumers.Consumers;

public class DeleteMessageConsumer : IConsumer<Batch<DeleteMessage>>
{
    private readonly ILogger<DeleteMessageConsumer> _logger;
    private readonly IOrderedItemIndexingRepository _orderedItemIndexingRepository;

    public DeleteMessageConsumer(
        ILogger<DeleteMessageConsumer> logger,
        IOrderedItemIndexingRepository orderedItemIndexingRepository)
    {
        _logger = logger;
        _orderedItemIndexingRepository = orderedItemIndexingRepository;
    }

    public async Task Consume(ConsumeContext<Batch<DeleteMessage>> context)
    {
        try
        {
            var orderedItems = context.Message
                    .Select(message => message.Message.OrderedItem)
                    .Where(item => item is not null);

            if (orderedItems.Any())
                await _orderedItemIndexingRepository.DeleteDocuments(orderedItems);
        }
        catch (TransportException exception)
        {
            _logger.LogError(exception, "Error occurred while deleting documents.");
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Unexpected error.");
        }
    }
}

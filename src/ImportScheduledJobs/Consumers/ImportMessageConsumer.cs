using ImportScheduledJobs.QueryObjects.Models;
using MassTransit;
using Nest;

namespace ImportScheduledJobs.Consumers;

public class ImportMessageConsumer : IConsumer<Batch<ImportMessage>>
{
    private readonly ILogger<ImportMessageConsumer> _logger;
    private readonly IElasticClient _elasticClient;

    public ImportMessageConsumer(
        ILogger<ImportMessageConsumer> logger,
        IElasticClient elasticClient)
    {
        this._logger = logger;
        this._elasticClient = elasticClient;
    }

    public async Task Consume(ConsumeContext<Batch<ImportMessage>> context)
    {
        var messages = context.Message.Select(message => message.Message.OrderedItem);
        var indexManyResponse = await _elasticClient.IndexManyAsync<OrderedItem>(messages);

        if (indexManyResponse.Errors)
            foreach (var itemWithError in indexManyResponse.ItemsWithErrors)
                _logger.LogError($"Failed to index document {itemWithError.Id}: {itemWithError.Error}");
    }
}

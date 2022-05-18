using MassTransit;
using Nest;

namespace ImportScheduledJobs.Consumers;

public class DeleteMessageConsumer : IConsumer<Batch<DeleteMessage>>
{
    private readonly ILogger<DeleteMessageConsumer> _logger;
    private readonly IElasticClient _elasticClient;

    public DeleteMessageConsumer(
        ILogger<DeleteMessageConsumer> logger,
        IElasticClient elasticClient)
    {
        this._logger = logger;
        this._elasticClient = elasticClient;
    }

    public async Task Consume(ConsumeContext<Batch<DeleteMessage>> context)
    {
        var items = context.Message.Select(message => message.Message.OrderedItem);
        var indexManyResponse = await _elasticClient.DeleteManyAsync(items);
    }
}

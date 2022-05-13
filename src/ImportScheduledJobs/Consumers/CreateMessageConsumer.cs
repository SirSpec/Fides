using MassTransit;
using Nest;

namespace ImportScheduledJobs.Consumers;

public class CreateMessageConsumer : IConsumer<Batch<CreateMessage>>
{
    private readonly ILogger<CreateMessageConsumer> logger;
    private readonly IElasticClient elasticClient;

    public CreateMessageConsumer(
        ILogger<CreateMessageConsumer> logger,
        IElasticClient elasticClient)
    {
        this.logger = logger;
        this.elasticClient = elasticClient;
    }

    public async Task Consume(ConsumeContext<Batch<CreateMessage>> context)
    {
        var messages = context.Message.Select(message => message.Message);
        await elasticClient.IndexManyAsync<CreateMessage>(messages);
    }
}

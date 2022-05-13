using MassTransit;

namespace ImportScheduledJobs.Consumers;

public class OrderAuditConsumerDefinition : ConsumerDefinition<CreateMessageConsumer>
{
    public OrderAuditConsumerDefinition()
    {
        EndpointName = "fides";
        Endpoint(configure => configure.PrefetchCount = 1);
    }

    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<CreateMessageConsumer> consumerConfigurator)
    {
        consumerConfigurator
            .Options<BatchOptions>(options => options
            .SetMessageLimit(10)
            .SetTimeLimit(1000)
            .SetConcurrencyLimit(1));
    }
}
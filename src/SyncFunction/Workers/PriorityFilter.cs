using MassTransit;

namespace SyncFunction.Workers;

public class PriorityFilter<T> : IFilter<SendContext<T>> where T : class
{
    private readonly byte priority;

    public PriorityFilter(byte priority) =>
        this.priority = priority;

    public void Probe(ProbeContext context)
    {
    }

    public Task Send(SendContext<T> context, IPipe<SendContext<T>> next)
    {
        if (context.TryGetPayload<RabbitMqSendContext>(out var rabbitMqSendContext))
            rabbitMqSendContext.SetPriority(priority);

        return next.Send(context);
    }
}

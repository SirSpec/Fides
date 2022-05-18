using MassTransit;

namespace ImportScheduledJobs.Workers;

public class PriorityFilter<T> : IFilter<PublishContext<T>> where T : class
{
    private readonly byte priority;

    public PriorityFilter(byte priority) =>
        this.priority = priority;

    public void Probe(ProbeContext context)
    {
    }

    public Task Send(PublishContext<T> context, IPipe<PublishContext<T>> next)
    {
        if (context.TryGetPayload<RabbitMqSendContext>(out var rabbitMqSendContext))
            rabbitMqSendContext.SetPriority(priority);

        return next.Send(context);
    }
}

using ImportScheduledJobs.Entities;

namespace ImportScheduledJobs.Mappers;

public interface IOrderItemMapper
{
    public OrderedItem Map(OrderItem orderItem);
}

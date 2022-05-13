using ImportScheduledJobs.Mappers;

namespace ImportScheduledJobs.Consumers;

public class CreateMessage
{
    public OrderedItem OrderedItem { get; set; }

    public CreateMessage(OrderedItem orderedItem)
    {
        OrderedItem = orderedItem;
    }
}

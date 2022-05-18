using ImportScheduledJobs.Mappers;

namespace ImportScheduledJobs.Consumers;

public record DeleteMessage(OrderedItem OrderedItem);

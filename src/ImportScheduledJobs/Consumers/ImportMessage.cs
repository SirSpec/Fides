using ImportScheduledJobs.Mappers;

namespace ImportScheduledJobs.Consumers;

public record ImportMessage(OrderedItem OrderedItem);

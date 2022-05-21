using ImportScheduledJobs.QueryObjects.Models;

namespace ImportScheduledJobs.Consumers;

public record ImportMessage(OrderedItem OrderedItem);

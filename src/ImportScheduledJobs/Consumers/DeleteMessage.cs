using ImportScheduledJobs.QueryObjects.Models;

namespace ImportScheduledJobs.Consumers;

public record DeleteMessage(OrderedItem OrderedItem);

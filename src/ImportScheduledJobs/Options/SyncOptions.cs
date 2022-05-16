namespace ImportScheduledJobs.Options;

public class SyncOptions
{
    public int ImportPageSize { get; set; }
    public string? QueueName { get; set; }

    public Uri GetQueueUri() =>
        new Uri($"queue:{QueueName ?? throw new ArgumentNullException($"{nameof(QueueName)} is null.")}");
}
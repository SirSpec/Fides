namespace ImportScheduledJobs.Options;

public class MassTransitOptions
{
    public const int DefaultRetryLimit = 6;
    public const int DefaultMinIntervalInSeconds = 0;
    public const int DefaultMaxIntervalInSeconds = 60;
    public const int DefaultIntervalDeltaInSeconds = 5;

    public string? Host { get; set; }
    public string? VirtualHost { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public int? RetryLimit { get; set; }
    public int? MinIntervalInSeconds { get; set; }
    public int? MaxIntervalInSeconds { get; set; }
    public int? IntervalDeltaInSeconds { get; set; }
}
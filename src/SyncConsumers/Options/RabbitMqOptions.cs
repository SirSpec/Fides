namespace SyncConsumers.Options;

public class RabbitMqOptions
{
    public const int DefaultRetryLimit = 6;
    public const int DefaultMinIntervalInSeconds = 0;
    public const int DefaultMaxIntervalInSeconds = 60;
    public const int DefaultIntervalDeltaInSeconds = 5;
    public const int DefaultMessageLimit = 100;
    public const int DefaultPrefetchCount = DefaultMessageLimit;
    public const int DefaultConcurrencyLimit = 5;
    public const int DefaultTimeLimit = 5;
    public const byte DefaultPriorityLimit = 2;

    public string? Host { get; set; }
    public string? VirtualHost { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? Endpoint { get; set; }
    public int? RetryLimit { get; set; }
    public int? MinIntervalInSeconds { get; set; }
    public int? MaxIntervalInSeconds { get; set; }
    public int? IntervalDeltaInSeconds { get; set; }
    public int? MessageLimit { get; set; }
    public int? PrefetchCount { get; set; }
    public int? ConcurrencyLimit { get; set; }
    public int? TimeLimit { get; set; }
    public byte? PriorityLimit { get; set; }
}
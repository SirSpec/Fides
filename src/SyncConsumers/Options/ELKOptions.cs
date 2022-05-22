namespace SyncConsumers.Options;

public class ELKOptions
{
    public const int DefaultMaximumRetries = 6;
    public const int DefaultMaxRetryTimeoutInSeconds = 6;

    public string? ElasticSearchUri { get; set; }
    public string? DefaultIndex { get; set; }
    public int? MaximumRetries { get; set; }
    public int? MaxRetryTimeoutInSeconds { get; set; }
}
namespace SyncFunction.Options;

public class KeyVaultOptions
{
    public const int DefaultRetryDelay = 2;
    public const int DefaultRetryMaxDelay = 30;
    public const int DefaultMaxRetries = 5;
    public const int DefaultReloadInterval = 12;

    public int? RetryDelay { get; set; }
    public int? RetryMaxDelay { get; set; }
    public int? MaxRetries { get; set; }
    public int? ReloadInterval { get; set; }
}
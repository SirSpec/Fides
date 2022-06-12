using Azure.Core;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using SyncConsumers.Constants;
using SyncConsumers.Options;

namespace Microsoft.Extensions.Configuration;

public static class ConfigurationBuilderExtensions
{
    public static void AddAzureKeyVaultConfiguration(this IConfigurationBuilder configurationBuilder)
    {
        var builtConfig = configurationBuilder.Build();
        var keyVaultOptions = builtConfig
            .GetSection(nameof(KeyVaultOptions))
            .Get<KeyVaultOptions>();

        if (builtConfig.GetSection("environment").Get<string>() == "Production")
        {
            configurationBuilder.AddAzureKeyVault(
                new Uri(builtConfig.GetConnectionString(ConnectionStrings.KeyVaultUri)),
                new DefaultAzureCredential(
                    new DefaultAzureCredentialOptions
                    {
                        Retry =
                        {
                            Delay= TimeSpan.FromSeconds(keyVaultOptions.RetryDelay ?? KeyVaultOptions.DefaultRetryDelay),
                            MaxDelay = TimeSpan.FromSeconds(keyVaultOptions.RetryMaxDelay ?? KeyVaultOptions.DefaultRetryMaxDelay),
                            MaxRetries = keyVaultOptions.MaxRetries ?? KeyVaultOptions.DefaultMaxRetries,
                            Mode = RetryMode.Exponential
                        }
                    }
                ),
                new AzureKeyVaultConfigurationOptions
                {
                    ReloadInterval = TimeSpan.FromHours(keyVaultOptions.ReloadInterval ?? KeyVaultOptions.DefaultReloadInterval)
                });
        }
    }
}

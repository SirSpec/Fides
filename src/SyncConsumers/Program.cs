using Azure.Core;
using Azure.Identity;
using NLog.Web;
using SyncConsumers.Consumers;
using SyncConsumers.Options;
using SyncConsumers.Repositories;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostBuilderContext, serviceCollection) =>
    {
        serviceCollection.Configure<SyncOptions>(
            hostBuilderContext.Configuration.GetSection(nameof(SyncOptions)));

        serviceCollection.AddMassTransitEndpoints(hostBuilderContext.Configuration);
        serviceCollection.AddElasticClient(hostBuilderContext.Configuration);

        serviceCollection.AddScoped<ImportMessageConsumer>();
        serviceCollection.AddScoped<DeleteMessageConsumer>();
        serviceCollection.AddScoped<IOrderedItemIndexingRepository, OrderedItemIndexingRepository>();
    })
    .ConfigureAppConfiguration((hostBuilderContext, configurationBuilder) =>
    {
        if (hostBuilderContext.HostingEnvironment.IsProduction())
        {
            var builtConfig = configurationBuilder.Build();

            configurationBuilder.AddAzureKeyVault(
                new Uri(builtConfig.GetConnectionString("KeyVaultUri")),
                new DefaultAzureCredential(
                    new DefaultAzureCredentialOptions
                    {
                        Retry =
                        {
                            Delay= TimeSpan.FromSeconds(2),
                            MaxDelay = TimeSpan.FromSeconds(30),
                            MaxRetries = 5,
                            Mode = RetryMode.Exponential
                        }
                    }
                ));
        }
    })
    .ConfigureLogging((hostBuilderContext, configurationBuilder) =>
    {
        var fileName = hostBuilderContext.HostingEnvironment.IsProduction()
            ? "nlog.config"
            : "nlog.development.config";

        configurationBuilder.AddNLog(fileName);
    })
    .Build();

await host.RunAsync();

using NLog.Web;
using SyncConsumers.Consumers;
using SyncConsumers.Options;
using SyncConsumers.Repositories;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostBuilderContext, serviceCollection) =>
    {
        serviceCollection.Configure<RabbitMqOptions>(
            hostBuilderContext.Configuration.GetSection(nameof(RabbitMqOptions)));

        serviceCollection.AddMassTransitEndpoints(hostBuilderContext.Configuration);
        serviceCollection.AddElasticClient(hostBuilderContext.Configuration);

        serviceCollection.AddScoped<ImportMessageConsumer>();
        serviceCollection.AddScoped<DeleteMessageConsumer>();
        serviceCollection.AddScoped<IOrderedItemIndexingRepository, OrderedItemIndexingRepository>();
    })
    .ConfigureAppConfiguration((hostBuilderContext, configurationBuilder) =>
    {
        if (hostBuilderContext.HostingEnvironment.IsProduction())
            configurationBuilder.AddAzureKeyVaultConfiguration();
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

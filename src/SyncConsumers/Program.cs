using SyncConsumers.Consumers;
using SyncConsumers.Extensions;
using SyncConsumers.Options;
using SyncConsumers.Repositories;
using NLog.Web;

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
    .UseNLog()
    .Build();

await host.RunAsync();

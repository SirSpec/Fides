using ImportScheduledJobs.Consumers;
using ImportScheduledJobs.Options;
using MassTransit;
using Nest;

namespace ImportScheduledJobs.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddMassTransitEndpoints(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var syncOptions = configuration
            .GetSection(nameof(SyncOptions))
            .Get<SyncOptions>();

        serviceCollection.AddMassTransit(busRegistrationConfigurator =>
        {
            busRegistrationConfigurator.UsingRabbitMq((busRegistrationContext, rabbitMqBusFactoryConfigurator) =>
            {
                rabbitMqBusFactoryConfigurator.ReceiveEndpoint(syncOptions.Endpoint!, receiveEndpointConfigurator =>
                {
                    receiveEndpointConfigurator.PrefetchCount = syncOptions.PrefetchCount ?? SyncOptions.DefaultPrefetchCount;
                    receiveEndpointConfigurator.EnablePriority(syncOptions.PriorityLimit ?? SyncOptions.DefaultPriorityLimit);

                    receiveEndpointConfigurator.Batch<ImportMessage>(batchConfigurator =>
                    {
                        batchConfigurator.MessageLimit = syncOptions.MessageLimit ?? SyncOptions.DefaultMessageLimit;
                        batchConfigurator.ConcurrencyLimit = syncOptions.ConcurrencyLimit ?? SyncOptions.DefaultConcurrencyLimit;
                        batchConfigurator.TimeLimit = TimeSpan.FromSeconds(syncOptions.TimeLimit ?? SyncOptions.DefaultTimeLimit);

                        batchConfigurator.Consumer<ImportMessageConsumer, ImportMessage>(serviceCollection.BuildServiceProvider());
                    });

                    receiveEndpointConfigurator.Batch<DeleteMessage>(batchConfigurator =>
                    {
                        batchConfigurator.MessageLimit = syncOptions.MessageLimit ?? SyncOptions.DefaultMessageLimit;
                        batchConfigurator.ConcurrencyLimit = syncOptions.ConcurrencyLimit ?? SyncOptions.DefaultConcurrencyLimit;
                        batchConfigurator.TimeLimit = TimeSpan.FromSeconds(syncOptions.TimeLimit ?? SyncOptions.DefaultTimeLimit);

                        batchConfigurator.Consumer<DeleteMessageConsumer, DeleteMessage>(serviceCollection.BuildServiceProvider());
                    });
                });

                rabbitMqBusFactoryConfigurator.Host(
                    syncOptions.Host,
                    syncOptions.VirtualHost,
                    rabbitMqHostConfigurator =>
                    {
                        rabbitMqHostConfigurator.Username(syncOptions.UserName);
                        rabbitMqHostConfigurator.Password(syncOptions.Password);
                    });

                rabbitMqBusFactoryConfigurator.UseMessageRetry(retryConfigurator =>
                    retryConfigurator.Exponential(
                        retryLimit: syncOptions.RetryLimit ?? SyncOptions.DefaultRetryLimit,
                        minInterval: TimeSpan.FromSeconds(
                            syncOptions.MinIntervalInSeconds ?? SyncOptions.DefaultMinIntervalInSeconds),
                        maxInterval: TimeSpan.FromSeconds(
                            syncOptions.MaxIntervalInSeconds ?? SyncOptions.DefaultMaxIntervalInSeconds),
                        intervalDelta: TimeSpan.FromSeconds(
                            syncOptions.IntervalDeltaInSeconds ?? SyncOptions.DefaultIntervalDeltaInSeconds)
                    ));

                rabbitMqBusFactoryConfigurator.ConfigureEndpoints(busRegistrationContext);
            });
        });
    }

    public static void AddElasticClient(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var elkOptions = configuration.GetSection(nameof(ELKOptions)).Get<ELKOptions>();
        var settings = new ConnectionSettings(new Uri(elkOptions.ElasticSearchUri!))
            .DefaultIndex(elkOptions.DefaultIndex)
            .MaximumRetries(elkOptions.MaximumRetries ?? ELKOptions.DefaultMaximumRetries)
            .MaxRetryTimeout(
                TimeSpan.FromSeconds(elkOptions.MaxRetryTimeoutInSeconds ?? ELKOptions.DefaultMaxRetryTimeoutInSeconds));

        serviceCollection.AddSingleton<IElasticClient>(new ElasticClient(settings));
    }
}

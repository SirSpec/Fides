using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using SyncFunction.Options;

namespace SyncFunction.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddMassTransitEndpoints(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var syncOptions = configuration
            .GetSection(nameof(SyncOptions))
            .Get<SyncOptions>();

        var busControl = Bus.Factory.CreateUsingRabbitMq(rabbitMqBusFactoryConfigurator =>
        {
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
        });

        serviceCollection.AddSingleton<IBusControl>(busControl);
        serviceCollection.AddSingleton<IPublishEndpoint>(busControl);
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

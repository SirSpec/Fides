using MassTransit;
using Microsoft.Extensions.Configuration;
using Nest;
using SyncFunction.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddMassTransitEndpoints(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var rabbitMQOptions = configuration
            .GetSection(nameof(RabbitMqOptions))
            .Get<RabbitMqOptions>();

        var busControl = Bus.Factory.CreateUsingRabbitMq(rabbitMqBusFactoryConfigurator =>
        {
            rabbitMqBusFactoryConfigurator.Host(
                rabbitMQOptions.Host,
                rabbitMQOptions.VirtualHost,
                rabbitMqHostConfigurator =>
                {
                    rabbitMqHostConfigurator.Username(rabbitMQOptions.UserName);
                    rabbitMqHostConfigurator.Password(rabbitMQOptions.Password);
                });

            rabbitMqBusFactoryConfigurator.UseMessageRetry(retryConfigurator =>
                retryConfigurator.Exponential(
                    retryLimit: rabbitMQOptions.RetryLimit ?? RabbitMqOptions.DefaultRetryLimit,
                    minInterval: TimeSpan.FromSeconds(
                        rabbitMQOptions.MinIntervalInSeconds ?? RabbitMqOptions.DefaultMinIntervalInSeconds),
                    maxInterval: TimeSpan.FromSeconds(
                        rabbitMQOptions.MaxIntervalInSeconds ?? RabbitMqOptions.DefaultMaxIntervalInSeconds),
                    intervalDelta: TimeSpan.FromSeconds(
                        rabbitMQOptions.IntervalDeltaInSeconds ?? RabbitMqOptions.DefaultIntervalDeltaInSeconds)
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

using MassTransit;
using Nest;
using SharedKernel.Consumers;
using SyncConsumers.Consumers;
using SyncConsumers.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddMassTransitEndpoints(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var rabbitMQOptions = configuration
            .GetSection(nameof(RabbitMqOptions))
            .Get<RabbitMqOptions>();

        serviceCollection.AddMassTransit(busRegistrationConfigurator =>
        {
            busRegistrationConfigurator.UsingRabbitMq((busRegistrationContext, rabbitMqBusFactoryConfigurator) =>
            {
                rabbitMqBusFactoryConfigurator.ReceiveEndpoint(rabbitMQOptions.Endpoint!, receiveEndpointConfigurator =>
                {
                    receiveEndpointConfigurator.PrefetchCount = rabbitMQOptions.PrefetchCount ?? RabbitMqOptions.DefaultPrefetchCount;
                    receiveEndpointConfigurator.EnablePriority(rabbitMQOptions.PriorityLimit ?? RabbitMqOptions.DefaultPriorityLimit);

                    receiveEndpointConfigurator.Batch<ImportMessage>(batchConfigurator =>
                    {
                        batchConfigurator.MessageLimit = rabbitMQOptions.MessageLimit ?? RabbitMqOptions.DefaultMessageLimit;
                        batchConfigurator.ConcurrencyLimit = rabbitMQOptions.ConcurrencyLimit ?? RabbitMqOptions.DefaultConcurrencyLimit;
                        batchConfigurator.TimeLimit = TimeSpan.FromSeconds(rabbitMQOptions.TimeLimit ?? RabbitMqOptions.DefaultTimeLimit);

                        batchConfigurator.Consumer<ImportMessageConsumer, ImportMessage>(serviceCollection.BuildServiceProvider());
                    });

                    receiveEndpointConfigurator.Batch<DeleteMessage>(batchConfigurator =>
                    {
                        batchConfigurator.MessageLimit = rabbitMQOptions.MessageLimit ?? RabbitMqOptions.DefaultMessageLimit;
                        batchConfigurator.ConcurrencyLimit = rabbitMQOptions.ConcurrencyLimit ?? RabbitMqOptions.DefaultConcurrencyLimit;
                        batchConfigurator.TimeLimit = TimeSpan.FromSeconds(rabbitMQOptions.TimeLimit ?? RabbitMqOptions.DefaultTimeLimit);

                        batchConfigurator.Consumer<DeleteMessageConsumer, DeleteMessage>(serviceCollection.BuildServiceProvider());
                    });
                });

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

                rabbitMqBusFactoryConfigurator.ConfigureEndpoints(busRegistrationContext);
            });
        });
    }

    public static void AddElasticClient(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var elkOptions = configuration.GetSection(nameof(ELKOptions)).Get<ELKOptions>();
        var settings = new ConnectionSettings(new Uri(elkOptions.ElasticSearchUri!))
            .EnableApiVersioningHeader()
            .DefaultIndex(elkOptions.DefaultIndex)
            .MaximumRetries(elkOptions.MaximumRetries ?? ELKOptions.DefaultMaximumRetries)
            .MaxRetryTimeout(
                TimeSpan.FromSeconds(elkOptions.MaxRetryTimeoutInSeconds ?? ELKOptions.DefaultMaxRetryTimeoutInSeconds));

        serviceCollection.AddSingleton<IElasticClient>(new ElasticClient(settings));
    }
}

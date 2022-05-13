using ImportScheduledJobs.Consumers;
using ImportScheduledJobs.Options;
using MassTransit;
using Nest;

namespace ImportScheduledJobs.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddMassTransitEndpoints(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var massTransitOptions = configuration
            .GetSection(nameof(MassTransitOptions))
            .Get<MassTransitOptions>();

        serviceCollection.AddMassTransit(busRegistrationConfigurator =>
        {
            busRegistrationConfigurator.AddConsumer<CreateMessageConsumer>(typeof(OrderAuditConsumerDefinition));

            busRegistrationConfigurator.UsingRabbitMq((busRegistrationContext, rabbitMqBusFactoryConfigurator) =>
            {
                rabbitMqBusFactoryConfigurator.Host(
                    massTransitOptions.Host,
                    massTransitOptions.VirtualHost,
                    rabbitMqHostConfigurator =>
                    {
                        rabbitMqHostConfigurator.Username(massTransitOptions.UserName);
                        rabbitMqHostConfigurator.Password(massTransitOptions.Password);
                    });

                rabbitMqBusFactoryConfigurator.UseMessageRetry(retryConfigurator =>
                    retryConfigurator.Exponential(
                        retryLimit: massTransitOptions.RetryLimit ?? MassTransitOptions.DefaultRetryLimit,
                        minInterval: TimeSpan.FromSeconds(
                            massTransitOptions.MinIntervalInSeconds ?? MassTransitOptions.DefaultMinIntervalInSeconds),
                        maxInterval: TimeSpan.FromSeconds(
                            massTransitOptions.MaxIntervalInSeconds ?? MassTransitOptions.DefaultMaxIntervalInSeconds),
                        intervalDelta: TimeSpan.FromSeconds(
                            massTransitOptions.IntervalDeltaInSeconds ?? MassTransitOptions.DefaultIntervalDeltaInSeconds)
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
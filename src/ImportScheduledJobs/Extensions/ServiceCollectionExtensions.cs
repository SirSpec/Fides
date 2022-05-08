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

                rabbitMqBusFactoryConfigurator.ConfigureEndpoints(busRegistrationContext);
            });
        });
    }

    public static void AddElasticClient(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var elkOptions = configuration.GetSection(nameof(ELKOptions)).Get<ELKOptions>();
        var settings = new ConnectionSettings(new Uri(elkOptions.ElasticSearchUri!))
            .DefaultIndex(elkOptions.DefaultIndex);

        serviceCollection.AddSingleton<IElasticClient>(new ElasticClient(settings));
    }
}
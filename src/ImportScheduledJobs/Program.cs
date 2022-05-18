using ImportScheduledJobs.Consumers;
using ImportScheduledJobs.Extensions;
using ImportScheduledJobs.Mappers;
using ImportScheduledJobs.Options;
using ImportScheduledJobs.Repositories;
using ImportScheduledJobs.Workers;
using Microsoft.EntityFrameworkCore;
using NLog.Web;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostBuilderContext, serviceCollection) =>
    {
        serviceCollection.Configure<SyncOptions>(
            hostBuilderContext.Configuration.GetSection(nameof(SyncOptions)));

        serviceCollection.AddMassTransitEndpoints(hostBuilderContext.Configuration);
        serviceCollection.AddElasticClient(hostBuilderContext.Configuration);
        serviceCollection.AddDbContext<StoresDbContext>(options =>
        {
            if (hostBuilderContext.HostingEnvironment.IsDevelopment())
                options
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging();

            options.UseSqlServer(
                hostBuilderContext.Configuration.GetConnectionString("StoresDbContext"),
                sqlServerOptions => sqlServerOptions.EnableRetryOnFailure());
        });

        serviceCollection.AddScoped<ImportMessageConsumer>();
        serviceCollection.AddScoped<DeleteMessageConsumer>();
        serviceCollection.AddScoped<IOrderItemRepository, OrderItemRepository>();
        serviceCollection.AddScoped<IOrderItemMapper, OrderItemMapper>();
    })
    .UseNLog()
    .Build();

await host.RunAsync();

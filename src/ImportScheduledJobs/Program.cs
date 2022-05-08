using ImportScheduledJobs;
using ImportScheduledJobs.Extensions;
using Microsoft.EntityFrameworkCore;
using NLog.Web;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostBuilderContext, serviceCollection) =>
    {
        serviceCollection.AddMassTransitEndpoints(hostBuilderContext.Configuration);
        serviceCollection.AddElasticClient(hostBuilderContext.Configuration);
        serviceCollection.AddDbContext<MyDbContext>(options =>
            options.UseSqlServer(
                hostBuilderContext.Configuration.GetConnectionString("MyDbContext")));

        serviceCollection.AddHostedService<Worker>();
    })
    .UseNLog()
    .Build();

await host.RunAsync();

using ImportScheduledJobs.Consumers;
using ImportScheduledJobs.Mappers;
using ImportScheduledJobs.Options;
using ImportScheduledJobs.Repositories;
using MassTransit;
using Microsoft.Extensions.Options;

namespace ImportScheduledJobs;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _services;
    private readonly SyncOptions _syncOptions;

    public Worker(
        ILogger<Worker> logger,
        IServiceProvider services,
        IOptions<SyncOptions> syncOptions)
    {
        _logger = logger;
        _services = services;
        _syncOptions = syncOptions.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Import worker start.");

        await using (var scope = _services.CreateAsyncScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IOrderItemRepository>();
            var publishEndpoint = scope.ServiceProvider.GetRequiredService<ISendEndpointProvider>();
            var _orderItemMapper = scope.ServiceProvider.GetRequiredService<IOrderItemMapper>();
            var endpoint = await publishEndpoint.GetSendEndpoint(new Uri("queue:fides"));
            var results = await repository.GetSoldItemsSortedByBrandAndPriceAync(_syncOptions.ImportPageSize);

            await results.ForEachPageAsync(
                async page =>
                {
                    var mapped = page.Select(orderedItem => new CreateMessage(_orderItemMapper.Map(orderedItem)));
                    await endpoint.SendBatch<CreateMessage>(mapped, stoppingToken);
                },
                stoppingToken
            );
        }

        _logger.LogInformation("Import worker end.");
    }
}

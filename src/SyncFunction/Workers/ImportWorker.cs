using SyncFunction.Options;
using SyncFunction.Repositories;
using MassTransit;
using Microsoft.Extensions.Options;
using SyncFunction.QueryObjects.Mappers;
using SyncFunction.QueryObjects.Queries;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Consumers;

namespace SyncFunction.Workers;

public class ImportWorker : BackgroundService
{
    private readonly ILogger<ImportWorker> _logger;
    private readonly IServiceProvider _services;
    private readonly ICompletedOrdersQuery _completedOrdersQuery;
    private readonly SyncOptions _syncOptions;

    public ImportWorker(
        ILogger<ImportWorker> logger,
        IServiceProvider services,
        IOptions<SyncOptions> syncOptions,
        ICompletedOrdersQuery completedOrdersQuery)
    {
        _logger = logger;
        _services = services;
        _completedOrdersQuery = completedOrdersQuery;
        _syncOptions = syncOptions.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"{nameof(ImportWorker)} start.");

        await using (var scope = _services.CreateAsyncScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IOrderItemRepository>();
            var publishEndpoint = scope.ServiceProvider.GetRequiredService<ISendEndpointProvider>();
            var bus = scope.ServiceProvider.GetRequiredService<IBus>();
            var _orderItemMapper = scope.ServiceProvider.GetRequiredService<IOrderItemMapper>();

            var endpoint = await publishEndpoint.GetSendEndpoint(_syncOptions.GetEndpointUri());
            var results = await repository.GetOrdersSortedByBrandAndPriceAync(
                _syncOptions.ImportPageSize,
                _completedOrdersQuery);

            //while (results.HasNextPage)
            //{
            //    var page = await results.NextPageAsync(_orderItemMapper.Query, stoppingToken);
            //    var messages = page.Select(item => new ImportMessage(item));
            //    await bus.PublishBatch<ImportMessage>(
            //        messages, Pipe.New<PublishContext<ImportMessage>>(pipeConfigurator =>
            //            pipeConfigurator.UseFilter(new PriorityFilter<ImportMessage>(2))));
            //}
        }

        _logger.LogInformation($"{nameof(ImportWorker)} end.");
    }
}

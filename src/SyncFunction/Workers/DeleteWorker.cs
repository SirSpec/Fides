using SyncFunction.Options;
using SyncFunction.Repositories;
using MassTransit;
using Microsoft.Extensions.Options;
using Nest;
using SyncFunction.QueryObjects.Queries;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedKernel.QueryObjects.Models;

namespace SyncFunction.Workers;

public class DeleteWorker : BackgroundService
{
    private readonly ILogger<DeleteWorker> _logger;
    private readonly IServiceProvider _services;
    private readonly ICompletedOrdersQuery _completedOrdersQuery;
    private readonly SyncOptions _syncOptions;

    public DeleteWorker(
        ILogger<DeleteWorker> logger,
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
        _logger.LogInformation($"{nameof(DeleteWorker)} start.");

        await using (var scope = _services.CreateAsyncScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IOrderItemRepository>();
            var elasticClient = scope.ServiceProvider.GetRequiredService<IElasticClient>();
            var publishEndpoint = scope.ServiceProvider.GetRequiredService<ISendEndpointProvider>();

            var endpoint = await publishEndpoint.GetSendEndpoint(_syncOptions.GetEndpointUri());
            var results = await repository.GetOrdersSortedByBrandAndPriceAync(
                _syncOptions.ImportPageSize,
                _completedOrdersQuery);

            var searchResponse = await elasticClient.SearchAsync<OrderedItem>(s => s
                .Scroll("10s")
            );

            int count = 0;

            while (searchResponse.Documents.Any())
            {
                count += searchResponse.Documents.Count;
                searchResponse = await elasticClient.ScrollAsync<OrderedItem>("10s", searchResponse.ScrollId);
            }
        }

        _logger.LogInformation($"{nameof(DeleteWorker)} end.");
    }
}

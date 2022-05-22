using MassTransit;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nest;
using SyncFunction.Options;
using SyncFunction.QueryObjects.Mappers;
using SyncFunction.QueryObjects.Queries;
using SyncFunction.Repositories;

namespace SyncFunction;

public class DeleteFunction
{
    public const string Name = $"{nameof(SyncOrchestrator)}_{nameof(DeleteFunction)}";

    private readonly ILogger<SyncOrchestrator> _logger;
    private readonly ICompletedOrdersQuery _completedOrdersQuery;
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly IOrderItemMapper _orderItemMapper;
    private readonly IElasticClient _elasticClient;
    private readonly IBusControl _busControl;
    private readonly SyncOptions _syncOptions;

    public DeleteFunction(
        ILogger<SyncOrchestrator> logger,
        ICompletedOrdersQuery completedOrdersQuery,
        IOrderItemRepository orderItemRepository,
        IOrderItemMapper orderItemMapper,
        IElasticClient elasticClient,
        IBusControl busControl,
        IOptions<SyncOptions> syncOptions)
    {
        _logger = logger;
        _completedOrdersQuery = completedOrdersQuery;
        _orderItemRepository = orderItemRepository;
        _orderItemMapper = orderItemMapper;
        _elasticClient = elasticClient;
        _busControl = busControl;
        _syncOptions = syncOptions.Value;
    }

    [FunctionName(Name)]
    public async Task Delete([ActivityTrigger] string name, ILogger log)
    {
        log.LogInformation($"{nameof(Delete)} start.");

        //var results = await repository.GetOrdersSortedByBrandAndPriceAync(
        //    _syncOptions.ImportPageSize,
        //    _completedOrdersQuery);

        //var searchResponse = await elasticClient.SearchAsync<OrderedItem>(s => s
        //    .Scroll("10s")
        //);

        //int count = 0;

        //while (searchResponse.Documents.Any())
        //{
        //    count += searchResponse.Documents.Count;
        //    searchResponse = await elasticClient.ScrollAsync<OrderedItem>("10s", searchResponse.ScrollId);
        //}

        log.LogInformation($"{nameof(Delete)} end.");
    }
}

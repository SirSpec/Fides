using MassTransit;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nest;
using SharedKernel.QueryObjects.Models;
using SyncFunction.Options;
using SyncFunction.QueryObjects.Mappers;
using SyncFunction.QueryObjects.Queries;
using SyncFunction.Repositories;

namespace SyncFunction;

public class DeleteFunction
{
    public const string Name = $"{nameof(SyncOrchestrator)}_{nameof(DeleteFunction)}";

    private readonly ICompletedOrdersQuery _completedOrdersQuery;
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly IOrderItemMapper _orderItemMapper;
    private readonly IElasticClient _elasticClient;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly SyncOptions _syncOptions;

    public DeleteFunction(
        ICompletedOrdersQuery completedOrdersQuery,
        IOrderItemRepository orderItemRepository,
        IOrderItemMapper orderItemMapper,
        IElasticClient elasticClient,
        IPublishEndpoint publishEndpoint,
        IOptions<SyncOptions> syncOptions)
    {
        _completedOrdersQuery = completedOrdersQuery;
        _orderItemRepository = orderItemRepository;
        _orderItemMapper = orderItemMapper;
        _elasticClient = elasticClient;
        _publishEndpoint = publishEndpoint;
        _syncOptions = syncOptions.Value;
    }

    [FunctionName(Name)]
    public async Task Delete([ActivityTrigger] string name, ILogger log)
    {
        var searchResponse = await _elasticClient.SearchAsync<OrderedItem>(searchDescriptor => searchDescriptor.Scroll("10s"));

        while (searchResponse.Documents.Any())
        {
            var orderItems = await _orderItemRepository.GetExistingOrderItemIds(
                searchResponse.Documents,
                _completedOrdersQuery);

            var documentsToDelete = searchResponse
                .Documents
                .Where(document =>
                    !orderItems.Any(orderItem =>
                        orderItem.OrderId == document.Id.OrderId && orderItem.ItemId == document.Id.ItemId));

            if (documentsToDelete.Any())
            {
                await _elasticClient.DeleteManyAsync(documentsToDelete);
            }

            searchResponse = await _elasticClient.ScrollAsync<OrderedItem>("10s", searchResponse.ScrollId);
        }
    }
}

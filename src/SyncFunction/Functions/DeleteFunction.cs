using MassTransit;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nest;
using SharedKernel.QueryObjects.Models;
using SyncFunction.Options;
using SyncFunction.QueryObjects.Queries;
using SyncFunction.Repositories;

namespace SyncFunction.Functions;

public class DeleteFunction
{
    public const string Name = $"{nameof(SyncOrchestrator)}_{nameof(DeleteFunction)}";

    private readonly ICompletedOrdersQuery _completedOrdersQuery;
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly IElasticClient _elasticClient;
    private readonly ELKOptions _elkOptions;

    public DeleteFunction(
        ICompletedOrdersQuery completedOrdersQuery,
        IOrderItemRepository orderItemRepository,
        IElasticClient elasticClient,
        IOptions<ELKOptions> elkOptions)
    {
        _completedOrdersQuery = completedOrdersQuery;
        _orderItemRepository = orderItemRepository;
        _elasticClient = elasticClient;
        _elkOptions = elkOptions.Value;
    }

    [FunctionName(Name)]
    public async Task Delete([ActivityTrigger] object @object, ILogger log)
    {
        var searchResponse = await _elasticClient.SearchAsync<OrderedItem>(
            searchDescriptor => searchDescriptor.Scroll(_elkOptions.ScrollTime ?? ELKOptions.DefaultScrollTime));

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

            searchResponse = await _elasticClient.ScrollAsync<OrderedItem>(
                _elkOptions.ScrollTime ?? ELKOptions.DefaultScrollTime,
                searchResponse.ScrollId);
        }
    }
}

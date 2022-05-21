using ImportScheduledJobs.QueryObjects.Models;
using Nest;

namespace ImportScheduledJobs.Repositories;

public class OrderedItemIndexingRepository : IOrderedItemIndexingRepository
{
    private readonly ILogger<OrderedItemIndexingRepository> _logger;
    private readonly IElasticClient _elasticClient;

    public OrderedItemIndexingRepository(
        ILogger<OrderedItemIndexingRepository> logger,
        IElasticClient elasticClient)
    {
        _logger = logger;
        _elasticClient = elasticClient;
    }

    public async Task IndexDocuments(IEnumerable<OrderedItem> orderedItems)
    {
        var indexManyResponse = await _elasticClient.IndexManyAsync<OrderedItem>(orderedItems);

        if (indexManyResponse.IsValid is false)
        {
            _logger.LogError(indexManyResponse.OriginalException, $"Failed to index documents.");
        }

        if (indexManyResponse.Errors)
        {
            foreach (var itemWithError in indexManyResponse.ItemsWithErrors)
                _logger.LogError($"Failed to index document {itemWithError.Id}: {itemWithError.Error}");
        }
    }

    public async Task DeleteDocuments(IEnumerable<OrderedItem> orderedItems)
    {
        var deleteManyResponse = await _elasticClient.DeleteManyAsync<OrderedItem>(orderedItems);

        if (deleteManyResponse.IsValid is false)
        {
            _logger.LogError(deleteManyResponse.OriginalException, $"Failed to delete documents.");
        }

        if (deleteManyResponse.Errors)
        {
            foreach (var itemWithError in deleteManyResponse.ItemsWithErrors)
                _logger.LogError($"Failed to delete document {itemWithError.Id}: {itemWithError.Error}");
        }
    }
}

using SharedKernel.QueryObjects.Models;

namespace SyncConsumers.Repositories;

public interface IOrderedItemIndexingRepository
{
    Task IndexDocuments(IEnumerable<OrderedItem> orderedItems);
    Task DeleteDocuments(IEnumerable<OrderedItem> orderedItems);
}

using ImportScheduledJobs.QueryObjects.Models;

namespace ImportScheduledJobs.Repositories;

public interface IOrderedItemIndexingRepository
{
    Task IndexDocuments(IEnumerable<OrderedItem> orderedItems);
}

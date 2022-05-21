using Microsoft.EntityFrameworkCore;

namespace ImportScheduledJobs.Extensions;

public static class PaginatedQueryableExtensions
{
    public static async ValueTask<PaginatedQueryable<TType>> ToPaginatedAsync<TType>(
        this IQueryable<TType> source,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var count = source is IAsyncEnumerable<TType>
            ? await source.CountAsync(cancellationToken)
            : source.Count();

        return new PaginatedQueryable<TType>(
            source,
            count: count,
            pageSize: pageSize);
    }
}

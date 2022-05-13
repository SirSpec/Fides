using Microsoft.EntityFrameworkCore;

namespace ImportScheduledJobs.Extensions;

public static class PaginatedQueryableExtensions
{
    public static async Task<PaginatedQueryable<TType>> ToPaginatedAsync<TType>(
        this IQueryable<TType> source,
        int pageSize,
        CancellationToken cancellationToken = default) =>
            new PaginatedQueryable<TType>(
                source,
                count: await source.CountAsync(cancellationToken),
                pageSize: pageSize);
}

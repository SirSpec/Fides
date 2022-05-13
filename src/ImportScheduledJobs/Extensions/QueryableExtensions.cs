using Microsoft.EntityFrameworkCore;

namespace ImportScheduledJobs.Extensions;

public static class QueryableExtensions
{
    public static async Task ForEachPageAsync<TType>(
        this IQueryable<TType> source,
        Func<IEnumerable<TType>, Task> func,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        if (pageSize > 0)
        {
            var count = await source
                .CountAsync(cancellationToken)
                .ConfigureAwait(false);

            for (int i = 0; i * pageSize < count; i++)
            {
                var results = await source
                    .Skip(i * pageSize)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false);

                await func(results);
            }
        }
    }
}
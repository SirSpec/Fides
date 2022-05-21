using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace ImportScheduledJobs.Extensions;

public static class QueryableExtensions
{
    public static async Task ForEachPageAsync<TType, TResult>(
        this IQueryable<TType> source,
        Expression<Func<TType, TResult>> func,
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
                    .Select(func)
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
        }
    }
}
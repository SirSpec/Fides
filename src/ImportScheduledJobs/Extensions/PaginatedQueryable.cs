using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace ImportScheduledJobs.Extensions;

public class PaginatedQueryable<TType>
{
    private readonly IQueryable<TType> _source;
    private readonly int _pageSize;
    private readonly int _totalPages;
    private int _pageIndex;

    public PaginatedQueryable(IQueryable<TType> source, int count, int pageSize)
    {
        _source = source;
        _pageSize = pageSize;
        _totalPages = (int)Math.Ceiling(count / (double)pageSize);
    }

    public bool HasNextPage =>
        _pageIndex < _totalPages;

    public async Task<IEnumerable<TResult>> NextPageAsync<TResult>(
        Expression<Func<TType, TResult>> func,
        CancellationToken cancellationToken = default) =>
            HasNextPage
                ? await _source
                    .Skip(_pageIndex++ * _pageSize)
                    .Take(_pageSize)
                    .Select(func)
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false)
                : Enumerable.Empty<TResult>();

    public static PaginatedQueryable<TType> Empty() => new PaginatedQueryable<TType>(
        Enumerable.Empty<TType>().AsQueryable(), 0, 0);
}

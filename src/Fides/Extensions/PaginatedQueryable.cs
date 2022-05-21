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
        if (source is IAsyncEnumerable<TType>)
        {
            _source = source;
            _pageSize = pageSize;
            _totalPages = (int)Math.Ceiling(count / (double)pageSize);
        }
        else throw new ArgumentException(
            $"IQueryable<{typeof(TType).Name}> {nameof(source)} doesn't implement IAsyncEnumerable<{typeof(TType).Name}>.");
    }

    public bool HasNextPage =>
        _pageIndex < _totalPages;

    public virtual async Task<IEnumerable<TResult>> NextPageAsync<TResult>(
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

    public static PaginatedQueryable<TType> Empty()
    {
        return new PaginatedQueryable<TType>(Enumerable.Empty<TType>().AsQueryable(), count: 0, pageSize: 0);
    }
}

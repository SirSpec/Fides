using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace SharedKernel.Extensions;

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

    public static PaginatedQueryable<TType> Empty() =>
        new PaginatedQueryable<TType>(Enumerable.Empty<TType>().AsQueryable(), count: 0, pageSize: 0);

    public async ValueTask<IEnumerable<TResult>> NextPageAsync<TResult>(
        Expression<Func<TType, TResult>> func,
        CancellationToken cancellationToken = default)
    {
        if (HasNextPage)
        {
            var results = _source
                .Skip(_pageIndex++ * _pageSize)
                .Take(_pageSize)
                .Select(func);

            return _source is IAsyncEnumerable<TType>
                ? await results
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false)
                : results.ToList();
        }
        else return Enumerable.Empty<TResult>();
    }
}

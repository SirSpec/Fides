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

    public async ValueTask<IEnumerable<TType>> NextPageAsync(CancellationToken cancellationToken = default) =>
        _pageIndex < _totalPages
            ? await _source
                .Skip(_pageIndex++ * _pageSize)
                .Take(_pageSize)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false)
            : Enumerable.Empty<TType>();

    public async Task ForEachPageAsync(Func<IEnumerable<TType>, Task> func, CancellationToken cancellationToken = default) =>
        await _source
            .ForEachPageAsync(
                func,
                pageSize: _pageSize,
                cancellationToken)
            .ConfigureAwait(false);
}
using SharedKernel.Extensions;
using Xunit;

namespace SyncFunction;

public class PaginatedQueryableTest
{
    [Fact]
    public void Empty_HasNextPage_False()
    {
        var sut = PaginatedQueryable<int>.Empty();

        Assert.False(sut.HasNextPage);
    }

    [Fact]
    public async Task Empty_NextPage_EmptyEnumerable()
    {
        var sut = PaginatedQueryable<int>.Empty();

        var result = await sut.NextPageAsync(value => value);

        Assert.Empty(result);
    }

    [Fact]
    public async Task NextPage_11ElementsPageSize5__3Pages()
    {
        const int elementsCount = 11;
        const int pageSize = 5;
        var sut = await Enumerable.Repeat<int>(int.MinValue, elementsCount).AsQueryable().ToPaginatedAsync(pageSize);

        var result1 = await sut.NextPageAsync(value => value);
        var result2 = await sut.NextPageAsync(value => value);
        var result3 = await sut.NextPageAsync(value => value);
        var result4 = await sut.NextPageAsync(value => value);

        Assert.Equal(pageSize, result1.Count());
        Assert.Equal(pageSize, result2.Count());
        Assert.Single(result3);
        Assert.Empty(result4);
    }
}

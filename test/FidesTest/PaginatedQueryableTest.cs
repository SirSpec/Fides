using System.Linq;
using System.Threading.Tasks;
using ImportScheduledJobs.Extensions;
using Xunit;

namespace Fides;

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
    public async Task NextPage_10ElementsPageSize5__2Pages()
    {
        var sut = new PaginatedQueryable<int>(Enumerable.Repeat<int>(0, 10).AsQueryable(), count: 10, pageSize: 5);
        var result1 = await sut.NextPageAsync(value => value);
        var result2 = await sut.NextPageAsync(value => value);
        var result3 = await sut.NextPageAsync(value => value);

        Assert.Equal(5, result1.Count());
        Assert.Equal(5, result2.Count());
        Assert.Empty(result3);
    }
}

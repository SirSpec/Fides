using System.Linq.Expressions;

namespace ImportScheduledJobs.Mappers;

public interface IQueryObject<T, TResult>
{
    Expression<Func<T, TResult>> Query { get; }
}
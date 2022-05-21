using System.Linq.Expressions;

namespace ImportScheduledJobs.QueryObjects;

public interface IQueryObject<T, TResult>
{
    Expression<Func<T, TResult>> Query { get; }
}
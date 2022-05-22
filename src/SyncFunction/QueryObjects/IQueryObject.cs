using System.Linq.Expressions;

namespace SyncFunction.QueryObjects;

public interface IQueryObject<T, TResult>
{
    Expression<Func<T, TResult>> Query { get; }
}

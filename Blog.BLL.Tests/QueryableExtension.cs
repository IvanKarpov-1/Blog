using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Blog.BLL.Tests;

public static class QueryableExtensions
{
    public static IQueryable<T> AsAsyncQueryable<T>(this IEnumerable<T> input)
    {
        return new TestAsyncEnumerable<T>(input);
    }

}

internal class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
{
    private readonly IQueryProvider _inner;

    public TestAsyncQueryProvider(IQueryProvider inner)
    {
        _inner = inner;
    }

    TResult IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
    {
        var expectedResultType = typeof(TResult).GetGenericArguments()[0];
        var result =
            typeof(IQueryProvider)
            .GetMethod(
                name: nameof(IQueryProvider.Execute),
                genericParameterCount: 1,
                types: new[] { typeof(Expression) })
            ?.MakeGenericMethod(expectedResultType)
            .Invoke(this, new[] { expression });

        var fromResultMethod = typeof(Task)
            .GetMethod(nameof(Task.FromResult))
            ?.MakeGenericMethod(expectedResultType);

        return (TResult)fromResultMethod.Invoke(null, new[] { result });
    }

    IQueryable IQueryProvider.CreateQuery(Expression expression)
    {
        return new TestAsyncEnumerable<TEntity>(expression);
    }

    IQueryable<TElement> IQueryProvider.CreateQuery<TElement>(Expression expression)
    {
        return new TestAsyncEnumerable<TElement>(expression);
    }

    object? IQueryProvider.Execute(Expression expression)
    {
        return _inner.Execute(expression);
    }

    TResult IQueryProvider.Execute<TResult>(Expression expression)
    {
        return _inner.Execute<TResult>(expression);
    }
}

internal class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
{
    public TestAsyncEnumerable(IEnumerable<T> enumerable)
        : base(enumerable)
    { }

    public TestAsyncEnumerable(Expression expression)
        : base(expression)
    { }

    IAsyncEnumerator<T> IAsyncEnumerable<T>.GetAsyncEnumerator(CancellationToken cancellationToken)
    {
        return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
    }

    IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);
}

internal class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
{
    private readonly IEnumerator<T> _inner;

    public TestAsyncEnumerator(IEnumerator<T> inner)
    {
        _inner = inner;
    }

    T IAsyncEnumerator<T>.Current => _inner.Current;

    ValueTask<bool> IAsyncEnumerator<T>.MoveNextAsync()
    {
        return new ValueTask<bool>(_inner.MoveNext());
    }

    ValueTask IAsyncDisposable.DisposeAsync()
    {
        _inner.Dispose();
        return default;
    }
}
using System.Collections;

IEnumerable<int> source = Enumerable.Range(1, 1000).ToArray();
Console.WriteLine(Enumerable.Select(source, x => x * 2).Sum());
Console.WriteLine(Select(source, x => x * 2).Sum());




static IEnumerable<TResult> SelectCompiler<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, TResult> selector)
{

    ArgumentNullException.ThrowIfNull(source);
    ArgumentNullException.ThrowIfNull(selector);

    return Impl(source, selector);

    static IEnumerable<TResult> Impl(IEnumerable<TSource> source, Func<TSource, TResult> selector)
    {
        foreach (TSource item in source)
        {
            yield return selector(item);
        }
    }

}

static IEnumerable<TResult> SelectManual<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, TResult> selector)
{

    ArgumentNullException.ThrowIfNull(source);
    ArgumentNullException.ThrowIfNull(selector);

    return new SelectManualEnumerable<TSource, TResult>(source, selector);

    // static IEnumerable<TResult> Impl(IEnumerable<TSource> source, Func<TSource, TResult> selector)
    // {
    //     foreach (TSource item in source)
    //     {
    //         yield return selector(item);
    //     }
    // }

}

sealed class SelectManualEnumerable<TSource, TResult> : IEnumerable<TResult>
{
    private readonly IEnumerable<TSource> _source;
    private readonly Func<TSource, TResult> _selector;

    public SelectManualEnumerable(IEnumerable<TSource> source, Func<TSource, TResult> selector)
    {
        _source = source;
        _selector = selector;
    }

    public IEnumerator<TResult> GetEnumerator()
    {
        return new Enumerator(_source, _selector);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private sealed class Enumerator : IEnumerator<TResult>
    {
        private readonly IEnumerable<TSource> _source;
        private readonly Func<TSource, TResult> _selector;
        private TResult _current = default!;

        public Enumerator(IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            _source = source;
            _selector = selector;
        }

        public TResult Current => _current;

        object? IEnumerator.Current => Current;
        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            IEnumerator<TSource> enumerator = _source.GetEnumerator();
            try
            {
                if (enumerator.MoveNext())
                {
                    _current = _selector(enumerator.Current);
                    \
                }
            }
            finally
            {
                enumerator?.Dispose();
            }

        }

        public void Reset()
        {
            throw new NotSupportedException();
        }
    }
}


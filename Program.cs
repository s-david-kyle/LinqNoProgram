using System.Collections;
using System.Diagnostics;

IEnumerable<int> source = Enumerable.Range(0, 1000).ToArray();
Console.WriteLine(Enumerable.Select(source, x => x * 2).Sum());
Console.WriteLine(SelectCompiler(source, x => x * 2).Sum());
Console.WriteLine(SelectManual(source, x => x * 2).Sum());




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

sealed class SelectManualEnumerable<TSource, TResult> : IEnumerable<TResult>, IEnumerator<TResult>
{
    private readonly IEnumerable<TSource> _source;
    private readonly Func<TSource, TResult> _selector;

    IEnumerator<TSource>? _enumerator;
    private int _state = 0;
    private TResult _current = default!;

    public SelectManualEnumerable(IEnumerable<TSource> source, Func<TSource, TResult> selector)
    {
        _source = source;
        _selector = selector;
    }

    public IEnumerator<TResult> GetEnumerator()
    {
        if (_state == 0)
        {
            _state = 1;
            return this;
        }

        return new SelectManualEnumerable<TSource, TResult>(_source, _selector) { _state = 1 };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public TResult Current => _current;

    object? IEnumerator.Current => Current;

    public bool MoveNext()
    {
        switch (_state)
        {
            case 1:
                _enumerator = _source.GetEnumerator();
                _state = 2;
                goto case 2;
            case 2:
                Debug.Assert(_enumerator != null);
                try
                {
                    if (_enumerator.MoveNext())
                    {
                        _current = _selector(_enumerator.Current);
                        return true;
                    }
                }
                catch
                {
                    Dispose();
                    throw;
                }
                break;
        }

        Dispose();
        return false;



    }

    public void Dispose()
    {
        _enumerator?.Dispose();
    }

    public void Reset()
    {
        _state = -1;
        throw new NotSupportedException();
    }

}


using System.Security.Cryptography;

Console.WriteLine(0);
IEnumerable<int> e = Select<int, int>(null, x => x * 2);
Console.WriteLine(1);
IEnumerator<int> enumerator = e.GetEnumerator();
Console.WriteLine(2);
_ = enumerator.MoveNext();

static IEnumerable<TResult> Select<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, TResult> selector)
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



IEnumerable<int> source = new int[] { 1, 2, 3, 4, 5 };
foreach (int i in Select(source, x => x * 2))
{
    Console.WriteLine(i);
}


static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
{
    foreach (TSource item in source)
    {
        yield return selector(item);
    }
}



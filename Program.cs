IEnumerable<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
foreach (var number in Select(numbers, x => x * 2))
{
    Console.WriteLine(number);
}

static IEnumerable<TResult> Select<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, TResult> selector)
{

    ArgumentNullException.ThrowIfNull(source);
    ArgumentNullException.ThrowIfNull(selector);

    foreach (var item in source)
    {
        yield return selector(item);
    }
}



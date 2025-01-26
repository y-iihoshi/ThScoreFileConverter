using System.Collections.Generic;
using System.Linq;

namespace TemplateGenerator.Extensions;

internal static class EnumerableExtensions
{
    public static IEnumerable<T> RotateLeft<T>(this IEnumerable<T> source, int count = 1)
    {
        return source.Skip(count).Concat(source.Take(count));
    }

    public static IEnumerable<T> RotateRight<T>(this IEnumerable<T> source, int count = 1)
    {
        return source.TakeLast(count).Concat(source.SkipLast(count));
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Diagnostics;
using ThScoreFileConverter.Core.Extensions;

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

    public static Dictionary<string, TValue> ToStringKeyedDictionary<TKey, TValue>(
        this IEnumerable<(TKey Key, TValue Value)> source)
        where TKey : struct, Enum
    {
        Guard.IsNotNull(source);

        return source.ToDictionary(static pair => pair.Key.ToShortName(), static pair => pair.Value);
    }

    public static Dictionary<string, string> ToStringDictionary<TEnum>(this IEnumerable<TEnum> source)
        where TEnum : struct, Enum
    {
        Guard.IsNotNull(source);

        return source.ToDictionary(static element => element.ToShortName(), static element => element.ToString());
    }

    public static Dictionary<string, TValue> ToPatternKeyedDictionary<TKey, TValue>(
        this IEnumerable<(TKey Key, TValue Value)> source)
        where TKey : struct, Enum
    {
        Guard.IsNotNull(source);

        return source.ToDictionary(static pair => pair.Key.ToPattern(), static pair => pair.Value);
    }

    public static Dictionary<string, string> ToPatternDictionary<TEnum>(this IEnumerable<TEnum> source)
        where TEnum : struct, Enum
    {
        Guard.IsNotNull(source);

        return source.ToDictionary(static element => element.ToPattern(), static element => element.ToString());
    }
}

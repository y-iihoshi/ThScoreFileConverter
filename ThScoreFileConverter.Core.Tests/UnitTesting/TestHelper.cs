using System;
using System.Collections.Generic;
using System.Linq;

namespace ThScoreFileConverter.Core.Tests.UnitTesting;

public static class TestHelper
{
    public static IEnumerable<object[]> GetInvalidEnumerators<TEnum>()
        where TEnum : struct, Enum
    {
        Assert.AreSame(typeof(int), Enum.GetUnderlyingType(typeof(TEnum)));

#pragma warning disable CA2021 // Don't call Enumerable.Cast<T> or Enumerable.OfType<T> with incompatible types
        var values = Enum.GetValues<TEnum>().Cast<int>().ToArray();
#pragma warning restore CA2021 // Don't call Enumerable.Cast<T> or Enumerable.OfType<T> with incompatible types

        yield return new object[] { values.Min() - 1 };
        yield return new object[] { values.Max() + 1 };
    }
}

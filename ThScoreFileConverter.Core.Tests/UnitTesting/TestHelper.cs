using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ThScoreFileConverter.Core.Tests.UnitTesting;

public static class TestHelper
{
    public static TResult Cast<TResult>(object value)
        where TResult : struct
    {
        var type = typeof(TResult);
        return type.IsEnum
            ? (TResult)Enum.ToObject(type, value)
            : (TResult)Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
    }

    public static IEnumerable<object[]> GetInvalidEnumerators(Type type)
    {
        var values = Enum.GetValues(type).Cast<int>();
        yield return new object[] { values.Min() - 1 };
        yield return new object[] { values.Max() + 1 };
    }
}

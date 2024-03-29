﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ThScoreFileConverter.Core.Tests.UnitTesting;

public static class TestHelper
{
    public static IEnumerable<object[]> GetInvalidEnumerators<TEnum>()
        where TEnum : struct, Enum
    {
        var values = Enum.GetValues(typeof(TEnum)).Cast<int>().ToArray();
        yield return new object[] { values.Min() - 1 };
        yield return new object[] { values.Max() + 1 };
    }
}

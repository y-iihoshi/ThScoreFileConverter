﻿using System.Collections.Generic;
using System.Linq;

namespace ThScoreFileConverter.Core.Tests.UnitTesting;

public static class CollectionAssertExtensions
{
    public static void AreEqual<T>(this CollectionAssert _, IEnumerable<T> expected, IEnumerable<T> actual)
    {
        CollectionAssert.AreEqual(expected?.ToArray(), actual?.ToArray());
    }

    public static void AreNotEqual<T>(this CollectionAssert _, IEnumerable<T> expected, IEnumerable<T> actual)
    {
        CollectionAssert.AreNotEqual(expected?.ToArray(), actual?.ToArray());
    }
}

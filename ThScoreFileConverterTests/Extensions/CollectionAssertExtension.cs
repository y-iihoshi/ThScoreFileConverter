using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ThScoreFileConverterTests.Extensions
{
    public static class CollectionAssertExtension
    {
        public static void AreEqual<T>(this CollectionAssert _, IEnumerable<T> expected, IEnumerable<T> actual)
            => CollectionAssert.AreEqual(expected?.ToArray(), actual?.ToArray());

        public static void AreNotEqual<T>(this CollectionAssert _, IEnumerable<T> expected, IEnumerable<T> actual)
            => CollectionAssert.AreNotEqual(expected?.ToArray(), actual?.ToArray());
    }
}

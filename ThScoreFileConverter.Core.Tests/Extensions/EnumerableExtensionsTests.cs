using System;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Tests.UnitTesting;

#if !NET8_0_OR_GREATER
using System.Collections.Generic;
using System.Linq;
#endif

namespace ThScoreFileConverter.Core.Tests.Extensions;

[TestClass]
public class EnumerableExtensionsTests
{
#if !NET8_0_OR_GREATER
    [TestMethod]
    public void ToDictionaryKeyValuePairTest()
    {
        var pairs = new[]
        {
            new KeyValuePair<int, string>(123, "abc"),
            new KeyValuePair<int, string>(456, "def"),
        };

        var dictionary = pairs.ToDictionary();

        Assert.AreEqual(pairs.Length, dictionary.Count);
        foreach (var pair in pairs)
        {
            Assert.IsTrue(dictionary.ContainsKey(pair.Key));
            Assert.AreEqual(pair.Value, dictionary[pair.Key]);
        }
    }

    [TestMethod]
    public void ToDictionaryKeyValuePairTestNull()
    {
        IEnumerable<KeyValuePair<int, string>> pairs = null!;

        _ = Assert.ThrowsException<ArgumentNullException>(() => pairs.ToDictionary());
    }

    [TestMethod]
    public void ToDictionaryPairTest()
    {
        var pairs = new[]
        {
            (123, "abc"),
            (456, "def"),
        };

        var dictionary = pairs.ToDictionary();

        Assert.AreEqual(pairs.Length, dictionary.Count);
        foreach (var pair in pairs)
        {
            Assert.IsTrue(dictionary.ContainsKey(pair.Item1));
            Assert.AreEqual(pair.Item2, dictionary[pair.Item1]);
        }
    }

    [TestMethod]
    public void ToDictionaryPairTestNull()
    {
        IEnumerable<(int, string)> pairs = null!;

        _ = Assert.ThrowsException<ArgumentNullException>(() => pairs.ToDictionary());
    }
#endif

    [TestMethod]
    public void CartesianTest()
    {
        var first = new[] { 1, 2 };
        var second = new[] { 3, 4, 5 };
        var expected = new[] { (1, 3), (1, 4), (1, 5), (2, 3), (2, 4), (2, 5) };

        CollectionAssert.That.AreEqual(expected, first.Cartesian(second));
    }

    [TestMethod]
    public void CartesianTestFirstNull()
    {
        int[] first = null!;
        var second = new[] { 3, 4, 5 };

        _ = Assert.ThrowsException<ArgumentNullException>(() => first.Cartesian(second));
    }

    [TestMethod]
    public void CartesianTestSecondNull()
    {
        var first = new[] { 1, 2 };
        int[] second = null!;

        _ = Assert.ThrowsException<ArgumentNullException>(() => first.Cartesian(second));
    }
}

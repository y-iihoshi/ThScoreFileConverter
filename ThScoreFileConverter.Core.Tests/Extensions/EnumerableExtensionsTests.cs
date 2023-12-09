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
#if NETFRAMEWORK
    [TestMethod]
    public void SkipLastTest()
    {
        var array = new int[] { 1, 2, 3, 4, 5 };
        var expected = array.Take(3);
        var actual = array.SkipLast(2);
        CollectionAssert.That.AreEqual(expected, actual);
    }

    [TestMethod]
    public void SkipLastTestNull()
    {
        int[] array = null!;
        _ = Assert.ThrowsException<ArgumentNullException>(() => array.SkipLast(2));
    }

    [TestMethod]
    public void SkipLastTestNegative()
    {
        var array = new int[] { 1, 2, 3, 4, 5 };
        var actual = array.SkipLast(-1);
        CollectionAssert.That.AreEqual(array, actual);
    }

    [TestMethod]
    public void SkipLastTestZero()
    {
        var array = new int[] { 1, 2, 3, 4, 5 };
        var actual = array.SkipLast(0);
        CollectionAssert.That.AreEqual(array, actual);
    }

    [TestMethod]
    public void SkipLastTestExceeded()
    {
        var array = new int[] { 1, 2, 3, 4, 5 };
        var expected = Array.Empty<int>();
        var actual = array.SkipLast(array.Length + 1);
        CollectionAssert.That.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TakeLastTest()
    {
        var array = new int[] { 1, 2, 3, 4, 5 };
        var expected = array.Skip(3);
        var actual = array.TakeLast(2);
        CollectionAssert.That.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TakeLastTestNull()
    {
        int[] array = null!;
        _ = Assert.ThrowsException<ArgumentNullException>(() => array.TakeLast(2));
    }

    [TestMethod]
    public void TakeLastTestNegative()
    {
        var array = new int[] { 1, 2, 3, 4, 5 };
        var expected = Array.Empty<int>();
        var actual = array.TakeLast(-1);
        CollectionAssert.That.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TakeLastTestZero()
    {
        var array = new int[] { 1, 2, 3, 4, 5 };
        var expected = Array.Empty<int>();
        var actual = array.TakeLast(0);
        CollectionAssert.That.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TakeLastTestExceeded()
    {
        var array = new int[] { 1, 2, 3, 4, 5 };
        var actual = array.TakeLast(array.Length + 1);
        CollectionAssert.That.AreEqual(array, actual);
    }
#endif

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

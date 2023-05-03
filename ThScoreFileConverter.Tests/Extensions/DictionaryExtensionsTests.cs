#if NETFRAMEWORK

using System;
using System.Collections.Generic;
using ThScoreFileConverter.Extensions;

namespace ThScoreFileConverter.Tests.Extensions;

[TestClass]
public class DictionaryExtensionsTests
{
    [TestMethod]
    public void TryAddTest()
    {
        var dictionary = new Dictionary<int, int>();
        var added = dictionary.TryAdd(1, 2);
        Assert.IsTrue(added);
        Assert.AreEqual(2, dictionary[1]);
    }

    [TestMethod]
    public void TryAddTestNull()
    {
        Dictionary<int, int> dictionary = null!;
        _ = Assert.ThrowsException<ArgumentNullException>(() => dictionary.TryAdd(1, 2));
    }

    [TestMethod]
    public void TryAddTestTwice()
    {
        var dictionary = new Dictionary<int, int> { { 1, 2 } };
        var added = dictionary.TryAdd(1, 3);
        Assert.IsFalse(added);
        Assert.AreEqual(2, dictionary[1]);
    }
}

#endif

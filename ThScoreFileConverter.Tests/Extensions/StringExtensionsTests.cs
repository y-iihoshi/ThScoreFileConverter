using System;
using ThScoreFileConverter.Extensions;

namespace ThScoreFileConverter.Tests.Extensions;

[TestClass]
public class StringExtensionsTests
{
    [TestMethod]
    public void ToNonNullStringTest()
    {
        var value = 12;
#pragma warning disable CA1305 // Specify IFormatProvider
        Assert.AreEqual(value.ToString(), value.ToNonNullString());
#pragma warning restore CA1305 // Specify IFormatProvider
    }

    [TestMethod]
    public void ToNonNullStringTestNullObj()
    {
        object obj = null!;
        Assert.AreEqual("(null)", obj.ToNonNullString());
    }

    [TestMethod]
    public void ToNonNullStringTestNonNullStr()
    {
        object obj = null!;
        var nonNullStr = "null";
        Assert.AreEqual(nonNullStr, obj.ToNonNullString(nonNullStr));
    }

    [TestMethod]
    public void ToNonNullStringTestNullStr()
    {
        var value = 12;
        _ = Assert.ThrowsException<ArgumentNullException>(() => value.ToNonNullString(null!));
    }
}

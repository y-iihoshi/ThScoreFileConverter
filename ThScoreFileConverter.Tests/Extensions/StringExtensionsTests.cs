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
        value.ToNonNullString().ShouldBe(value.ToString());
#pragma warning restore CA1305 // Specify IFormatProvider
    }

    [TestMethod]
    public void ToNonNullStringTestNullObj()
    {
        object obj = null!;
        obj.ToNonNullString().ShouldBe("(null)");
    }

    [TestMethod]
    public void ToNonNullStringTestNonNullStr()
    {
        object obj = null!;
        var nonNullStr = "null";
        obj.ToNonNullString(nonNullStr).ShouldBe(nonNullStr);
    }

    [TestMethod]
    public void ToNonNullStringTestNullStr()
    {
        var value = 12;
        _ = Should.Throw<ArgumentNullException>(() => value.ToNonNullString(null!));
    }
}

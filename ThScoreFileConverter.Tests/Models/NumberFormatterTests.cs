using System.Globalization;
using NSubstitute;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverter.Tests.Models;

[TestClass]
public class NumberFormatterTests
{
    private static INumberFormatter MockNumberFormatter()
    {
        // NOTE: NSubstitute v5.0.0 has no substitute for Moq's It.IsAny<It.IsValueType>.
        var mock = Substitute.For<INumberFormatter>();
        _ = mock.FormatNumber(Arg.Any<ushort>()).Returns(callInfo => $"invoked: {(ushort)callInfo[0]}");
        _ = mock.FormatNumber(Arg.Any<int>()).Returns(callInfo => $"invoked: {(int)callInfo[0]}");
        _ = mock.FormatNumber(Arg.Any<uint>()).Returns(callInfo => $"invoked: {(uint)callInfo[0]}");
        _ = mock.FormatNumber(Arg.Any<long>()).Returns(callInfo => $"invoked: {(long)callInfo[0]}");
        _ = mock.FormatPercent(Arg.Any<double>(), Arg.Any<int>())
            .Returns(callInfo => $"invoked: {((double)callInfo[0]).ToString($"F{(int)callInfo[1]}", CultureInfo.InvariantCulture)}%");
        return mock;
    }

    internal static INumberFormatter Mock { get; } = MockNumberFormatter();

    [TestMethod]
    public void FormatNumberTest()
    {
        var mock = Substitute.For<ISettings>();

        var formatter = new NumberFormatter(mock);
        Assert.AreEqual("12345678", formatter.FormatNumber(12345678));
    }

    [TestMethod]
    public void FormatNumberTestWithSeparator()
    {
        var mock = Substitute.For<ISettings>();
        _ = mock.OutputNumberGroupSeparator.Returns(true);

        var formatter = new NumberFormatter(mock);
        Assert.AreEqual("12,345,678", formatter.FormatNumber(12345678));
    }

    [TestMethod]
    public void FormatNumberTestWithoutSeparator()
    {
        var mock = Substitute.For<ISettings>();
        _ = mock.OutputNumberGroupSeparator.Returns(false);

        var formatter = new NumberFormatter(mock);
        Assert.AreEqual("12345678", formatter.FormatNumber(12345678));
    }

    [TestMethod]
    public void FormatPercentTest()
    {
        var mock = Substitute.For<ISettings>();

        var formatter = new NumberFormatter(mock);
        Assert.AreEqual("12.35%", formatter.FormatPercent(12.345, 2));
    }

    [TestMethod]
    public void FormatPercentTestNegativePrecision()
    {
        var mock = Substitute.For<ISettings>();

        var formatter = new NumberFormatter(mock);
        _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => formatter.FormatPercent(12.345, -1));
    }

    [TestMethod]
    public void FormatPercentTestZeroPrecision()
    {
        var mock = Substitute.For<ISettings>();

        var formatter = new NumberFormatter(mock);
        Assert.AreEqual("12%", formatter.FormatPercent(12.345, 0));
    }

    [TestMethod]
    public void FormatPercentTestExceededPrecision()
    {
        var mock = Substitute.For<ISettings>();

        var formatter = new NumberFormatter(mock);
        _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => formatter.FormatPercent(12.345, 100));
    }
}

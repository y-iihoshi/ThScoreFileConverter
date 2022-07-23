using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverter.Tests.Models;

[TestClass]
public class NumberFormatterTests
{

    [TestMethod]
    public void FormatNumberTest()
    {
        var mock = new Mock<ISettings>();

        var formatter = new NumberFormatter(mock.Object);
        Assert.AreEqual("12345678", formatter.FormatNumber(12345678));
    }

    [TestMethod]
    public void FormatNumberTestWithSeparator()
    {
        var mock = new Mock<ISettings>();
        _ = mock.Setup(s => s.OutputNumberGroupSeparator).Returns(true);

        var formatter = new NumberFormatter(mock.Object);
        Assert.AreEqual("12,345,678", formatter.FormatNumber(12345678));
    }

    [TestMethod]
    public void FormatNumberTestWithoutSeparator()
    {
        var mock = new Mock<ISettings>();
        _ = mock.Setup(s => s.OutputNumberGroupSeparator).Returns(false);

        var formatter = new NumberFormatter(mock.Object);
        Assert.AreEqual("12345678", formatter.FormatNumber(12345678));
    }

    [TestMethod]
    public void FormatPercentTest()
    {
        var mock = new Mock<ISettings>();

        var formatter = new NumberFormatter(mock.Object);
        Assert.AreEqual("12.35%", formatter.FormatPercent(12.345, 2));
    }

    [TestMethod]
    public void FormatPercentTestNegativePrecision()
    {
        var mock = new Mock<ISettings>();

        var formatter = new NumberFormatter(mock.Object);
        _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => formatter.FormatPercent(12.345, -1));
    }

    [TestMethod]
    public void FormatPercentTestZeroPrecision()
    {
        var mock = new Mock<ISettings>();

        var formatter = new NumberFormatter(mock.Object);
        Assert.AreEqual("12%", formatter.FormatPercent(12.345, 0));
    }

    [TestMethod]
    public void FormatPercentTestExceededPrecision()
    {
        var mock = new Mock<ISettings>();

        var formatter = new NumberFormatter(mock.Object);
        _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => formatter.FormatPercent(12.345, 100));
    }
}

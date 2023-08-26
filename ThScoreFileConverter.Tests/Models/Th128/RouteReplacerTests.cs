using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th128;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th128;

namespace ThScoreFileConverter.Tests.Models.Th128;

[TestClass]
public class RouteReplacerTests
{
    private static IEnumerable<IClearData> CreateClearDataList()
    {
        var levels = EnumHelper<Level>.Enumerable;

        var clearData1 = Substitute.For<IClearData>();
        _ = clearData1.Route.Returns(RouteWithTotal.A2);
        _ = clearData1.TotalPlayCount.Returns(23);
        _ = clearData1.PlayTime.Returns(4567890);
        _ = clearData1.ClearCounts.Returns(levels.ToDictionary(level => level, level => 100 - (int)level));

        var clearData2 = Substitute.For<IClearData>();
        _ = clearData2.Route.Returns(RouteWithTotal.B1);
        _ = clearData2.TotalPlayCount.Returns(12);
        _ = clearData2.PlayTime.Returns(3456789);
        _ = clearData2.ClearCounts.Returns(levels.ToDictionary(level => level, level => 50 - (int)level));

        return new[] { clearData1, clearData2 };
    }

    internal static IReadOnlyDictionary<RouteWithTotal, IClearData> ClearDataDictionary { get; } =
        CreateClearDataList().ToDictionary(clearData => clearData.Route);

    private static INumberFormatter MockNumberFormatter()
    {
        // NOTE: NSubstitute v5.0.0 has no substitute for Moq's It.IsAny<It.IsValueType>.
        var mock = Substitute.For<INumberFormatter>();
        _ = mock.FormatNumber(Arg.Any<long>()).Returns(callInfo => $"invoked: {(long)callInfo[0]}");
        return mock;
    }

    [TestMethod]
    public void RouteReplacerTest()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new RouteReplacer(ClearDataDictionary, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void RouteReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<RouteWithTotal, IClearData>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new RouteReplacer(dictionary, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestTotalPlayCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new RouteReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 23", replacer.Replace("%T128ROUTEA21"));
    }

    [TestMethod]
    public void ReplaceTestPlayTime()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new RouteReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("21:08:51", replacer.Replace("%T128ROUTEA22"));
    }

    [TestMethod]
    public void ReplaceTestClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new RouteReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 490", replacer.Replace("%T128ROUTEA23"));
    }

    [TestMethod]
    public void ReplaceTestRouteTotalTotalPlayCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new RouteReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 35", replacer.Replace("%T128ROUTETL1"));
    }

    [TestMethod]
    public void ReplaceTestRouteTotalPlayTime()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new RouteReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("37:09:04", replacer.Replace("%T128ROUTETL2"));
    }

    [TestMethod]
    public void ReplaceTestRouteTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new RouteReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 730", replacer.Replace("%T128ROUTETL3"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<RouteWithTotal, IClearData>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new RouteReplacer(dictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T128ROUTEA21"));
        Assert.AreEqual("0:00:00", replacer.Replace("%T128ROUTEA22"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T128ROUTEA23"));
    }

    [TestMethod]
    public void ReplaceTestEmptyClearCounts()
    {
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Route.Returns(RouteWithTotal.A2);
        _ = clearData.ClearCounts.Returns(ImmutableDictionary<Level, int>.Empty);
        var dictionary = new[] { clearData }.ToDictionary(data => data.Route);
        var formatterMock = MockNumberFormatter();

        var replacer = new RouteReplacer(dictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T128ROUTEA23"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new RouteReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T128XXXXXA21", replacer.Replace("%T128XXXXXA21"));
    }

    [TestMethod]
    public void ReplaceTestInvalidRoute()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new RouteReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T128ROUTEXX1", replacer.Replace("%T128ROUTEXX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new RouteReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T128ROUTEA2X", replacer.Replace("%T128ROUTEA2X"));
    }
}

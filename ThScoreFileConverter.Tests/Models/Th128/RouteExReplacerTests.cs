using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th128;
using ThScoreFileConverter.Models.Th128;

namespace ThScoreFileConverter.Tests.Models.Th128;

[TestClass]
public class RouteExReplacerTests
{
    private static IClearData[] CreateClearDataList()
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

        return [clearData1, clearData2];
    }

    internal static IReadOnlyDictionary<RouteWithTotal, IClearData> ClearDataDictionary { get; } =
        CreateClearDataList().ToDictionary(clearData => clearData.Route);

    [TestMethod]
    public void RouteExReplacerTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new RouteExReplacer(ClearDataDictionary, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void RouteExReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<RouteWithTotal, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new RouteExReplacer(dictionary, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestTotalPlayCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new RouteExReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 23", replacer.Replace("%T128ROUTEEXHA21"));
    }

    [TestMethod]
    public void ReplaceTestPlayTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new RouteExReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("21:08:51", replacer.Replace("%T128ROUTEEXHA22"));
    }

    [TestMethod]
    public void ReplaceTestClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new RouteExReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 98", replacer.Replace("%T128ROUTEEXHA23"));
    }

    [TestMethod]
    public void ReplaceTestLevelTotalTotalPlayCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new RouteExReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 23", replacer.Replace("%T128ROUTEEXTA21"));
    }

    [TestMethod]
    public void ReplaceTestLevelTotalPlayTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new RouteExReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("21:08:51", replacer.Replace("%T128ROUTEEXTA22"));
    }

    [TestMethod]
    public void ReplaceTestLevelTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new RouteExReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 490", replacer.Replace("%T128ROUTEEXTA23"));
    }

    [TestMethod]
    public void ReplaceTestRouteTotalTotalPlayCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new RouteExReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 35", replacer.Replace("%T128ROUTEEXHTL1"));
    }

    [TestMethod]
    public void ReplaceTestRouteTotalPlayTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new RouteExReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("37:09:04", replacer.Replace("%T128ROUTEEXHTL2"));
    }

    [TestMethod]
    public void ReplaceTestRouteTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new RouteExReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 146", replacer.Replace("%T128ROUTEEXHTL3"));
    }

    [TestMethod]
    public void ReplaceTestTotalTotalPlayCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new RouteExReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 35", replacer.Replace("%T128ROUTEEXTTL1"));
    }

    [TestMethod]
    public void ReplaceTestTotalPlayTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new RouteExReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("37:09:04", replacer.Replace("%T128ROUTEEXTTL2"));
    }

    [TestMethod]
    public void ReplaceTestTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new RouteExReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 730", replacer.Replace("%T128ROUTEEXTTL3"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<RouteWithTotal, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new RouteExReplacer(dictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T128ROUTEEXHA21"));
        Assert.AreEqual("0:00:00", replacer.Replace("%T128ROUTEEXHA22"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T128ROUTEEXHA23"));
    }

    [TestMethod]
    public void ReplaceTestEmptyClearCounts()
    {
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Route.Returns(RouteWithTotal.A2);
        _ = clearData.ClearCounts.Returns(ImmutableDictionary<Level, int>.Empty);
        var dictionary = new[] { clearData }.ToDictionary(data => data.Route);
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new RouteExReplacer(dictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T128ROUTEEXHA23"));
    }

    [TestMethod]
    public void ReplaceTestLevelExtra()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new RouteExReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T128ROUTEEXXA21", replacer.Replace("%T128ROUTEEXXA21"));
    }

    [TestMethod]
    public void ReplaceTestRouteExtra()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new RouteExReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T128ROUTEEXHEX1", replacer.Replace("%T128ROUTEEXHEX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new RouteExReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T128XXXXXXXHA21", replacer.Replace("%T128XXXXXXXHA21"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new RouteExReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T128ROUTEEXYA21", replacer.Replace("%T128ROUTEEXYA21"));
    }

    [TestMethod]
    public void ReplaceTestInvalidRoute()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new RouteExReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T128ROUTEEXHXX1", replacer.Replace("%T128ROUTEEXHXX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new RouteExReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T128ROUTEEXHA2X", replacer.Replace("%T128ROUTEEXHA2X"));
    }
}

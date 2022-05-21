using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th128;

namespace ThScoreFileConverter.Tests.Models.Th128;

[TestClass]
public class RouteExReplacerTests
{
    private static IEnumerable<IClearData> CreateClearDataList()
    {
        var levels = EnumHelper<Level>.Enumerable;
        return new[]
        {
            Mock.Of<IClearData>(
                m => (m.Route == RouteWithTotal.A2)
                     && (m.TotalPlayCount == 23)
                     && (m.PlayTime == 4567890)
                     && (m.ClearCounts == levels.ToDictionary(level => level, level => 100 - (int)level))),
            Mock.Of<IClearData>(
                m => (m.Route == RouteWithTotal.B1)
                     && (m.TotalPlayCount == 12)
                     && (m.PlayTime == 3456789)
                     && (m.ClearCounts == levels.ToDictionary(level => level, level => 50 - (int)level))),
        };
    }

    internal static IReadOnlyDictionary<RouteWithTotal, IClearData> ClearDataDictionary { get; } =
        CreateClearDataList().ToDictionary(clearData => clearData.Route);

    private static Mock<INumberFormatter> MockNumberFormatter()
    {
        var mock = new Mock<INumberFormatter>();
        _ = mock.Setup(formatter => formatter.FormatNumber(It.IsAny<It.IsValueType>()))
            .Returns((object value) => "invoked: " + value.ToString());
        return mock;
    }

    [TestMethod]
    public void RouteExReplacerTest()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new RouteExReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void RouteExReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<RouteWithTotal, IClearData>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new RouteExReplacer(dictionary, formatterMock.Object);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestTotalPlayCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new RouteExReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 23", replacer.Replace("%T128ROUTEEXHA21"));
    }

    [TestMethod]
    public void ReplaceTestPlayTime()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new RouteExReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("21:08:51", replacer.Replace("%T128ROUTEEXHA22"));
    }

    [TestMethod]
    public void ReplaceTestClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new RouteExReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 98", replacer.Replace("%T128ROUTEEXHA23"));
    }

    [TestMethod]
    public void ReplaceTestLevelTotalTotalPlayCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new RouteExReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 23", replacer.Replace("%T128ROUTEEXTA21"));
    }

    [TestMethod]
    public void ReplaceTestLevelTotalPlayTime()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new RouteExReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("21:08:51", replacer.Replace("%T128ROUTEEXTA22"));
    }

    [TestMethod]
    public void ReplaceTestLevelTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new RouteExReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 490", replacer.Replace("%T128ROUTEEXTA23"));
    }

    [TestMethod]
    public void ReplaceTestRouteTotalTotalPlayCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new RouteExReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 35", replacer.Replace("%T128ROUTEEXHTL1"));
    }

    [TestMethod]
    public void ReplaceTestRouteTotalPlayTime()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new RouteExReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("37:09:04", replacer.Replace("%T128ROUTEEXHTL2"));
    }

    [TestMethod]
    public void ReplaceTestRouteTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new RouteExReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 146", replacer.Replace("%T128ROUTEEXHTL3"));
    }

    [TestMethod]
    public void ReplaceTestTotalTotalPlayCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new RouteExReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 35", replacer.Replace("%T128ROUTEEXTTL1"));
    }

    [TestMethod]
    public void ReplaceTestTotalPlayTime()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new RouteExReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("37:09:04", replacer.Replace("%T128ROUTEEXTTL2"));
    }

    [TestMethod]
    public void ReplaceTestTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new RouteExReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 730", replacer.Replace("%T128ROUTEEXTTL3"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<RouteWithTotal, IClearData>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new RouteExReplacer(dictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T128ROUTEEXHA21"));
        Assert.AreEqual("0:00:00", replacer.Replace("%T128ROUTEEXHA22"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T128ROUTEEXHA23"));
    }

    [TestMethod]
    public void ReplaceTestEmptyClearCounts()
    {
        var dictionary = new[]
        {
            Mock.Of<IClearData>(
                m => (m.Route == RouteWithTotal.A2) && (m.ClearCounts == ImmutableDictionary<Level, int>.Empty))
        }.ToDictionary(clearData => clearData.Route);
        var formatterMock = MockNumberFormatter();

        var replacer = new RouteExReplacer(dictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T128ROUTEEXHA23"));
    }

    [TestMethod]
    public void ReplaceTestLevelExtra()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new RouteExReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T128ROUTEEXXA21", replacer.Replace("%T128ROUTEEXXA21"));
    }

    [TestMethod]
    public void ReplaceTestRouteExtra()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new RouteExReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T128ROUTEEXHEX1", replacer.Replace("%T128ROUTEEXHEX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new RouteExReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T128XXXXXXXHA21", replacer.Replace("%T128XXXXXXXHA21"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new RouteExReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T128ROUTEEXYA21", replacer.Replace("%T128ROUTEEXYA21"));
    }

    [TestMethod]
    public void ReplaceTestInvalidRoute()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new RouteExReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T128ROUTEEXHXX1", replacer.Replace("%T128ROUTEEXHXX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new RouteExReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T128ROUTEEXHA2X", replacer.Replace("%T128ROUTEEXHA2X"));
    }
}

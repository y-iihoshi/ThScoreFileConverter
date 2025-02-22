﻿using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th128;
using ThScoreFileConverter.Models.Th128;

namespace ThScoreFileConverter.Tests.Models.Th128;

[TestClass]
public class RouteReplacerTests
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
    public void RouteReplacerTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new RouteReplacer(ClearDataDictionary, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void RouteReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<RouteWithTotal, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new RouteReplacer(dictionary, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTestTotalPlayCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new RouteReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T128ROUTEA21").ShouldBe("invoked: 23");
    }

    [TestMethod]
    public void ReplaceTestPlayTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new RouteReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T128ROUTEA22").ShouldBe("21:08:51");
    }

    [TestMethod]
    public void ReplaceTestClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new RouteReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T128ROUTEA23").ShouldBe("invoked: 490");
    }

    [TestMethod]
    public void ReplaceTestRouteTotalTotalPlayCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new RouteReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T128ROUTETL1").ShouldBe("invoked: 35");
    }

    [TestMethod]
    public void ReplaceTestRouteTotalPlayTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new RouteReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T128ROUTETL2").ShouldBe("37:09:04");
    }

    [TestMethod]
    public void ReplaceTestRouteTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new RouteReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T128ROUTETL3").ShouldBe("invoked: 730");
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<RouteWithTotal, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new RouteReplacer(dictionary, formatterMock);
        replacer.Replace("%T128ROUTEA21").ShouldBe("invoked: 0");
        replacer.Replace("%T128ROUTEA22").ShouldBe("0:00:00");
        replacer.Replace("%T128ROUTEA23").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestEmptyClearCounts()
    {
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Route.Returns(RouteWithTotal.A2);
        _ = clearData.ClearCounts.Returns(ImmutableDictionary<Level, int>.Empty);
        var dictionary = new[] { clearData }.ToDictionary(data => data.Route);
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new RouteReplacer(dictionary, formatterMock);
        replacer.Replace("%T128ROUTEA23").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new RouteReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T128XXXXXA21").ShouldBe("%T128XXXXXA21");
    }

    [TestMethod]
    public void ReplaceTestInvalidRoute()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new RouteReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T128ROUTEXX1").ShouldBe("%T128ROUTEXX1");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new RouteReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T128ROUTEA2X").ShouldBe("%T128ROUTEA2X");
    }
}

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th128;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th128.StageProgress>;

namespace ThScoreFileConverter.Tests.Models.Th128;

[TestClass]
public class ScoreReplacerTests
{
    internal static IReadOnlyDictionary<RouteWithTotal, IClearData> ClearDataDictionary { get; } =
        new[] { ClearDataTests.MockClearData().Object }.ToDictionary(clearData => clearData.Route);

    private static Mock<INumberFormatter> MockNumberFormatter()
    {
        var mock = new Mock<INumberFormatter>();
        _ = mock.Setup(formatter => formatter.FormatNumber(It.IsAny<It.IsValueType>()))
            .Returns((object value) => "invoked: " + value.ToString());
        _ = mock.Setup(formatter => formatter.FormatPercent(It.IsAny<double>(), It.IsAny<int>()))
            .Returns((double value, int precision) => "invoked: " + value.ToString($"F{precision}") + "%");
        return mock;
    }

    [TestMethod]
    public void ScoreReplacerTest()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ScoreReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<RouteWithTotal, IClearData>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(dictionary, formatterMock.Object);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestName()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("Player1", replacer.Replace("%T128SCRHA221"));
    }

    [TestMethod]
    public void ReplaceTestScore()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 123446701", replacer.Replace("%T128SCRHA222"));
    }

    [TestMethod]
    public void ReplaceTestStage()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("A2 Clear", replacer.Replace("%T128SCRHA223"));
    }

    [TestMethod]
    public void ReplaceTestDateTime()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock.Object);
        var expected = DateTimeHelper.GetString(34567890);
        Assert.AreEqual(expected, replacer.Replace("%T128SCRHA224"));
    }

    [TestMethod]
    public void ReplaceTestSlowRate()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 1.200%", replacer.Replace("%T128SCRHA225"));  // really...?
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<RouteWithTotal, IClearData>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(dictionary, formatterMock.Object);
        Assert.AreEqual("--------", replacer.Replace("%T128SCRHA221"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T128SCRHA222"));
        Assert.AreEqual("-------", replacer.Replace("%T128SCRHA223"));
        Assert.AreEqual(DateTimeHelper.GetString(null), replacer.Replace("%T128SCRHA224"));
        Assert.AreEqual("-----%", replacer.Replace("%T128SCRHA225"));
    }

    [TestMethod]
    public void ReplaceTestEmptyRankings()
    {
        var dictionary = new[]
        {
            Mock.Of<IClearData>(
                m => (m.Route == RouteWithTotal.A2)
                     && (m.Rankings == ImmutableDictionary<Level, IReadOnlyList<IScoreData>>.Empty))
        }.ToDictionary(clearData => clearData.Route);
        var formatterMock = MockNumberFormatter();

        var replacer = new ScoreReplacer(dictionary, formatterMock.Object);
        Assert.AreEqual("--------", replacer.Replace("%T128SCRHA221"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T128SCRHA222"));
        Assert.AreEqual("-------", replacer.Replace("%T128SCRHA223"));
        Assert.AreEqual(DateTimeHelper.GetString(null), replacer.Replace("%T128SCRHA224"));
        Assert.AreEqual("-----%", replacer.Replace("%T128SCRHA225"));
    }

    [TestMethod]
    public void ReplaceTestEmptyRanking()
    {
        var dictionary = new[]
        {
            Mock.Of<IClearData>(
                m => (m.Route == RouteWithTotal.A2)
                     && (m.Rankings == EnumHelper<Level>.Enumerable.ToDictionary(
                        level => level,
                        level => ImmutableList<IScoreData>.Empty as IReadOnlyList<IScoreData>)))
        }.ToDictionary(clearData => clearData.Route);
        var formatterMock = MockNumberFormatter();

        var replacer = new ScoreReplacer(dictionary, formatterMock.Object);
        Assert.AreEqual("--------", replacer.Replace("%T128SCRHA221"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T128SCRHA222"));
        Assert.AreEqual("-------", replacer.Replace("%T128SCRHA223"));
        Assert.AreEqual(DateTimeHelper.GetString(null), replacer.Replace("%T128SCRHA224"));
        Assert.AreEqual("-----%", replacer.Replace("%T128SCRHA225"));
    }

    [TestMethod]
    public void ReplaceTestLevelExtra()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T128SCRXA223", replacer.Replace("%T128SCRXA223"));
    }

    [TestMethod]
    public void ReplaceTestRouteExtra()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T128SCRHEX23", replacer.Replace("%T128SCRHEX23"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T128XXXHA221", replacer.Replace("%T128XXXHA221"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T128SCRYA221", replacer.Replace("%T128SCRYA221"));
    }

    [TestMethod]
    public void ReplaceTestInvalidRoute()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T128SCRHXX21", replacer.Replace("%T128SCRHXX21"));
    }

    [TestMethod]
    public void ReplaceTestInvalidRank()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T128SCRHA2X1", replacer.Replace("%T128SCRHA2X1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T128SCRHA22X", replacer.Replace("%T128SCRHA22X"));
    }
}

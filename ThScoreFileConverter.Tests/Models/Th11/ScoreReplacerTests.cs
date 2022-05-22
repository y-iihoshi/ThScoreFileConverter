using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th11;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th11;
using IClearData = ThScoreFileConverter.Models.Th10.IClearData<ThScoreFileConverter.Core.Models.Th11.CharaWithTotal>;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th10.StageProgress>;
using StageProgress = ThScoreFileConverter.Models.Th10.StageProgress;

namespace ThScoreFileConverter.Tests.Models.Th11;

[TestClass]
public class ScoreReplacerTests
{
    internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
        new[] { ClearDataTests.MockClearData().Object }.ToDictionary(clearData => clearData.Chara);

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
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(dictionary, formatterMock.Object);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestName()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("Player1", replacer.Replace("%T11SCRHRS21"));
    }

    [TestMethod]
    public void ReplaceTestScore()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 123446701", replacer.Replace("%T11SCRHRS22"));
    }

    [TestMethod]
    public void ReplaceTestStage()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("Stage 5", replacer.Replace("%T11SCRHRS23"));
    }

    [TestMethod]
    public void ReplaceTestDateTime()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock.Object);
        var expected = DateTimeHelper.GetString(34567890);
        Assert.AreEqual(expected, replacer.Replace("%T11SCRHRS24"));
    }

    [TestMethod]
    public void ReplaceTestSlowRate()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 1.200%", replacer.Replace("%T11SCRHRS25"));  // really...?
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(dictionary, formatterMock.Object);
        Assert.AreEqual("--------", replacer.Replace("%T11SCRHRS21"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T11SCRHRS22"));
        Assert.AreEqual("-------", replacer.Replace("%T11SCRHRS23"));
        Assert.AreEqual(DateTimeHelper.GetString(null), replacer.Replace("%T11SCRHRS24"));
        Assert.AreEqual("-----%", replacer.Replace("%T11SCRHRS25"));
    }

    [TestMethod]
    public void ReplaceTestEmptyRankings()
    {
        var dictionary = new[]
        {
            Mock.Of<IClearData>(
                m => (m.Chara == CharaWithTotal.ReimuSuika)
                     && (m.Rankings == ImmutableDictionary<Level, IReadOnlyList<IScoreData>>.Empty))
        }.ToDictionary(clearData => clearData.Chara);
        var formatterMock = MockNumberFormatter();

        var replacer = new ScoreReplacer(dictionary, formatterMock.Object);
        Assert.AreEqual("--------", replacer.Replace("%T11SCRHRS21"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T11SCRHRS22"));
        Assert.AreEqual("-------", replacer.Replace("%T11SCRHRS23"));
        Assert.AreEqual(DateTimeHelper.GetString(null), replacer.Replace("%T11SCRHRS24"));
        Assert.AreEqual("-----%", replacer.Replace("%T11SCRHRS25"));
    }

    [TestMethod]
    public void ReplaceTestEmptyRanking()
    {
        var dictionary = new[]
        {
            Mock.Of<IClearData>(
                m => (m.Chara == CharaWithTotal.ReimuSuika)
                     && (m.Rankings == EnumHelper<Level>.Enumerable.ToDictionary(
                        level => level,
                        level => ImmutableList<IScoreData>.Empty as IReadOnlyList<IScoreData>)))
        }.ToDictionary(clearData => clearData.Chara);
        var formatterMock = MockNumberFormatter();

        var replacer = new ScoreReplacer(dictionary, formatterMock.Object);
        Assert.AreEqual("--------", replacer.Replace("%T11SCRHRS21"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T11SCRHRS22"));
        Assert.AreEqual("-------", replacer.Replace("%T11SCRHRS23"));
        Assert.AreEqual(DateTimeHelper.GetString(null), replacer.Replace("%T11SCRHRS24"));
        Assert.AreEqual("-----%", replacer.Replace("%T11SCRHRS25"));
    }

    [TestMethod]
    public void ReplaceTestStageExtra()
    {
        static IScoreData CreateScoreData()
        {
            var mock = new Mock<IScoreData>();
            _ = mock.SetupGet(s => s.DateTime).Returns(34567890u);
            _ = mock.SetupGet(s => s.StageProgress).Returns(StageProgress.Extra);
            return mock.Object;
        }

        static IClearData CreateClearData()
        {
            var mock = new Mock<IClearData>();
            _ = mock.SetupGet(c => c.Chara).Returns(CharaWithTotal.ReimuSuika);
            _ = mock.SetupGet(c => c.Rankings).Returns(
                EnumHelper<Level>.Enumerable.ToDictionary(
                    level => level,
                    level => Enumerable.Range(0, 10).Select(index => CreateScoreData()).ToList()
                        as IReadOnlyList<IScoreData>));
            return mock.Object;
        }

        var dictionary = new[] { CreateClearData() }.ToDictionary(clearData => clearData.Chara);
        var formatterMock = MockNumberFormatter();

        var replacer = new ScoreReplacer(dictionary, formatterMock.Object);
        Assert.AreEqual("Not Clear", replacer.Replace("%T11SCRHRS23"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T11XXXHRS21", replacer.Replace("%T11XXXHRS21"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T11SCRYRS21", replacer.Replace("%T11SCRYRS21"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T11SCRHXX21", replacer.Replace("%T11SCRHXX21"));
    }

    [TestMethod]
    public void ReplaceTestInvalidRank()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T11SCRHRSX1", replacer.Replace("%T11SCRHRSX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T11SCRHRS2X", replacer.Replace("%T11SCRHRS2X"));
    }
}

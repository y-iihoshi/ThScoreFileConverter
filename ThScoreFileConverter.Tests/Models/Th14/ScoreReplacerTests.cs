using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using Moq;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th14;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th14;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Core.Models.Th14.CharaWithTotal,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Th14.LevelPractice,
    ThScoreFileConverter.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Models.Th14.StagePractice,
    ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>>;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>;
using StageProgress = ThScoreFileConverter.Models.Th13.StageProgress;

namespace ThScoreFileConverter.Tests.Models.Th14;

[TestClass]
public class ScoreReplacerTests
{
    internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
        new[] { ClearDataTests.MockClearData().Object }.ToDictionary(clearData => clearData.Chara);

    private static Mock<INumberFormatter> MockNumberFormatter()
    {
        var mock = new Mock<INumberFormatter>();
        _ = mock.Setup(formatter => formatter.FormatNumber(It.IsAny<It.IsValueType>()))
            .Returns((object value) => $"invoked: {value}");
        _ = mock.Setup(formatter => formatter.FormatPercent(It.IsAny<double>(), It.IsAny<int>()))
            .Returns((double value, int precision) => $"invoked: {value.ToString($"F{precision}", CultureInfo.InvariantCulture)}%");
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
        Assert.AreEqual("Player1", replacer.Replace("%T14SCRHRB21"));
    }

    [TestMethod]
    public void ReplaceTestScore()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 123446701", replacer.Replace("%T14SCRHRB22"));
    }

    [TestMethod]
    public void ReplaceTestStage()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("Stage 5", replacer.Replace("%T14SCRHRB23"));
    }

    [TestMethod]
    public void ReplaceTestDateTime()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock.Object);
        var expected = DateTimeHelper.GetString(34567890);
        Assert.AreEqual(expected, replacer.Replace("%T14SCRHRB24"));
    }

    [TestMethod]
    public void ReplaceTestSlowRate()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 1.200%", replacer.Replace("%T14SCRHRB25"));  // really...?
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(dictionary, formatterMock.Object);
        Assert.AreEqual("--------", replacer.Replace("%T14SCRHRB21"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T14SCRHRB22"));
        Assert.AreEqual("-------", replacer.Replace("%T14SCRHRB23"));
        Assert.AreEqual(DateTimeHelper.GetString(null), replacer.Replace("%T14SCRHRB24"));
        Assert.AreEqual("-----%", replacer.Replace("%T14SCRHRB25"));
    }

    [TestMethod]
    public void ReplaceTestEmptyRankings()
    {
        var dictionary = new[]
        {
            Mock.Of<IClearData>(
                m => (m.Chara == CharaWithTotal.ReimuB)
                     && (m.Rankings == ImmutableDictionary<LevelPracticeWithTotal, IReadOnlyList<IScoreData>>.Empty))
        }.ToDictionary(clearData => clearData.Chara);
        var formatterMock = MockNumberFormatter();

        var replacer = new ScoreReplacer(dictionary, formatterMock.Object);
        Assert.AreEqual("--------", replacer.Replace("%T14SCRHRB21"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T14SCRHRB22"));
        Assert.AreEqual("-------", replacer.Replace("%T14SCRHRB23"));
        Assert.AreEqual(DateTimeHelper.GetString(null), replacer.Replace("%T14SCRHRB24"));
        Assert.AreEqual("-----%", replacer.Replace("%T14SCRHRB25"));
    }

    [TestMethod]
    public void ReplaceTestEmptyRanking()
    {
        var dictionary = new[]
        {
            Mock.Of<IClearData>(
                m => (m.Chara == CharaWithTotal.ReimuB)
                     && (m.Rankings == EnumHelper<LevelPracticeWithTotal>.Enumerable.ToDictionary(
                        level => level,
                        level => ImmutableList<IScoreData>.Empty as IReadOnlyList<IScoreData>)))
        }.ToDictionary(clearData => clearData.Chara);
        var formatterMock = MockNumberFormatter();

        var replacer = new ScoreReplacer(dictionary, formatterMock.Object);
        Assert.AreEqual("--------", replacer.Replace("%T14SCRHRB21"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T14SCRHRB22"));
        Assert.AreEqual("-------", replacer.Replace("%T14SCRHRB23"));
        Assert.AreEqual(DateTimeHelper.GetString(null), replacer.Replace("%T14SCRHRB24"));
        Assert.AreEqual("-----%", replacer.Replace("%T14SCRHRB25"));
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
            _ = mock.SetupGet(c => c.Chara).Returns(CharaWithTotal.ReimuB);
            _ = mock.SetupGet(c => c.Rankings).Returns(
                EnumHelper<LevelPracticeWithTotal>.Enumerable.ToDictionary(
                    level => level,
                    level => Enumerable.Range(0, 10).Select(index => CreateScoreData()).ToList()
                        as IReadOnlyList<IScoreData>));
            return mock.Object;
        }

        var dictionary = new[] { CreateClearData() }.ToDictionary(clearData => clearData.Chara);
        var formatterMock = MockNumberFormatter();

        var replacer = new ScoreReplacer(dictionary, formatterMock.Object);
        Assert.AreEqual("Not Clear", replacer.Replace("%T14SCRHRB23"));
    }

    [TestMethod]
    public void ReplaceTestStageExtraClear()
    {
        static IScoreData CreateScoreData()
        {
            var mock = new Mock<IScoreData>();
            _ = mock.SetupGet(s => s.DateTime).Returns(34567890u);
            _ = mock.SetupGet(s => s.StageProgress).Returns(StageProgress.ExtraClear);
            return mock.Object;
        }

        static IClearData CreateClearData()
        {
            var mock = new Mock<IClearData>();
            _ = mock.SetupGet(c => c.Chara).Returns(CharaWithTotal.ReimuB);
            _ = mock.SetupGet(c => c.Rankings).Returns(
                EnumHelper<LevelPracticeWithTotal>.Enumerable.ToDictionary(
                    level => level,
                    level => Enumerable.Range(0, 10).Select(index => CreateScoreData()).ToList()
                        as IReadOnlyList<IScoreData>));
            return mock.Object;
        }

        var dictionary = new[] { CreateClearData() }.ToDictionary(clearData => clearData.Chara);
        var formatterMock = MockNumberFormatter();

        var replacer = new ScoreReplacer(dictionary, formatterMock.Object);
        Assert.AreEqual("All Clear", replacer.Replace("%T14SCRHRB23"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T14XXXHRB21", replacer.Replace("%T14XXXHRB21"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T14SCRYRB21", replacer.Replace("%T14SCRYRB21"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T14SCRHXX21", replacer.Replace("%T14SCRHXX21"));
    }

    [TestMethod]
    public void ReplaceTestInvalidRank()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T14SCRHRBX1", replacer.Replace("%T14SCRHRBX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T14SCRHRB2X", replacer.Replace("%T14SCRHRB2X"));
    }
}

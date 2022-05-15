using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th15;
using GameMode = ThScoreFileConverter.Models.Th15.GameMode;
using StageProgress = ThScoreFileConverter.Models.Th13.StageProgress;

namespace ThScoreFileConverterTests.Models.Th15;

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
        Assert.AreEqual("Player1", replacer.Replace("%T15SCRPHMR21"));
    }

    [TestMethod]
    public void ReplaceTestScore()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 123446701", replacer.Replace("%T15SCRPHMR22"));
    }

    [TestMethod]
    public void ReplaceTestStage()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("Stage 5", replacer.Replace("%T15SCRPHMR23"));
    }

    [TestMethod]
    public void ReplaceTestDateTime()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock.Object);
        var expected = DateTimeHelper.GetString(34567890);
        Assert.AreEqual(expected, replacer.Replace("%T15SCRPHMR24"));
    }

    [TestMethod]
    public void ReplaceTestSlowRate()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 1.200%", replacer.Replace("%T15SCRPHMR25"));  // really...?
    }

    [TestMethod]
    public void ReplaceTestRetryCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 1", replacer.Replace("%T15SCRPHMR26"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(dictionary, formatterMock.Object);
        Assert.AreEqual("--------", replacer.Replace("%T15SCRPHMR21"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T15SCRPHMR22"));
        Assert.AreEqual("-------", replacer.Replace("%T15SCRPHMR23"));
        Assert.AreEqual(DateTimeHelper.GetString(null), replacer.Replace("%T15SCRPHMR24"));
        Assert.AreEqual("-----%", replacer.Replace("%T15SCRPHMR25"));
        Assert.AreEqual("-----", replacer.Replace("%T15SCRPHMR26"));
    }

    [TestMethod]
    public void ReplaceTestEmptyGameModes()
    {
        var dictionary = new[]
        {
            Mock.Of<IClearData>(
                m => (m.Chara == CharaWithTotal.Marisa)
                     && (m.GameModeData == ImmutableDictionary<GameMode, IClearDataPerGameMode>.Empty))
        }.ToDictionary(clearData => clearData.Chara);
        var formatterMock = MockNumberFormatter();

        var replacer = new ScoreReplacer(dictionary, formatterMock.Object);
        Assert.AreEqual("--------", replacer.Replace("%T15SCRPHMR21"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T15SCRPHMR22"));
        Assert.AreEqual("-------", replacer.Replace("%T15SCRPHMR23"));
        Assert.AreEqual(DateTimeHelper.GetString(null), replacer.Replace("%T15SCRPHMR24"));
        Assert.AreEqual("-----%", replacer.Replace("%T15SCRPHMR25"));
        Assert.AreEqual("-----", replacer.Replace("%T15SCRPHMR26"));
    }

    [TestMethod]
    public void ReplaceTestEmptyRankings()
    {
        var dictionary = new[]
        {
            Mock.Of<IClearData>(
                m => (m.Chara == CharaWithTotal.Marisa)
                     && (m.GameModeData == new Dictionary<GameMode, IClearDataPerGameMode>
                        {
                            {
                                GameMode.Pointdevice,
                                Mock.Of<IClearDataPerGameMode>(
                                    c => c.Rankings == ImmutableDictionary<LevelWithTotal, IReadOnlyList<IScoreData>>.Empty)
                            },
                        })
                )
        }.ToDictionary(element => element.Chara);
        var formatterMock = MockNumberFormatter();

        var replacer = new ScoreReplacer(dictionary, formatterMock.Object);
        Assert.AreEqual("--------", replacer.Replace("%T15SCRPHMR21"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T15SCRPHMR22"));
        Assert.AreEqual("-------", replacer.Replace("%T15SCRPHMR23"));
        Assert.AreEqual(DateTimeHelper.GetString(null), replacer.Replace("%T15SCRPHMR24"));
        Assert.AreEqual("-----%", replacer.Replace("%T15SCRPHMR25"));
        Assert.AreEqual("-----", replacer.Replace("%T15SCRPHMR26"));
    }

    [TestMethod]
    public void ReplaceTestEmptyRanking()
    {
        var dictionary = new[]
        {
            Mock.Of<IClearData>(
                m => (m.Chara == CharaWithTotal.Marisa)
                     && (m.GameModeData == new Dictionary<GameMode, IClearDataPerGameMode>
                        {
                            {
                                GameMode.Pointdevice,
                                Mock.Of<IClearDataPerGameMode>(
                                    c => c.Rankings == EnumHelper<LevelWithTotal>.Enumerable.ToDictionary(
                                        level => level,
                                        level => ImmutableList<IScoreData>.Empty as IReadOnlyList<IScoreData>))
                            },
                        }))
        }.ToDictionary(clearData => clearData.Chara);
        var formatterMock = MockNumberFormatter();

        var replacer = new ScoreReplacer(dictionary, formatterMock.Object);
        Assert.AreEqual("--------", replacer.Replace("%T15SCRPHMR21"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T15SCRPHMR22"));
        Assert.AreEqual("-------", replacer.Replace("%T15SCRPHMR23"));
        Assert.AreEqual(DateTimeHelper.GetString(null), replacer.Replace("%T15SCRPHMR24"));
        Assert.AreEqual("-----%", replacer.Replace("%T15SCRPHMR25"));
        Assert.AreEqual("-----", replacer.Replace("%T15SCRPHMR26"));
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

        static IClearDataPerGameMode CreateClearDataPerGameMode()
        {
            var mock = new Mock<IClearDataPerGameMode>();
            _ = mock.SetupGet(c => c.Rankings).Returns(
                EnumHelper<LevelWithTotal>.Enumerable.ToDictionary(
                    level => level,
                    level => Enumerable.Range(0, 10).Select(index => CreateScoreData()).ToList()
                        as IReadOnlyList<IScoreData>));
            return mock.Object;
        }

        static IClearData CreateClearData()
        {
            var mock = new Mock<IClearData>();
            _ = mock.SetupGet(c => c.Chara).Returns(CharaWithTotal.Marisa);
            _ = mock.SetupGet(c => c.GameModeData).Returns(
                new[] { (GameMode.Pointdevice, CreateClearDataPerGameMode()) }.ToDictionary());
            return mock.Object;
        }

        var dictionary = new[] { CreateClearData() }.ToDictionary(clearData => clearData.Chara);
        var formatterMock = MockNumberFormatter();

        var replacer = new ScoreReplacer(dictionary, formatterMock.Object);
        Assert.AreEqual("Not Clear", replacer.Replace("%T15SCRPHMR23"));
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

        static IClearDataPerGameMode CreateClearDataPerGameMode()
        {
            var mock = new Mock<IClearDataPerGameMode>();
            _ = mock.SetupGet(c => c.Rankings).Returns(
                EnumHelper<LevelWithTotal>.Enumerable.ToDictionary(
                    level => level,
                    level => Enumerable.Range(0, 10).Select(index => CreateScoreData()).ToList()
                        as IReadOnlyList<IScoreData>));
            return mock.Object;
        }

        static IClearData CreateClearData()
        {
            var mock = new Mock<IClearData>();
            _ = mock.SetupGet(c => c.Chara).Returns(CharaWithTotal.Marisa);
            _ = mock.SetupGet(c => c.GameModeData).Returns(
                new[] { (GameMode.Pointdevice, CreateClearDataPerGameMode()) }.ToDictionary());
            return mock.Object;
        }

        var dictionary = new[] { CreateClearData() }.ToDictionary(clearData => clearData.Chara);
        var formatterMock = MockNumberFormatter();

        var replacer = new ScoreReplacer(dictionary, formatterMock.Object);
        Assert.AreEqual("All Clear", replacer.Replace("%T15SCRPHMR23"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T15XXXPHMR21", replacer.Replace("%T15XXXPHMR21"));
    }

    [TestMethod]
    public void ReplaceTestInvalidGameMode()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T15SCRXHMR21", replacer.Replace("%T15SCRXHMR21"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T15SCRPYMR21", replacer.Replace("%T15SCRPYMR21"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T15SCRPHXX21", replacer.Replace("%T15SCRPHXX21"));
    }

    [TestMethod]
    public void ReplaceTestInvalidRank()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T15SCRPHMRX1", replacer.Replace("%T15SCRPHMRX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T15SCRPHMR2X", replacer.Replace("%T15SCRPHMR2X"));
    }
}

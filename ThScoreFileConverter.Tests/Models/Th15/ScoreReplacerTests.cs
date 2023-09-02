using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th15;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models.Th15;
using GameMode = ThScoreFileConverter.Core.Models.Th15.GameMode;
using StageProgress = ThScoreFileConverter.Models.Th13.StageProgress;

namespace ThScoreFileConverter.Tests.Models.Th15;

[TestClass]
public class ScoreReplacerTests
{
    internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
        new[] { ClearDataTests.MockClearData() }.ToDictionary(clearData => clearData.Chara);

    [TestMethod]
    public void ScoreReplacerTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ScoreReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(dictionary, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestName()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("Player1", replacer.Replace("%T15SCRPHMR21"));
    }

    [TestMethod]
    public void ReplaceTestScore()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 123446701", replacer.Replace("%T15SCRPHMR22"));
    }

    [TestMethod]
    public void ReplaceTestStage()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("Stage 5", replacer.Replace("%T15SCRPHMR23"));
    }

    [TestMethod]
    public void ReplaceTestDateTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        var expected = DateTimeHelper.GetString(34567890);
        Assert.AreEqual(expected, replacer.Replace("%T15SCRPHMR24"));
    }

    [TestMethod]
    public void ReplaceTestSlowRate()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 1.200%", replacer.Replace("%T15SCRPHMR25"));  // really...?
    }

    [TestMethod]
    public void ReplaceTestRetryCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 1", replacer.Replace("%T15SCRPHMR26"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(dictionary, formatterMock);
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
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.Marisa);
        _ = clearData.GameModeData.Returns(ImmutableDictionary<GameMode, IClearDataPerGameMode>.Empty);
        var dictionary = new[] { clearData }.ToDictionary(data => data.Chara);
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new ScoreReplacer(dictionary, formatterMock);
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
        var clearDataPerGameMode = Substitute.For<IClearDataPerGameMode>();
        _ = clearDataPerGameMode.Rankings.Returns(ImmutableDictionary<LevelWithTotal, IReadOnlyList<IScoreData>>.Empty);
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.Marisa);
        _ = clearData.GameModeData.Returns(new[] { (GameMode.Pointdevice, clearDataPerGameMode) }.ToDictionary());
        var dictionary = new[] { clearData }.ToDictionary(data => data.Chara);
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new ScoreReplacer(dictionary, formatterMock);
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
        var clearDataPerGameMode = Substitute.For<IClearDataPerGameMode>();
        _ = clearDataPerGameMode.Rankings.Returns(
            EnumHelper<LevelWithTotal>.Enumerable.ToDictionary(
                level => level,
                level => ImmutableList<IScoreData>.Empty as IReadOnlyList<IScoreData>));
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.Marisa);
        _ = clearData.GameModeData.Returns(new[] { (GameMode.Pointdevice, clearDataPerGameMode) }.ToDictionary());
        var dictionary = new[] { clearData }.ToDictionary(data => data.Chara);
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new ScoreReplacer(dictionary, formatterMock);
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
        static IScoreData MockScoreData()
        {
            var mock = Substitute.For<IScoreData>();
            _ = mock.DateTime.Returns(34567890u);
            _ = mock.StageProgress.Returns(StageProgress.Extra);
            return mock;
        }

        static IClearDataPerGameMode MockClearDataPerGameMode()
        {
            var rankings = EnumHelper<LevelWithTotal>.Enumerable.ToDictionary(
                level => level,
                level => Enumerable.Range(0, 10).Select(index => MockScoreData()).ToList() as IReadOnlyList<IScoreData>);
            var mock = Substitute.For<IClearDataPerGameMode>();
            _ = mock.Rankings.Returns(rankings);
            return mock;
        }

        static IClearData MockClearData()
        {
            var gameModeData = new[] { (GameMode.Pointdevice, MockClearDataPerGameMode()) }.ToDictionary();
            var mock = Substitute.For<IClearData>();
            _ = mock.Chara.Returns(CharaWithTotal.Marisa);
            _ = mock.GameModeData.Returns(gameModeData);
            return mock;
        }

        var dictionary = new[] { MockClearData() }.ToDictionary(clearData => clearData.Chara);
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new ScoreReplacer(dictionary, formatterMock);
        Assert.AreEqual("Not Clear", replacer.Replace("%T15SCRPHMR23"));
    }

    [TestMethod]
    public void ReplaceTestStageExtraClear()
    {
        static IScoreData MockScoreData()
        {
            var mock = Substitute.For<IScoreData>();
            _ = mock.DateTime.Returns(34567890u);
            _ = mock.StageProgress.Returns(StageProgress.ExtraClear);
            return mock;
        }

        static IClearDataPerGameMode MockClearDataPerGameMode()
        {
            var rankings = EnumHelper<LevelWithTotal>.Enumerable.ToDictionary(
                level => level,
                level => Enumerable.Range(0, 10).Select(index => MockScoreData()).ToList() as IReadOnlyList<IScoreData>);
            var mock = Substitute.For<IClearDataPerGameMode>();
            _ = mock.Rankings.Returns(rankings);
            return mock;
        }

        static IClearData MockClearData()
        {
            var gameModeData = new[] { (GameMode.Pointdevice, MockClearDataPerGameMode()) }.ToDictionary();
            var mock = Substitute.For<IClearData>();
            _ = mock.Chara.Returns(CharaWithTotal.Marisa);
            _ = mock.GameModeData.Returns(gameModeData);
            return mock;
        }

        var dictionary = new[] { MockClearData() }.ToDictionary(clearData => clearData.Chara);
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new ScoreReplacer(dictionary, formatterMock);
        Assert.AreEqual("All Clear", replacer.Replace("%T15SCRPHMR23"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T15XXXPHMR21", replacer.Replace("%T15XXXPHMR21"));
    }

    [TestMethod]
    public void ReplaceTestInvalidGameMode()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T15SCRXHMR21", replacer.Replace("%T15SCRXHMR21"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T15SCRPYMR21", replacer.Replace("%T15SCRPYMR21"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T15SCRPHXX21", replacer.Replace("%T15SCRPHXX21"));
    }

    [TestMethod]
    public void ReplaceTestInvalidRank()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T15SCRPHMRX1", replacer.Replace("%T15SCRPHMRX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T15SCRPHMR2X", replacer.Replace("%T15SCRPHMR2X"));
    }
}

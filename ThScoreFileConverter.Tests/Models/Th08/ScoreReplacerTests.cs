using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th08;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th08;
using IHighScore = ThScoreFileConverter.Models.Th08.IHighScore;

namespace ThScoreFileConverter.Tests.Models.Th08;

[TestClass]
public class ScoreReplacerTests
{
    internal static IReadOnlyDictionary<(Chara, Level), IReadOnlyList<IHighScore>> Rankings { get; } =
        new[] { new[] { HighScoreTests.MockHighScore() } }.ToDictionary(
            ranking => (ranking[0].Chara, ranking[0].Level), ranking => ranking as IReadOnlyList<IHighScore>);

    private static INumberFormatter MockNumberFormatter()
    {
        // NOTE: NSubstitute v5.0.0 has no substitute for Moq's It.IsAny<It.IsValueType>.
        var mock = Substitute.For<INumberFormatter>();
        _ = mock.FormatNumber(Arg.Any<ushort>()).Returns(callInfo => $"invoked: {(ushort)callInfo[0]}");
        _ = mock.FormatNumber(Arg.Any<int>()).Returns(callInfo => $"invoked: {(int)callInfo[0]}");
        _ = mock.FormatNumber(Arg.Any<uint>()).Returns(callInfo => $"invoked: {(uint)callInfo[0]}");
        _ = mock.FormatPercent(Arg.Any<double>(), Arg.Any<int>())
            .Returns(callInfo => $"invoked: {((double)callInfo[0]).ToString($"F{(int)callInfo[1]}", CultureInfo.InvariantCulture)}%");
        return mock;
    }

    [TestMethod]
    public void ScoreReplacerTest()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ScoreReplacerTestEmptyRankings()
    {
        var rankings = ImmutableDictionary<(Chara, Level), IReadOnlyList<IHighScore>>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(rankings, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ScoreReplacerTestEmptyScores()
    {
        var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>
        {
            { Rankings.First().Key, ImmutableList<IHighScore>.Empty },
        };
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(rankings, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestName()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("Player1", replacer.Replace("%T08SCRHMA11"));
    }

    [TestMethod]
    public void ReplaceTestScore()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);

        Assert.AreEqual("invoked: 12345672", replacer.Replace("%T08SCRHMA12"));
    }

    [TestMethod]
    public void ReplaceTestStage()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("Stage 3", replacer.Replace("%T08SCRHMA13"));
    }

    [TestMethod]
    public void ReplaceTestStageExtra()
    {
        var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>
        {
            { (Chara.MarisaAlice, Level.Extra), ImmutableList<IHighScore>.Empty },
        };
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(rankings, formatterMock);
        Assert.AreEqual("Extra Stage", replacer.Replace("%T08SCRXMA13"));
    }

    [TestMethod]
    public void ReplaceTestDate()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("01/23", replacer.Replace("%T08SCRHMA14"));
    }

    [TestMethod]
    public void ReplaceTestSlowRate()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("invoked: 9.870%", replacer.Replace("%T08SCRHMA15"));
    }

    [TestMethod]
    public void ReplaceTestPlayTime()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("4:34:20", replacer.Replace("%T08SCRHMA16"));
    }

    [TestMethod]
    public void ReplaceTestPlayerNum()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("invoked: 6", replacer.Replace("%T08SCRHMA17"));
    }

    [TestMethod]
    public void ReplaceTestPointItem()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);

        Assert.AreEqual("invoked: 1234", replacer.Replace("%T08SCRHMA18"));
    }

    [TestMethod]
    public void ReplaceTestTimePoint()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);

        Assert.AreEqual("invoked: 65432", replacer.Replace("%T08SCRHMA19"));
    }

    [TestMethod]
    public void ReplaceTestMissCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("invoked: 9", replacer.Replace("%T08SCRHMA10"));
    }

    [TestMethod]
    public void ReplaceTestBombCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("invoked: 6", replacer.Replace("%T08SCRHMA1A"));
    }

    [TestMethod]
    public void ReplaceTestLastSpellCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("invoked: 12", replacer.Replace("%T08SCRHMA1B"));
    }

    [TestMethod]
    public void ReplaceTestPauseCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("invoked: 3", replacer.Replace("%T08SCRHMA1C"));
    }

    [TestMethod]
    public void ReplaceTestContinueCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("invoked: 2", replacer.Replace("%T08SCRHMA1D"));
    }

    [TestMethod]
    public void ReplaceTestHumanRate()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("invoked: 78.90%", replacer.Replace("%T08SCRHMA1E"));
    }

    [TestMethod]
    public void ReplaceTestGotSpellCards()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual(
            $"No.003 灯符「ファイヤフライフェノメノン」{Environment.NewLine}No.007 蠢符「リトルバグ」",
            replacer.Replace("%T08SCRHMA1F"));
    }

    [TestMethod]
    public void ReplaceTestGotNoSpellCards()
    {
        var mock = HighScoreTests.MockHighScore();
        _ = mock.CardFlags.Returns(Enumerable.Range(1, 222).ToDictionary(id => id, _ => (byte)0));
        var rankings = new[] { new[] { mock } }.ToDictionary(
            ranking => (ranking[0].Chara, ranking[0].Level), ranking => ranking as IReadOnlyList<IHighScore>);
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(rankings, formatterMock);
        Assert.AreEqual(string.Empty, replacer.Replace("%T08SCRHMA1F"));
    }

    [TestMethod]
    public void ReplaceTestGotNonexistentSpellCards()
    {
        var mock = HighScoreTests.MockHighScore();
        _ = mock.CardFlags.Returns(new Dictionary<int, byte> { { 223, 1 } });
        var rankings = new[] { new[] { mock } }.ToDictionary(
            ranking => (ranking[0].Chara, ranking[0].Level), ranking => ranking as IReadOnlyList<IHighScore>);
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(rankings, formatterMock);
        Assert.AreEqual(string.Empty, replacer.Replace("%T08SCRHMA1F"));
    }

    [TestMethod]
    public void ReplaceTestNumGotSpellCards()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("invoked: 2", replacer.Replace("%T08SCRHMA1G"));
    }

    [TestMethod]
    public void ReplaceTestEmptyRankings()
    {
        var rankings = ImmutableDictionary<(Chara, Level), IReadOnlyList<IHighScore>>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(rankings, formatterMock);
        Assert.AreEqual("--------", replacer.Replace("%T08SCRHMA11"));
    }

    [TestMethod]
    public void ReplaceTestEmptyScores()
    {
        var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>
        {
            { Rankings.First().Key, ImmutableList<IHighScore>.Empty },
        };
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(rankings, formatterMock);
        Assert.AreEqual("--------", replacer.Replace("%T08SCRHMA11"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("--------", replacer.Replace("%T08SCRHRY11"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentLevel()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("--------", replacer.Replace("%T08SCRNMA11"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentRank()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("--------", replacer.Replace("%T08SCRHMA21"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("%T08XXXHMA11", replacer.Replace("%T08XXXHMA11"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("%T08SCRYMA11", replacer.Replace("%T08SCRYMA11"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("%T08SCRHXX11", replacer.Replace("%T08SCRHXX11"));
    }

    [TestMethod]
    public void ReplaceTestInvalidRank()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("%T08SCRHMAX1", replacer.Replace("%T08SCRHMAX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("%T08SCRHMA1X", replacer.Replace("%T08SCRHMA1X"));
    }
}

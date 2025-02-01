using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th08;
using ThScoreFileConverter.Models.Th08;
using IHighScore = ThScoreFileConverter.Models.Th08.IHighScore;

namespace ThScoreFileConverter.Tests.Models.Th08;

[TestClass]
public class ScoreReplacerTests
{
    internal static IReadOnlyDictionary<(Chara, Level), IReadOnlyList<IHighScore>> Rankings { get; } =
        new[] { new[] { HighScoreTests.MockHighScore() } }.ToDictionary(
            ranking => (ranking[0].Chara, ranking[0].Level), ranking => ranking as IReadOnlyList<IHighScore>);

    [TestMethod]
    public void ScoreReplacerTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ScoreReplacerTestEmptyRankings()
    {
        var rankings = ImmutableDictionary<(Chara, Level), IReadOnlyList<IHighScore>>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(rankings, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ScoreReplacerTestEmptyScores()
    {
        var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>
        {
            { Rankings.First().Key, ImmutableList<IHighScore>.Empty },
        };
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(rankings, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTestName()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        replacer.Replace("%T08SCRHMA11").ShouldBe("Player1");
    }

    [TestMethod]
    public void ReplaceTestScore()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);

        replacer.Replace("%T08SCRHMA12").ShouldBe("invoked: 12345672");
    }

    [TestMethod]
    public void ReplaceTestStage()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        replacer.Replace("%T08SCRHMA13").ShouldBe("Stage 3");
    }

    [TestMethod]
    public void ReplaceTestStageExtra()
    {
        var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>
        {
            { (Chara.MarisaAlice, Level.Extra), ImmutableList<IHighScore>.Empty },
        };
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(rankings, formatterMock);
        replacer.Replace("%T08SCRXMA13").ShouldBe("Extra Stage");
    }

    [TestMethod]
    public void ReplaceTestDate()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        replacer.Replace("%T08SCRHMA14").ShouldBe("01/23");
    }

    [TestMethod]
    public void ReplaceTestSlowRate()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        replacer.Replace("%T08SCRHMA15").ShouldBe("invoked: 9.870%");
    }

    [TestMethod]
    public void ReplaceTestPlayTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        replacer.Replace("%T08SCRHMA16").ShouldBe("4:34:20");
    }

    [TestMethod]
    public void ReplaceTestPlayerNum()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        replacer.Replace("%T08SCRHMA17").ShouldBe("invoked: 6");
    }

    [TestMethod]
    public void ReplaceTestPointItem()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);

        replacer.Replace("%T08SCRHMA18").ShouldBe("invoked: 1234");
    }

    [TestMethod]
    public void ReplaceTestTimePoint()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);

        replacer.Replace("%T08SCRHMA19").ShouldBe("invoked: 65432");
    }

    [TestMethod]
    public void ReplaceTestMissCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        replacer.Replace("%T08SCRHMA10").ShouldBe("invoked: 9");
    }

    [TestMethod]
    public void ReplaceTestBombCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        replacer.Replace("%T08SCRHMA1A").ShouldBe("invoked: 6");
    }

    [TestMethod]
    public void ReplaceTestLastSpellCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        replacer.Replace("%T08SCRHMA1B").ShouldBe("invoked: 12");
    }

    [TestMethod]
    public void ReplaceTestPauseCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        replacer.Replace("%T08SCRHMA1C").ShouldBe("invoked: 3");
    }

    [TestMethod]
    public void ReplaceTestContinueCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        replacer.Replace("%T08SCRHMA1D").ShouldBe("invoked: 2");
    }

    [TestMethod]
    public void ReplaceTestHumanRate()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        replacer.Replace("%T08SCRHMA1E").ShouldBe("invoked: 78.90%");
    }

    [TestMethod]
    public void ReplaceTestGotSpellCards()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        replacer.Replace("%T08SCRHMA1F").ShouldBe(
            $"No.003 灯符「ファイヤフライフェノメノン」{Environment.NewLine}No.007 蠢符「リトルバグ」");
    }

    [TestMethod]
    public void ReplaceTestGotNoSpellCards()
    {
        var mock = HighScoreTests.MockHighScore();
        _ = mock.CardFlags.Returns(Enumerable.Range(1, 222).ToDictionary(id => id, _ => (byte)0));
        var rankings = new[] { new[] { mock } }.ToDictionary(
            ranking => (ranking[0].Chara, ranking[0].Level), ranking => ranking as IReadOnlyList<IHighScore>);
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(rankings, formatterMock);
        replacer.Replace("%T08SCRHMA1F").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestGotNonexistentSpellCards()
    {
        var mock = HighScoreTests.MockHighScore();
        _ = mock.CardFlags.Returns(new Dictionary<int, byte> { { 223, 1 } });
        var rankings = new[] { new[] { mock } }.ToDictionary(
            ranking => (ranking[0].Chara, ranking[0].Level), ranking => ranking as IReadOnlyList<IHighScore>);
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(rankings, formatterMock);
        replacer.Replace("%T08SCRHMA1F").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestNumGotSpellCards()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        replacer.Replace("%T08SCRHMA1G").ShouldBe("invoked: 2");
    }

    [TestMethod]
    public void ReplaceTestEmptyRankings()
    {
        var rankings = ImmutableDictionary<(Chara, Level), IReadOnlyList<IHighScore>>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(rankings, formatterMock);
        replacer.Replace("%T08SCRHMA11").ShouldBe("--------");
    }

    [TestMethod]
    public void ReplaceTestEmptyScores()
    {
        var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>
        {
            { Rankings.First().Key, ImmutableList<IHighScore>.Empty },
        };
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(rankings, formatterMock);
        replacer.Replace("%T08SCRHMA11").ShouldBe("--------");
    }

    [TestMethod]
    public void ReplaceTestNonexistentChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        replacer.Replace("%T08SCRHRY11").ShouldBe("--------");
    }

    [TestMethod]
    public void ReplaceTestNonexistentLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        replacer.Replace("%T08SCRNMA11").ShouldBe("--------");
    }

    [TestMethod]
    public void ReplaceTestNonexistentRank()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        replacer.Replace("%T08SCRHMA21").ShouldBe("--------");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        replacer.Replace("%T08XXXHMA11").ShouldBe("%T08XXXHMA11");
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        replacer.Replace("%T08SCRYMA11").ShouldBe("%T08SCRYMA11");
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        replacer.Replace("%T08SCRHXX11").ShouldBe("%T08SCRHXX11");
    }

    [TestMethod]
    public void ReplaceTestInvalidRank()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        replacer.Replace("%T08SCRHMAX1").ShouldBe("%T08SCRHMAX1");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        replacer.Replace("%T08SCRHMA1X").ShouldBe("%T08SCRHMA1X");
    }
}

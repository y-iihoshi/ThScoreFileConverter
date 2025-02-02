using System.Collections.Immutable;
using ThScoreFileConverter.Core.Models.Th07;
using ThScoreFileConverter.Models.Th07;
using IHighScore = ThScoreFileConverter.Models.Th07.IHighScore<
    ThScoreFileConverter.Core.Models.Th07.Chara,
    ThScoreFileConverter.Core.Models.Th07.Level,
    ThScoreFileConverter.Models.Th07.StageProgress>;

namespace ThScoreFileConverter.Tests.Models.Th07;

[TestClass]
public class ScoreReplacerTests
{
    private static IReadOnlyList<IHighScore>[] CreateRankings()
    {
        return [[HighScoreTests.MockHighScore()]];
    }

    internal static IReadOnlyDictionary<(Chara, Level), IReadOnlyList<IHighScore>> Rankings { get; } =
        CreateRankings().ToDictionary(ranking => (ranking[0].Chara, ranking[0].Level));

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
        replacer.Replace("%T07SCRHRB11").ShouldBe("Player1");
    }

    [TestMethod]
    public void ReplaceTestScore()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);

        replacer.Replace("%T07SCRHRB12").ShouldBe("invoked: 12345672");
    }

    [TestMethod]
    public void ReplaceTestStage()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        replacer.Replace("%T07SCRHRB13").ShouldBe("Stage 3");
    }

    [TestMethod]
    public void ReplaceTestDate()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        replacer.Replace("%T07SCRHRB14").ShouldBe("01/23");
    }

    [TestMethod]
    public void ReplaceTestSlowRate()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        replacer.Replace("%T07SCRHRB15").ShouldBe("invoked: 9.870%");
    }

    [TestMethod]
    public void ReplaceTestEmptyRankings()
    {
        var rankings = ImmutableDictionary<(Chara, Level), IReadOnlyList<IHighScore>>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(rankings, formatterMock);
        replacer.Replace("%T07SCRHRB11").ShouldBe("--------");
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
        replacer.Replace("%T07SCRHRB11").ShouldBe("--------");
    }

    [TestMethod]
    public void ReplaceTestNonexistentChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        replacer.Replace("%T07SCRHRA11").ShouldBe("--------");
    }

    [TestMethod]
    public void ReplaceTestNonexistentLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        replacer.Replace("%T07SCRNRB11").ShouldBe("--------");
    }

    [TestMethod]
    public void ReplaceTestNonexistentRank()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        replacer.Replace("%T07SCRHRB21").ShouldBe("--------");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        replacer.Replace("%T07XXXHRB11").ShouldBe("%T07XXXHRB11");
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        replacer.Replace("%T07SCRHXX11").ShouldBe("%T07SCRHXX11");
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        replacer.Replace("%T07SCRYRB11").ShouldBe("%T07SCRYRB11");
    }

    [TestMethod]
    public void ReplaceTestInvalidRank()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        replacer.Replace("%T07SCRHRBX1").ShouldBe("%T07SCRHRBX1");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        replacer.Replace("%T07SCRHRB1X").ShouldBe("%T07SCRHRB1X");
    }
}

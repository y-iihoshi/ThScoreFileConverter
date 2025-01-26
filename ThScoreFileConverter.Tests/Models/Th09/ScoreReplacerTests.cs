using System.Collections.Immutable;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th09;
using ThScoreFileConverter.Models.Th09;

namespace ThScoreFileConverter.Tests.Models.Th09;

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
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ScoreReplacerTestEmptyRankings()
    {
        var rankings = ImmutableDictionary<(Chara, Level), IReadOnlyList<IHighScore>>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
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
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(rankings, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestName()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("Player1", replacer.Replace("%T09SCRHMR11"));
    }

    [TestMethod]
    public void ReplaceTestScore()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);

        Assert.AreEqual("invoked: 12345672", replacer.Replace("%T09SCRHMR12"));
    }

    [TestMethod]
    public void ReplaceTestDate()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("06/01/23", replacer.Replace("%T09SCRHMR13"));
    }

    [TestMethod]
    public void ReplaceTestEmptyRankings()
    {
        var rankings = ImmutableDictionary<(Chara, Level), IReadOnlyList<IHighScore>>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(rankings, formatterMock);
        Assert.AreEqual("--------", replacer.Replace("%T09SCRHMR11"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T09SCRHMR12"));
        Assert.AreEqual("--/--/--", replacer.Replace("%T09SCRHMR13"));
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
        Assert.AreEqual("--------", replacer.Replace("%T09SCRHMR11"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T09SCRHMR12"));
        Assert.AreEqual("--/--/--", replacer.Replace("%T09SCRHMR13"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("--------", replacer.Replace("%T09SCRHRM11"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("--------", replacer.Replace("%T09SCRNMR11"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentRank()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("--------", replacer.Replace("%T09SCRHMR21"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("%T09XXXHMR11", replacer.Replace("%T09XXXHMR11"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("%T09SCRYMR11", replacer.Replace("%T09SCRYMR11"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("%T09SCRHXX11", replacer.Replace("%T09SCRHXX11"));
    }

    [TestMethod]
    public void ReplaceTestInvalidRank()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("%T09SCRHMRX1", replacer.Replace("%T09SCRHMRX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("%T09SCRHMR1X", replacer.Replace("%T09SCRHMR1X"));
    }
}

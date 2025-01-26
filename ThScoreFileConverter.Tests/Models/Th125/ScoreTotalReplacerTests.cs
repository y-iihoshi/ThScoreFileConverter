using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Models.Th125;
using ThScoreFileConverter.Models.Th125;

namespace ThScoreFileConverter.Tests.Models.Th125;

[TestClass]
public class ScoreTotalReplacerTests
{
    private static IScore[] CreateScores()
    {
        var mock1 = ScoreTests.MockScore();

        var mock2 = ScoreTests.MockScore();
        _ = mock2.LevelScene.Returns((Level.Spoiler, 4));
        _ = mock2.Chara.Returns(Chara.Aya);

        var mock3 = ScoreTests.MockScore();
        _ = mock3.LevelScene.Returns((Level.Spoiler, 5));

        return [mock1, mock2, mock3];
    }

    internal static IReadOnlyList<IScore> Scores { get; } = CreateScores();

    [TestMethod]
    public void ScoreTotalReplacerTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ScoreTotalReplacerTestEmpty()
    {
        var scores = ImmutableList<IScore>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(scores, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestTotalScoreInGame()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        Assert.AreEqual("invoked: 2469134", replacer.Replace("%T125SCRTLA11"));
        Assert.AreEqual("invoked: 1234567", replacer.Replace("%T125SCRTLH11"));
    }

    [TestMethod]
    public void ReplaceTestTotalBestShotScoreInGame()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        Assert.AreEqual("invoked: 46912", replacer.Replace("%T125SCRTLA12"));
        Assert.AreEqual("invoked: 23456", replacer.Replace("%T125SCRTLH12"));
    }

    [TestMethod]
    public void ReplaceTestTotalTrialCountInGame()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        Assert.AreEqual("invoked: 19752", replacer.Replace("%T125SCRTLA13"));
        Assert.AreEqual("invoked: 9876", replacer.Replace("%T125SCRTLH13"));
    }

    [TestMethod]
    public void ReplaceTestTotalFirstSuccessInGame()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        Assert.AreEqual("invoked: 10864", replacer.Replace("%T125SCRTLA14"));
        Assert.AreEqual("invoked: 5432", replacer.Replace("%T125SCRTLH14"));
    }

    [TestMethod]
    public void ReplaceTestNumSucceededScenesInGame()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        Assert.AreEqual("invoked: 2", replacer.Replace("%T125SCRTLA15"));
        Assert.AreEqual("invoked: 1", replacer.Replace("%T125SCRTLH15"));
    }

    [TestMethod]
    public void ReplaceTestTotalScorePerChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        Assert.AreEqual("invoked: 1234567", replacer.Replace("%T125SCRTLA21"));
        Assert.AreEqual("invoked: 2469134", replacer.Replace("%T125SCRTLH21"));
    }

    [TestMethod]
    public void ReplaceTestTotalBestShotScorePerChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        Assert.AreEqual("invoked: 23456", replacer.Replace("%T125SCRTLA22"));
        Assert.AreEqual("invoked: 46912", replacer.Replace("%T125SCRTLH22"));
    }

    [TestMethod]
    public void ReplaceTestTotalTrialCountPerChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        Assert.AreEqual("invoked: 9876", replacer.Replace("%T125SCRTLA23"));
        Assert.AreEqual("invoked: 19752", replacer.Replace("%T125SCRTLH23"));
    }

    [TestMethod]
    public void ReplaceTestTotalFirstSuccessPerChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        Assert.AreEqual("invoked: 5432", replacer.Replace("%T125SCRTLA24"));
        Assert.AreEqual("invoked: 10864", replacer.Replace("%T125SCRTLH24"));
    }

    [TestMethod]
    public void ReplaceTestNumSucceededScenesPerChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        Assert.AreEqual("invoked: 1", replacer.Replace("%T125SCRTLA25"));
        Assert.AreEqual("invoked: 2", replacer.Replace("%T125SCRTLH25"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var scores = ImmutableList<IScore>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(scores, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T125SCRTLH11"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T125SCRTLH12"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T125SCRTLH13"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T125SCRTLH14"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T125SCRTLH15"));
    }

    [TestMethod]
    public void ReplaceTestNullScore()
    {
        var scores = new List<IScore>() { null! };
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(scores, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T125SCRTLH11"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T125SCRTLH12"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T125SCRTLH13"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T125SCRTLH14"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T125SCRTLH15"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        Assert.AreEqual("%T125XXXXXH11", replacer.Replace("%T125XXXXXH11"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        Assert.AreEqual("%T125SCRTLX11", replacer.Replace("%T125SCRTLX11"));
    }

    [TestMethod]
    public void ReplaceTestInvalidMethod()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        Assert.AreEqual("%T125SCRTLHX1", replacer.Replace("%T125SCRTLHX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        Assert.AreEqual("%T125SCRTLH1X", replacer.Replace("%T125SCRTLH1X"));
    }
}

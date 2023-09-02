using System.Collections.Generic;
using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Models.Th095;
using ThScoreFileConverter.Models.Th095;

namespace ThScoreFileConverter.Tests.Models.Th095;

[TestClass]
public class ScoreTotalReplacerTests
{
    private static IReadOnlyList<IScore> CreateScores()
    {
        var mock1 = ScoreTests.MockScore();

        var mock2 = ScoreTests.MockScore();
        _ = mock2.LevelScene.Returns((Level.Nine, 7));
        _ = mock2.HighScore.Returns(0);

        return new[] { mock1, mock2 };
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
    public void ReplaceTestTotalScore()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        Assert.AreEqual("invoked: 1234567", replacer.Replace("%T95SCRTL1"));
    }

    [TestMethod]
    public void ReplaceTestTotalBestShotScore()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        Assert.AreEqual("invoked: 46912", replacer.Replace("%T95SCRTL2"));
    }

    [TestMethod]
    public void ReplaceTestTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        Assert.AreEqual("invoked: 19752", replacer.Replace("%T95SCRTL3"));
    }

    [TestMethod]
    public void ReplaceTestNumSucceededScenes()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        Assert.AreEqual("invoked: 1", replacer.Replace("%T95SCRTL4"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var scores = ImmutableList<IScore>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(scores, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T95SCRTL1"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T95SCRTL2"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T95SCRTL3"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T95SCRTL4"));
    }

    [TestMethod]
    public void ReplaceTestNullScore()
    {
        var scores = new List<IScore>() { null! };
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(scores, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T95SCRTL1"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T95SCRTL2"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T95SCRTL3"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T95SCRTL4"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        Assert.AreEqual("%T95XXXXX1", replacer.Replace("%T95XXXXX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        Assert.AreEqual("%T95SCRTLX", replacer.Replace("%T95SCRTLX"));
    }
}

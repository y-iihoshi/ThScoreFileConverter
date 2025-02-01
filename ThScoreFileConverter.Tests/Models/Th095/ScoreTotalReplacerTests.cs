using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Models.Th095;
using ThScoreFileConverter.Models.Th095;

namespace ThScoreFileConverter.Tests.Models.Th095;

[TestClass]
public class ScoreTotalReplacerTests
{
    private static IScore[] CreateScores()
    {
        var mock1 = ScoreTests.MockScore();

        var mock2 = ScoreTests.MockScore();
        _ = mock2.LevelScene.Returns((Level.Nine, 7));
        _ = mock2.HighScore.Returns(0);

        return [mock1, mock2];
    }

    internal static IReadOnlyList<IScore> Scores { get; } = CreateScores();

    [TestMethod]
    public void ScoreTotalReplacerTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ScoreTotalReplacerTestEmpty()
    {
        var scores = ImmutableList<IScore>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(scores, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTestTotalScore()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        replacer.Replace("%T95SCRTL1").ShouldBe("invoked: 1234567");
    }

    [TestMethod]
    public void ReplaceTestTotalBestShotScore()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        replacer.Replace("%T95SCRTL2").ShouldBe("invoked: 46912");
    }

    [TestMethod]
    public void ReplaceTestTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        replacer.Replace("%T95SCRTL3").ShouldBe("invoked: 19752");
    }

    [TestMethod]
    public void ReplaceTestNumSucceededScenes()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        replacer.Replace("%T95SCRTL4").ShouldBe("invoked: 1");
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var scores = ImmutableList<IScore>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(scores, formatterMock);
        replacer.Replace("%T95SCRTL1").ShouldBe("invoked: 0");
        replacer.Replace("%T95SCRTL2").ShouldBe("invoked: 0");
        replacer.Replace("%T95SCRTL3").ShouldBe("invoked: 0");
        replacer.Replace("%T95SCRTL4").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestNullScore()
    {
        var scores = new List<IScore>() { null! };
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(scores, formatterMock);
        replacer.Replace("%T95SCRTL1").ShouldBe("invoked: 0");
        replacer.Replace("%T95SCRTL2").ShouldBe("invoked: 0");
        replacer.Replace("%T95SCRTL3").ShouldBe("invoked: 0");
        replacer.Replace("%T95SCRTL4").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        replacer.Replace("%T95XXXXX1").ShouldBe("%T95XXXXX1");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        replacer.Replace("%T95SCRTLX").ShouldBe("%T95SCRTLX");
    }
}

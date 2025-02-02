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
    public void ReplaceTestTotalScoreInGame()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        replacer.Replace("%T125SCRTLA11").ShouldBe("invoked: 2469134");
        replacer.Replace("%T125SCRTLH11").ShouldBe("invoked: 1234567");
    }

    [TestMethod]
    public void ReplaceTestTotalBestShotScoreInGame()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        replacer.Replace("%T125SCRTLA12").ShouldBe("invoked: 46912");
        replacer.Replace("%T125SCRTLH12").ShouldBe("invoked: 23456");
    }

    [TestMethod]
    public void ReplaceTestTotalTrialCountInGame()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        replacer.Replace("%T125SCRTLA13").ShouldBe("invoked: 19752");
        replacer.Replace("%T125SCRTLH13").ShouldBe("invoked: 9876");
    }

    [TestMethod]
    public void ReplaceTestTotalFirstSuccessInGame()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        replacer.Replace("%T125SCRTLA14").ShouldBe("invoked: 10864");
        replacer.Replace("%T125SCRTLH14").ShouldBe("invoked: 5432");
    }

    [TestMethod]
    public void ReplaceTestNumSucceededScenesInGame()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        replacer.Replace("%T125SCRTLA15").ShouldBe("invoked: 2");
        replacer.Replace("%T125SCRTLH15").ShouldBe("invoked: 1");
    }

    [TestMethod]
    public void ReplaceTestTotalScorePerChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        replacer.Replace("%T125SCRTLA21").ShouldBe("invoked: 1234567");
        replacer.Replace("%T125SCRTLH21").ShouldBe("invoked: 2469134");
    }

    [TestMethod]
    public void ReplaceTestTotalBestShotScorePerChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        replacer.Replace("%T125SCRTLA22").ShouldBe("invoked: 23456");
        replacer.Replace("%T125SCRTLH22").ShouldBe("invoked: 46912");
    }

    [TestMethod]
    public void ReplaceTestTotalTrialCountPerChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        replacer.Replace("%T125SCRTLA23").ShouldBe("invoked: 9876");
        replacer.Replace("%T125SCRTLH23").ShouldBe("invoked: 19752");
    }

    [TestMethod]
    public void ReplaceTestTotalFirstSuccessPerChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        replacer.Replace("%T125SCRTLA24").ShouldBe("invoked: 5432");
        replacer.Replace("%T125SCRTLH24").ShouldBe("invoked: 10864");
    }

    [TestMethod]
    public void ReplaceTestNumSucceededScenesPerChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        replacer.Replace("%T125SCRTLA25").ShouldBe("invoked: 1");
        replacer.Replace("%T125SCRTLH25").ShouldBe("invoked: 2");
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var scores = ImmutableList<IScore>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(scores, formatterMock);
        replacer.Replace("%T125SCRTLH11").ShouldBe("invoked: 0");
        replacer.Replace("%T125SCRTLH12").ShouldBe("invoked: 0");
        replacer.Replace("%T125SCRTLH13").ShouldBe("invoked: 0");
        replacer.Replace("%T125SCRTLH14").ShouldBe("invoked: 0");
        replacer.Replace("%T125SCRTLH15").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestNullScore()
    {
        var scores = new List<IScore>() { null! };
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(scores, formatterMock);
        replacer.Replace("%T125SCRTLH11").ShouldBe("invoked: 0");
        replacer.Replace("%T125SCRTLH12").ShouldBe("invoked: 0");
        replacer.Replace("%T125SCRTLH13").ShouldBe("invoked: 0");
        replacer.Replace("%T125SCRTLH14").ShouldBe("invoked: 0");
        replacer.Replace("%T125SCRTLH15").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        replacer.Replace("%T125XXXXXH11").ShouldBe("%T125XXXXXH11");
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        replacer.Replace("%T125SCRTLX11").ShouldBe("%T125SCRTLX11");
    }

    [TestMethod]
    public void ReplaceTestInvalidMethod()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        replacer.Replace("%T125SCRTLHX1").ShouldBe("%T125SCRTLHX1");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        replacer.Replace("%T125SCRTLH1X").ShouldBe("%T125SCRTLH1X");
    }
}

using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Models.Th165;

namespace ThScoreFileConverter.Tests.Models.Th165;

[TestClass]
public class ScoreTotalReplacerTests
{
    private static IScore[] CreateScores()
    {
        var mock1 = ScoreTests.MockScore();
        var mock2 = ScoreTests.MockScore();
        _ = mock2.Number.Returns(mock1.Number + 1);
        return [mock1, mock2];
    }

    internal static IReadOnlyList<IScore> Scores { get; } = CreateScores();

    internal static IStatus Status { get; } = StatusTests.MockStatus();

    [TestMethod]
    public void ScoreTotalReplacerTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, Status, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ScoreTotalReplacerTestEmptyScores()
    {
        var scores = ImmutableList<IScore>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(scores, Status, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTestTotalScore()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, Status, formatterMock);
        replacer.Replace("%T165SCRTL1").ShouldBe("invoked: 2469134");
    }

    [TestMethod]
    public void ReplaceTestTotalChallengeCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, Status, formatterMock);
        replacer.Replace("%T165SCRTL2").ShouldBe("invoked: 112");
    }

    [TestMethod]
    public void ReplaceTestTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, Status, formatterMock);
        replacer.Replace("%T165SCRTL3").ShouldBe("invoked: 68");
    }

    [TestMethod]
    public void ReplaceTestNumSucceededScenes()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, Status, formatterMock);
        replacer.Replace("%T165SCRTL4").ShouldBe("invoked: 2");
    }

    [TestMethod]
    public void ReplaceTestTotalNumPhotos()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, Status, formatterMock);
        replacer.Replace("%T165SCRTL5").ShouldBe("invoked: 156");
    }

    [TestMethod]
    public void ReplaceTestNumNicknames()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, Status, formatterMock);
        replacer.Replace("%T165SCRTL6").ShouldBe("invoked: 34");
    }

    [TestMethod]
    public void ReplaceTestEmptyScores()
    {
        var scores = ImmutableList<IScore>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(scores, Status, formatterMock);
        replacer.Replace("%T165SCRTL1").ShouldBe("invoked: 0");
        replacer.Replace("%T165SCRTL2").ShouldBe("invoked: 0");
        replacer.Replace("%T165SCRTL3").ShouldBe("invoked: 0");
        replacer.Replace("%T165SCRTL4").ShouldBe("invoked: 0");
        replacer.Replace("%T165SCRTL5").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestNullScore()
    {
        var scores = new List<IScore> { null! };
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(scores, Status, formatterMock);
        replacer.Replace("%T165SCRTL1").ShouldBe("invoked: 0");
        replacer.Replace("%T165SCRTL2").ShouldBe("invoked: 0");
        replacer.Replace("%T165SCRTL3").ShouldBe("invoked: 0");
        replacer.Replace("%T165SCRTL4").ShouldBe("invoked: 0");
        replacer.Replace("%T165SCRTL5").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, Status, formatterMock);
        replacer.Replace("%T165XXXXX1").ShouldBe("%T165XXXXX1");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, Status, formatterMock);
        replacer.Replace("%T165SCRTLX").ShouldBe("%T165SCRTLX");
    }
}

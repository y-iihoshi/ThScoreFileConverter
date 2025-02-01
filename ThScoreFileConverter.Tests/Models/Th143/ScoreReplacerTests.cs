using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Models.Th143;
using ThScoreFileConverter.Models.Th143;

namespace ThScoreFileConverter.Tests.Models.Th143;

[TestClass]
public class ScoreReplacerTests
{
    internal static IReadOnlyList<IScore> Scores { get; } = [ScoreTests.MockScore()];

    [TestMethod]
    public void ScoreReplacerTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ScoreReplacerTestEmpty()
    {
        var scores = ImmutableList<IScore>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(scores, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTestHighScore()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        replacer.Replace("%T143SCRL441").ShouldBe("invoked: 4567890");
    }

    [TestMethod]
    public void ReplaceTestChallengeCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        replacer.Replace("%T143SCRL442").ShouldBe("invoked: 300");
    }

    [TestMethod]
    public void ReplaceTestChallengeCountNoItem()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        replacer.Replace("%T143SCRL402").ShouldBe("-");
    }

    [TestMethod]
    public void ReplaceTestClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        replacer.Replace("%T143SCRL443").ShouldBe("invoked: 30");
    }

    [TestMethod]
    public void ReplaceTestScene10()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        replacer.Replace("%T143SCRL041").ShouldBe("invoked: 0");
        replacer.Replace("%T143SCRL042").ShouldBe("invoked: 0");
        replacer.Replace("%T143SCRL043").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var scores = ImmutableList<IScore>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(scores, formatterMock);
        replacer.Replace("%T143SCRL441").ShouldBe("invoked: 0");
        replacer.Replace("%T143SCRL442").ShouldBe("invoked: 0");
        replacer.Replace("%T143SCRL443").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestNullScore()
    {
        var scores = new List<IScore> { null! };
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(scores, formatterMock);
        replacer.Replace("%T143SCRL441").ShouldBe("invoked: 0");
        replacer.Replace("%T143SCRL442").ShouldBe("invoked: 0");
        replacer.Replace("%T143SCRL443").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestZeroNumber()
    {
        var mock = ScoreTests.MockScore();
        _ = mock.Number.Returns(0);
        var scores = new[] { mock };
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new ScoreReplacer(scores, formatterMock);
        replacer.Replace("%T143SCRL441").ShouldBe("invoked: 0");
        replacer.Replace("%T143SCRL442").ShouldBe("invoked: 0");
        replacer.Replace("%T143SCRL443").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestExceededNumber()
    {
        var mock = ScoreTests.MockScore();
        _ = mock.Number.Returns(76);
        var scores = new[] { mock };
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new ScoreReplacer(scores, formatterMock);
        replacer.Replace("%T143SCRL441").ShouldBe("invoked: 0");
        replacer.Replace("%T143SCRL442").ShouldBe("invoked: 0");
        replacer.Replace("%T143SCRL443").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestMismatchNumber()
    {
        var mock = ScoreTests.MockScore();
        _ = mock.Number.Returns(70);
        var scores = new[] { mock };
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new ScoreReplacer(scores, formatterMock);
        replacer.Replace("%T143SCRL441").ShouldBe("invoked: 0");
        replacer.Replace("%T143SCRL442").ShouldBe("invoked: 0");
        replacer.Replace("%T143SCRL443").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestEmptyChallengeCounts()
    {
        var mock = ScoreTests.MockScore();
        _ = mock.ChallengeCounts.Returns(ImmutableDictionary<ItemWithTotal, int>.Empty);
        var scores = new[] { mock };
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new ScoreReplacer(scores, formatterMock);
        replacer.Replace("%T143SCRL442").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestEmptyClearCounts()
    {
        var mock = ScoreTests.MockScore();
        _ = mock.ClearCounts.Returns(ImmutableDictionary<ItemWithTotal, int>.Empty);
        var scores = new[] { mock };
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new ScoreReplacer(scores, formatterMock);
        replacer.Replace("%T143SCRL443").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestNonexistentDayScene()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        replacer.Replace("%T143SCR1741").ShouldBe("%T143SCR1741");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        replacer.Replace("%T143XXXL441").ShouldBe("%T143XXXL441");
    }

    [TestMethod]
    public void ReplaceTestInvalidDay()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        replacer.Replace("%T143SCRX441").ShouldBe("%T143SCRX441");
    }

    [TestMethod]
    public void ReplaceTestInvalidScene()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        replacer.Replace("%T143SCRLX41").ShouldBe("%T143SCRLX41");
    }

    [TestMethod]
    public void ReplaceTestInvalidItem()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        replacer.Replace("%T143SCRL4X1").ShouldBe("%T143SCRL4X1");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        replacer.Replace("%T143SCRL44X").ShouldBe("%T143SCRL44X");
    }
}

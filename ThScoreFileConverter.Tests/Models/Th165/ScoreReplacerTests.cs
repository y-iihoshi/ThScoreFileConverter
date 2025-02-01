using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Models.Th165;

namespace ThScoreFileConverter.Tests.Models.Th165;

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
        replacer.Replace("%T165SCR0441").ShouldBe("invoked: 1234567");
    }

    [TestMethod]
    public void ReplaceTestChallengeCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        replacer.Replace("%T165SCR0442").ShouldBe("invoked: 56");
    }

    [TestMethod]
    public void ReplaceTestClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        replacer.Replace("%T165SCR0443").ShouldBe("invoked: 34");
    }

    [TestMethod]
    public void ReplaceTestNumPhotos()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        replacer.Replace("%T165SCR0444").ShouldBe("invoked: 78");
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var scores = ImmutableList<IScore>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(scores, formatterMock);
        replacer.Replace("%T165SCR0441").ShouldBe("invoked: 0");
        replacer.Replace("%T165SCR0442").ShouldBe("invoked: 0");
        replacer.Replace("%T165SCR0443").ShouldBe("invoked: 0");
        replacer.Replace("%T165SCR0444").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestNullScore()
    {
        var scores = new List<IScore> { null! };
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(scores, formatterMock);
        replacer.Replace("%T165SCR0441").ShouldBe("invoked: 0");
        replacer.Replace("%T165SCR0442").ShouldBe("invoked: 0");
        replacer.Replace("%T165SCR0443").ShouldBe("invoked: 0");
        replacer.Replace("%T165SCR0444").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestZeroNumber()
    {
        var mock = ScoreTests.MockScore();
        _ = mock.Number.Returns(0);
        var scores = new[] { mock };
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new ScoreReplacer(scores, formatterMock);
        replacer.Replace("%T165SCR0441").ShouldBe("invoked: 0");
        replacer.Replace("%T165SCR0442").ShouldBe("invoked: 0");
        replacer.Replace("%T165SCR0443").ShouldBe("invoked: 0");
        replacer.Replace("%T165SCR0444").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestExceededNumber()
    {
        var mock = ScoreTests.MockScore();
        _ = mock.Number.Returns(104);
        var scores = new[] { mock };
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new ScoreReplacer(scores, formatterMock);
        replacer.Replace("%T165SCR0441").ShouldBe("invoked: 0");
        replacer.Replace("%T165SCR0442").ShouldBe("invoked: 0");
        replacer.Replace("%T165SCR0443").ShouldBe("invoked: 0");
        replacer.Replace("%T165SCR0444").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestMismatchNumber()
    {
        var mock = ScoreTests.MockScore();
        _ = mock.Number.Returns(70);
        var scores = new[] { mock };
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new ScoreReplacer(scores, formatterMock);
        replacer.Replace("%T165SCR0441").ShouldBe("invoked: 0");
        replacer.Replace("%T165SCR0442").ShouldBe("invoked: 0");
        replacer.Replace("%T165SCR0443").ShouldBe("invoked: 0");
        replacer.Replace("%T165SCR0444").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestNonexistentDayScene()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        replacer.Replace("%T165SCR0131").ShouldBe("%T165SCR0131");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        replacer.Replace("%T165XXX0441").ShouldBe("%T165XXX0441");
    }

    [TestMethod]
    public void ReplaceTestInvalidDay()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        replacer.Replace("%T165SCRXX41").ShouldBe("%T165SCRXX41");
    }

    [TestMethod]
    public void ReplaceTestInvalidScene()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        replacer.Replace("%T165SCR04X1").ShouldBe("%T165SCR04X1");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        replacer.Replace("%T165SCR044X").ShouldBe("%T165SCR044X");
    }
}

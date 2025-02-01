using System.Collections.Immutable;
using ThScoreFileConverter.Models.Th095;

namespace ThScoreFileConverter.Tests.Models.Th095;

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
        replacer.Replace("%T95SCR961").ShouldBe("invoked: 1234567");
    }

    [TestMethod]
    public void ReplaceTestBestShotScore()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        replacer.Replace("%T95SCR962").ShouldBe("invoked: 23456");
    }

    [TestMethod]
    public void ReplaceTestTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        replacer.Replace("%T95SCR963").ShouldBe("invoked: 9876");
    }

    [TestMethod]
    public void ReplaceTestSlowRate()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        replacer.Replace("%T95SCR964").ShouldBe("invoked: 2.340%");  // really...?
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var scores = ImmutableList<IScore>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(scores, formatterMock);
        replacer.Replace("%T95SCR961").ShouldBe("invoked: 0");
        replacer.Replace("%T95SCR962").ShouldBe("invoked: 0");
        replacer.Replace("%T95SCR963").ShouldBe("invoked: 0");
        replacer.Replace("%T95SCR964").ShouldBe("-----%");
    }

    [TestMethod]
    public void ReplaceTestNullScore()
    {
        var scores = new List<IScore> { null! };
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(scores, formatterMock);
        replacer.Replace("%T95SCR961").ShouldBe("invoked: 0");
        replacer.Replace("%T95SCR962").ShouldBe("invoked: 0");
        replacer.Replace("%T95SCR963").ShouldBe("invoked: 0");
        replacer.Replace("%T95SCR964").ShouldBe("-----%");
    }

    [TestMethod]
    public void ReplaceTestNonexistentLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        replacer.Replace("%T95SCR861").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestNonexistentScene()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        replacer.Replace("%T95SCR951").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestNonexistentSpellCard()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        replacer.Replace("%T95SCR991").ShouldBe("%T95SCR991");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        replacer.Replace("%T95XXX961").ShouldBe("%T95XXX961");
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        replacer.Replace("%T95SCRY61").ShouldBe("%T95SCRY61");
    }

    [TestMethod]
    public void ReplaceTestInvalidScene()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        replacer.Replace("%T95SCR9X1").ShouldBe("%T95SCR9X1");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        replacer.Replace("%T95SCR96X").ShouldBe("%T95SCR96X");
    }
}

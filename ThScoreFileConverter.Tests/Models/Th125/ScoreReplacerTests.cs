using System.Collections.Immutable;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models.Th125;

namespace ThScoreFileConverter.Tests.Models.Th125;

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
        replacer.Replace("%T125SCRH971").ShouldBe("invoked: 1234567");
    }

    [TestMethod]
    public void ReplaceTestBestShotScore()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        replacer.Replace("%T125SCRH972").ShouldBe("invoked: 23456");
    }

    [TestMethod]
    public void ReplaceTestTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        replacer.Replace("%T125SCRH973").ShouldBe("invoked: 9876");
    }

    [TestMethod]
    public void ReplaceTestFirstSuccess()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        replacer.Replace("%T125SCRH974").ShouldBe("invoked: 5432");
    }

    [TestMethod]
    public void ReplaceTestDateTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        var expected = DateTimeHelper.GetString(34567890);
        replacer.Replace("%T125SCRH975").ShouldBe(expected);
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var scores = ImmutableList<IScore>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(scores, formatterMock);
        replacer.Replace("%T125SCRH971").ShouldBe("invoked: 0");
        replacer.Replace("%T125SCRH972").ShouldBe("invoked: 0");
        replacer.Replace("%T125SCRH973").ShouldBe("invoked: 0");
        replacer.Replace("%T125SCRH974").ShouldBe("invoked: 0");
        replacer.Replace("%T125SCRH975").ShouldBe(DateTimeHelper.GetString(null));
    }

    [TestMethod]
    public void ReplaceTestNullScore()
    {
        var scores = new List<IScore> { null! };
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(scores, formatterMock);
        replacer.Replace("%T125SCRH971").ShouldBe("invoked: 0");
        replacer.Replace("%T125SCRH972").ShouldBe("invoked: 0");
        replacer.Replace("%T125SCRH973").ShouldBe("invoked: 0");
        replacer.Replace("%T125SCRH974").ShouldBe("invoked: 0");
        replacer.Replace("%T125SCRH975").ShouldBe(DateTimeHelper.GetString(null));
    }

    [TestMethod]
    public void ReplaceTestNonexistentLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        replacer.Replace("%T125SCRH861").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestNonexistentScene()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        replacer.Replace("%T125SCRH951").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestNonexistentSpellCard()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        replacer.Replace("%T125SCRH991").ShouldBe("%T125SCRH991");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        replacer.Replace("%T125XXXH971").ShouldBe("%T125XXXH971");
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        replacer.Replace("%T125SCRX971").ShouldBe("%T125SCRX971");
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        replacer.Replace("%T125SCRHY61").ShouldBe("%T125SCRHY61");
    }

    [TestMethod]
    public void ReplaceTestInvalidScene()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        replacer.Replace("%T125SCRH9X1").ShouldBe("%T125SCRH9X1");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        replacer.Replace("%T125SCRH97X").ShouldBe("%T125SCRH97X");
    }
}

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
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ScoreReplacerTestEmpty()
    {
        var scores = ImmutableList<IScore>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(scores, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestHighScore()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        Assert.AreEqual("invoked: 1234567", replacer.Replace("%T125SCRH971"));
    }

    [TestMethod]
    public void ReplaceTestBestShotScore()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        Assert.AreEqual("invoked: 23456", replacer.Replace("%T125SCRH972"));
    }

    [TestMethod]
    public void ReplaceTestTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        Assert.AreEqual("invoked: 9876", replacer.Replace("%T125SCRH973"));
    }

    [TestMethod]
    public void ReplaceTestFirstSuccess()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        Assert.AreEqual("invoked: 5432", replacer.Replace("%T125SCRH974"));
    }

    [TestMethod]
    public void ReplaceTestDateTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        var expected = DateTimeHelper.GetString(34567890);
        Assert.AreEqual(expected, replacer.Replace("%T125SCRH975"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var scores = ImmutableList<IScore>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(scores, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T125SCRH971"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T125SCRH972"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T125SCRH973"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T125SCRH974"));
        Assert.AreEqual(DateTimeHelper.GetString(null), replacer.Replace("%T125SCRH975"));
    }

    [TestMethod]
    public void ReplaceTestNullScore()
    {
        var scores = new List<IScore> { null! };
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(scores, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T125SCRH971"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T125SCRH972"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T125SCRH973"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T125SCRH974"));
        Assert.AreEqual(DateTimeHelper.GetString(null), replacer.Replace("%T125SCRH975"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T125SCRH861"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentScene()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T125SCRH951"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentSpellCard()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        Assert.AreEqual("%T125SCRH991", replacer.Replace("%T125SCRH991"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        Assert.AreEqual("%T125XXXH971", replacer.Replace("%T125XXXH971"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        Assert.AreEqual("%T125SCRX971", replacer.Replace("%T125SCRX971"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        Assert.AreEqual("%T125SCRHY61", replacer.Replace("%T125SCRHY61"));
    }

    [TestMethod]
    public void ReplaceTestInvalidScene()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        Assert.AreEqual("%T125SCRH9X1", replacer.Replace("%T125SCRH9X1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        Assert.AreEqual("%T125SCRH97X", replacer.Replace("%T125SCRH97X"));
    }
}

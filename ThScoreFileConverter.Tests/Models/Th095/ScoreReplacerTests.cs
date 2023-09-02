using System.Collections.Generic;
using System.Collections.Immutable;
using ThScoreFileConverter.Models.Th095;

namespace ThScoreFileConverter.Tests.Models.Th095;

[TestClass]
public class ScoreReplacerTests
{
    internal static IReadOnlyList<IScore> Scores { get; } = new[] { ScoreTests.MockScore() };

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
        Assert.AreEqual("invoked: 1234567", replacer.Replace("%T95SCR961"));
    }

    [TestMethod]
    public void ReplaceTestBestShotScore()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        Assert.AreEqual("invoked: 23456", replacer.Replace("%T95SCR962"));
    }

    [TestMethod]
    public void ReplaceTestTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        Assert.AreEqual("invoked: 9876", replacer.Replace("%T95SCR963"));
    }

    [TestMethod]
    public void ReplaceTestSlowRate()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        Assert.AreEqual("invoked: 2.340%", replacer.Replace("%T95SCR964"));  // really...?
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var scores = ImmutableList<IScore>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(scores, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T95SCR961"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T95SCR962"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T95SCR963"));
        Assert.AreEqual("-----%", replacer.Replace("%T95SCR964"));
    }

    [TestMethod]
    public void ReplaceTestNullScore()
    {
        var scores = new List<IScore> { null! };
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(scores, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T95SCR961"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T95SCR962"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T95SCR963"));
        Assert.AreEqual("-----%", replacer.Replace("%T95SCR964"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T95SCR861"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentScene()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T95SCR951"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentSpellCard()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        Assert.AreEqual("%T95SCR991", replacer.Replace("%T95SCR991"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        Assert.AreEqual("%T95XXX961", replacer.Replace("%T95XXX961"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        Assert.AreEqual("%T95SCRY61", replacer.Replace("%T95SCRY61"));
    }

    [TestMethod]
    public void ReplaceTestInvalidScene()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        Assert.AreEqual("%T95SCR9X1", replacer.Replace("%T95SCR9X1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(Scores, formatterMock);
        Assert.AreEqual("%T95SCR96X", replacer.Replace("%T95SCR96X"));
    }
}

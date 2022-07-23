using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th095;

namespace ThScoreFileConverter.Tests.Models.Th095;

[TestClass]
public class ScoreReplacerTests
{
    internal static IReadOnlyList<IScore> Scores { get; } = new[] { ScoreTests.MockScore().Object };

    private static Mock<INumberFormatter> MockNumberFormatter()
    {
        var mock = new Mock<INumberFormatter>();
        _ = mock.Setup(formatter => formatter.FormatNumber(It.IsAny<It.IsValueType>()))
            .Returns((object value) => "invoked: " + value.ToString());
        _ = mock.Setup(formatter => formatter.FormatPercent(It.IsAny<double>(), It.IsAny<int>()))
            .Returns((double value, int precision) => "invoked: " + value.ToString($"F{precision}") + "%");
        return mock;
    }

    [TestMethod]
    public void ScoreReplacerTest()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Scores, formatterMock.Object);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ScoreReplacerTestEmpty()
    {
        var scores = ImmutableList<IScore>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(scores, formatterMock.Object);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestHighScore()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Scores, formatterMock.Object);
        Assert.AreEqual("invoked: 1234567", replacer.Replace("%T95SCR961"));
    }

    [TestMethod]
    public void ReplaceTestBestShotScore()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Scores, formatterMock.Object);
        Assert.AreEqual("invoked: 23456", replacer.Replace("%T95SCR962"));
    }

    [TestMethod]
    public void ReplaceTestTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Scores, formatterMock.Object);
        Assert.AreEqual("invoked: 9876", replacer.Replace("%T95SCR963"));
    }

    [TestMethod]
    public void ReplaceTestSlowRate()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Scores, formatterMock.Object);
        Assert.AreEqual("invoked: 2.340%", replacer.Replace("%T95SCR964"));  // really...?
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var scores = ImmutableList<IScore>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(scores, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T95SCR961"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T95SCR962"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T95SCR963"));
        Assert.AreEqual("-----%", replacer.Replace("%T95SCR964"));
    }

    [TestMethod]
    public void ReplaceTestNullScore()
    {
        var scores = new List<IScore> { null! };
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(scores, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T95SCR961"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T95SCR962"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T95SCR963"));
        Assert.AreEqual("-----%", replacer.Replace("%T95SCR964"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentLevel()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Scores, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T95SCR861"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentScene()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Scores, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T95SCR951"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentSpellCard()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Scores, formatterMock.Object);
        Assert.AreEqual("%T95SCR991", replacer.Replace("%T95SCR991"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Scores, formatterMock.Object);
        Assert.AreEqual("%T95XXX961", replacer.Replace("%T95XXX961"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Scores, formatterMock.Object);
        Assert.AreEqual("%T95SCRY61", replacer.Replace("%T95SCRY61"));
    }

    [TestMethod]
    public void ReplaceTestInvalidScene()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Scores, formatterMock.Object);
        Assert.AreEqual("%T95SCR9X1", replacer.Replace("%T95SCR9X1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Scores, formatterMock.Object);
        Assert.AreEqual("%T95SCR96X", replacer.Replace("%T95SCR96X"));
    }
}

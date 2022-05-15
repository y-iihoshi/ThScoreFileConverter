using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models.Th095;
using INumberFormatter = ThScoreFileConverter.Models.INumberFormatter;

namespace ThScoreFileConverterTests.Models.Th095;

[TestClass]
public class ScoreTotalReplacerTests
{
    private static IReadOnlyList<IScore> CreateScores()
    {
        var mock1 = ScoreTests.MockScore();

        var mock2 = ScoreTests.MockScore();
        _ = mock2.SetupGet(m => m.LevelScene).Returns((Level.Nine, 7));
        _ = mock2.SetupGet(m => m.HighScore).Returns(0);

        return new[] { mock1.Object, mock2.Object };
    }

    internal static IReadOnlyList<IScore> Scores { get; } = CreateScores();

    private static Mock<INumberFormatter> MockNumberFormatter()
    {
        var mock = new Mock<INumberFormatter>();
        _ = mock.Setup(formatter => formatter.FormatNumber(It.IsAny<It.IsValueType>()))
            .Returns((object value) => "invoked: " + value.ToString());
        return mock;
    }

    [TestMethod]
    public void ScoreTotalReplacerTest()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreTotalReplacer(Scores, formatterMock.Object);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ScoreTotalReplacerTestEmpty()
    {
        var scores = ImmutableList<IScore>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreTotalReplacer(scores, formatterMock.Object);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestTotalScore()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreTotalReplacer(Scores, formatterMock.Object);
        Assert.AreEqual("invoked: 1234567", replacer.Replace("%T95SCRTL1"));
    }

    [TestMethod]
    public void ReplaceTestTotalBestShotScore()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreTotalReplacer(Scores, formatterMock.Object);
        Assert.AreEqual("invoked: 46912", replacer.Replace("%T95SCRTL2"));
    }

    [TestMethod]
    public void ReplaceTestTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreTotalReplacer(Scores, formatterMock.Object);
        Assert.AreEqual("invoked: 19752", replacer.Replace("%T95SCRTL3"));
    }

    [TestMethod]
    public void ReplaceTestNumSucceededScenes()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreTotalReplacer(Scores, formatterMock.Object);
        Assert.AreEqual("invoked: 1", replacer.Replace("%T95SCRTL4"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var scores = ImmutableList<IScore>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreTotalReplacer(scores, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T95SCRTL1"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T95SCRTL2"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T95SCRTL3"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T95SCRTL4"));
    }

    [TestMethod]
    public void ReplaceTestNullScore()
    {
        var scores = new List<IScore>() { null! };
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreTotalReplacer(scores, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T95SCRTL1"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T95SCRTL2"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T95SCRTL3"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T95SCRTL4"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreTotalReplacer(Scores, formatterMock.Object);
        Assert.AreEqual("%T95XXXXX1", replacer.Replace("%T95XXXXX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreTotalReplacer(Scores, formatterMock.Object);
        Assert.AreEqual("%T95SCRTLX", replacer.Replace("%T95SCRTLX"));
    }
}

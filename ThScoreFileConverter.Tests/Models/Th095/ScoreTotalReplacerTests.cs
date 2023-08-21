using System.Collections.Generic;
using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Models.Th095;
using ThScoreFileConverter.Models.Th095;
using INumberFormatter = ThScoreFileConverter.Models.INumberFormatter;

namespace ThScoreFileConverter.Tests.Models.Th095;

[TestClass]
public class ScoreTotalReplacerTests
{
    private static IReadOnlyList<IScore> CreateScores()
    {
        var mock1 = ScoreTests.MockScore();

        var mock2 = ScoreTests.MockScore();
        _ = mock2.LevelScene.Returns((Level.Nine, 7));
        _ = mock2.HighScore.Returns(0);

        return new[] { mock1, mock2 };
    }

    internal static IReadOnlyList<IScore> Scores { get; } = CreateScores();

    private static INumberFormatter MockNumberFormatter()
    {
        // NOTE: NSubstitute v5.0.0 has no substitute for Moq's It.IsAny<It.IsValueType>.
        var mock = Substitute.For<INumberFormatter>();
        _ = mock.FormatNumber(Arg.Any<int>()).Returns(callInfo => $"invoked: {(int)callInfo[0]}");
        _ = mock.FormatNumber(Arg.Any<long>()).Returns(callInfo => $"invoked: {(long)callInfo[0]}");
        return mock;
    }

    [TestMethod]
    public void ScoreTotalReplacerTest()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ScoreTotalReplacerTestEmpty()
    {
        var scores = ImmutableList<IScore>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreTotalReplacer(scores, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestTotalScore()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        Assert.AreEqual("invoked: 1234567", replacer.Replace("%T95SCRTL1"));
    }

    [TestMethod]
    public void ReplaceTestTotalBestShotScore()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        Assert.AreEqual("invoked: 46912", replacer.Replace("%T95SCRTL2"));
    }

    [TestMethod]
    public void ReplaceTestTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        Assert.AreEqual("invoked: 19752", replacer.Replace("%T95SCRTL3"));
    }

    [TestMethod]
    public void ReplaceTestNumSucceededScenes()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        Assert.AreEqual("invoked: 1", replacer.Replace("%T95SCRTL4"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var scores = ImmutableList<IScore>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreTotalReplacer(scores, formatterMock);
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
        var replacer = new ScoreTotalReplacer(scores, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T95SCRTL1"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T95SCRTL2"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T95SCRTL3"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T95SCRTL4"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        Assert.AreEqual("%T95XXXXX1", replacer.Replace("%T95XXXXX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreTotalReplacer(Scores, formatterMock);
        Assert.AreEqual("%T95SCRTLX", replacer.Replace("%T95SCRTLX"));
    }
}

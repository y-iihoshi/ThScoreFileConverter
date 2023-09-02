using System.Collections.Generic;
using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Models.Th165;

namespace ThScoreFileConverter.Tests.Models.Th165;

[TestClass]
public class ScoreTotalReplacerTests
{
    private static IReadOnlyList<IScore> CreateScores()
    {
        var mock1 = ScoreTests.MockScore();
        var mock2 = ScoreTests.MockScore();
        _ = mock2.Number.Returns(mock1.Number + 1);
        return new[] { mock1, mock2 };
    }

    internal static IReadOnlyList<IScore> Scores { get; } = CreateScores();

    internal static IStatus Status { get; } = StatusTests.MockStatus();

    [TestMethod]
    public void ScoreTotalReplacerTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, Status, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ScoreTotalReplacerTestEmptyScores()
    {
        var scores = ImmutableList<IScore>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(scores, Status, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestTotalScore()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, Status, formatterMock);
        Assert.AreEqual("invoked: 2469134", replacer.Replace("%T165SCRTL1"));
    }

    [TestMethod]
    public void ReplaceTestTotalChallengeCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, Status, formatterMock);
        Assert.AreEqual("invoked: 112", replacer.Replace("%T165SCRTL2"));
    }

    [TestMethod]
    public void ReplaceTestTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, Status, formatterMock);
        Assert.AreEqual("invoked: 68", replacer.Replace("%T165SCRTL3"));
    }

    [TestMethod]
    public void ReplaceTestNumSucceededScenes()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, Status, formatterMock);
        Assert.AreEqual("invoked: 2", replacer.Replace("%T165SCRTL4"));
    }

    [TestMethod]
    public void ReplaceTestTotalNumPhotos()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, Status, formatterMock);
        Assert.AreEqual("invoked: 156", replacer.Replace("%T165SCRTL5"));
    }

    [TestMethod]
    public void ReplaceTestNumNicknames()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, Status, formatterMock);
        Assert.AreEqual("invoked: 34", replacer.Replace("%T165SCRTL6"));
    }

    [TestMethod]
    public void ReplaceTestEmptyScores()
    {
        var scores = ImmutableList<IScore>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(scores, Status, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T165SCRTL1"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T165SCRTL2"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T165SCRTL3"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T165SCRTL4"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T165SCRTL5"));
    }

    [TestMethod]
    public void ReplaceTestNullScore()
    {
        var scores = new List<IScore> { null! };
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(scores, Status, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T165SCRTL1"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T165SCRTL2"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T165SCRTL3"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T165SCRTL4"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T165SCRTL5"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, Status, formatterMock);
        Assert.AreEqual("%T165XXXXX1", replacer.Replace("%T165XXXXX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, Status, formatterMock);
        Assert.AreEqual("%T165SCRTLX", replacer.Replace("%T165SCRTLX"));
    }
}

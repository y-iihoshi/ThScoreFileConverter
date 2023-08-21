using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Models.Th095;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models.Th095;
using INumberFormatter = ThScoreFileConverter.Models.INumberFormatter;

namespace ThScoreFileConverter.Tests.Models.Th095;

[TestClass]
public class ShotExReplacerTests
{
    private static IReadOnlyList<IScore> CreateScores()
    {
        var headerMock = BestShotHeaderTests.MockBestShotHeader();

        var scoreMock = ScoreTests.MockScore();
        _ = scoreMock.LevelScene.Returns(_ => (headerMock.Level, headerMock.Scene));

        return new[] { scoreMock };
    }

    internal static IReadOnlyDictionary<(Level, int), (string, IBestShotHeader<Level>)> BestShots { get; } =
        new List<(string, IBestShotHeader<Level> header)>
        {
            (@"C:\path\to\output\bestshots\bs_02_3.png", BestShotHeaderTests.MockBestShotHeader()),
        }.ToDictionary(element => (element.header.Level, (int)element.header.Scene));

    internal static IReadOnlyList<IScore> Scores { get; } = CreateScores();

    private static INumberFormatter MockNumberFormatter()
    {
        // NOTE: NSubstitute v5.0.0 has no substitute for Moq's It.IsAny<It.IsValueType>.
        var mock = Substitute.For<INumberFormatter>();
        _ = mock.FormatNumber(Arg.Any<int>()).Returns(callInfo => $"invoked: {(int)callInfo[0]}");
        _ = mock.FormatPercent(Arg.Any<double>(), Arg.Any<int>())
            .Returns(callInfo => $"invoked: {((double)callInfo[0]).ToString($"F{(int)callInfo[1]}", CultureInfo.InvariantCulture)}%");
        return mock;
    }

    [TestMethod]
    public void ShotExReplacerTest()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ShotExReplacerTestEmptyBestShots()
    {
        var bestshots = ImmutableDictionary<(Level, int), (string, IBestShotHeader<Level>)>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(bestshots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ShotExReplacerTestInvalidBestShotPath()
    {
        var bestshots = new List<(string, IBestShotHeader<Level> header)>
        {
            ("abcde", BestShotHeaderTests.MockBestShotHeader()),
        }.ToDictionary(element => (element.header.Level, (int)element.header.Scene));
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(bestshots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ShotExReplacerTestEmptyScores()
    {
        var scores = ImmutableList<IScore>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, scores, formatterMock, @"C:\path\to\output\");
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ShotExReplacerTestEmptyOutputFilePath()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, string.Empty);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ShotExReplacerTestInvalidOutputFilePath()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, "abcde");
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestPath()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual(@"bestshots/bs_02_3.png", replacer.Replace("%T95SHOTEX231"));
    }

    [TestMethod]
    public void ReplaceTestWidth()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("4", replacer.Replace("%T95SHOTEX232"));
    }

    [TestMethod]
    public void ReplaceTestHeight()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("5", replacer.Replace("%T95SHOTEX233"));
    }

    [TestMethod]
    public void ReplaceTestScore()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("invoked: 6", replacer.Replace("%T95SHOTEX234"));
    }

    [TestMethod]
    public void ReplaceTestSlowRate()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("invoked: 7.000000%", replacer.Replace("%T95SHOTEX235"));
    }

    [TestMethod]
    public void ReplaceTestDateTime()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        var expected = DateTimeHelper.GetString(34567890);
        Assert.AreEqual(expected, replacer.Replace("%T95SHOTEX236"));
    }

    [TestMethod]
    public void ReplaceTestEmptyBestShots()
    {
        var bestshots = ImmutableDictionary<(Level, int), (string, IBestShotHeader<Level>)>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(bestshots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual(string.Empty, replacer.Replace("%T95SHOTEX231"));
        Assert.AreEqual("0", replacer.Replace("%T95SHOTEX232"));
        Assert.AreEqual("0", replacer.Replace("%T95SHOTEX233"));
        Assert.AreEqual("--------", replacer.Replace("%T95SHOTEX234"));
        Assert.AreEqual("-----%", replacer.Replace("%T95SHOTEX235"));
        Assert.AreEqual(DateTimeHelper.GetString(null), replacer.Replace("%T95SHOTEX236"));
    }

    [TestMethod]
    public void ReplaceTestInvalidBestShotPaths()
    {
        var bestshots = new List<(string, IBestShotHeader<Level> header)>
        {
            ("abcde", BestShotHeaderTests.MockBestShotHeader()),
        }.ToDictionary(element => (element.header.Level, (int)element.header.Scene));
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(bestshots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual(string.Empty, replacer.Replace("%T95SHOTEX231"));
    }

    [TestMethod]
    public void ReplaceTestEmptyScores()
    {
        var scores = ImmutableList<IScore>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual(DateTimeHelper.GetString(null), replacer.Replace("%T95SHOTEX236"));
    }

    [TestMethod]
    public void ReplaceTestNullScore()
    {
        var scores = new List<IScore> { null! };
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual(DateTimeHelper.GetString(null), replacer.Replace("%T95SHOTEX236"));
    }

    [TestMethod]
    public void ReplaceTestEmptyOutputFilePath()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, string.Empty);
        Assert.AreEqual(string.Empty, replacer.Replace("%T95SHOTEX231"));
    }

    [TestMethod]
    public void ReplaceTestInvalidOutputFilePath()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, "abcde");
        Assert.AreEqual(string.Empty, replacer.Replace("%T95SHOTEX231"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentLevel()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual(string.Empty, replacer.Replace("%T95SHOTEX131"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentScene()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual(string.Empty, replacer.Replace("%T95SHOTEX221"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentSpellCard()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("%T95SHOTEX991", replacer.Replace("%T95SHOTEX991"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("%T95XXXXXX231", replacer.Replace("%T95XXXXXX231"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("%T95SHOTEXY31", replacer.Replace("%T95SHOTEXY31"));
    }

    [TestMethod]
    public void ReplaceTestInvalidScene()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("%T95SHOTEX2X1", replacer.Replace("%T95SHOTEX2X1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("%T95SHOTEX23X", replacer.Replace("%T95SHOTEX23X"));
    }
}

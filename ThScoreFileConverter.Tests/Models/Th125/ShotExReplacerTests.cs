using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Models.Th125;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models.Th125;
using INumberFormatter = ThScoreFileConverter.Models.INumberFormatter;

namespace ThScoreFileConverter.Tests.Models.Th125;

[TestClass]
public class ShotExReplacerTests
{
    private static IReadOnlyList<IScore> CreateScores()
    {
        var headerMock = BestShotHeaderTests.MockBestShotHeader();
        var levelScene = (headerMock.Level, headerMock.Scene);
        var scoreMock = ScoreTests.MockScore();
        _ = scoreMock.LevelScene.Returns(levelScene);

        return new[] { scoreMock };
    }

    internal static IReadOnlyDictionary<(Chara, Level, int), (string, IBestShotHeader)> BestShots { get; } =
        new List<(string, IBestShotHeader header)>
        {
            (@"C:\path\to\output\bestshots\bs2_02_3.png", BestShotHeaderTests.MockBestShotHeader()),
        }.ToDictionary(element => (Chara.Hatate, element.header.Level, (int)element.header.Scene));

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
        var bestshots = ImmutableDictionary<(Chara, Level, int), (string, IBestShotHeader)>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(bestshots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ShotExReplacerTestInvalidBestShotPath()
    {
        var bestshots = new List<(string, IBestShotHeader header)>
        {
            ("abcde", BestShotHeaderTests.MockBestShotHeader()),
        }.ToDictionary(element => (Chara.Hatate, element.header.Level, (int)element.header.Scene));
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
        Assert.AreEqual(@"bestshots/bs2_02_3.png", replacer.Replace("%T125SHOTEXH231"));
    }

    [TestMethod]
    public void ReplaceTestWidth()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("4", replacer.Replace("%T125SHOTEXH232"));
    }

    [TestMethod]
    public void ReplaceTestHeight()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("5", replacer.Replace("%T125SHOTEXH233"));
    }

    [TestMethod]
    public void ReplaceTestScore()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("invoked: 13", replacer.Replace("%T125SHOTEXH234"));
    }

    [TestMethod]
    public void ReplaceTestSlowRate()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("invoked: 11.000000%", replacer.Replace("%T125SHOTEXH235"));
    }

    [TestMethod]
    public void ReplaceTestDateTime()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        var expected = DateTimeHelper.GetString(34567890);
        Assert.AreEqual(expected, replacer.Replace("%T125SHOTEXH236"));
    }

    [TestMethod]
    public void ReplaceTestDetailInfo()
    {
        // NOTE: NSubstitute v5.0.0 has no substitute for Moq's It.IsAny<It.IsValueType>.
        var formatterMock = MockNumberFormatter();
        _ = formatterMock.FormatNumber(Arg.Any<int>()).Returns(callInfo => callInfo[0].ToString() ?? string.Empty);
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        var expected = string.Join(Environment.NewLine, new string[]
        {
            @"Base Point           14",
            @"",
            @"Boss Shot!      * 16.00",
            @"Two Shot!        * 1.50",
            @"Nice Shot!      * 17.00",
            @"Angle Bonus     * 18.00",
            @"",
            @"Result Score         13",
        });
        Assert.AreEqual(expected, replacer.Replace("%T125SHOTEXH237"));
    }

    [TestMethod]
    public void ReplaceTestEmptyBestShots()
    {
        var bestshots = ImmutableDictionary<(Chara, Level, int), (string, IBestShotHeader)>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(bestshots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual(string.Empty, replacer.Replace("%T125SHOTEXH231"));
        Assert.AreEqual("0", replacer.Replace("%T125SHOTEXH232"));
        Assert.AreEqual("0", replacer.Replace("%T125SHOTEXH233"));
        Assert.AreEqual("--------", replacer.Replace("%T125SHOTEXH234"));
        Assert.AreEqual("-----%", replacer.Replace("%T125SHOTEXH235"));
        Assert.AreEqual(DateTimeHelper.GetString(null), replacer.Replace("%T125SHOTEXH236"));
        Assert.AreEqual(string.Empty, replacer.Replace("%T125SHOTEXH237"));
    }

    [TestMethod]
    public void ReplaceTestInvalidBestShotPaths()
    {
        var bestshots = new List<(string, IBestShotHeader header)>
        {
            ("abcde", BestShotHeaderTests.MockBestShotHeader()),
        }.ToDictionary(element => (Chara.Hatate, element.header.Level, (int)element.header.Scene));
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(bestshots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual(string.Empty, replacer.Replace("%T125SHOTEXH231"));
    }

    [TestMethod]
    public void ReplaceTestEmptyScores()
    {
        var scores = ImmutableList<IScore>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual(DateTimeHelper.GetString(null), replacer.Replace("%T125SHOTEXH236"));
    }

    [TestMethod]
    public void ReplaceTestNullScore()
    {
        var scores = new List<IScore> { null! };
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual(DateTimeHelper.GetString(null), replacer.Replace("%T125SHOTEXH236"));
    }

    [TestMethod]
    public void ReplaceTestEmptyOutputFilePath()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, string.Empty);
        Assert.AreEqual(string.Empty, replacer.Replace("%T125SHOTEXH231"));
    }

    [TestMethod]
    public void ReplaceTestInvalidOutputFilePath()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, "abcde");
        Assert.AreEqual(string.Empty, replacer.Replace("%T125SHOTEXH231"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual(string.Empty, replacer.Replace("%T125SHOTEXA231"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentLevel()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual(string.Empty, replacer.Replace("%T125SHOTEXH131"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentScene()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual(string.Empty, replacer.Replace("%T125SHOTEXH221"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentSpellCard()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("%T125SHOTEXH991", replacer.Replace("%T125SHOTEXH991"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("%T125XXXXXXH231", replacer.Replace("%T125XXXXXXH231"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("%T125SHOTEXX231", replacer.Replace("%T125SHOTEXX231"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("%T125SHOTEXHY31", replacer.Replace("%T125SHOTEXHY31"));
    }

    [TestMethod]
    public void ReplaceTestInvalidScene()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("%T125SHOTEXH2X1", replacer.Replace("%T125SHOTEXH2X1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("%T125SHOTEXH23X", replacer.Replace("%T125SHOTEXH23X"));
    }
}

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using Moq;
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

        var scoreMock = ScoreTests.MockScore();
        _ = scoreMock.SetupGet(m => m.LevelScene).Returns((headerMock.Object.Level, headerMock.Object.Scene));

        return new[] { scoreMock.Object };
    }

    internal static IReadOnlyDictionary<(Chara, Level, int), (string, IBestShotHeader)> BestShots { get; } =
        new List<(string, IBestShotHeader header)>
        {
            (@"C:\path\to\output\bestshots\bs2_02_3.png", BestShotHeaderTests.MockBestShotHeader().Object),
        }.ToDictionary(element => (Chara.Hatate, element.header.Level, (int)element.header.Scene));

    internal static IReadOnlyList<IScore> Scores { get; } = CreateScores();

    private static Mock<INumberFormatter> MockNumberFormatter()
    {
        var mock = new Mock<INumberFormatter>();
        _ = mock.Setup(formatter => formatter.FormatNumber(It.IsAny<It.IsValueType>()))
            .Returns((object value) => $"invoked: {value}");
        _ = mock.Setup(formatter => formatter.FormatPercent(It.IsAny<double>(), It.IsAny<int>()))
            .Returns((double value, int precision) => $"invoked: {value.ToString($"F{precision}", CultureInfo.InvariantCulture)}%");
        return mock;
    }

    [TestMethod]
    public void ShotExReplacerTest()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock.Object, @"C:\path\to\output\");
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ShotExReplacerTestEmptyBestShots()
    {
        var bestshots = ImmutableDictionary<(Chara, Level, int), (string, IBestShotHeader)>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(bestshots, Scores, formatterMock.Object, @"C:\path\to\output\");
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ShotExReplacerTestInvalidBestShotPath()
    {
        var bestshots = new List<(string, IBestShotHeader header)>
        {
            ("abcde", BestShotHeaderTests.MockBestShotHeader().Object),
        }.ToDictionary(element => (Chara.Hatate, element.header.Level, (int)element.header.Scene));
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(bestshots, Scores, formatterMock.Object, @"C:\path\to\output\");
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ShotExReplacerTestEmptyScores()
    {
        var scores = ImmutableList<IScore>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, scores, formatterMock.Object, @"C:\path\to\output\");
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ShotExReplacerTestEmptyOutputFilePath()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock.Object, string.Empty);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ShotExReplacerTestInvalidOutputFilePath()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock.Object, "abcde");
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestPath()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock.Object, @"C:\path\to\output\");
        Assert.AreEqual(@"bestshots/bs2_02_3.png", replacer.Replace("%T125SHOTEXH231"));
    }

    [TestMethod]
    public void ReplaceTestWidth()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock.Object, @"C:\path\to\output\");
        Assert.AreEqual("4", replacer.Replace("%T125SHOTEXH232"));
    }

    [TestMethod]
    public void ReplaceTestHeight()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock.Object, @"C:\path\to\output\");
        Assert.AreEqual("5", replacer.Replace("%T125SHOTEXH233"));
    }

    [TestMethod]
    public void ReplaceTestScore()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock.Object, @"C:\path\to\output\");
        Assert.AreEqual("invoked: 13", replacer.Replace("%T125SHOTEXH234"));
    }

    [TestMethod]
    public void ReplaceTestSlowRate()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock.Object, @"C:\path\to\output\");
        Assert.AreEqual("invoked: 11.000000%", replacer.Replace("%T125SHOTEXH235"));
    }

    [TestMethod]
    public void ReplaceTestDateTime()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock.Object, @"C:\path\to\output\");
        var expected = DateTimeHelper.GetString(34567890);
        Assert.AreEqual(expected, replacer.Replace("%T125SHOTEXH236"));
    }

    [TestMethod]
    public void ReplaceTestDetailInfo()
    {
        var formatterMock = MockNumberFormatter();
        _ = formatterMock
            .Setup(formatter => formatter.FormatNumber(It.IsAny<It.IsValueType>()))
            .Returns((object value) => value.ToString() ?? string.Empty);
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock.Object, @"C:\path\to\output\");
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
        var replacer = new ShotExReplacer(bestshots, Scores, formatterMock.Object, @"C:\path\to\output\");
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
            ("abcde", BestShotHeaderTests.MockBestShotHeader().Object),
        }.ToDictionary(element => (Chara.Hatate, element.header.Level, (int)element.header.Scene));
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(bestshots, Scores, formatterMock.Object, @"C:\path\to\output\");
        Assert.AreEqual(string.Empty, replacer.Replace("%T125SHOTEXH231"));
    }

    [TestMethod]
    public void ReplaceTestEmptyScores()
    {
        var scores = ImmutableList<IScore>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, scores, formatterMock.Object, @"C:\path\to\output\");
        Assert.AreEqual(DateTimeHelper.GetString(null), replacer.Replace("%T125SHOTEXH236"));
    }

    [TestMethod]
    public void ReplaceTestNullScore()
    {
        var scores = new List<IScore> { null! };
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, scores, formatterMock.Object, @"C:\path\to\output\");
        Assert.AreEqual(DateTimeHelper.GetString(null), replacer.Replace("%T125SHOTEXH236"));
    }

    [TestMethod]
    public void ReplaceTestEmptyOutputFilePath()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock.Object, string.Empty);
        Assert.AreEqual(string.Empty, replacer.Replace("%T125SHOTEXH231"));
    }

    [TestMethod]
    public void ReplaceTestInvalidOutputFilePath()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock.Object, "abcde");
        Assert.AreEqual(string.Empty, replacer.Replace("%T125SHOTEXH231"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock.Object, @"C:\path\to\output\");
        Assert.AreEqual(string.Empty, replacer.Replace("%T125SHOTEXA231"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentLevel()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock.Object, @"C:\path\to\output\");
        Assert.AreEqual(string.Empty, replacer.Replace("%T125SHOTEXH131"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentScene()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock.Object, @"C:\path\to\output\");
        Assert.AreEqual(string.Empty, replacer.Replace("%T125SHOTEXH221"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentSpellCard()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock.Object, @"C:\path\to\output\");
        Assert.AreEqual("%T125SHOTEXH991", replacer.Replace("%T125SHOTEXH991"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock.Object, @"C:\path\to\output\");
        Assert.AreEqual("%T125XXXXXXH231", replacer.Replace("%T125XXXXXXH231"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock.Object, @"C:\path\to\output\");
        Assert.AreEqual("%T125SHOTEXX231", replacer.Replace("%T125SHOTEXX231"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock.Object, @"C:\path\to\output\");
        Assert.AreEqual("%T125SHOTEXHY31", replacer.Replace("%T125SHOTEXHY31"));
    }

    [TestMethod]
    public void ReplaceTestInvalidScene()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock.Object, @"C:\path\to\output\");
        Assert.AreEqual("%T125SHOTEXH2X1", replacer.Replace("%T125SHOTEXH2X1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock.Object, @"C:\path\to\output\");
        Assert.AreEqual("%T125SHOTEXH23X", replacer.Replace("%T125SHOTEXH23X"));
    }
}

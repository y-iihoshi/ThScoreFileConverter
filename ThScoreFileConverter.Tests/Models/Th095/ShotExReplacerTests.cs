﻿using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Models.Th095;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models.Th095;

namespace ThScoreFileConverter.Tests.Models.Th095;

[TestClass]
public class ShotExReplacerTests
{
    private static IScore[] CreateScores()
    {
        var headerMock = BestShotHeaderTests.MockBestShotHeader();

        var levelScene = (headerMock.Level, headerMock.Scene);
        var scoreMock = ScoreTests.MockScore();
        _ = scoreMock.LevelScene.Returns(levelScene);

        return [scoreMock];
    }

    internal static IReadOnlyDictionary<(Level, int), (string, IBestShotHeader<Level>)> BestShots { get; } =
        new List<(string, IBestShotHeader<Level> header)>
        {
            (@"C:\path\to\output\bestshots\bs_02_3.png", BestShotHeaderTests.MockBestShotHeader()),
        }.ToDictionary(element => (element.header.Level, (int)element.header.Scene));

    internal static IReadOnlyList<IScore> Scores { get; } = CreateScores();

    [TestMethod]
    public void ShotExReplacerTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ShotExReplacerTestEmptyBestShots()
    {
        var bestshots = ImmutableDictionary<(Level, int), (string, IBestShotHeader<Level>)>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
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
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(bestshots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ShotExReplacerTestEmptyScores()
    {
        var scores = ImmutableList<IScore>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, scores, formatterMock, @"C:\path\to\output\");
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ShotExReplacerTestEmptyOutputFilePath()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, string.Empty);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ShotExReplacerTestInvalidOutputFilePath()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, "abcde");
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestPath()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual(@"bestshots/bs_02_3.png", replacer.Replace("%T95SHOTEX231"));
    }

    [TestMethod]
    public void ReplaceTestWidth()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("4", replacer.Replace("%T95SHOTEX232"));
    }

    [TestMethod]
    public void ReplaceTestHeight()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("5", replacer.Replace("%T95SHOTEX233"));
    }

    [TestMethod]
    public void ReplaceTestScore()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("invoked: 6", replacer.Replace("%T95SHOTEX234"));
    }

    [TestMethod]
    public void ReplaceTestSlowRate()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("invoked: 7.000000%", replacer.Replace("%T95SHOTEX235"));
    }

    [TestMethod]
    public void ReplaceTestDateTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        var expected = DateTimeHelper.GetString(34567890);
        Assert.AreEqual(expected, replacer.Replace("%T95SHOTEX236"));
    }

    [TestMethod]
    public void ReplaceTestEmptyBestShots()
    {
        var bestshots = ImmutableDictionary<(Level, int), (string, IBestShotHeader<Level>)>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
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
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(bestshots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual(string.Empty, replacer.Replace("%T95SHOTEX231"));
    }

    [TestMethod]
    public void ReplaceTestEmptyScores()
    {
        var scores = ImmutableList<IScore>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual(DateTimeHelper.GetString(null), replacer.Replace("%T95SHOTEX236"));
    }

    [TestMethod]
    public void ReplaceTestNullScore()
    {
        var scores = new List<IScore> { null! };
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual(DateTimeHelper.GetString(null), replacer.Replace("%T95SHOTEX236"));
    }

    [TestMethod]
    public void ReplaceTestEmptyOutputFilePath()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, string.Empty);
        Assert.AreEqual(string.Empty, replacer.Replace("%T95SHOTEX231"));
    }

    [TestMethod]
    public void ReplaceTestInvalidOutputFilePath()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, "abcde");
        Assert.AreEqual(string.Empty, replacer.Replace("%T95SHOTEX231"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual(string.Empty, replacer.Replace("%T95SHOTEX131"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentScene()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual(string.Empty, replacer.Replace("%T95SHOTEX221"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentSpellCard()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("%T95SHOTEX991", replacer.Replace("%T95SHOTEX991"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("%T95XXXXXX231", replacer.Replace("%T95XXXXXX231"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("%T95SHOTEXY31", replacer.Replace("%T95SHOTEXY31"));
    }

    [TestMethod]
    public void ReplaceTestInvalidScene()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("%T95SHOTEX2X1", replacer.Replace("%T95SHOTEX2X1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("%T95SHOTEX23X", replacer.Replace("%T95SHOTEX23X"));
    }
}

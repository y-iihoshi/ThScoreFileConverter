using System.Collections.Immutable;
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
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ShotExReplacerTestEmptyBestShots()
    {
        var bestshots = ImmutableDictionary<(Level, int), (string, IBestShotHeader<Level>)>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(bestshots, Scores, formatterMock, @"C:\path\to\output\");
        _ = replacer.ShouldNotBeNull();
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
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ShotExReplacerTestEmptyScores()
    {
        var scores = ImmutableList<IScore>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, scores, formatterMock, @"C:\path\to\output\");
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ShotExReplacerTestEmptyOutputFilePath()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, string.Empty);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ShotExReplacerTestInvalidOutputFilePath()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, "abcde");
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTestPath()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T95SHOTEX231").ShouldBe(@"bestshots/bs_02_3.png");
    }

    [TestMethod]
    public void ReplaceTestWidth()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T95SHOTEX232").ShouldBe("4");
    }

    [TestMethod]
    public void ReplaceTestHeight()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T95SHOTEX233").ShouldBe("5");
    }

    [TestMethod]
    public void ReplaceTestScore()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T95SHOTEX234").ShouldBe("invoked: 6");
    }

    [TestMethod]
    public void ReplaceTestSlowRate()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T95SHOTEX235").ShouldBe("invoked: 7.000000%");
    }

    [TestMethod]
    public void ReplaceTestDateTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        var expected = DateTimeHelper.GetString(34567890);
        replacer.Replace("%T95SHOTEX236").ShouldBe(expected);
    }

    [TestMethod]
    public void ReplaceTestEmptyBestShots()
    {
        var bestshots = ImmutableDictionary<(Level, int), (string, IBestShotHeader<Level>)>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(bestshots, Scores, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T95SHOTEX231").ShouldBeEmpty();
        replacer.Replace("%T95SHOTEX232").ShouldBe("0");
        replacer.Replace("%T95SHOTEX233").ShouldBe("0");
        replacer.Replace("%T95SHOTEX234").ShouldBe("--------");
        replacer.Replace("%T95SHOTEX235").ShouldBe("-----%");
        replacer.Replace("%T95SHOTEX236").ShouldBe(DateTimeHelper.GetString(null));
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
        replacer.Replace("%T95SHOTEX231").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestEmptyScores()
    {
        var scores = ImmutableList<IScore>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, scores, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T95SHOTEX236").ShouldBe(DateTimeHelper.GetString(null));
    }

    [TestMethod]
    public void ReplaceTestNullScore()
    {
        var scores = new List<IScore> { null! };
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, scores, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T95SHOTEX236").ShouldBe(DateTimeHelper.GetString(null));
    }

    [TestMethod]
    public void ReplaceTestEmptyOutputFilePath()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, string.Empty);
        replacer.Replace("%T95SHOTEX231").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestInvalidOutputFilePath()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, "abcde");
        replacer.Replace("%T95SHOTEX231").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestNonexistentLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T95SHOTEX131").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestNonexistentScene()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T95SHOTEX221").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestNonexistentSpellCard()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T95SHOTEX991").ShouldBe("%T95SHOTEX991");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T95XXXXXX231").ShouldBe("%T95XXXXXX231");
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T95SHOTEXY31").ShouldBe("%T95SHOTEXY31");
    }

    [TestMethod]
    public void ReplaceTestInvalidScene()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T95SHOTEX2X1").ShouldBe("%T95SHOTEX2X1");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T95SHOTEX23X").ShouldBe("%T95SHOTEX23X");
    }
}

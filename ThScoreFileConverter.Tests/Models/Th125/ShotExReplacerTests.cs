using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Models.Th125;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models.Th125;
using INumberFormatter = ThScoreFileConverter.Models.INumberFormatter;

namespace ThScoreFileConverter.Tests.Models.Th125;

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

    internal static IReadOnlyDictionary<(Chara, Level, int), (string, IBestShotHeader)> BestShots { get; } =
        new List<(string, IBestShotHeader header)>
        {
            (@"C:\path\to\output\bestshots\bs2_02_3.png", BestShotHeaderTests.MockBestShotHeader()),
        }.ToDictionary(element => (Chara.Hatate, element.header.Level, (int)element.header.Scene));

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
        var bestshots = ImmutableDictionary<(Chara, Level, int), (string, IBestShotHeader)>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(bestshots, Scores, formatterMock, @"C:\path\to\output\");
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ShotExReplacerTestInvalidBestShotPath()
    {
        var bestshots = new List<(string, IBestShotHeader header)>
        {
            ("abcde", BestShotHeaderTests.MockBestShotHeader()),
        }.ToDictionary(element => (Chara.Hatate, element.header.Level, (int)element.header.Scene));
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
        replacer.Replace("%T125SHOTEXH231").ShouldBe(@"bestshots/bs2_02_3.png");
    }

    [TestMethod]
    public void ReplaceTestWidth()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T125SHOTEXH232").ShouldBe("4");
    }

    [TestMethod]
    public void ReplaceTestHeight()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T125SHOTEXH233").ShouldBe("5");
    }

    [TestMethod]
    public void ReplaceTestScore()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T125SHOTEXH234").ShouldBe("invoked: 13");
    }

    [TestMethod]
    public void ReplaceTestSlowRate()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T125SHOTEXH235").ShouldBe("invoked: 11.000000%");
    }

    [TestMethod]
    public void ReplaceTestDateTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        var expected = DateTimeHelper.GetString(34567890);
        replacer.Replace("%T125SHOTEXH236").ShouldBe(expected);
    }

    [TestMethod]
    public void ReplaceTestDetailInfo()
    {
        static string[] GetExpectedStringArray()
        {
            return [
                @"Base Point           14",
                @"",
                @"Boss Shot!      * 16.00",
                @"Two Shot!        * 1.50",
                @"Nice Shot!      * 17.00",
                @"Angle Bonus     * 18.00",
                @"",
                @"Result Score         13",
            ];
        }

        /// NOTE: Should not use <see cref="NumberFormatterTests.Mock"/> here
        var formatterMock = Substitute.For<INumberFormatter>();
        _ = formatterMock.FormatNumber(Arg.Any<int>()).Returns(callInfo => callInfo[0].ToString() ?? string.Empty);
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        var expected = string.Join(Environment.NewLine, GetExpectedStringArray());
        replacer.Replace("%T125SHOTEXH237").ShouldBe(expected);
    }

    [TestMethod]
    public void ReplaceTestEmptyBestShots()
    {
        var bestshots = ImmutableDictionary<(Chara, Level, int), (string, IBestShotHeader)>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(bestshots, Scores, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T125SHOTEXH231").ShouldBeEmpty();
        replacer.Replace("%T125SHOTEXH232").ShouldBe("0");
        replacer.Replace("%T125SHOTEXH233").ShouldBe("0");
        replacer.Replace("%T125SHOTEXH234").ShouldBe("--------");
        replacer.Replace("%T125SHOTEXH235").ShouldBe("-----%");
        replacer.Replace("%T125SHOTEXH236").ShouldBe(DateTimeHelper.GetString(null));
        replacer.Replace("%T125SHOTEXH237").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestInvalidBestShotPaths()
    {
        var bestshots = new List<(string, IBestShotHeader header)>
        {
            ("abcde", BestShotHeaderTests.MockBestShotHeader()),
        }.ToDictionary(element => (Chara.Hatate, element.header.Level, (int)element.header.Scene));
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(bestshots, Scores, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T125SHOTEXH231").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestEmptyScores()
    {
        var scores = ImmutableList<IScore>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, scores, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T125SHOTEXH236").ShouldBe(DateTimeHelper.GetString(null));
    }

    [TestMethod]
    public void ReplaceTestNullScore()
    {
        var scores = new List<IScore> { null! };
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, scores, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T125SHOTEXH236").ShouldBe(DateTimeHelper.GetString(null));
    }

    [TestMethod]
    public void ReplaceTestEmptyOutputFilePath()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, string.Empty);
        replacer.Replace("%T125SHOTEXH231").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestInvalidOutputFilePath()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, "abcde");
        replacer.Replace("%T125SHOTEXH231").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestNonexistentChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T125SHOTEXA231").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestNonexistentLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T125SHOTEXH131").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestNonexistentScene()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T125SHOTEXH221").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestNonexistentSpellCard()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T125SHOTEXH991").ShouldBe("%T125SHOTEXH991");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T125XXXXXXH231").ShouldBe("%T125XXXXXXH231");
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T125SHOTEXX231").ShouldBe("%T125SHOTEXX231");
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T125SHOTEXHY31").ShouldBe("%T125SHOTEXHY31");
    }

    [TestMethod]
    public void ReplaceTestInvalidScene()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T125SHOTEXH2X1").ShouldBe("%T125SHOTEXH2X1");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, Scores, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T125SHOTEXH23X").ShouldBe("%T125SHOTEXH23X");
    }
}

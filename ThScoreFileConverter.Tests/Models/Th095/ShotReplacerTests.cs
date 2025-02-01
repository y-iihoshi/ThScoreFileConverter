using System.Collections.Immutable;
using ThScoreFileConverter.Core.Models.Th095;
using ThScoreFileConverter.Models.Th095;

namespace ThScoreFileConverter.Tests.Models.Th095;

[TestClass]
public class ShotReplacerTests
{
    internal static IReadOnlyDictionary<(Level, int), (string, IBestShotHeader<Level>)> BestShots { get; } =
        new List<(string, IBestShotHeader<Level> header)>
        {
            (@"C:\path\to\output\bestshots\bs_02_3.png", BestShotHeaderTests.MockBestShotHeader()),
        }.ToDictionary(element => (element.header.Level, (int)element.header.Scene));

    [TestMethod]
    public void ShotReplacerTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ShotReplacerTestEmptyBestShots()
    {
        var bestshots = ImmutableDictionary<(Level, int), (string, IBestShotHeader<Level>)>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotReplacer(bestshots, formatterMock, @"C:\path\to\output\");
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ShotReplacerTestInvalidBestShotPath()
    {
        var bestshots = new List<(string, IBestShotHeader<Level> header)>
        {
            ("abcde", BestShotHeaderTests.MockBestShotHeader()),
        }.ToDictionary(element => (element.header.Level, (int)element.header.Scene));
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotReplacer(bestshots, formatterMock, @"C:\path\to\output\");
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ShotReplacerTestEmptyOutputFilePath()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotReplacer(BestShots, formatterMock, string.Empty);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ShotReplacerTestInvalidOutputFilePath()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotReplacer(BestShots, formatterMock, "abcde");
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTest()
    {
        static string[] GetExpectedStringArray()
        {
            return [
                @"<img src=""bestshots/bs_02_3.png"" alt=""ClearData: invoked: 6",
                @"Slow: invoked: 7.000000%",
                @"SpellName: abcde"" title=""ClearData: invoked: 6",
                @"Slow: invoked: 7.000000%",
                @"SpellName: abcde"" border=0>",
            ];
        }

        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        var expected = string.Join(Environment.NewLine, GetExpectedStringArray());

        replacer.Replace("%T95SHOT23").ShouldBe(expected);
    }

    [TestMethod]
    public void ReplaceTestEmptyBestShots()
    {
        var bestshots = ImmutableDictionary<(Level, int), (string, IBestShotHeader<Level>)>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotReplacer(bestshots, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T95SHOT23").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestInvalidBestShotPaths()
    {
        var bestshots = new List<(string, IBestShotHeader<Level> header)>
        {
            ("abcde", BestShotHeaderTests.MockBestShotHeader()),
        }.ToDictionary(element => (element.header.Level, (int)element.header.Scene));
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotReplacer(bestshots, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T95SHOT23").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestEmptyOutputFilePath()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotReplacer(BestShots, formatterMock, string.Empty);
        replacer.Replace("%T95SHOT23").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestInvalidOutputFilePath()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotReplacer(BestShots, formatterMock, "abcde");
        replacer.Replace("%T95SHOT23").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestNonexistentLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T95SHOT13").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestNonexistentScene()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T95SHOT22").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestNonexistentSpellCard()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T95SHOT99").ShouldBe("%T95SHOT99");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T95XXXX23").ShouldBe("%T95XXXX23");
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T95SHOTY3").ShouldBe("%T95SHOTY3");
    }

    [TestMethod]
    public void ReplaceTestInvalidScene()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T95SHOT2X").ShouldBe("%T95SHOT2X");
    }
}

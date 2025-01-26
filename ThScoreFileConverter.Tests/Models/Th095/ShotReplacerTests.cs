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
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ShotReplacerTestEmptyBestShots()
    {
        var bestshots = ImmutableDictionary<(Level, int), (string, IBestShotHeader<Level>)>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotReplacer(bestshots, formatterMock, @"C:\path\to\output\");
        Assert.IsNotNull(replacer);
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
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ShotReplacerTestEmptyOutputFilePath()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotReplacer(BestShots, formatterMock, string.Empty);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ShotReplacerTestInvalidOutputFilePath()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotReplacer(BestShots, formatterMock, "abcde");
        Assert.IsNotNull(replacer);
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

        Assert.AreEqual(expected, replacer.Replace("%T95SHOT23"));
    }

    [TestMethod]
    public void ReplaceTestEmptyBestShots()
    {
        var bestshots = ImmutableDictionary<(Level, int), (string, IBestShotHeader<Level>)>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotReplacer(bestshots, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual(string.Empty, replacer.Replace("%T95SHOT23"));
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
        Assert.AreEqual(string.Empty, replacer.Replace("%T95SHOT23"));
    }

    [TestMethod]
    public void ReplaceTestEmptyOutputFilePath()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotReplacer(BestShots, formatterMock, string.Empty);
        Assert.AreEqual(string.Empty, replacer.Replace("%T95SHOT23"));
    }

    [TestMethod]
    public void ReplaceTestInvalidOutputFilePath()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotReplacer(BestShots, formatterMock, "abcde");
        Assert.AreEqual(string.Empty, replacer.Replace("%T95SHOT23"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual(string.Empty, replacer.Replace("%T95SHOT13"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentScene()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual(string.Empty, replacer.Replace("%T95SHOT22"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentSpellCard()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("%T95SHOT99", replacer.Replace("%T95SHOT99"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("%T95XXXX23", replacer.Replace("%T95XXXX23"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("%T95SHOTY3", replacer.Replace("%T95SHOTY3"));
    }

    [TestMethod]
    public void ReplaceTestInvalidScene()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("%T95SHOT2X", replacer.Replace("%T95SHOT2X"));
    }
}

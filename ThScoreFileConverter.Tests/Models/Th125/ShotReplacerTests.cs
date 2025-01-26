using System.Collections.Immutable;
using ThScoreFileConverter.Core.Models.Th125;
using ThScoreFileConverter.Models.Th125;

namespace ThScoreFileConverter.Tests.Models.Th125;

[TestClass]
public class ShotReplacerTests
{
    internal static IReadOnlyDictionary<(Chara, Level, int), (string, IBestShotHeader)> BestShots { get; } =
        new List<(string, IBestShotHeader header)>
        {
            (@"C:\path\to\output\bestshots\bs2_02_3.png", BestShotHeaderTests.MockBestShotHeader()),
        }.ToDictionary(element => (Chara.Hatate, element.header.Level, (int)element.header.Scene));

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
        var bestshots = ImmutableDictionary<(Chara, Level, int), (string, IBestShotHeader)>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotReplacer(bestshots, formatterMock, @"C:\path\to\output\");
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ShotReplacerTestInvalidBestShotPath()
    {
        var bestshots = new List<(string, IBestShotHeader header)>
        {
            ("abcde", BestShotHeaderTests.MockBestShotHeader()),
        }.ToDictionary(element => (Chara.Hatate, element.header.Level, (int)element.header.Scene));
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
                @"<img src=""bestshots/bs2_02_3.png"" alt=""ClearData: invoked: 13",
                @"Slow: invoked: 11.000000%",
                @"SpellName: abcde"" title=""ClearData: invoked: 13",
                @"Slow: invoked: 11.000000%",
                @"SpellName: abcde"" border=0>",
            ];
        }

        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        var expected = string.Join(Environment.NewLine, GetExpectedStringArray());

        Assert.AreEqual(expected, replacer.Replace("%T125SHOTH23"));
    }

    [TestMethod]
    public void ReplaceTestEmptyBestShots()
    {
        var bestshots = ImmutableDictionary<(Chara, Level, int), (string, IBestShotHeader)>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotReplacer(bestshots, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual(string.Empty, replacer.Replace("%T125SHOTH23"));
    }

    [TestMethod]
    public void ReplaceTestInvalidBestShotPaths()
    {
        var bestshots = new List<(string, IBestShotHeader header)>
        {
            ("abcde", BestShotHeaderTests.MockBestShotHeader()),
        }.ToDictionary(element => (Chara.Hatate, element.header.Level, (int)element.header.Scene));
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotReplacer(bestshots, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual(string.Empty, replacer.Replace("%T125SHOTH23"));
    }

    [TestMethod]
    public void ReplaceTestEmptyOutputFilePath()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotReplacer(BestShots, formatterMock, string.Empty);
        Assert.AreEqual(string.Empty, replacer.Replace("%T125SHOTH23"));
    }

    [TestMethod]
    public void ReplaceTestInvalidOutputFilePath()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotReplacer(BestShots, formatterMock, "abcde");
        Assert.AreEqual(string.Empty, replacer.Replace("%T125SHOTH23"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual(string.Empty, replacer.Replace("%T125SHOTA23"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual(string.Empty, replacer.Replace("%T125SHOTH13"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentScene()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual(string.Empty, replacer.Replace("%T125SHOTH22"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentSpellCard()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("%T125SHOTH99", replacer.Replace("%T125SHOTH99"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("%T125XXXXH23", replacer.Replace("%T125XXXXH23"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("%T125SHOTX23", replacer.Replace("%T125SHOTX23"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("%T125SHOTHY3", replacer.Replace("%T125SHOTHY3"));
    }

    [TestMethod]
    public void ReplaceTestInvalidScene()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("%T125SHOTH2X", replacer.Replace("%T125SHOTH2X"));
    }
}

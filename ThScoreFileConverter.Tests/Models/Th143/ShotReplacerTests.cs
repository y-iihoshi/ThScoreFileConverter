using System.Collections.Immutable;
using ThScoreFileConverter.Core.Models.Th143;
using ThScoreFileConverter.Models.Th143;

namespace ThScoreFileConverter.Tests.Models.Th143;

[TestClass]
public class ShotReplacerTests
{
    internal static IReadOnlyDictionary<(Day, int), (string, IBestShotHeader)> BestShots { get; } =
        new List<(string, IBestShotHeader header)>
        {
            (@"C:\path\to\output\bestshots\sc02_03.png", BestShotHeaderTests.MockBestShotHeader()),
        }.ToDictionary(element => (element.header.Day, (int)element.header.Scene));

    [TestMethod]
    public void ShotReplacerTest()
    {
        var replacer = new ShotReplacer(BestShots, @"C:\path\to\output\");
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ShotReplacerTestEmptyBestShots()
    {
        var bestshots = ImmutableDictionary<(Day, int), (string, IBestShotHeader)>.Empty;
        var replacer = new ShotReplacer(bestshots, @"C:\path\to\output\");
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ShotReplacerTestInvalidBestShotPath()
    {
        var bestshots = new List<(string, IBestShotHeader header)>
        {
            ("abcde", BestShotHeaderTests.MockBestShotHeader()),
        }.ToDictionary(element => (element.header.Day, (int)element.header.Scene));
        var replacer = new ShotReplacer(bestshots, @"C:\path\to\output\");
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ShotReplacerTestEmptyOutputFilePath()
    {
        var replacer = new ShotReplacer(BestShots, string.Empty);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ShotReplacerTestInvalidOutputFilePath()
    {
        var replacer = new ShotReplacer(BestShots, "abcde");
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTest()
    {
        static string[] GetExpectedStringArray()
        {
            return [
                @"<img src=""bestshots/sc02_03.png""",
                @"alt=""SpellName: 劈音「ピアッシングサークル」""",
                @"title=""SpellName: 劈音「ピアッシングサークル」"" border=0>",
            ];
        }

        var replacer = new ShotReplacer(BestShots, @"C:\path\to\output\");
        var expected = string.Join(" ", GetExpectedStringArray());

        replacer.Replace("%T143SHOT23").ShouldBe(expected);
    }

    [TestMethod]
    public void ReplaceTestScene10()
    {
        var replacer = new ShotReplacer(BestShots, @"C:\path\to\output\");
        replacer.Replace("%T143SHOTL0").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestEmptyBestShots()
    {
        var bestshots = ImmutableDictionary<(Day, int), (string, IBestShotHeader)>.Empty;
        var replacer = new ShotReplacer(bestshots, @"C:\path\to\output\");
        replacer.Replace("%T143SHOT23").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestInvalidBestShotPaths()
    {
        var bestshots = new List<(string, IBestShotHeader header)>
        {
            ("abcde", BestShotHeaderTests.MockBestShotHeader()),
        }.ToDictionary(element => (element.header.Day, (int)element.header.Scene));
        var replacer = new ShotReplacer(bestshots, @"C:\path\to\output\");
        replacer.Replace("%T143SHOT23").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestEmptyOutputFilePath()
    {
        var replacer = new ShotReplacer(BestShots, string.Empty);
        replacer.Replace("%T143SHOT23").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestInvalidOutputFilePath()
    {
        var replacer = new ShotReplacer(BestShots, "abcde");
        replacer.Replace("%T143SHOT23").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestNonexistentDay()
    {
        var replacer = new ShotReplacer(BestShots, @"C:\path\to\output\");
        replacer.Replace("%T143SHOT13").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestNonexistentScene()
    {
        var replacer = new ShotReplacer(BestShots, @"C:\path\to\output\");
        replacer.Replace("%T143SHOT22").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestNonexistentSpellCard()
    {
        var replacer = new ShotReplacer(BestShots, @"C:\path\to\output\");
        replacer.Replace("%T143SHOT17").ShouldBe("%T143SHOT17");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new ShotReplacer(BestShots, @"C:\path\to\output\");
        replacer.Replace("%T143XXXX23").ShouldBe("%T143XXXX23");
    }

    [TestMethod]
    public void ReplaceTestInvalidDay()
    {
        var replacer = new ShotReplacer(BestShots, @"C:\path\to\output\");
        replacer.Replace("%T143SHOTX3").ShouldBe("%T143SHOTX3");
    }

    [TestMethod]
    public void ReplaceTestInvalidScene()
    {
        var replacer = new ShotReplacer(BestShots, @"C:\path\to\output\");
        replacer.Replace("%T143SHOT2X").ShouldBe("%T143SHOT2X");
    }
}

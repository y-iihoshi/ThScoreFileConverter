using System.Collections.Immutable;
using ThScoreFileConverter.Core.Models.Th165;
using ThScoreFileConverter.Models.Th165;

namespace ThScoreFileConverter.Tests.Models.Th165;

[TestClass]
public class ShotReplacerTests
{
    internal static IReadOnlyDictionary<(Day, int), (string, IBestShotHeader)> BestShots { get; } =
        new List<(string, IBestShotHeader header)>
        {
            (@"C:\path\to\output\bestshots\bs02_03.png", BestShotHeaderTests.MockBestShotHeader()),
        }.ToDictionary(element => (element.header.Weekday, (int)element.header.Dream));

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
            ("abcde", BestShotHeaderTests.MockInitialBestShotHeader()),
        }.ToDictionary(element => (element.header.Weekday, (int)element.header.Dream));
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
                @"<img src=""bestshots/bs02_03.png""",
                @"alt=""SpellName: 弾符「ラビットファルコナー」""",
                @"title=""SpellName: 弾符「ラビットファルコナー」"" border=0>",
            ];
        }

        var replacer = new ShotReplacer(BestShots, @"C:\path\to\output\");
        var expected = string.Join(" ", GetExpectedStringArray());

        replacer.Replace("%T165SHOT023").ShouldBe(expected);
    }

    [TestMethod]
    public void ReplaceTestEmptyBestShots()
    {
        var bestshots = ImmutableDictionary<(Day, int), (string, IBestShotHeader)>.Empty;
        var replacer = new ShotReplacer(bestshots, @"C:\path\to\output\");
        replacer.Replace("%T165SHOT023").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestInvalidBestShotPaths()
    {
        var bestshots = new List<(string, IBestShotHeader header)>
        {
            ("abcde", BestShotHeaderTests.MockInitialBestShotHeader()),
        }.ToDictionary(element => (element.header.Weekday, (int)element.header.Dream));
        var replacer = new ShotReplacer(bestshots, @"C:\path\to\output\");
        replacer.Replace("%T165SHOT023").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestEmptyOutputFilePath()
    {
        var replacer = new ShotReplacer(BestShots, string.Empty);
        replacer.Replace("%T165SHOT023").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestInvalidOutputFilePath()
    {
        var replacer = new ShotReplacer(BestShots, "abcde");
        replacer.Replace("%T165SHOT023").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestNonexistentDay()
    {
        var replacer = new ShotReplacer(BestShots, @"C:\path\to\output\");
        replacer.Replace("%T165SHOT033").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestNonexistentScene()
    {
        var replacer = new ShotReplacer(BestShots, @"C:\path\to\output\");
        replacer.Replace("%T165SHOT022").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestNonexistentSpellCard()
    {
        var replacer = new ShotReplacer(BestShots, @"C:\path\to\output\");
        replacer.Replace("%T165SHOT013").ShouldBe("%T165SHOT013");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new ShotReplacer(BestShots, @"C:\path\to\output\");
        replacer.Replace("%T165XXXX023").ShouldBe("%T165XXXX023");
    }

    [TestMethod]
    public void ReplaceTestInvalidDay()
    {
        var replacer = new ShotReplacer(BestShots, @"C:\path\to\output\");
        replacer.Replace("%T165SHOTXX3").ShouldBe("%T165SHOTXX3");
    }

    [TestMethod]
    public void ReplaceTestInvalidScene()
    {
        var replacer = new ShotReplacer(BestShots, @"C:\path\to\output\");
        replacer.Replace("%T165SHOT02X").ShouldBe("%T165SHOT02X");
    }
}

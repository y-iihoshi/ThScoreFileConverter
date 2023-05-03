using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ThScoreFileConverter.Core.Models.Th165;
using ThScoreFileConverter.Models.Th165;

namespace ThScoreFileConverter.Tests.Models.Th165;

[TestClass]
public class ShotReplacerTests
{
    internal static IReadOnlyDictionary<(Day, int), (string, IBestShotHeader)> BestShots { get; } =
        new List<(string, IBestShotHeader header)>
        {
            (@"C:\path\to\output\bestshots\bs02_03.png", BestShotHeaderTests.MockBestShotHeader().Object),
        }.ToDictionary(element => (element.header.Weekday, (int)element.header.Dream));

    [TestMethod]
    public void ShotReplacerTest()
    {
        var replacer = new ShotReplacer(BestShots, @"C:\path\to\output\");
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ShotReplacerTestEmptyBestShots()
    {
        var bestshots = ImmutableDictionary<(Day, int), (string, IBestShotHeader)>.Empty;
        var replacer = new ShotReplacer(bestshots, @"C:\path\to\output\");
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ShotReplacerTestInvalidBestShotPath()
    {
        var bestshots = new List<(string, IBestShotHeader header)>
        {
            ("abcde", BestShotHeaderTests.MockInitialBestShotHeader().Object),
        }.ToDictionary(element => (element.header.Weekday, (int)element.header.Dream));
        var replacer = new ShotReplacer(bestshots, @"C:\path\to\output\");
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ShotReplacerTestEmptyOutputFilePath()
    {
        var replacer = new ShotReplacer(BestShots, string.Empty);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ShotReplacerTestInvalidOutputFilePath()
    {
        var replacer = new ShotReplacer(BestShots, "abcde");
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTest()
    {
        var replacer = new ShotReplacer(BestShots, @"C:\path\to\output\");
        var expected = string.Join(" ", new string[]
        {
            @"<img src=""bestshots/bs02_03.png""",
            @"alt=""SpellName: 弾符「ラビットファルコナー」""",
            @"title=""SpellName: 弾符「ラビットファルコナー」"" border=0>",
        });

        Assert.AreEqual(expected, replacer.Replace("%T165SHOT023"));
    }

    [TestMethod]
    public void ReplaceTestEmptyBestShots()
    {
        var bestshots = ImmutableDictionary<(Day, int), (string, IBestShotHeader)>.Empty;
        var replacer = new ShotReplacer(bestshots, @"C:\path\to\output\");
        Assert.AreEqual(string.Empty, replacer.Replace("%T165SHOT023"));
    }

    [TestMethod]
    public void ReplaceTestInvalidBestShotPaths()
    {
        var bestshots = new List<(string, IBestShotHeader header)>
        {
            ("abcde", BestShotHeaderTests.MockInitialBestShotHeader().Object),
        }.ToDictionary(element => (element.header.Weekday, (int)element.header.Dream));
        var replacer = new ShotReplacer(bestshots, @"C:\path\to\output\");
        Assert.AreEqual(string.Empty, replacer.Replace("%T165SHOT023"));
    }

    [TestMethod]
    public void ReplaceTestEmptyOutputFilePath()
    {
        var replacer = new ShotReplacer(BestShots, string.Empty);
        Assert.AreEqual(string.Empty, replacer.Replace("%T165SHOT023"));
    }

    [TestMethod]
    public void ReplaceTestInvalidOutputFilePath()
    {
        var replacer = new ShotReplacer(BestShots, "abcde");
        Assert.AreEqual(string.Empty, replacer.Replace("%T165SHOT023"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentDay()
    {
        var replacer = new ShotReplacer(BestShots, @"C:\path\to\output\");
        Assert.AreEqual(string.Empty, replacer.Replace("%T165SHOT033"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentScene()
    {
        var replacer = new ShotReplacer(BestShots, @"C:\path\to\output\");
        Assert.AreEqual(string.Empty, replacer.Replace("%T165SHOT022"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentSpellCard()
    {
        var replacer = new ShotReplacer(BestShots, @"C:\path\to\output\");
        Assert.AreEqual("%T165SHOT013", replacer.Replace("%T165SHOT013"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new ShotReplacer(BestShots, @"C:\path\to\output\");
        Assert.AreEqual("%T165XXXX023", replacer.Replace("%T165XXXX023"));
    }

    [TestMethod]
    public void ReplaceTestInvalidDay()
    {
        var replacer = new ShotReplacer(BestShots, @"C:\path\to\output\");
        Assert.AreEqual("%T165SHOTXX3", replacer.Replace("%T165SHOTXX3"));
    }

    [TestMethod]
    public void ReplaceTestInvalidScene()
    {
        var replacer = new ShotReplacer(BestShots, @"C:\path\to\output\");
        Assert.AreEqual("%T165SHOT02X", replacer.Replace("%T165SHOT02X"));
    }
}

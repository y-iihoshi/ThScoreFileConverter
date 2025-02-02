using System.Collections.Immutable;
using ThScoreFileConverter.Core.Models.Th143;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models.Th143;

namespace ThScoreFileConverter.Tests.Models.Th143;

[TestClass]
public class ShotExReplacerTests
{
    internal static IReadOnlyDictionary<(Day, int), (string, IBestShotHeader)> BestShots { get; } =
        new List<(string, IBestShotHeader header)>
        {
            (@"C:\path\to\output\bestshots\sc02_03.png", BestShotHeaderTests.MockBestShotHeader()),
        }.ToDictionary(element => (element.header.Day, (int)element.header.Scene));

    [TestMethod]
    public void ShotExReplacerTest()
    {
        var replacer = new ShotExReplacer(BestShots, @"C:\path\to\output\");
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ShotExReplacerTestEmptyBestShots()
    {
        var bestshots = ImmutableDictionary<(Day, int), (string, IBestShotHeader)>.Empty;
        var replacer = new ShotExReplacer(bestshots, @"C:\path\to\output\");
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ShotExReplacerTestInvalidBestShotPath()
    {
        var bestshots = new List<(string, IBestShotHeader header)>
        {
            ("abcde", BestShotHeaderTests.MockBestShotHeader()),
        }.ToDictionary(element => (element.header.Day, (int)element.header.Scene));
        var replacer = new ShotExReplacer(bestshots, @"C:\path\to\output\");
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ShotExReplacerTestEmptyOutputFilePath()
    {
        var replacer = new ShotExReplacer(BestShots, string.Empty);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ShotExReplacerTestInvalidOutputFilePath()
    {
        var replacer = new ShotExReplacer(BestShots, "abcde");
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTestPath()
    {
        var replacer = new ShotExReplacer(BestShots, @"C:\path\to\output\");
        replacer.Replace("%T143SHOTEX231").ShouldBe(@"bestshots/sc02_03.png");
    }

    [TestMethod]
    public void ReplaceTestWidth()
    {
        var replacer = new ShotExReplacer(BestShots, @"C:\path\to\output\");
        replacer.Replace("%T143SHOTEX232").ShouldBe("4");
    }

    [TestMethod]
    public void ReplaceTestHeight()
    {
        var replacer = new ShotExReplacer(BestShots, @"C:\path\to\output\");
        replacer.Replace("%T143SHOTEX233").ShouldBe("5");
    }

    [TestMethod]
    public void ReplaceTestDateTime()
    {
        var replacer = new ShotExReplacer(BestShots, @"C:\path\to\output\");
        var expected = DateTimeHelper.GetString(6);
        replacer.Replace("%T143SHOTEX234").ShouldBe(expected);
    }

    [TestMethod]
    public void ReplaceTestScene10()
    {
        var replacer = new ShotExReplacer(BestShots, @"C:\path\to\output\");
        replacer.Replace("%T143SHOTEXL01").ShouldBeEmpty();
        replacer.Replace("%T143SHOTEXL02").ShouldBe("0");
        replacer.Replace("%T143SHOTEXL03").ShouldBe("0");
        replacer.Replace("%T143SHOTEXL04").ShouldBe(DateTimeHelper.GetString(null));
    }

    [TestMethod]
    public void ReplaceTestEmptyBestShots()
    {
        var bestshots = ImmutableDictionary<(Day, int), (string, IBestShotHeader)>.Empty;
        var replacer = new ShotExReplacer(bestshots, @"C:\path\to\output\");
        replacer.Replace("%T143SHOTEX231").ShouldBeEmpty();
        replacer.Replace("%T143SHOTEX232").ShouldBe("0");
        replacer.Replace("%T143SHOTEX233").ShouldBe("0");
        replacer.Replace("%T143SHOTEX234").ShouldBe(DateTimeHelper.GetString(null));
    }

    [TestMethod]
    public void ReplaceTestInvalidBestShotPaths()
    {
        var bestshots = new List<(string, IBestShotHeader header)>
        {
            ("abcde", BestShotHeaderTests.MockBestShotHeader()),
        }.ToDictionary(element => (element.header.Day, (int)element.header.Scene));
        var replacer = new ShotExReplacer(bestshots, @"C:\path\to\output\");
        replacer.Replace("%T143SHOTEX231").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestEmptyOutputFilePath()
    {
        var replacer = new ShotExReplacer(BestShots, string.Empty);
        replacer.Replace("%T143SHOTEX231").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestInvalidOutputFilePath()
    {
        var replacer = new ShotExReplacer(BestShots, "abcde");
        replacer.Replace("%T143SHOTEX231").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestNonexistentDay()
    {
        var replacer = new ShotExReplacer(BestShots, @"C:\path\to\output\");
        replacer.Replace("%T143SHOTEX131").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestNonexistentScene()
    {
        var replacer = new ShotExReplacer(BestShots, @"C:\path\to\output\");
        replacer.Replace("%T143SHOTEX221").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestNonexistentSpellCard()
    {
        var replacer = new ShotExReplacer(BestShots, @"C:\path\to\output\");
        replacer.Replace("%T143SHOTEX171").ShouldBe("%T143SHOTEX171");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new ShotExReplacer(BestShots, @"C:\path\to\output\");
        replacer.Replace("%T143XXXXXX231").ShouldBe("%T143XXXXXX231");
    }

    [TestMethod]
    public void ReplaceTestInvalidDay()
    {
        var replacer = new ShotExReplacer(BestShots, @"C:\path\to\output\");
        replacer.Replace("%T143SHOTEXX31").ShouldBe("%T143SHOTEXX31");
    }

    [TestMethod]
    public void ReplaceTestInvalidScene()
    {
        var replacer = new ShotExReplacer(BestShots, @"C:\path\to\output\");
        replacer.Replace("%T143SHOTEX2X1").ShouldBe("%T143SHOTEX2X1");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var replacer = new ShotExReplacer(BestShots, @"C:\path\to\output\");
        replacer.Replace("%T143SHOTEX23X").ShouldBe("%T143SHOTEX23X");
    }
}

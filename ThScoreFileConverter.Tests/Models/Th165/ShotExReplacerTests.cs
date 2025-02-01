using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Models.Th165;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models.Th165;

namespace ThScoreFileConverter.Tests.Models.Th165;

[TestClass]
public class ShotExReplacerTests
{
    internal static IReadOnlyDictionary<(Day, int), (string, IBestShotHeader)> BestShots { get; } =
        new List<(string, IBestShotHeader header)>
        {
            (@"C:\path\to\output\bestshots\bs02_03.png", BestShotHeaderTests.MockBestShotHeader()),
        }.ToDictionary(element => (element.header.Weekday, (int)element.header.Dream));

    [TestMethod]
    public void ShotExReplacerTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ShotExReplacerTestEmptyBestShots()
    {
        var bestshots = ImmutableDictionary<(Day, int), (string, IBestShotHeader)>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(bestshots, formatterMock, @"C:\path\to\output\");
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ShotExReplacerTestInvalidBestShotPath()
    {
        var bestshots = new List<(string, IBestShotHeader header)>
        {
            ("abcde", BestShotHeaderTests.MockBestShotHeader()),
        }.ToDictionary(element => (element.header.Weekday, (int)element.header.Dream));
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(bestshots, formatterMock, @"C:\path\to\output\");
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ShotExReplacerTestEmptyOutputFilePath()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, formatterMock, string.Empty);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ShotExReplacerTestInvalidOutputFilePath()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, formatterMock, "abcde");
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTestPath()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T165SHOTEX0231").ShouldBe(@"bestshots/bs02_03.png");
    }

    [TestMethod]
    public void ReplaceTestWidth()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T165SHOTEX0232").ShouldBe("4");
    }

    [TestMethod]
    public void ReplaceTestHeight()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T165SHOTEX0233").ShouldBe("5");
    }

    [TestMethod]
    public void ReplaceTestDateTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        var expected = DateTimeHelper.GetString(11);
        replacer.Replace("%T165SHOTEX0234").ShouldBe(expected);
    }

    [TestMethod]
    public void ReplaceTestHashtags()
    {
        static string[] GetHashtags()
        {
            return [
                "＃敵が見切れてる",
                "＃敵を収めたよ",
                "＃敵がど真ん中",
                "＃敵が丸見えｗ",
                "＃動物園！",
            ];
        }

        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T165SHOTEX0235").ShouldBe(string.Join(Environment.NewLine, GetHashtags()));
    }

    [TestMethod]
    public void ReplaceTestNumViews()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T165SHOTEX0236").ShouldBe("invoked: 19");
    }

    [TestMethod]
    public void ReplaceTestNumLikes()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T165SHOTEX0237").ShouldBe("invoked: 20");
    }

    [TestMethod]
    public void ReplaceTestNumFavs()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T165SHOTEX0238").ShouldBe("invoked: 21");
    }

    [TestMethod]
    public void ReplaceTestScore()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T165SHOTEX0239").ShouldBe("invoked: 13");
    }

    [TestMethod]
    public void ReplaceTestEmptyBestShots()
    {
        var bestshots = ImmutableDictionary<(Day, int), (string, IBestShotHeader)>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(bestshots, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T165SHOTEX0231").ShouldBeEmpty();
        replacer.Replace("%T165SHOTEX0232").ShouldBe("0");
        replacer.Replace("%T165SHOTEX0233").ShouldBe("0");
        replacer.Replace("%T165SHOTEX0234").ShouldBe(DateTimeHelper.GetString(null));
        replacer.Replace("%T165SHOTEX0235").ShouldBeEmpty();
        replacer.Replace("%T165SHOTEX0236").ShouldBe("invoked: 0");
        replacer.Replace("%T165SHOTEX0237").ShouldBe("invoked: 0");
        replacer.Replace("%T165SHOTEX0238").ShouldBe("invoked: 0");
        replacer.Replace("%T165SHOTEX0239").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestEmptyHashtags()
    {
        var mock = BestShotHeaderTests.MockBestShotHeader();
        _ = mock.Fields.Returns(new HashtagFields(0, 0, 0));
        var bestshots = new List<(string, IBestShotHeader header)>
        {
            (@"C:\path\to\output\bestshots\bs02_03.png", mock),
        }.ToDictionary(element => (element.header.Weekday, (int)element.header.Dream));
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(bestshots, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T165SHOTEX0235").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestInvalidBestShotPaths()
    {
        var bestshots = new List<(string, IBestShotHeader header)>
        {
            ("abcde", BestShotHeaderTests.MockBestShotHeader()),
        }.ToDictionary(element => (element.header.Weekday, (int)element.header.Dream));
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(bestshots, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T165SHOTEX0231").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestEmptyOutputFilePath()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, formatterMock, string.Empty);
        replacer.Replace("%T165SHOTEX0231").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestInvalidOutputFilePath()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, formatterMock, "abcde");
        replacer.Replace("%T165SHOTEX0231").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestNonexistentDay()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T165SHOTEX0331").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestNonexistentScene()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T165SHOTEX0221").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestNonexistentSpellCard()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T165SHOTEX0131").ShouldBe("%T165SHOTEX0131");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T165XXXXXX0231").ShouldBe("%T165XXXXXX0231");
    }

    [TestMethod]
    public void ReplaceTestInvalidDay()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T165SHOTEXXX31").ShouldBe("%T165SHOTEXXX31");
    }

    [TestMethod]
    public void ReplaceTestInvalidScene()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T165SHOTEX02X1").ShouldBe("%T165SHOTEX02X1");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        replacer.Replace("%T165SHOTEX023X").ShouldBe("%T165SHOTEX023X");
    }
}

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ShotExReplacerTestEmptyBestShots()
    {
        var bestshots = ImmutableDictionary<(Day, int), (string, IBestShotHeader)>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(bestshots, formatterMock, @"C:\path\to\output\");
        Assert.IsNotNull(replacer);
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
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ShotExReplacerTestEmptyOutputFilePath()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, formatterMock, string.Empty);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ShotExReplacerTestInvalidOutputFilePath()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, formatterMock, "abcde");
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestPath()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual(@"bestshots/bs02_03.png", replacer.Replace("%T165SHOTEX0231"));
    }

    [TestMethod]
    public void ReplaceTestWidth()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("4", replacer.Replace("%T165SHOTEX0232"));
    }

    [TestMethod]
    public void ReplaceTestHeight()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("5", replacer.Replace("%T165SHOTEX0233"));
    }

    [TestMethod]
    public void ReplaceTestDateTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        var expected = DateTimeHelper.GetString(11);
        Assert.AreEqual(expected, replacer.Replace("%T165SHOTEX0234"));
    }

    [TestMethod]
    public void ReplaceTestHashtags()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual(string.Join(Environment.NewLine, new[]
        {
            "＃敵が見切れてる",
            "＃敵を収めたよ",
            "＃敵がど真ん中",
            "＃敵が丸見えｗ",
            "＃動物園！",
        }), replacer.Replace("%T165SHOTEX0235"));
    }

    [TestMethod]
    public void ReplaceTestNumViews()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("invoked: 19", replacer.Replace("%T165SHOTEX0236"));
    }

    [TestMethod]
    public void ReplaceTestNumLikes()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("invoked: 20", replacer.Replace("%T165SHOTEX0237"));
    }

    [TestMethod]
    public void ReplaceTestNumFavs()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("invoked: 21", replacer.Replace("%T165SHOTEX0238"));
    }

    [TestMethod]
    public void ReplaceTestScore()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("invoked: 13", replacer.Replace("%T165SHOTEX0239"));
    }

    [TestMethod]
    public void ReplaceTestEmptyBestShots()
    {
        var bestshots = ImmutableDictionary<(Day, int), (string, IBestShotHeader)>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(bestshots, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual(string.Empty, replacer.Replace("%T165SHOTEX0231"));
        Assert.AreEqual("0", replacer.Replace("%T165SHOTEX0232"));
        Assert.AreEqual("0", replacer.Replace("%T165SHOTEX0233"));
        Assert.AreEqual(DateTimeHelper.GetString(null), replacer.Replace("%T165SHOTEX0234"));
        Assert.AreEqual(string.Empty, replacer.Replace("%T165SHOTEX0235"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T165SHOTEX0236"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T165SHOTEX0237"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T165SHOTEX0238"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T165SHOTEX0239"));
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
        Assert.AreEqual(string.Empty, replacer.Replace("%T165SHOTEX0235"));
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
        Assert.AreEqual(string.Empty, replacer.Replace("%T165SHOTEX0231"));
    }

    [TestMethod]
    public void ReplaceTestEmptyOutputFilePath()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, formatterMock, string.Empty);
        Assert.AreEqual(string.Empty, replacer.Replace("%T165SHOTEX0231"));
    }

    [TestMethod]
    public void ReplaceTestInvalidOutputFilePath()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, formatterMock, "abcde");
        Assert.AreEqual(string.Empty, replacer.Replace("%T165SHOTEX0231"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentDay()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual(string.Empty, replacer.Replace("%T165SHOTEX0331"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentScene()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual(string.Empty, replacer.Replace("%T165SHOTEX0221"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentSpellCard()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("%T165SHOTEX0131", replacer.Replace("%T165SHOTEX0131"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("%T165XXXXXX0231", replacer.Replace("%T165XXXXXX0231"));
    }

    [TestMethod]
    public void ReplaceTestInvalidDay()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("%T165SHOTEXXX31", replacer.Replace("%T165SHOTEXXX31"));
    }

    [TestMethod]
    public void ReplaceTestInvalidScene()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("%T165SHOTEX02X1", replacer.Replace("%T165SHOTEX02X1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ShotExReplacer(BestShots, formatterMock, @"C:\path\to\output\");
        Assert.AreEqual("%T165SHOTEX023X", replacer.Replace("%T165SHOTEX023X"));
    }
}

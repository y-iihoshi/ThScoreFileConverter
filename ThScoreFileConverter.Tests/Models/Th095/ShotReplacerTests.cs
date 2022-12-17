using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Moq;
using ThScoreFileConverter.Core.Models.Th095;
using ThScoreFileConverter.Models.Th095;
using INumberFormatter = ThScoreFileConverter.Models.INumberFormatter;

namespace ThScoreFileConverter.Tests.Models.Th095;

[TestClass]
public class ShotReplacerTests
{
    internal static IReadOnlyDictionary<(Level, int), (string, IBestShotHeader<Level>)> BestShots { get; } =
        new List<(string, IBestShotHeader<Level> header)>
        {
            (@"C:\path\to\output\bestshots\bs_02_3.png", BestShotHeaderTests.MockBestShotHeader().Object),
        }.ToDictionary(element => (element.header.Level, (int)element.header.Scene));

    private static Mock<INumberFormatter> MockNumberFormatter()
    {
        var mock = new Mock<INumberFormatter>();
        _ = mock.Setup(formatter => formatter.FormatNumber(It.IsAny<It.IsValueType>()))
            .Returns((object value) => "invoked: " + value.ToString());
        _ = mock.Setup(formatter => formatter.FormatPercent(It.IsAny<double>(), It.IsAny<int>()))
            .Returns((double value, int precision) => "invoked: " + value.ToString($"F{precision}") + "%");
        return mock;
    }

    [TestMethod]
    public void ShotReplacerTest()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotReplacer(BestShots, formatterMock.Object, @"C:\path\to\output\");
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ShotReplacerTestEmptyBestShots()
    {
        var bestshots = ImmutableDictionary<(Level, int), (string, IBestShotHeader<Level>)>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotReplacer(bestshots, formatterMock.Object, @"C:\path\to\output\");
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ShotReplacerTestInvalidBestShotPath()
    {
        var bestshots = new List<(string, IBestShotHeader<Level> header)>
        {
            ("abcde", BestShotHeaderTests.MockBestShotHeader().Object),
        }.ToDictionary(element => (element.header.Level, (int)element.header.Scene));
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotReplacer(bestshots, formatterMock.Object, @"C:\path\to\output\");
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ShotReplacerTestEmptyOutputFilePath()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotReplacer(BestShots, formatterMock.Object, string.Empty);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ShotReplacerTestInvalidOutputFilePath()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotReplacer(BestShots, formatterMock.Object, "abcde");
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTest()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotReplacer(BestShots, formatterMock.Object, @"C:\path\to\output\");
        var expected = string.Join(Environment.NewLine, new string[]
        {
            @"<img src=""bestshots/bs_02_3.png"" alt=""ClearData: invoked: 6",
            @"Slow: invoked: 7.000000%",
            @"SpellName: abcde"" title=""ClearData: invoked: 6",
            @"Slow: invoked: 7.000000%",
            @"SpellName: abcde"" border=0>",
        });

        Assert.AreEqual(expected, replacer.Replace("%T95SHOT23"));
    }

    [TestMethod]
    public void ReplaceTestEmptyBestShots()
    {
        var bestshots = ImmutableDictionary<(Level, int), (string, IBestShotHeader<Level>)>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotReplacer(bestshots, formatterMock.Object, @"C:\path\to\output\");
        Assert.AreEqual(string.Empty, replacer.Replace("%T95SHOT23"));
    }

    [TestMethod]
    public void ReplaceTestInvalidBestShotPaths()
    {
        var bestshots = new List<(string, IBestShotHeader<Level> header)>
        {
            ("abcde", BestShotHeaderTests.MockBestShotHeader().Object),
        }.ToDictionary(element => (element.header.Level, (int)element.header.Scene));
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotReplacer(bestshots, formatterMock.Object, @"C:\path\to\output\");
        Assert.AreEqual(string.Empty, replacer.Replace("%T95SHOT23"));
    }

    [TestMethod]
    public void ReplaceTestEmptyOutputFilePath()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotReplacer(BestShots, formatterMock.Object, string.Empty);
        Assert.AreEqual(string.Empty, replacer.Replace("%T95SHOT23"));
    }

    [TestMethod]
    public void ReplaceTestInvalidOutputFilePath()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotReplacer(BestShots, formatterMock.Object, "abcde");
        Assert.AreEqual(string.Empty, replacer.Replace("%T95SHOT23"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentLevel()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotReplacer(BestShots, formatterMock.Object, @"C:\path\to\output\");
        Assert.AreEqual(string.Empty, replacer.Replace("%T95SHOT13"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentScene()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotReplacer(BestShots, formatterMock.Object, @"C:\path\to\output\");
        Assert.AreEqual(string.Empty, replacer.Replace("%T95SHOT22"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentSpellCard()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotReplacer(BestShots, formatterMock.Object, @"C:\path\to\output\");
        Assert.AreEqual("%T95SHOT99", replacer.Replace("%T95SHOT99"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotReplacer(BestShots, formatterMock.Object, @"C:\path\to\output\");
        Assert.AreEqual("%T95XXXX23", replacer.Replace("%T95XXXX23"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotReplacer(BestShots, formatterMock.Object, @"C:\path\to\output\");
        Assert.AreEqual("%T95SHOTY3", replacer.Replace("%T95SHOTY3"));
    }

    [TestMethod]
    public void ReplaceTestInvalidScene()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ShotReplacer(BestShots, formatterMock.Object, @"C:\path\to\output\");
        Assert.AreEqual("%T95SHOT2X", replacer.Replace("%T95SHOT2X"));
    }
}

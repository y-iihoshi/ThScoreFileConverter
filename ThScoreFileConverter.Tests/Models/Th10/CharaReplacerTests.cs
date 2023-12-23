using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th10;
using ThScoreFileConverter.Models.Th10;
using IClearData = ThScoreFileConverter.Models.Th10.IClearData<ThScoreFileConverter.Core.Models.Th10.CharaWithTotal>;

namespace ThScoreFileConverter.Tests.Models.Th10;

[TestClass]
public class CharaReplacerTests
{
    private static IClearData[] CreateClearDataList()
    {
        var levels = EnumHelper<Level>.Enumerable;

        var clearData1 = Substitute.For<IClearData>();
        _ = clearData1.Chara.Returns(CharaWithTotal.ReimuB);
        _ = clearData1.TotalPlayCount.Returns(23);
        _ = clearData1.PlayTime.Returns(4567890);
        _ = clearData1.ClearCounts.Returns(levels.ToDictionary(level => level, level => 100 - (int)level));

        var clearData2 = Substitute.For<IClearData>();
        _ = clearData2.Chara.Returns(CharaWithTotal.MarisaA);
        _ = clearData2.TotalPlayCount.Returns(12);
        _ = clearData2.PlayTime.Returns(3456789);
        _ = clearData2.ClearCounts.Returns(levels.ToDictionary(level => level, level => 50 - (int)level));

        return [clearData1, clearData2];
    }

    internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
        CreateClearDataList().ToDictionary(element => element.Chara);

    [TestMethod]
    public void CharaReplacerTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CharaReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(dictionary, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestTotalPlayCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 23", replacer.Replace("%T10CHARARB1"));
    }

    [TestMethod]
    public void ReplaceTestPlayTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("21:08:51", replacer.Replace("%T10CHARARB2"));
    }

    [TestMethod]
    public void ReplaceTestClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 490", replacer.Replace("%T10CHARARB3"));
    }

    [TestMethod]
    public void ReplaceTestCharaTotalTotalPlayCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 35", replacer.Replace("%T10CHARATL1"));
    }

    [TestMethod]
    public void ReplaceTestCharaTotalPlayTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("37:09:04", replacer.Replace("%T10CHARATL2"));
    }

    [TestMethod]
    public void ReplaceTestCharaTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 730", replacer.Replace("%T10CHARATL3"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(dictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T10CHARARB1"));
        Assert.AreEqual("0:00:00", replacer.Replace("%T10CHARARB2"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T10CHARARB3"));
    }

    [TestMethod]
    public void ReplaceTestEmptyClearCounts()
    {
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.ReimuB);
        _ = clearData.ClearCounts.Returns(ImmutableDictionary<Level, int>.Empty);
        var dictionary = new[] { clearData }.ToDictionary(data => data.Chara);
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new CharaReplacer(dictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T10CHARARB3"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T10XXXXXRB1", replacer.Replace("%T10XXXXXRB1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T10CHARAXX1", replacer.Replace("%T10CHARAXX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T10CHARARBX", replacer.Replace("%T10CHARARBX"));
    }
}

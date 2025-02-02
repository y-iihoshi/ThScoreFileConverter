using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th10;
using ThScoreFileConverter.Models.Th10;
using IClearData = ThScoreFileConverter.Models.Th10.IClearData<ThScoreFileConverter.Core.Models.Th10.CharaWithTotal>;

namespace ThScoreFileConverter.Tests.Models.Th10;

[TestClass]
public class CharaExReplacerTests
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
        CreateClearDataList().ToDictionary(clearData => clearData.Chara);

    [TestMethod]
    public void CharaExReplacerTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void CharaExReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(dictionary, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTestTotalPlayCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T10CHARAEXHRB1").ShouldBe("invoked: 23");
    }

    [TestMethod]
    public void ReplaceTestPlayTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T10CHARAEXHRB2").ShouldBe("21:08:51");
    }

    [TestMethod]
    public void ReplaceTestClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T10CHARAEXHRB3").ShouldBe("invoked: 98");
    }

    [TestMethod]
    public void ReplaceTestLevelTotalTotalPlayCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T10CHARAEXTRB1").ShouldBe("invoked: 23");
    }

    [TestMethod]
    public void ReplaceTestLevelTotalPlayTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T10CHARAEXTRB2").ShouldBe("21:08:51");
    }

    [TestMethod]
    public void ReplaceTestLevelTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T10CHARAEXTRB3").ShouldBe("invoked: 490");
    }

    [TestMethod]
    public void ReplaceTestCharaTotalTotalPlayCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T10CHARAEXHTL1").ShouldBe("invoked: 35");
    }

    [TestMethod]
    public void ReplaceTestCharaTotalPlayTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T10CHARAEXHTL2").ShouldBe("37:09:04");
    }

    [TestMethod]
    public void ReplaceTestCharaTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T10CHARAEXHTL3").ShouldBe("invoked: 146");
    }

    [TestMethod]
    public void ReplaceTestTotalTotalPlayCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T10CHARAEXTTL1").ShouldBe("invoked: 35");
    }

    [TestMethod]
    public void ReplaceTestTotalPlayTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T10CHARAEXTTL2").ShouldBe("37:09:04");
    }

    [TestMethod]
    public void ReplaceTestTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T10CHARAEXTTL3").ShouldBe("invoked: 730");
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(dictionary, formatterMock);
        replacer.Replace("%T10CHARAEXHRB1").ShouldBe("invoked: 0");
        replacer.Replace("%T10CHARAEXHRB2").ShouldBe("0:00:00");
        replacer.Replace("%T10CHARAEXHRB3").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestEmptyClearCounts()
    {
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.ReimuB);
        _ = clearData.ClearCounts.Returns(ImmutableDictionary<Level, int>.Empty);
        var dictionary = new[] { clearData }.ToDictionary(data => data.Chara);
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new CharaExReplacer(dictionary, formatterMock);
        replacer.Replace("%T10CHARAEXHRB3").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T10XXXXXXXHRB1").ShouldBe("%T10XXXXXXXHRB1");
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T10CHARAEXYRB1").ShouldBe("%T10CHARAEXYRB1");
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T10CHARAEXHXX1").ShouldBe("%T10CHARAEXHXX1");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T10CHARAEXHRBX").ShouldBe("%T10CHARAEXHRBX");
    }
}

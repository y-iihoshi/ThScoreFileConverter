using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th13;
using ThScoreFileConverter.Models.Th13;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Core.Models.Th13.CharaWithTotal,
    ThScoreFileConverter.Core.Models.Th13.LevelPractice,
    ThScoreFileConverter.Core.Models.Th13.LevelPractice,
    ThScoreFileConverter.Core.Models.Th13.LevelPracticeWithTotal,
    ThScoreFileConverter.Core.Models.Th13.StagePractice,
    ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>>;

namespace ThScoreFileConverter.Tests.Models.Th13;

[TestClass]
public class CharaExReplacerTests
{
    private static IClearData[] CreateClearDataList()
    {
        var levels = EnumHelper<LevelPracticeWithTotal>.Enumerable;

        var clearData1 = Substitute.For<IClearData>();
        _ = clearData1.Chara.Returns(CharaWithTotal.Marisa);
        _ = clearData1.TotalPlayCount.Returns(23);
        _ = clearData1.PlayTime.Returns(4567890);
        _ = clearData1.ClearCounts.Returns(levels.ToDictionary(level => level, level => 100 - (int)level));

        var clearData2 = Substitute.For<IClearData>();
        _ = clearData2.Chara.Returns(CharaWithTotal.Sanae);
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
        replacer.Replace("%T13CHARAEXHMR1").ShouldBe("invoked: 23");
    }

    [TestMethod]
    public void ReplaceTestPlayTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T13CHARAEXHMR2").ShouldBe("21:08:51");
    }

    [TestMethod]
    public void ReplaceTestClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T13CHARAEXHMR3").ShouldBe("invoked: 98");
    }

    [TestMethod]
    public void ReplaceTestLevelTotalTotalPlayCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T13CHARAEXTMR1").ShouldBe("invoked: 23");
    }

    [TestMethod]
    public void ReplaceTestLevelTotalPlayTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T13CHARAEXTMR2").ShouldBe("21:08:51");
    }

    [TestMethod]
    public void ReplaceTestLevelTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T13CHARAEXTMR3").ShouldBe("invoked: 585");
    }

    [TestMethod]
    public void ReplaceTestCharaTotalTotalPlayCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T13CHARAEXHTL1").ShouldBe("invoked: 35");
    }

    [TestMethod]
    public void ReplaceTestCharaTotalPlayTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T13CHARAEXHTL2").ShouldBe("37:09:04");
    }

    [TestMethod]
    public void ReplaceTestCharaTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T13CHARAEXHTL3").ShouldBe("invoked: 146");
    }

    [TestMethod]
    public void ReplaceTestTotalTotalPlayCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T13CHARAEXTTL1").ShouldBe("invoked: 35");
    }

    [TestMethod]
    public void ReplaceTestTotalPlayTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T13CHARAEXTTL2").ShouldBe("37:09:04");
    }

    [TestMethod]
    public void ReplaceTestTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T13CHARAEXTTL3").ShouldBe("invoked: 870");
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(dictionary, formatterMock);
        replacer.Replace("%T13CHARAEXHMR1").ShouldBe("invoked: 0");
        replacer.Replace("%T13CHARAEXHMR2").ShouldBe("0:00:00");
        replacer.Replace("%T13CHARAEXHMR3").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestEmptyClearCounts()
    {
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.Marisa);
        _ = clearData.ClearCounts.Returns(ImmutableDictionary<LevelPracticeWithTotal, int>.Empty);
        var dictionary = new[] { clearData }.ToDictionary(data => data.Chara);
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new CharaExReplacer(dictionary, formatterMock);
        replacer.Replace("%T13CHARAEXHMR3").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T13XXXXXXXHMR1").ShouldBe("%T13XXXXXXXHMR1");
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T13CHARAEXYMR1").ShouldBe("%T13CHARAEXYMR1");
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T13CHARAEXHXX1").ShouldBe("%T13CHARAEXHXX1");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T13CHARAEXHMRX").ShouldBe("%T13CHARAEXHMRX");
    }
}

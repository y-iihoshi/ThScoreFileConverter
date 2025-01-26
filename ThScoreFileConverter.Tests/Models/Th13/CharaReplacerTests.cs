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
public class CharaReplacerTests
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
        Assert.AreEqual("invoked: 23", replacer.Replace("%T13CHARAMR1"));
    }

    [TestMethod]
    public void ReplaceTestPlayTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("21:08:51", replacer.Replace("%T13CHARAMR2"));
    }

    [TestMethod]
    public void ReplaceTestClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 585", replacer.Replace("%T13CHARAMR3"));
    }

    [TestMethod]
    public void ReplaceTestCharaTotalTotalPlayCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 35", replacer.Replace("%T13CHARATL1"));
    }

    [TestMethod]
    public void ReplaceTestCharaTotalPlayTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("37:09:04", replacer.Replace("%T13CHARATL2"));
    }

    [TestMethod]
    public void ReplaceTestCharaTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 870", replacer.Replace("%T13CHARATL3"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(dictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T13CHARAMR1"));
        Assert.AreEqual("0:00:00", replacer.Replace("%T13CHARAMR2"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T13CHARAMR3"));
    }

    [TestMethod]
    public void ReplaceTestEmptyClearCounts()
    {
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.Marisa);
        _ = clearData.ClearCounts.Returns(ImmutableDictionary<LevelPracticeWithTotal, int>.Empty);
        var dictionary = new[] { clearData }.ToDictionary(data => data.Chara);
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new CharaReplacer(dictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T13CHARAMR3"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T13XXXXXMR1", replacer.Replace("%T13XXXXXMR1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T13CHARAXX1", replacer.Replace("%T13CHARAXX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T13CHARAMRX", replacer.Replace("%T13CHARAMRX"));
    }
}

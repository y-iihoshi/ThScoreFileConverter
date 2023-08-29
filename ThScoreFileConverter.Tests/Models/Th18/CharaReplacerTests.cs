using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th18;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th18;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Core.Models.Th18.CharaWithTotal,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Core.Models.Stage,
    ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>>;
using LevelPracticeWithTotal = ThScoreFileConverter.Core.Models.Th14.LevelPracticeWithTotal;

namespace ThScoreFileConverter.Tests.Models.Th18;

[TestClass]
public class CharaReplacerTests
{
    private static IEnumerable<IClearData> CreateClearDataList()
    {
        var levels = EnumHelper<LevelPracticeWithTotal>.Enumerable;

        var clearData1 = Substitute.For<IClearData>();
        _ = clearData1.Chara.Returns(CharaWithTotal.Reimu);
        _ = clearData1.TotalPlayCount.Returns(23);
        _ = clearData1.PlayTime.Returns(4567890);
        _ = clearData1.ClearCounts.Returns(levels.ToDictionary(level => level, level => 100 - (int)level));

        var clearData2 = Substitute.For<IClearData>();
        _ = clearData2.Chara.Returns(CharaWithTotal.Marisa);
        _ = clearData2.TotalPlayCount.Returns(12);
        _ = clearData2.PlayTime.Returns(3456789);
        _ = clearData2.ClearCounts.Returns(levels.ToDictionary(level => level, level => 50 - (int)level));

        return new[] { clearData1, clearData2 };
    }

    internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
        CreateClearDataList().ToDictionary(clearData => clearData.Chara);

    private static INumberFormatter MockNumberFormatter()
    {
        // NOTE: NSubstitute v5.0.0 has no substitute for Moq's It.IsAny<It.IsValueType>.
        var mock = Substitute.For<INumberFormatter>();
        _ = mock.FormatNumber(Arg.Any<long>()).Returns(callInfo => $"invoked: {(long)callInfo[0]}");
        return mock;
    }

    [TestMethod]
    public void CharaReplacerTest()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CharaReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(dictionary, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestTotalPlayCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 23", replacer.Replace("%T18CHARARM1"));
    }

    [TestMethod]
    public void ReplaceTestPlayTime()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("12:41:18", replacer.Replace("%T18CHARARM2"));
    }

    [TestMethod]
    public void ReplaceTestClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 490", replacer.Replace("%T18CHARARM3"));
    }

    [TestMethod]
    public void ReplaceTestCharaTotalTotalPlayCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 35", replacer.Replace("%T18CHARATL1"));
    }

    [TestMethod]
    public void ReplaceTestCharaTotalPlayTime()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("22:17:26", replacer.Replace("%T18CHARATL2"));
    }

    [TestMethod]
    public void ReplaceTestCharaTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 730", replacer.Replace("%T18CHARATL3"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(dictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T18CHARARM1"));
        Assert.AreEqual("0:00:00", replacer.Replace("%T18CHARARM2"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T18CHARARM3"));
    }

    [TestMethod]
    public void ReplaceTestEmptyClearCounts()
    {
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.Reimu);
        _ = clearData.ClearCounts.Returns(ImmutableDictionary<LevelPracticeWithTotal, int>.Empty);
        var dictionary = new[] { clearData }.ToDictionary(data => data.Chara);
        var formatterMock = MockNumberFormatter();

        var replacer = new CharaReplacer(dictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T18CHARARM3"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T18XXXXXRM1", replacer.Replace("%T18XXXXXRM1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T18CHARAXX1", replacer.Replace("%T18CHARAXX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T18CHARARBX", replacer.Replace("%T18CHARARBX"));
    }
}

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th13;
using ThScoreFileConverter.Models;
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
    private static IEnumerable<IClearData> CreateClearDataList()
    {
        var levels = EnumHelper<LevelPracticeWithTotal>.Enumerable;
        return new[]
        {
            Mock.Of<IClearData>(
                m => (m.Chara == CharaWithTotal.Marisa)
                     && (m.TotalPlayCount == 23)
                     && (m.PlayTime == 4567890)
                     && (m.ClearCounts == levels.ToDictionary(level => level, level => 100 - (int)level))),
            Mock.Of<IClearData>(
                m => (m.Chara == CharaWithTotal.Sanae)
                     && (m.TotalPlayCount == 12)
                     && (m.PlayTime == 3456789)
                     && (m.ClearCounts == levels.ToDictionary(level => level, level => 50 - (int)level))),
        };
    }

    internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
        CreateClearDataList().ToDictionary(clearData => clearData.Chara);

    private static Mock<INumberFormatter> MockNumberFormatter()
    {
        var mock = new Mock<INumberFormatter>();
        _ = mock.Setup(formatter => formatter.FormatNumber(It.IsAny<It.IsValueType>()))
            .Returns((object value) => "invoked: " + value.ToString());
        return mock;
    }

    [TestMethod]
    public void CharaExReplacerTest()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CharaExReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaExReplacer(dictionary, formatterMock.Object);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestTotalPlayCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 23", replacer.Replace("%T13CHARAEXHMR1"));
    }

    [TestMethod]
    public void ReplaceTestPlayTime()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("21:08:51", replacer.Replace("%T13CHARAEXHMR2"));
    }

    [TestMethod]
    public void ReplaceTestClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 98", replacer.Replace("%T13CHARAEXHMR3"));
    }

    [TestMethod]
    public void ReplaceTestLevelTotalTotalPlayCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 23", replacer.Replace("%T13CHARAEXTMR1"));
    }

    [TestMethod]
    public void ReplaceTestLevelTotalPlayTime()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("21:08:51", replacer.Replace("%T13CHARAEXTMR2"));
    }

    [TestMethod]
    public void ReplaceTestLevelTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 585", replacer.Replace("%T13CHARAEXTMR3"));
    }

    [TestMethod]
    public void ReplaceTestCharaTotalTotalPlayCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 35", replacer.Replace("%T13CHARAEXHTL1"));
    }

    [TestMethod]
    public void ReplaceTestCharaTotalPlayTime()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("37:09:04", replacer.Replace("%T13CHARAEXHTL2"));
    }

    [TestMethod]
    public void ReplaceTestCharaTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 146", replacer.Replace("%T13CHARAEXHTL3"));
    }

    [TestMethod]
    public void ReplaceTestTotalTotalPlayCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 35", replacer.Replace("%T13CHARAEXTTL1"));
    }

    [TestMethod]
    public void ReplaceTestTotalPlayTime()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("37:09:04", replacer.Replace("%T13CHARAEXTTL2"));
    }

    [TestMethod]
    public void ReplaceTestTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 870", replacer.Replace("%T13CHARAEXTTL3"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaExReplacer(dictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T13CHARAEXHMR1"));
        Assert.AreEqual("0:00:00", replacer.Replace("%T13CHARAEXHMR2"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T13CHARAEXHMR3"));
    }

    [TestMethod]
    public void ReplaceTestEmptyClearCounts()
    {
        var dictionary = new[]
        {
            Mock.Of<IClearData>(
                m => (m.Chara == CharaWithTotal.Marisa)
                     && (m.ClearCounts == ImmutableDictionary<LevelPracticeWithTotal, int>.Empty))
        }.ToDictionary(clearData => clearData.Chara);
        var formatterMock = MockNumberFormatter();

        var replacer = new CharaExReplacer(dictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T13CHARAEXHMR3"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T13XXXXXXXHMR1", replacer.Replace("%T13XXXXXXXHMR1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T13CHARAEXYMR1", replacer.Replace("%T13CHARAEXYMR1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T13CHARAEXHXX1", replacer.Replace("%T13CHARAEXHXX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T13CHARAEXHMRX", replacer.Replace("%T13CHARAEXHMRX"));
    }
}

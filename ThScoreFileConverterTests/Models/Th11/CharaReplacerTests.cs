using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th11;
using IClearData = ThScoreFileConverter.Models.Th10.IClearData<ThScoreFileConverter.Models.Th11.CharaWithTotal>;

namespace ThScoreFileConverterTests.Models.Th11;

[TestClass]
public class CharaReplacerTests
{
    private static IEnumerable<IClearData> CreateClearDataList()
    {
        var levels = EnumHelper<Level>.Enumerable;
        return new[]
        {
            Mock.Of<IClearData>(
                m => (m.Chara == CharaWithTotal.ReimuSuika)
                     && (m.TotalPlayCount == 23)
                     && (m.PlayTime == 4567890)
                     && (m.ClearCounts == levels.ToDictionary(level => level, level => 100 - (int)level))),
            Mock.Of<IClearData>(
                m => (m.Chara == CharaWithTotal.MarisaAlice)
                     && (m.TotalPlayCount == 12)
                     && (m.PlayTime == 3456789)
                     && (m.ClearCounts == levels.ToDictionary(level => level, level => 50 - (int)level))),
        };
    }

    internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
        CreateClearDataList().ToDictionary(element => element.Chara);

    private static Mock<INumberFormatter> MockNumberFormatter()
    {
        var mock = new Mock<INumberFormatter>();
        _ = mock.Setup(formatter => formatter.FormatNumber(It.IsAny<It.IsValueType>()))
            .Returns((object value) => "invoked: " + value.ToString());
        return mock;
    }

    [TestMethod]
    public void CharaReplacerTest()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CharaReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(dictionary, formatterMock.Object);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestTotalPlayCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 23", replacer.Replace("%T11CHARARS1"));
    }

    [TestMethod]
    public void ReplaceTestPlayTime()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("21:08:51", replacer.Replace("%T11CHARARS2"));
    }

    [TestMethod]
    public void ReplaceTestClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 490", replacer.Replace("%T11CHARARS3"));
    }

    [TestMethod]
    public void ReplaceTestCharaTotalTotalPlayCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 35", replacer.Replace("%T11CHARATL1"));
    }

    [TestMethod]
    public void ReplaceTestCharaTotalPlayTime()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("37:09:04", replacer.Replace("%T11CHARATL2"));
    }

    [TestMethod]
    public void ReplaceTestCharaTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 730", replacer.Replace("%T11CHARATL3"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(dictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T11CHARARS1"));
        Assert.AreEqual("0:00:00", replacer.Replace("%T11CHARARS2"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T11CHARARS3"));
    }

    [TestMethod]
    public void ReplaceTestEmptyClearCounts()
    {
        var dictionary = new[]
        {
            Mock.Of<IClearData>(
                m => (m.Chara == CharaWithTotal.ReimuSuika)
                     && (m.ClearCounts == ImmutableDictionary<Level, int>.Empty))
        }.ToDictionary(clearData => clearData.Chara);
        var formatterMock = MockNumberFormatter();

        var replacer = new CharaReplacer(dictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T11CHARARS3"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T11XXXXXRS1", replacer.Replace("%T11XXXXXRS1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T11CHARAXX1", replacer.Replace("%T11CHARAXX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T11CHARARSX", replacer.Replace("%T11CHARARSX"));
    }
}

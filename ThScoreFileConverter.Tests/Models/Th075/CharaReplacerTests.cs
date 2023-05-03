using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Moq;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th075;
using ThScoreFileConverter.Models.Th075;
using INumberFormatter = ThScoreFileConverter.Models.INumberFormatter;

namespace ThScoreFileConverter.Tests.Models.Th075;

[TestClass]
public class CharaReplacerTests
{
    internal static IReadOnlyDictionary<(CharaWithReserved, Level), IClearData> ClearData { get; } =
        EnumHelper<Level>.Enumerable.ToDictionary(
            level => (CharaWithReserved.Reimu, level),
            level => ClearDataTests.MockClearData().Object);

    private static Mock<INumberFormatter> MockNumberFormatter()
    {
        var mock = new Mock<INumberFormatter>();
        _ = mock.Setup(formatter => formatter.FormatNumber(It.IsAny<It.IsValueType>()))
            .Returns((object value) => $"invoked: {value}");
        return mock;
    }

    [TestMethod]
    public void CharaReplacerTest()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearData, formatterMock.Object);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CharaReplacerTestEmpty()
    {
        var clearData = ImmutableDictionary<(CharaWithReserved, Level), IClearData>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(clearData, formatterMock.Object);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestUseCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearData, formatterMock.Object);

        Assert.AreEqual("invoked: 1234", replacer.Replace("%T75CHRHRM1"));
    }

    [TestMethod]
    public void ReplaceTestClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearData, formatterMock.Object);

        Assert.AreEqual("invoked: 2345", replacer.Replace("%T75CHRHRM2"));
    }

    [TestMethod]
    public void ReplaceTestMaxCombo()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearData, formatterMock.Object);

        Assert.AreEqual("invoked: 3456", replacer.Replace("%T75CHRHRM3"));
    }

    [TestMethod]
    public void ReplaceTestMaxDamage()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearData, formatterMock.Object);

        Assert.AreEqual("invoked: 4567", replacer.Replace("%T75CHRHRM4"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var clearData = ImmutableDictionary<(CharaWithReserved, Level), IClearData>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(clearData, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T75CHRHRM1"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T75CHRHRM2"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T75CHRHRM3"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T75CHRHRM4"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentUseCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearData, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T75CHRHMR1"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearData, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T75CHRHMR2"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentMaxCombo()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearData, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T75CHRHMR3"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentMaxDamage()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearData, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T75CHRHMR4"));
    }

    [TestMethod]
    public void ReplaceTestMeiling()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearData, formatterMock.Object);
        Assert.AreEqual("%T75CHRHML1", replacer.Replace("%T75CHRHML1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearData, formatterMock.Object);
        Assert.AreEqual("%T75XXXHRM1", replacer.Replace("%T75XXXHRM1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearData, formatterMock.Object);
        Assert.AreEqual("%T75CHRXRM1", replacer.Replace("%T75CHRXRM1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearData, formatterMock.Object);
        Assert.AreEqual("%T75CHRHXX1", replacer.Replace("%T75CHRHXX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearData, formatterMock.Object);
        Assert.AreEqual("%T75CHRHRMX", replacer.Replace("%T75CHRHRMX"));
    }
}

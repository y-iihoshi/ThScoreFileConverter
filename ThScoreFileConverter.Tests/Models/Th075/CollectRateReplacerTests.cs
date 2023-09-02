using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th075;
using ThScoreFileConverter.Models.Th075;

namespace ThScoreFileConverter.Tests.Models.Th075;

[TestClass]
public class CollectRateReplacerTests
{
    private static IClearData MockClearData()
    {
        var mock = ClearDataTests.MockClearData();
        _ = mock.CardGotCount.Returns(Enumerable.Range(1, 100).Select(count => (short)(count % 5)).ToList());
        _ = mock.CardTrialCount.Returns(Enumerable.Range(1, 100).Select(count => (short)(count % 7)).ToList());
        _ = mock.CardTrulyGot.Returns(Enumerable.Range(1, 100).Select(count => (byte)(count % 3)).ToList());
        return mock;
    }

    internal static IReadOnlyDictionary<(CharaWithReserved, Level), IClearData> ClearData { get; } =
        EnumHelper<Level>.Enumerable.ToDictionary(
            level => (CharaWithReserved.Reimu, level),
            level => MockClearData());

    [TestMethod]
    public void CollectRateReplacerTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearData, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CollectRateReplacerTestEmpty()
    {
        var clearData = ImmutableDictionary<(CharaWithReserved, Level), IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(clearData, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearData, formatterMock);
        Assert.AreEqual("invoked: 20", replacer.Replace("%T75CRGHRM1"));
    }

    [TestMethod]
    public void ReplaceTestTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearData, formatterMock);
        Assert.AreEqual("invoked: 21", replacer.Replace("%T75CRGHRM2"));
    }

    [TestMethod]
    public void ReplaceTestTrulyGot()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearData, formatterMock);
        Assert.AreEqual("invoked: 16", replacer.Replace("%T75CRGHRM3"));
    }

    [TestMethod]
    public void ReplaceTestTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearData, formatterMock);
        Assert.AreEqual("invoked: 80", replacer.Replace("%T75CRGTRM1"));
    }

    [TestMethod]
    public void ReplaceTestTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearData, formatterMock);
        Assert.AreEqual("invoked: 86", replacer.Replace("%T75CRGTRM2"));
    }

    [TestMethod]
    public void ReplaceTestTotalTrulyGot()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearData, formatterMock);
        Assert.AreEqual("invoked: 67", replacer.Replace("%T75CRGTRM3"));
    }

    [TestMethod]
    public void ReplaceTestEmptyClearCount()
    {
        var clearData = ImmutableDictionary<(CharaWithReserved, Level), IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(clearData, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T75CRGHRM1"));
    }

    [TestMethod]
    public void ReplaceTestEmptyTrialCount()
    {
        var clearData = ImmutableDictionary<(CharaWithReserved, Level), IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(clearData, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T75CRGHRM2"));
    }

    [TestMethod]
    public void ReplaceTestEmptyTrulyGot()
    {
        var clearData = ImmutableDictionary<(CharaWithReserved, Level), IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(clearData, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T75CRGHRM3"));
    }

    [TestMethod]
    public void ReplaceTestMeiling()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearData, formatterMock);
        Assert.AreEqual("%T75CRGHML1", replacer.Replace("%T75CRGHML1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearData, formatterMock);
        Assert.AreEqual("%T75XXXHRM1", replacer.Replace("%T75XXXHRM1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearData, formatterMock);
        Assert.AreEqual("%T75CRGXRM1", replacer.Replace("%T75CRGXRM1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearData, formatterMock);
        Assert.AreEqual("%T75CRGHXX1", replacer.Replace("%T75CRGHXX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearData, formatterMock);
        Assert.AreEqual("%T75CRGHRMX", replacer.Replace("%T75CRGHRMX"));
    }
}

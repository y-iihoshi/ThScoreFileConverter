using System.Collections.Immutable;
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
        _ = mock.CardGotCount.Returns([.. Enumerable.Range(1, 100).Select(count => (short)(count % 5))]);
        _ = mock.CardTrialCount.Returns([.. Enumerable.Range(1, 100).Select(count => (short)(count % 7))]);
        _ = mock.CardTrulyGot.Returns([.. Enumerable.Range(1, 100).Select(count => (byte)(count % 3))]);
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
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void CollectRateReplacerTestEmpty()
    {
        var clearData = ImmutableDictionary<(CharaWithReserved, Level), IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(clearData, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTestClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearData, formatterMock);
        replacer.Replace("%T75CRGHRM1").ShouldBe("invoked: 20");
    }

    [TestMethod]
    public void ReplaceTestTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearData, formatterMock);
        replacer.Replace("%T75CRGHRM2").ShouldBe("invoked: 21");
    }

    [TestMethod]
    public void ReplaceTestTrulyGot()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearData, formatterMock);
        replacer.Replace("%T75CRGHRM3").ShouldBe("invoked: 16");
    }

    [TestMethod]
    public void ReplaceTestTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearData, formatterMock);
        replacer.Replace("%T75CRGTRM1").ShouldBe("invoked: 80");
    }

    [TestMethod]
    public void ReplaceTestTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearData, formatterMock);
        replacer.Replace("%T75CRGTRM2").ShouldBe("invoked: 86");
    }

    [TestMethod]
    public void ReplaceTestTotalTrulyGot()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearData, formatterMock);
        replacer.Replace("%T75CRGTRM3").ShouldBe("invoked: 67");
    }

    [TestMethod]
    public void ReplaceTestEmptyClearCount()
    {
        var clearData = ImmutableDictionary<(CharaWithReserved, Level), IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(clearData, formatterMock);
        replacer.Replace("%T75CRGHRM1").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestEmptyTrialCount()
    {
        var clearData = ImmutableDictionary<(CharaWithReserved, Level), IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(clearData, formatterMock);
        replacer.Replace("%T75CRGHRM2").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestEmptyTrulyGot()
    {
        var clearData = ImmutableDictionary<(CharaWithReserved, Level), IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(clearData, formatterMock);
        replacer.Replace("%T75CRGHRM3").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestMeiling()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearData, formatterMock);
        replacer.Replace("%T75CRGHML1").ShouldBe("%T75CRGHML1");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearData, formatterMock);
        replacer.Replace("%T75XXXHRM1").ShouldBe("%T75XXXHRM1");
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearData, formatterMock);
        replacer.Replace("%T75CRGXRM1").ShouldBe("%T75CRGXRM1");
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearData, formatterMock);
        replacer.Replace("%T75CRGHXX1").ShouldBe("%T75CRGHXX1");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearData, formatterMock);
        replacer.Replace("%T75CRGHRMX").ShouldBe("%T75CRGHRMX");
    }
}

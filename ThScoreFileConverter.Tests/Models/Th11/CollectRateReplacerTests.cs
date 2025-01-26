using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th11;
using ThScoreFileConverter.Models.Th11;
using Definitions = ThScoreFileConverter.Models.Th11.Definitions;
using IClearData = ThScoreFileConverter.Models.Th10.IClearData<ThScoreFileConverter.Core.Models.Th11.CharaWithTotal>;
using ISpellCard = ThScoreFileConverter.Models.Th10.ISpellCard<ThScoreFileConverter.Core.Models.Level>;

namespace ThScoreFileConverter.Tests.Models.Th11;

[TestClass]
public class CollectRateReplacerTests
{
    private static IClearData[] CreateClearDataList()
    {
        static ISpellCard MockSpellCard(int clearCount, int trialCount, int id, Level level)
        {
            var mock = Substitute.For<ISpellCard>();
            _ = mock.ClearCount.Returns(clearCount);
            _ = mock.TrialCount.Returns(trialCount);
            _ = mock.Id.Returns(id);
            _ = mock.Level.Returns(level);
            return mock;
        }

        var cards1 = Definitions.CardTable.ToDictionary(
            pair => pair.Key,
            pair => MockSpellCard(pair.Key % 3, pair.Key % 5, pair.Value.Id, pair.Value.Level));
        var mock1 = Substitute.For<IClearData>();
        _ = mock1.Chara.Returns(CharaWithTotal.ReimuSuika);
        _ = mock1.Cards.Returns(cards1);

        var cards2 = Definitions.CardTable.ToDictionary(
            pair => pair.Key,
            pair => MockSpellCard(pair.Key % 7, pair.Key % 11, pair.Value.Id, pair.Value.Level));
        var mock2 = Substitute.For<IClearData>();
        _ = mock2.Chara.Returns(CharaWithTotal.Total);
        _ = mock2.Cards.Returns(cards2);

        return [mock1, mock2];
    }

    internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
        CreateClearDataList().ToDictionary(clearData => clearData.Chara);

    [TestMethod]
    public void CollectRateReplacerTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CollectRateReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(dictionary, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 3", replacer.Replace("%T11CRGHRS31"));
    }

    [TestMethod]
    public void ReplaceTestTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 4", replacer.Replace("%T11CRGHRS32"));
    }

    [TestMethod]
    public void ReplaceTestLevelExtraClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 9", replacer.Replace("%T11CRGXRS31"));
    }

    [TestMethod]
    public void ReplaceTestLevelExtraTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 10", replacer.Replace("%T11CRGXRS32"));
    }

    [TestMethod]
    public void ReplaceTestLevelTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 10", replacer.Replace("%T11CRGTRS31"));
    }

    [TestMethod]
    public void ReplaceTestLevelTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 13", replacer.Replace("%T11CRGTRS32"));
    }

    [TestMethod]
    public void ReplaceTestCharaTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 4", replacer.Replace("%T11CRGHTL31"));
    }

    [TestMethod]
    public void ReplaceTestCharaTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 3", replacer.Replace("%T11CRGHTL32"));
    }

    [TestMethod]
    public void ReplaceTestStageTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 28", replacer.Replace("%T11CRGHRS01"));
    }

    [TestMethod]
    public void ReplaceTestStageTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 33", replacer.Replace("%T11CRGHRS02"));
    }

    [TestMethod]
    public void ReplaceTestTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 150", replacer.Replace("%T11CRGTTL01"));
    }

    [TestMethod]
    public void ReplaceTestTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 160", replacer.Replace("%T11CRGTTL02"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(dictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T11CRGHRS31"));
    }

    [TestMethod]
    public void ReplaceTestEmptyCards()
    {
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.ReimuSuika);
        _ = clearData.Cards.Returns(ImmutableDictionary<int, ISpellCard>.Empty);
        var dictionary = new[] { clearData }.ToDictionary(data => data.Chara);
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new CollectRateReplacer(dictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T11CRGHRS31"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T11XXXHRS31", replacer.Replace("%T11XXXHRS31"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T11CRGYRS31", replacer.Replace("%T11CRGYRS31"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T11CRGHXX31", replacer.Replace("%T11CRGHXX31"));
    }

    [TestMethod]
    public void ReplaceTestInvalidStage()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T11CRGHRSX1", replacer.Replace("%T11CRGHRSX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T11CRGHRS3X", replacer.Replace("%T11CRGHRS3X"));
    }
}

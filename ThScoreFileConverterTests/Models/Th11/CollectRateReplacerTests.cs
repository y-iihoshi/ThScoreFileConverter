using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th11;
using Definitions = ThScoreFileConverter.Models.Th11.Definitions;
using IClearData = ThScoreFileConverter.Models.Th10.IClearData<ThScoreFileConverter.Models.Th11.CharaWithTotal>;
using ISpellCard = ThScoreFileConverter.Models.Th10.ISpellCard<ThScoreFileConverter.Models.Level>;

namespace ThScoreFileConverterTests.Models.Th11;

[TestClass]
public class CollectRateReplacerTests
{
    private static IEnumerable<IClearData> CreateClearDataList()
    {
        static ISpellCard CreateSpellCard(int clearCount, int trialCount, int id, Level level)
        {
            var mock = new Mock<ISpellCard>();
            _ = mock.SetupGet(s => s.ClearCount).Returns(clearCount);
            _ = mock.SetupGet(s => s.TrialCount).Returns(trialCount);
            _ = mock.SetupGet(s => s.Id).Returns(id);
            _ = mock.SetupGet(s => s.Level).Returns(level);
            return mock.Object;
        }

        var mock1 = new Mock<IClearData>();
        _ = mock1.SetupGet(c => c.Chara).Returns(CharaWithTotal.ReimuSuika);
        _ = mock1.SetupGet(c => c.Cards).Returns(
            Definitions.CardTable.ToDictionary(
                pair => pair.Key,
                pair => CreateSpellCard(pair.Key % 3, pair.Key % 5, pair.Value.Id, pair.Value.Level)));

        var mock2 = new Mock<IClearData>();
        _ = mock2.SetupGet(c => c.Chara).Returns(CharaWithTotal.Total);
        _ = mock2.SetupGet(c => c.Cards).Returns(
            Definitions.CardTable.ToDictionary(
                pair => pair.Key,
                pair => CreateSpellCard(pair.Key % 7, pair.Key % 11, pair.Value.Id, pair.Value.Level)));

        return new[] { mock1.Object, mock2.Object };
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
    public void CollectRateReplacerTest()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CollectRateReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(dictionary, formatterMock.Object);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 3", replacer.Replace("%T11CRGHRS31"));
    }

    [TestMethod]
    public void ReplaceTestTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 4", replacer.Replace("%T11CRGHRS32"));
    }

    [TestMethod]
    public void ReplaceTestLevelExtraClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 9", replacer.Replace("%T11CRGXRS31"));
    }

    [TestMethod]
    public void ReplaceTestLevelExtraTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 10", replacer.Replace("%T11CRGXRS32"));
    }

    [TestMethod]
    public void ReplaceTestLevelTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 10", replacer.Replace("%T11CRGTRS31"));
    }

    [TestMethod]
    public void ReplaceTestLevelTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 13", replacer.Replace("%T11CRGTRS32"));
    }

    [TestMethod]
    public void ReplaceTestCharaTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 4", replacer.Replace("%T11CRGHTL31"));
    }

    [TestMethod]
    public void ReplaceTestCharaTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 3", replacer.Replace("%T11CRGHTL32"));
    }

    [TestMethod]
    public void ReplaceTestStageTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 28", replacer.Replace("%T11CRGHRS01"));
    }

    [TestMethod]
    public void ReplaceTestStageTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 33", replacer.Replace("%T11CRGHRS02"));
    }

    [TestMethod]
    public void ReplaceTestTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 150", replacer.Replace("%T11CRGTTL01"));
    }

    [TestMethod]
    public void ReplaceTestTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 160", replacer.Replace("%T11CRGTTL02"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(dictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T11CRGHRS31"));
    }

    [TestMethod]
    public void ReplaceTestEmptyCards()
    {
        var dictionary = new[]
        {
            Mock.Of<IClearData>(
                m => (m.Chara == CharaWithTotal.ReimuSuika)
                     && (m.Cards == ImmutableDictionary<int, ISpellCard>.Empty))
        }.ToDictionary(clearData => clearData.Chara);
        var formatterMock = MockNumberFormatter();

        var replacer = new CollectRateReplacer(dictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T11CRGHRS31"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T11XXXHRS31", replacer.Replace("%T11XXXHRS31"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T11CRGYRS31", replacer.Replace("%T11CRGYRS31"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T11CRGHXX31", replacer.Replace("%T11CRGHXX31"));
    }

    [TestMethod]
    public void ReplaceTestInvalidStage()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T11CRGHRSX1", replacer.Replace("%T11CRGHRSX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T11CRGHRS3X", replacer.Replace("%T11CRGHRS3X"));
    }
}

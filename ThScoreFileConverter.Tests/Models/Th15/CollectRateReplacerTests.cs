using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th15;
using Definitions = ThScoreFileConverter.Models.Th15.Definitions;
using GameMode = ThScoreFileConverter.Models.Th15.GameMode;
using ISpellCard = ThScoreFileConverter.Models.Th13.ISpellCard<ThScoreFileConverter.Models.Level>;

namespace ThScoreFileConverter.Tests.Models.Th15;

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

        static IClearDataPerGameMode CreateClearDataPerGameMode(IReadOnlyDictionary<int, ISpellCard> cards)
        {
            var mock = new Mock<IClearDataPerGameMode>();
            _ = mock.SetupGet(c => c.Cards).Returns(cards);
            return mock.Object;
        }

        var mock1 = new Mock<IClearData>();
        _ = mock1.SetupGet(c => c.Chara).Returns(CharaWithTotal.Marisa);
        _ = mock1.SetupGet(c => c.GameModeData).Returns(
            new Dictionary<GameMode, IClearDataPerGameMode>
            {
                {
                    GameMode.Pointdevice,
                    CreateClearDataPerGameMode(
                        Definitions.CardTable.ToDictionary(
                            pair => pair.Key,
                            pair => CreateSpellCard(pair.Key % 3, pair.Key % 5, pair.Value.Id, pair.Value.Level)))
                },
                {
                    GameMode.Legacy,
                    CreateClearDataPerGameMode(
                        Definitions.CardTable.ToDictionary(
                            pair => pair.Key,
                            pair => CreateSpellCard(pair.Key % 7, pair.Key % 11, pair.Value.Id, pair.Value.Level)))
                },
            });

        var mock2 = new Mock<IClearData>();
        _ = mock2.SetupGet(c => c.Chara).Returns(CharaWithTotal.Total);
        _ = mock2.SetupGet(c => c.GameModeData).Returns(
            new Dictionary<GameMode, IClearDataPerGameMode>
            {
                {
                    GameMode.Pointdevice,
                    CreateClearDataPerGameMode(
                        Definitions.CardTable.ToDictionary(
                            pair => pair.Key,
                            pair => CreateSpellCard(pair.Key % 7, pair.Key % 11, pair.Value.Id, pair.Value.Level)))
                },
                {
                    GameMode.Legacy,
                    CreateClearDataPerGameMode(
                        Definitions.CardTable.ToDictionary(
                            pair => pair.Key,
                            pair => CreateSpellCard(pair.Key % 3, pair.Key % 5, pair.Value.Id, pair.Value.Level)))
                },
            });

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
    public void ReplaceTestPointdeviceClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 4", replacer.Replace("%T15CRGPHMR31"));
    }

    [TestMethod]
    public void ReplaceTestPointdeviceTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 4", replacer.Replace("%T15CRGPHMR32"));
    }

    [TestMethod]
    public void ReplaceTestPointdeviceLevelExtraClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 9", replacer.Replace("%T15CRGPXMR31"));
    }

    [TestMethod]
    public void ReplaceTestPointdeviceLevelExtraTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 11", replacer.Replace("%T15CRGPXMR32"));
    }

    [TestMethod]
    public void ReplaceTestPointdeviceLevelTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 13", replacer.Replace("%T15CRGPTMR31"));
    }

    [TestMethod]
    public void ReplaceTestPointdeviceLevelTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 16", replacer.Replace("%T15CRGPTMR32"));
    }

    [TestMethod]
    public void ReplaceTestPointdeviceCharaTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 5", replacer.Replace("%T15CRGPHTL31"));
    }

    [TestMethod]
    public void ReplaceTestPointdeviceCharaTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 4", replacer.Replace("%T15CRGPHTL32"));
    }

    [TestMethod]
    public void ReplaceTestPointdeviceStageTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 18", replacer.Replace("%T15CRGPHMR01"));
    }

    [TestMethod]
    public void ReplaceTestPointdeviceStageTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 21", replacer.Replace("%T15CRGPHMR02"));
    }

    [TestMethod]
    public void ReplaceTestPointdeviceTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 102", replacer.Replace("%T15CRGPTTL01"));
    }

    [TestMethod]
    public void ReplaceTestPointdeviceTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 109", replacer.Replace("%T15CRGPTTL02"));
    }

    [TestMethod]
    public void ReplaceTestLegacyClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 5", replacer.Replace("%T15CRGLHMR31"));
    }

    [TestMethod]
    public void ReplaceTestLegacyTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 4", replacer.Replace("%T15CRGLHMR32"));
    }

    [TestMethod]
    public void ReplaceTestLegacyLevelExtraClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 11", replacer.Replace("%T15CRGLXMR31"));
    }

    [TestMethod]
    public void ReplaceTestLegacyLevelExtraTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 12", replacer.Replace("%T15CRGLXMR32"));
    }

    [TestMethod]
    public void ReplaceTestLegacyLevelTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 17", replacer.Replace("%T15CRGLTMR31"));
    }

    [TestMethod]
    public void ReplaceTestLegacyLevelTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 19", replacer.Replace("%T15CRGLTMR32"));
    }

    [TestMethod]
    public void ReplaceTestLegacyCharaTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 4", replacer.Replace("%T15CRGLHTL31"));
    }

    [TestMethod]
    public void ReplaceTestLegacyCharaTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 4", replacer.Replace("%T15CRGLHTL32"));
    }

    [TestMethod]
    public void ReplaceTestLegacyStageTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 23", replacer.Replace("%T15CRGLHMR01"));
    }

    [TestMethod]
    public void ReplaceTestLegacyStageTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 25", replacer.Replace("%T15CRGLHMR02"));
    }

    [TestMethod]
    public void ReplaceTestLegacyTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 80", replacer.Replace("%T15CRGLTTL01"));
    }

    [TestMethod]
    public void ReplaceTestLegacyTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 96", replacer.Replace("%T15CRGLTTL02"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(dictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T15CRGPHMR31"));
    }

    [TestMethod]
    public void ReplaceTestEmptyGameModes()
    {
        var dictionary = new[]
        {
            Mock.Of<IClearData>(
                m => (m.Chara == CharaWithTotal.Marisa)
                     && (m.GameModeData == ImmutableDictionary<GameMode, IClearDataPerGameMode>.Empty))
        }.ToDictionary(clearData => clearData.Chara);
        var formatterMock = MockNumberFormatter();

        var replacer = new CollectRateReplacer(dictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T15CRGPHMR31"));
    }

    [TestMethod]
    public void ReplaceTestEmptyCards()
    {
        var dictionary = new[]
        {
            Mock.Of<IClearData>(
                m => (m.Chara == CharaWithTotal.Marisa)
                     && (m.GameModeData == new Dictionary<GameMode, IClearDataPerGameMode>
                        {
                            {
                                GameMode.Pointdevice,
                                Mock.Of<IClearDataPerGameMode>(
                                    c => c.Cards == ImmutableDictionary<int, ISpellCard>.Empty)
                            },
                        }))
        }.ToDictionary(clearData => clearData.Chara);
        var formatterMock = MockNumberFormatter();

        var replacer = new CollectRateReplacer(dictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T15CRGPHMR31"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T15XXXPHMR31", replacer.Replace("%T15XXXPHMR31"));
    }

    [TestMethod]
    public void ReplaceTestInvalidGameMode()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T15CRGXHMR31", replacer.Replace("%T15CRGXHMR31"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T15CRGPYMR31", replacer.Replace("%T15CRGPYMR31"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T15CRGPHXX31", replacer.Replace("%T15CRGPHXX31"));
    }

    [TestMethod]
    public void ReplaceTestInvalidStage()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T15CRGPHMRX1", replacer.Replace("%T15CRGPHMRX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T15CRGPHMR3X", replacer.Replace("%T15CRGPHMR3X"));
    }
}

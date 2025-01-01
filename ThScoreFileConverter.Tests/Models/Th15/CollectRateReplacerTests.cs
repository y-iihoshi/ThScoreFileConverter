using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th15;
using ThScoreFileConverter.Models.Th15;
using Definitions = ThScoreFileConverter.Models.Th15.Definitions;
using GameMode = ThScoreFileConverter.Core.Models.Th15.GameMode;
using ISpellCard = ThScoreFileConverter.Models.Th13.ISpellCard<ThScoreFileConverter.Core.Models.Level>;

namespace ThScoreFileConverter.Tests.Models.Th15;

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

        static IClearDataPerGameMode MockClearDataPerGameMode(IReadOnlyDictionary<int, ISpellCard> cards)
        {
            var mock = Substitute.For<IClearDataPerGameMode>();
            _ = mock.Cards.Returns(cards);
            return mock;
        }

        var gameModeData1 = new Dictionary<GameMode, IClearDataPerGameMode>
        {
            {
                GameMode.Pointdevice,
                MockClearDataPerGameMode(
                    Definitions.CardTable.ToDictionary(
                        pair => pair.Key,
                        pair => MockSpellCard(pair.Key % 3, pair.Key % 5, pair.Value.Id, pair.Value.Level)))
            },
            {
                GameMode.Legacy,
                MockClearDataPerGameMode(
                    Definitions.CardTable.ToDictionary(
                        pair => pair.Key,
                        pair => MockSpellCard(pair.Key % 7, pair.Key % 11, pair.Value.Id, pair.Value.Level)))
            },
        };
        var mock1 = Substitute.For<IClearData>();
        _ = mock1.Chara.Returns(CharaWithTotal.Marisa);
        _ = mock1.GameModeData.Returns(gameModeData1);

        var gameModeData2 = new Dictionary<GameMode, IClearDataPerGameMode>
        {
            {
                GameMode.Pointdevice,
                MockClearDataPerGameMode(
                    Definitions.CardTable.ToDictionary(
                        pair => pair.Key,
                        pair => MockSpellCard(pair.Key % 7, pair.Key % 11, pair.Value.Id, pair.Value.Level)))
            },
            {
                GameMode.Legacy,
                MockClearDataPerGameMode(
                    Definitions.CardTable.ToDictionary(
                        pair => pair.Key,
                        pair => MockSpellCard(pair.Key % 3, pair.Key % 5, pair.Value.Id, pair.Value.Level)))
            },
        };
        var mock2 = Substitute.For<IClearData>();
        _ = mock2.Chara.Returns(CharaWithTotal.Total);
        _ = mock2.GameModeData.Returns(gameModeData2);

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
    public void ReplaceTestPointdeviceClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 4", replacer.Replace("%T15CRGPHMR31"));
    }

    [TestMethod]
    public void ReplaceTestPointdeviceTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 4", replacer.Replace("%T15CRGPHMR32"));
    }

    [TestMethod]
    public void ReplaceTestPointdeviceLevelExtraClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 9", replacer.Replace("%T15CRGPXMR31"));
    }

    [TestMethod]
    public void ReplaceTestPointdeviceLevelExtraTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 11", replacer.Replace("%T15CRGPXMR32"));
    }

    [TestMethod]
    public void ReplaceTestPointdeviceLevelTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 13", replacer.Replace("%T15CRGPTMR31"));
    }

    [TestMethod]
    public void ReplaceTestPointdeviceLevelTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 16", replacer.Replace("%T15CRGPTMR32"));
    }

    [TestMethod]
    public void ReplaceTestPointdeviceCharaTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 5", replacer.Replace("%T15CRGPHTL31"));
    }

    [TestMethod]
    public void ReplaceTestPointdeviceCharaTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 4", replacer.Replace("%T15CRGPHTL32"));
    }

    [TestMethod]
    public void ReplaceTestPointdeviceStageTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 18", replacer.Replace("%T15CRGPHMR01"));
    }

    [TestMethod]
    public void ReplaceTestPointdeviceStageTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 21", replacer.Replace("%T15CRGPHMR02"));
    }

    [TestMethod]
    public void ReplaceTestPointdeviceTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 102", replacer.Replace("%T15CRGPTTL01"));
    }

    [TestMethod]
    public void ReplaceTestPointdeviceTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 109", replacer.Replace("%T15CRGPTTL02"));
    }

    [TestMethod]
    public void ReplaceTestLegacyClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 5", replacer.Replace("%T15CRGLHMR31"));
    }

    [TestMethod]
    public void ReplaceTestLegacyTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 4", replacer.Replace("%T15CRGLHMR32"));
    }

    [TestMethod]
    public void ReplaceTestLegacyLevelExtraClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 11", replacer.Replace("%T15CRGLXMR31"));
    }

    [TestMethod]
    public void ReplaceTestLegacyLevelExtraTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 12", replacer.Replace("%T15CRGLXMR32"));
    }

    [TestMethod]
    public void ReplaceTestLegacyLevelTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 17", replacer.Replace("%T15CRGLTMR31"));
    }

    [TestMethod]
    public void ReplaceTestLegacyLevelTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 19", replacer.Replace("%T15CRGLTMR32"));
    }

    [TestMethod]
    public void ReplaceTestLegacyCharaTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 4", replacer.Replace("%T15CRGLHTL31"));
    }

    [TestMethod]
    public void ReplaceTestLegacyCharaTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 4", replacer.Replace("%T15CRGLHTL32"));
    }

    [TestMethod]
    public void ReplaceTestLegacyStageTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 23", replacer.Replace("%T15CRGLHMR01"));
    }

    [TestMethod]
    public void ReplaceTestLegacyStageTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 25", replacer.Replace("%T15CRGLHMR02"));
    }

    [TestMethod]
    public void ReplaceTestLegacyTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 80", replacer.Replace("%T15CRGLTTL01"));
    }

    [TestMethod]
    public void ReplaceTestLegacyTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 96", replacer.Replace("%T15CRGLTTL02"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(dictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T15CRGPHMR31"));
    }

    [TestMethod]
    public void ReplaceTestEmptyGameModes()
    {
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.Marisa);
        _ = clearData.GameModeData.Returns(ImmutableDictionary<GameMode, IClearDataPerGameMode>.Empty);
        var dictionary = new[] { clearData }.ToDictionary(data => data.Chara);
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new CollectRateReplacer(dictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T15CRGPHMR31"));
    }

    [TestMethod]
    public void ReplaceTestEmptyCards()
    {
        var clearDataPerGameMode = Substitute.For<IClearDataPerGameMode>();
        _ = clearDataPerGameMode.Cards.Returns(ImmutableDictionary<int, ISpellCard>.Empty);
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.Marisa);
        _ = clearData.GameModeData.Returns(new[] { (GameMode.Pointdevice, clearDataPerGameMode) }.ToDictionary());
        var dictionary = new[] { clearData }.ToDictionary(data => data.Chara);
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new CollectRateReplacer(dictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T15CRGPHMR31"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T15XXXPHMR31", replacer.Replace("%T15XXXPHMR31"));
    }

    [TestMethod]
    public void ReplaceTestInvalidGameMode()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T15CRGXHMR31", replacer.Replace("%T15CRGXHMR31"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T15CRGPYMR31", replacer.Replace("%T15CRGPYMR31"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T15CRGPHXX31", replacer.Replace("%T15CRGPHXX31"));
    }

    [TestMethod]
    public void ReplaceTestInvalidStage()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T15CRGPHMRX1", replacer.Replace("%T15CRGPHMRX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T15CRGPHMR3X", replacer.Replace("%T15CRGPHMR3X"));
    }
}

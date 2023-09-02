using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Models.Th13;
using ThScoreFileConverter.Models.Th13;
using Definitions = ThScoreFileConverter.Models.Th13.Definitions;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Core.Models.Th13.CharaWithTotal,
    ThScoreFileConverter.Core.Models.Th13.LevelPractice,
    ThScoreFileConverter.Core.Models.Th13.LevelPractice,
    ThScoreFileConverter.Core.Models.Th13.LevelPracticeWithTotal,
    ThScoreFileConverter.Core.Models.Th13.StagePractice,
    ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>>;
using ISpellCard = ThScoreFileConverter.Models.Th13.ISpellCard<ThScoreFileConverter.Core.Models.Th13.LevelPractice>;

namespace ThScoreFileConverter.Tests.Models.Th13;

[TestClass]
public class CollectRateReplacerTests
{
    private static IEnumerable<IClearData> CreateClearDataList()
    {
        static ISpellCard MockSpellCard(
            int clear, int practiceClear, int trial, int practiceTrial, int id, LevelPractice level)
        {
            var mock = Substitute.For<ISpellCard>();
            _ = mock.ClearCount.Returns(clear);
            _ = mock.PracticeClearCount.Returns(practiceClear);
            _ = mock.TrialCount.Returns(trial);
            _ = mock.PracticeTrialCount.Returns(practiceTrial);
            _ = mock.Id.Returns(id);
            _ = mock.Level.Returns(level);
            return mock;
        }

        var cards1 = Definitions.CardTable.ToDictionary(
            pair => pair.Key,
            pair => MockSpellCard(pair.Key % 3, pair.Key % 7, pair.Key % 5, pair.Key % 11, pair.Value.Id, pair.Value.Level));
        var mock1 = Substitute.For<IClearData>();
        _ = mock1.Chara.Returns(CharaWithTotal.Marisa);
        _ = mock1.Cards.Returns(cards1);

        var cards2 = Definitions.CardTable.ToDictionary(
            pair => pair.Key,
            pair => MockSpellCard(pair.Key % 7, pair.Key % 3, pair.Key % 11, pair.Key % 5, pair.Value.Id, pair.Value.Level));
        var mock2 = Substitute.For<IClearData>();
        _ = mock2.Chara.Returns(CharaWithTotal.Total);
        _ = mock2.Cards.Returns(cards2);

        return new[] { mock1, mock2 };
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
    public void ReplaceTestStoryClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 3", replacer.Replace("%T13CRGSHMR31"));
    }

    [TestMethod]
    public void ReplaceTestStoryTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 3", replacer.Replace("%T13CRGSHMR32"));
    }

    [TestMethod]
    public void ReplaceTestStoryLevelExtraClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 9", replacer.Replace("%T13CRGSXMR31"));
    }

    [TestMethod]
    public void ReplaceTestStoryLevelExtraTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 11", replacer.Replace("%T13CRGSXMR32"));
    }

    [TestMethod]
    public void ReplaceTestStoryLevelOverDriveClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T13CRGSDMR31", replacer.Replace("%T13CRGSDMR31"));
    }

    [TestMethod]
    public void ReplaceTestStoryLevelOverDriveTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T13CRGSDMR32", replacer.Replace("%T13CRGSDMR32"));
    }

    [TestMethod]
    public void ReplaceTestStoryLevelTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 10", replacer.Replace("%T13CRGSTMR31"));
    }

    [TestMethod]
    public void ReplaceTestStoryLevelTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 12", replacer.Replace("%T13CRGSTMR32"));
    }

    [TestMethod]
    public void ReplaceTestStoryCharaTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 3", replacer.Replace("%T13CRGSHTL31"));
    }

    [TestMethod]
    public void ReplaceTestStoryCharaTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 4", replacer.Replace("%T13CRGSHTL32"));
    }

    [TestMethod]
    public void ReplaceTestStoryStageTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 20", replacer.Replace("%T13CRGSHMR01"));
    }

    [TestMethod]
    public void ReplaceTestStoryStageTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 22", replacer.Replace("%T13CRGSHMR02"));
    }

    [TestMethod]
    public void ReplaceTestStoryTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 102", replacer.Replace("%T13CRGSTTL01"));
    }

    [TestMethod]
    public void ReplaceTestStoryTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 109", replacer.Replace("%T13CRGSTTL02"));
    }

    [TestMethod]
    public void ReplaceTestPracticeClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 3", replacer.Replace("%T13CRGPHMR31"));
    }

    [TestMethod]
    public void ReplaceTestPracticeTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 4", replacer.Replace("%T13CRGPHMR32"));
    }

    [TestMethod]
    public void ReplaceTestPracticeLevelExtraClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 11", replacer.Replace("%T13CRGPXMR31"));
    }

    [TestMethod]
    public void ReplaceTestPracticeLevelExtraTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 12", replacer.Replace("%T13CRGPXMR32"));
    }

    [TestMethod]
    public void ReplaceTestPracticeLevelOverDriveClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 7", replacer.Replace("%T13CRGPDMR31"));
    }

    [TestMethod]
    public void ReplaceTestPracticeLevelOverDriveTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 7", replacer.Replace("%T13CRGPDMR32"));
    }

    [TestMethod]
    public void ReplaceTestPracticeLevelTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 12", replacer.Replace("%T13CRGPTMR31"));
    }

    [TestMethod]
    public void ReplaceTestPracticeLevelTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 12", replacer.Replace("%T13CRGPTMR32"));
    }

    [TestMethod]
    public void ReplaceTestPracticeCharaTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 3", replacer.Replace("%T13CRGPHTL31"));
    }

    [TestMethod]
    public void ReplaceTestPracticeCharaTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 3", replacer.Replace("%T13CRGPHTL32"));
    }

    [TestMethod]
    public void ReplaceTestPracticeStageTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 23", replacer.Replace("%T13CRGPHMR01"));
    }

    [TestMethod]
    public void ReplaceTestPracticeStageTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 26", replacer.Replace("%T13CRGPHMR02"));
    }

    [TestMethod]
    public void ReplaceTestPracticeTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 85", replacer.Replace("%T13CRGPTTL01"));
    }

    [TestMethod]
    public void ReplaceTestPracticeTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 102", replacer.Replace("%T13CRGPTTL02"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(dictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T13CRGSHMR31"));
    }

    [TestMethod]
    public void ReplaceTestEmptyCards()
    {
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.Marisa);
        _ = clearData.Cards.Returns(ImmutableDictionary<int, ISpellCard>.Empty);
        var dictionary = new[] { clearData }.ToDictionary(data => data.Chara);
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new CollectRateReplacer(dictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T13CRGSHMR31"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T13XXXSHMR31", replacer.Replace("%T13XXXSHMR31"));
    }

    [TestMethod]
    public void ReplaceTestInvalidGameMode()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T13CRGXHMR31", replacer.Replace("%T13CRGXHMR31"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T13CRGSYMR31", replacer.Replace("%T13CRGSYMR31"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T13CRGSHXX31", replacer.Replace("%T13CRGSHXX31"));
    }

    [TestMethod]
    public void ReplaceTestInvalidStage()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T13CRGSHMRX1", replacer.Replace("%T13CRGSHMRX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T13CRGSHMR3X", replacer.Replace("%T13CRGSHMR3X"));
    }
}

using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th18;
using ThScoreFileConverter.Models.Th18;
using Definitions = ThScoreFileConverter.Models.Th18.Definitions;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Core.Models.Th18.CharaWithTotal,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Core.Models.Stage,
    ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>>;
using ISpellCard = ThScoreFileConverter.Models.Th13.ISpellCard<ThScoreFileConverter.Core.Models.Level>;

namespace ThScoreFileConverter.Tests.Models.Th18;

[TestClass]
public class CollectRateReplacerTests
{
    private static IClearData[] CreateClearDataList()
    {
        static ISpellCard MockSpellCard(
            int id, Level level, int clear, int trial, int practiceClear, int practiceTrial)
        {
            var mock = Substitute.For<ISpellCard>();
            _ = mock.Id.Returns(id);
            _ = mock.Level.Returns(level);
            _ = mock.ClearCount.Returns(clear);
            _ = mock.TrialCount.Returns(trial);
            _ = mock.PracticeClearCount.Returns(practiceClear);
            _ = mock.PracticeTrialCount.Returns(practiceTrial);
            return mock;
        }

        var cards1 = Definitions.CardTable.ToDictionary(
            pair => pair.Key,
            pair => MockSpellCard(pair.Value.Id, pair.Value.Level, pair.Key % 2, pair.Key % 3, pair.Key % 4, pair.Key % 5));
        var mock1 = Substitute.For<IClearData>();
        _ = mock1.Chara.Returns(CharaWithTotal.Marisa);
        _ = mock1.Cards.Returns(cards1);

        var cards2 = Definitions.CardTable.ToDictionary(
            pair => pair.Key,
            pair => MockSpellCard(pair.Value.Id, pair.Value.Level, pair.Key % 6, pair.Key % 7, pair.Key % 8, pair.Key));
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
    public void ReplaceTestStoryClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 3", replacer.Replace("%T18CRGSHMR31"));
    }

    [TestMethod]
    public void ReplaceTestStoryTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 2", replacer.Replace("%T18CRGSHMR32"));
    }

    [TestMethod]
    public void ReplaceTestPracticeClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 3", replacer.Replace("%T18CRGPHMR31"));
    }

    [TestMethod]
    public void ReplaceTestPracticeTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 3", replacer.Replace("%T18CRGPHMR32"));
    }

    [TestMethod]
    public void ReplaceTestStoryLevelExtraClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 7", replacer.Replace("%T18CRGSXMR31"));
    }

    [TestMethod]
    public void ReplaceTestStoryLevelExtraTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 9", replacer.Replace("%T18CRGSXMR32"));
    }

    [TestMethod]
    public void ReplaceTestPracticeLevelExtraClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 10", replacer.Replace("%T18CRGPXMR31"));
    }

    [TestMethod]
    public void ReplaceTestPracticeLevelExtraTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 10", replacer.Replace("%T18CRGPXMR32"));
    }

    [TestMethod]
    public void ReplaceTestStoryLevelTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 6", replacer.Replace("%T18CRGSTMR31"));
    }

    [TestMethod]
    public void ReplaceTestStoryLevelTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 8", replacer.Replace("%T18CRGSTMR32"));
    }

    [TestMethod]
    public void ReplaceTestPracticeLevelTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 9", replacer.Replace("%T18CRGPTMR31"));
    }

    [TestMethod]
    public void ReplaceTestPracticeLevelTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 10", replacer.Replace("%T18CRGPTMR32"));
    }

    [TestMethod]
    public void ReplaceTestStoryCharaTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 3", replacer.Replace("%T18CRGSHTL31"));
    }

    [TestMethod]
    public void ReplaceTestStoryCharaTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 3", replacer.Replace("%T18CRGSHTL32"));
    }

    [TestMethod]
    public void ReplaceTestPracticeCharaTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 3", replacer.Replace("%T18CRGPHTL31"));
    }

    [TestMethod]
    public void ReplaceTestPracticeCharaTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 3", replacer.Replace("%T18CRGPHTL32"));
    }

    [TestMethod]
    public void ReplaceTestStoryStageTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 21", replacer.Replace("%T18CRGSHMR01"));
    }

    [TestMethod]
    public void ReplaceTestStoryStageTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 14", replacer.Replace("%T18CRGSHMR02"));
    }

    [TestMethod]
    public void ReplaceTestPracticeStageTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 21", replacer.Replace("%T18CRGPHMR01"));
    }

    [TestMethod]
    public void ReplaceTestPracticeStageTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 17", replacer.Replace("%T18CRGPHMR02"));
    }

    [TestMethod]
    public void ReplaceTestStoryTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 81", replacer.Replace("%T18CRGSTTL01"));
    }

    [TestMethod]
    public void ReplaceTestStoryTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 84", replacer.Replace("%T18CRGSTTL02"));
    }

    [TestMethod]
    public void ReplaceTestPracticeTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 85", replacer.Replace("%T18CRGPTTL01"));
    }

    [TestMethod]
    public void ReplaceTestPracticeTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 97", replacer.Replace("%T18CRGPTTL02"));
    }

    [TestMethod]
    public void ReplaceTestStoryEmptyClearCount()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(dictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T18CRGSHMR31"));
    }

    [TestMethod]
    public void ReplaceTestStoryEmptyTrialCount()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(dictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T18CRGSHMR32"));
    }

    [TestMethod]
    public void ReplaceTestPracticeEmptyClearCount()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(dictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T18CRGPHMR31"));
    }

    [TestMethod]
    public void ReplaceTestPracticeEmptyTrialCount()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(dictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T18CRGPHMR32"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T18XXXSHMR31", replacer.Replace("%T18XXXSHMR31"));
    }

    [TestMethod]
    public void ReplaceTestInvalidGameMode()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T18CRGXHMR31", replacer.Replace("%T18CRGXHMR31"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T18CRGSYMR31", replacer.Replace("%T18CRGSYMR31"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T18CRGSHXX31", replacer.Replace("%T18CRGSHXX31"));
    }

    [TestMethod]
    public void ReplaceTestInvalidStage()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T18CRGSHMRX1", replacer.Replace("%T18CRGSHMRX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T18CRGSHMR3X", replacer.Replace("%T18CRGSHMR3X"));
    }
}

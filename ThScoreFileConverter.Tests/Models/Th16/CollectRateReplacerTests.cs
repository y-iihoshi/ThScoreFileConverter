using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Models.Th16;
using ThScoreFileConverter.Models.Th16;
using Definitions = ThScoreFileConverter.Models.Th16.Definitions;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Core.Models.Th16.CharaWithTotal,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Core.Models.Th14.StagePractice,
    ThScoreFileConverter.Models.Th16.IScoreData>;
using ISpellCard = ThScoreFileConverter.Models.Th13.ISpellCard<ThScoreFileConverter.Core.Models.Level>;
using Level = ThScoreFileConverter.Core.Models.Level;

namespace ThScoreFileConverter.Tests.Models.Th16;

[TestClass]
public class CollectRateReplacerTests
{
    private static IClearData[] CreateClearDataList()
    {
        static ISpellCard MockSpellCard(
            int clear, int practiceClear, int trial, int practiceTrial, int id, Level level)
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
        _ = mock1.Chara.Returns(CharaWithTotal.Aya);
        _ = mock1.Cards.Returns(cards1);

        var cards2 = Definitions.CardTable.ToDictionary(
            pair => pair.Key,
            pair => MockSpellCard(pair.Key % 7, pair.Key % 3, pair.Key % 11, pair.Key % 5, pair.Value.Id, pair.Value.Level));
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
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void CollectRateReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(dictionary, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTestStoryClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16CRGSHAY31").ShouldBe("invoked: 2");
    }

    [TestMethod]
    public void ReplaceTestStoryTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16CRGSHAY32").ShouldBe("invoked: 3");
    }

    [TestMethod]
    public void ReplaceTestStoryLevelExtraClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16CRGSXAY31").ShouldBe("invoked: 9");
    }

    [TestMethod]
    public void ReplaceTestStoryLevelExtraTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16CRGSXAY32").ShouldBe("invoked: 11");
    }

    [TestMethod]
    public void ReplaceTestStoryLevelTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16CRGSTAY31").ShouldBe("invoked: 9");
    }

    [TestMethod]
    public void ReplaceTestStoryLevelTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16CRGSTAY32").ShouldBe("invoked: 12");
    }

    [TestMethod]
    public void ReplaceTestStoryCharaTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16CRGSHTL31").ShouldBe("invoked: 3");
    }

    [TestMethod]
    public void ReplaceTestStoryCharaTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16CRGSHTL32").ShouldBe("invoked: 3");
    }

    [TestMethod]
    public void ReplaceTestStoryStageTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16CRGSHAY01").ShouldBe("invoked: 17");
    }

    [TestMethod]
    public void ReplaceTestStoryStageTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16CRGSHAY02").ShouldBe("invoked: 21");
    }

    [TestMethod]
    public void ReplaceTestStoryTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16CRGSTTL01").ShouldBe("invoked: 102");
    }

    [TestMethod]
    public void ReplaceTestStoryTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16CRGSTTL02").ShouldBe("invoked: 109");
    }

    [TestMethod]
    public void ReplaceTestPracticeClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16CRGPHAY31").ShouldBe("invoked: 3");
    }

    [TestMethod]
    public void ReplaceTestPracticeTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16CRGPHAY32").ShouldBe("invoked: 3");
    }

    [TestMethod]
    public void ReplaceTestPracticeLevelExtraClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16CRGPXAY31").ShouldBe("invoked: 11");
    }

    [TestMethod]
    public void ReplaceTestPracticeLevelExtraTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16CRGPXAY32").ShouldBe("invoked: 12");
    }

    [TestMethod]
    public void ReplaceTestPracticeLevelTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16CRGPTAY31").ShouldBe("invoked: 12");
    }

    [TestMethod]
    public void ReplaceTestPracticeLevelTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16CRGPTAY32").ShouldBe("invoked: 12");
    }

    [TestMethod]
    public void ReplaceTestPracticeCharaTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16CRGPHTL31").ShouldBe("invoked: 2");
    }

    [TestMethod]
    public void ReplaceTestPracticeCharaTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16CRGPHTL32").ShouldBe("invoked: 3");
    }

    [TestMethod]
    public void ReplaceTestPracticeStageTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16CRGPHAY01").ShouldBe("invoked: 22");
    }

    [TestMethod]
    public void ReplaceTestPracticeStageTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16CRGPHAY02").ShouldBe("invoked: 24");
    }

    [TestMethod]
    public void ReplaceTestPracticeTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16CRGPTTL01").ShouldBe("invoked: 80");
    }

    [TestMethod]
    public void ReplaceTestPracticeTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16CRGPTTL02").ShouldBe("invoked: 96");
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(dictionary, formatterMock);
        replacer.Replace("%T16CRGSHAY31").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestEmptyCards()
    {
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.Aya);
        _ = clearData.Cards.Returns(ImmutableDictionary<int, ISpellCard>.Empty);
        var dictionary = new[] { clearData }.ToDictionary(data => data.Chara);
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new CollectRateReplacer(dictionary, formatterMock);
        replacer.Replace("%T16CRGSHAY31").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16XXXSHAY31").ShouldBe("%T16XXXSHAY31");
    }

    [TestMethod]
    public void ReplaceTestInvalidGameMode()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16CRGXHAY31").ShouldBe("%T16CRGXHAY31");
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16CRGSYAY31").ShouldBe("%T16CRGSYAY31");
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16CRGSHXX31").ShouldBe("%T16CRGSHXX31");
    }

    [TestMethod]
    public void ReplaceTestInvalidStage()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16CRGSHAYX1").ShouldBe("%T16CRGSHAYX1");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16CRGSHAY3X").ShouldBe("%T16CRGSHAY3X");
    }
}

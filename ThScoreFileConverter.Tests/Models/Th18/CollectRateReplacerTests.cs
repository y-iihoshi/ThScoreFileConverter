using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Moq;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th18;
using ThScoreFileConverter.Models;
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
    private static IEnumerable<IClearData> CreateClearDataList()
    {
        static ISpellCard CreateSpellCard(
            int id, Level level, int clear, int trial, int practiceClear, int practiceTrial)
        {
            var mock = new Mock<ISpellCard>();
            _ = mock.SetupGet(s => s.Id).Returns(id);
            _ = mock.SetupGet(s => s.Level).Returns(level);
            _ = mock.SetupGet(s => s.ClearCount).Returns(clear);
            _ = mock.SetupGet(s => s.TrialCount).Returns(trial);
            _ = mock.SetupGet(s => s.PracticeClearCount).Returns(practiceClear);
            _ = mock.SetupGet(s => s.PracticeTrialCount).Returns(practiceTrial);
            return mock.Object;
        }

        var mock1 = new Mock<IClearData>();
        _ = mock1.SetupGet(c => c.Chara).Returns(CharaWithTotal.Marisa);
        _ = mock1.SetupGet(c => c.Cards).Returns(
            Definitions.CardTable.ToDictionary(
                pair => pair.Key,
                pair => CreateSpellCard(
                    pair.Value.Id, pair.Value.Level, pair.Key % 2, pair.Key % 3, pair.Key % 4, pair.Key % 5)));

        var mock2 = new Mock<IClearData>();
        _ = mock2.SetupGet(c => c.Chara).Returns(CharaWithTotal.Total);
        _ = mock2.SetupGet(c => c.Cards).Returns(
            Definitions.CardTable.ToDictionary(
                pair => pair.Key,
                pair => CreateSpellCard(
                    pair.Value.Id, pair.Value.Level, pair.Key % 6, pair.Key % 7, pair.Key % 8, pair.Key)));

        return new[] { mock1.Object, mock2.Object };
    }

    internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
        CreateClearDataList().ToDictionary(clearData => clearData.Chara);

    private static Mock<INumberFormatter> MockNumberFormatter()
    {
        var mock = new Mock<INumberFormatter>();
        _ = mock.Setup(formatter => formatter.FormatNumber(It.IsAny<It.IsValueType>()))
            .Returns((object value) => $"invoked: {value}");
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
    public void ReplaceTestStoryClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 3", replacer.Replace("%T18CRGSHMR31"));
    }

    [TestMethod]
    public void ReplaceTestStoryTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 2", replacer.Replace("%T18CRGSHMR32"));
    }

    [TestMethod]
    public void ReplaceTestPracticeClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 3", replacer.Replace("%T18CRGPHMR31"));
    }

    [TestMethod]
    public void ReplaceTestPracticeTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 3", replacer.Replace("%T18CRGPHMR32"));
    }

    [TestMethod]
    public void ReplaceTestStoryLevelExtraClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 7", replacer.Replace("%T18CRGSXMR31"));
    }

    [TestMethod]
    public void ReplaceTestStoryLevelExtraTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 9", replacer.Replace("%T18CRGSXMR32"));
    }

    [TestMethod]
    public void ReplaceTestPracticeLevelExtraClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 10", replacer.Replace("%T18CRGPXMR31"));
    }

    [TestMethod]
    public void ReplaceTestPracticeLevelExtraTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 10", replacer.Replace("%T18CRGPXMR32"));
    }

    [TestMethod]
    public void ReplaceTestStoryLevelTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 6", replacer.Replace("%T18CRGSTMR31"));
    }

    [TestMethod]
    public void ReplaceTestStoryLevelTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 8", replacer.Replace("%T18CRGSTMR32"));
    }

    [TestMethod]
    public void ReplaceTestPracticeLevelTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 9", replacer.Replace("%T18CRGPTMR31"));
    }

    [TestMethod]
    public void ReplaceTestPracticeLevelTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 10", replacer.Replace("%T18CRGPTMR32"));
    }

    [TestMethod]
    public void ReplaceTestStoryCharaTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 3", replacer.Replace("%T18CRGSHTL31"));
    }

    [TestMethod]
    public void ReplaceTestStoryCharaTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 3", replacer.Replace("%T18CRGSHTL32"));
    }

    [TestMethod]
    public void ReplaceTestPracticeCharaTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 3", replacer.Replace("%T18CRGPHTL31"));
    }

    [TestMethod]
    public void ReplaceTestPracticeCharaTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 3", replacer.Replace("%T18CRGPHTL32"));
    }

    [TestMethod]
    public void ReplaceTestStoryStageTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 21", replacer.Replace("%T18CRGSHMR01"));
    }

    [TestMethod]
    public void ReplaceTestStoryStageTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 14", replacer.Replace("%T18CRGSHMR02"));
    }

    [TestMethod]
    public void ReplaceTestPracticeStageTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 21", replacer.Replace("%T18CRGPHMR01"));
    }

    [TestMethod]
    public void ReplaceTestPracticeStageTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 17", replacer.Replace("%T18CRGPHMR02"));
    }

    [TestMethod]
    public void ReplaceTestStoryTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 81", replacer.Replace("%T18CRGSTTL01"));
    }

    [TestMethod]
    public void ReplaceTestStoryTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 84", replacer.Replace("%T18CRGSTTL02"));
    }

    [TestMethod]
    public void ReplaceTestPracticeTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 85", replacer.Replace("%T18CRGPTTL01"));
    }

    [TestMethod]
    public void ReplaceTestPracticeTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 97", replacer.Replace("%T18CRGPTTL02"));
    }

    [TestMethod]
    public void ReplaceTestStoryEmptyClearCount()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(dictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T18CRGSHMR31"));
    }

    [TestMethod]
    public void ReplaceTestStoryEmptyTrialCount()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(dictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T18CRGSHMR32"));
    }

    [TestMethod]
    public void ReplaceTestPracticeEmptyClearCount()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(dictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T18CRGPHMR31"));
    }

    [TestMethod]
    public void ReplaceTestPracticeEmptyTrialCount()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(dictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T18CRGPHMR32"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T18XXXSHMR31", replacer.Replace("%T18XXXSHMR31"));
    }

    [TestMethod]
    public void ReplaceTestInvalidGameMode()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T18CRGXHMR31", replacer.Replace("%T18CRGXHMR31"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T18CRGSYMR31", replacer.Replace("%T18CRGSYMR31"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T18CRGSHXX31", replacer.Replace("%T18CRGSHXX31"));
    }

    [TestMethod]
    public void ReplaceTestInvalidStage()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T18CRGSHMRX1", replacer.Replace("%T18CRGSHMRX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T18CRGSHMR3X", replacer.Replace("%T18CRGSHMR3X"));
    }
}

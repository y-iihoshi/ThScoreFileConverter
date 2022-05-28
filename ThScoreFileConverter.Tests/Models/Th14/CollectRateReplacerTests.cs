using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th14;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th14;
using Definitions = ThScoreFileConverter.Models.Th14.Definitions;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Core.Models.Th14.CharaWithTotal,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Models.Th14.LevelPractice,
    ThScoreFileConverter.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Models.Th14.StagePractice,
    ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>>;
using ISpellCard = ThScoreFileConverter.Models.Th13.ISpellCard<ThScoreFileConverter.Core.Models.Level>;

namespace ThScoreFileConverter.Tests.Models.Th14;

[TestClass]
public class CollectRateReplacerTests
{
    private static IEnumerable<IClearData> CreateClearDataList()
    {
        static ISpellCard CreateSpellCard(
            int clear, int practiceClear, int trial, int practiceTrial, int id, Level level)
        {
            var mock = new Mock<ISpellCard>();
            _ = mock.SetupGet(s => s.ClearCount).Returns(clear);
            _ = mock.SetupGet(s => s.PracticeClearCount).Returns(practiceClear);
            _ = mock.SetupGet(s => s.TrialCount).Returns(trial);
            _ = mock.SetupGet(s => s.PracticeTrialCount).Returns(practiceTrial);
            _ = mock.SetupGet(s => s.Id).Returns(id);
            _ = mock.SetupGet(s => s.Level).Returns(level);
            return mock.Object;
        }

        var mock1 = new Mock<IClearData>();
        _ = mock1.SetupGet(c => c.Chara).Returns(CharaWithTotal.ReimuB);
        _ = mock1.SetupGet(c => c.Cards).Returns(
            Definitions.CardTable.ToDictionary(
                pair => pair.Key,
                pair => CreateSpellCard(
                    pair.Key % 3, pair.Key % 7, pair.Key % 5, pair.Key % 11, pair.Value.Id, pair.Value.Level)));

        var mock2 = new Mock<IClearData>();
        _ = mock2.SetupGet(c => c.Chara).Returns(CharaWithTotal.Total);
        _ = mock2.SetupGet(c => c.Cards).Returns(
            Definitions.CardTable.ToDictionary(
                pair => pair.Key,
                pair => CreateSpellCard(
                    pair.Key % 7, pair.Key % 3, pair.Key % 11, pair.Key % 5, pair.Value.Id, pair.Value.Level)));

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
    public void ReplaceTestStoryClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 2", replacer.Replace("%T14CRGSHRB31"));
    }

    [TestMethod]
    public void ReplaceTestStoryTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 3", replacer.Replace("%T14CRGSHRB32"));
    }

    [TestMethod]
    public void ReplaceTestStoryLevelExtraClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 8", replacer.Replace("%T14CRGSXRB31"));
    }

    [TestMethod]
    public void ReplaceTestStoryLevelExtraTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 9", replacer.Replace("%T14CRGSXRB32"));
    }

    [TestMethod]
    public void ReplaceTestStoryLevelTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 9", replacer.Replace("%T14CRGSTRB31"));
    }

    [TestMethod]
    public void ReplaceTestStoryLevelTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 11", replacer.Replace("%T14CRGSTRB32"));
    }

    [TestMethod]
    public void ReplaceTestStoryCharaTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 3", replacer.Replace("%T14CRGSHTL31"));
    }

    [TestMethod]
    public void ReplaceTestStoryCharaTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 4", replacer.Replace("%T14CRGSHTL32"));
    }

    [TestMethod]
    public void ReplaceTestStoryStageTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 19", replacer.Replace("%T14CRGSHRB01"));
    }

    [TestMethod]
    public void ReplaceTestStoryStageTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 22", replacer.Replace("%T14CRGSHRB02"));
    }

    [TestMethod]
    public void ReplaceTestStoryTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 103", replacer.Replace("%T14CRGSTTL01"));
    }

    [TestMethod]
    public void ReplaceTestStoryTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 110", replacer.Replace("%T14CRGSTTL02"));
    }

    [TestMethod]
    public void ReplaceTestPracticeClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 3", replacer.Replace("%T14CRGPHRB31"));
    }

    [TestMethod]
    public void ReplaceTestPracticeTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 4", replacer.Replace("%T14CRGPHRB32"));
    }

    [TestMethod]
    public void ReplaceTestPracticeLevelExtraClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 10", replacer.Replace("%T14CRGPXRB31"));
    }

    [TestMethod]
    public void ReplaceTestPracticeLevelExtraTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 11", replacer.Replace("%T14CRGPXRB32"));
    }

    [TestMethod]
    public void ReplaceTestPracticeLevelTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 12", replacer.Replace("%T14CRGPTRB31"));
    }

    [TestMethod]
    public void ReplaceTestPracticeLevelTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 13", replacer.Replace("%T14CRGPTRB32"));
    }

    [TestMethod]
    public void ReplaceTestPracticeCharaTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 2", replacer.Replace("%T14CRGPHTL31"));
    }

    [TestMethod]
    public void ReplaceTestPracticeCharaTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 3", replacer.Replace("%T14CRGPHTL32"));
    }

    [TestMethod]
    public void ReplaceTestPracticeStageTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 24", replacer.Replace("%T14CRGPHRB01"));
    }

    [TestMethod]
    public void ReplaceTestPracticeStageTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 26", replacer.Replace("%T14CRGPHRB02"));
    }

    [TestMethod]
    public void ReplaceTestPracticeTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 80", replacer.Replace("%T14CRGPTTL01"));
    }

    [TestMethod]
    public void ReplaceTestPracticeTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 96", replacer.Replace("%T14CRGPTTL02"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(dictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T14CRGSHRB31"));
    }

    [TestMethod]
    public void ReplaceTestEmptyCards()
    {
        var dictionary = new[]
        {
            Mock.Of<IClearData>(
                m => (m.Chara == CharaWithTotal.ReimuB) && (m.Cards == ImmutableDictionary<int, ISpellCard>.Empty))
        }.ToDictionary(clearData => clearData.Chara);
        var formatterMock = MockNumberFormatter();

        var replacer = new CollectRateReplacer(dictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T14CRGSHRB31"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T14XXXSHRB31", replacer.Replace("%T14XXXSHRB31"));
    }

    [TestMethod]
    public void ReplaceTestInvalidGameMode()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T14CRGXHRB31", replacer.Replace("%T14CRGXHRB31"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T14CRGSYRB31", replacer.Replace("%T14CRGSYRB31"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T14CRGSHXX31", replacer.Replace("%T14CRGSHXX31"));
    }

    [TestMethod]
    public void ReplaceTestInvalidStage()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T14CRGSHRBX1", replacer.Replace("%T14CRGSHRBX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T14CRGSHRB3X", replacer.Replace("%T14CRGSHRB3X"));
    }
}

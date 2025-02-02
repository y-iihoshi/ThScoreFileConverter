using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Models.Th18;
using ThScoreFileConverter.Models.Th18;
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
public class CardReplacerTests
{
    private static IClearData[] CreateClearDataList()
    {
        static ISpellCard MockSpellCard(int index)
        {
            var mock = Substitute.For<ISpellCard>();
            _ = mock.HasTried.Returns(index % 2 != 0);
            return mock;
        }

        static int[] GetCardIndices()
        {
            return [1, 2];
        }

        var cards = GetCardIndices().ToDictionary(index => index, MockSpellCard);
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.Total);
        _ = clearData.Cards.Returns(cards);
        return [clearData];
    }

    internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
        CreateClearDataList().ToDictionary(clearData => clearData.Chara);

    [TestMethod]
    public void CardReplacerTest()
    {
        var replacer = new CardReplacer(ClearDataDictionary, true);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void CardReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var replacer = new CardReplacer(dictionary, true);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTestName()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        replacer.Replace("%T18CARD01N").ShouldBe("招符「弾幕万来」");
        replacer.Replace("%T18CARD02N").ShouldBe("招符「弾幕万来」");
    }

    [TestMethod]
    public void ReplaceTestRank()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        replacer.Replace("%T18CARD01R").ShouldBe("Easy");
        replacer.Replace("%T18CARD02R").ShouldBe("Normal");
    }

    [TestMethod]
    public void ReplaceTestHiddenName()
    {
        var replacer = new CardReplacer(ClearDataDictionary, true);
        replacer.Replace("%T18CARD01N").ShouldBe("招符「弾幕万来」");
        replacer.Replace("%T18CARD02N").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestHiddenRank()
    {
        var replacer = new CardReplacer(ClearDataDictionary, true);
        replacer.Replace("%T18CARD01R").ShouldBe("Easy");
        replacer.Replace("%T18CARD02R").ShouldBe("Normal");
    }

    [TestMethod]
    public void ReplaceTestEmptyCards()
    {
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.Total);
        _ = clearData.Cards.Returns(ImmutableDictionary<int, ISpellCard>.Empty);
        var dictionary = new[] { clearData }.ToDictionary(data => data.Chara);

        var replacer = new CardReplacer(dictionary, true);
        replacer.Replace("%T18CARD01N").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new CardReplacer(ClearDataDictionary, true);
        replacer.Replace("%T18XXXX01N").ShouldBe("%T18XXXX01N");
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var replacer = new CardReplacer(ClearDataDictionary, true);
        replacer.Replace("%T18CARD98N").ShouldBe("%T18CARD98N");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var replacer = new CardReplacer(ClearDataDictionary, true);
        replacer.Replace("%T18CARD01X").ShouldBe("%T18CARD01X");
    }
}

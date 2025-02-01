using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Models.Th16;
using ThScoreFileConverter.Models.Th16;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Core.Models.Th16.CharaWithTotal,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Core.Models.Th14.StagePractice,
    ThScoreFileConverter.Models.Th16.IScoreData>;
using ISpellCard = ThScoreFileConverter.Models.Th13.ISpellCard<ThScoreFileConverter.Core.Models.Level>;

namespace ThScoreFileConverter.Tests.Models.Th16;

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
        var replacer = new CardReplacer(ClearDataDictionary, false);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void CardReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var replacer = new CardReplacer(dictionary, false);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTestName()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        replacer.Replace("%T16CARD001N").ShouldBe("蝶符「ミニットスケールス」");
        replacer.Replace("%T16CARD002N").ShouldBe("蝶符「ミニットスケールス」");
    }

    [TestMethod]
    public void ReplaceTestRank()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        replacer.Replace("%T16CARD001R").ShouldBe("Easy");
        replacer.Replace("%T16CARD002R").ShouldBe("Normal");
    }

    [TestMethod]
    public void ReplaceTestHiddenName()
    {
        var replacer = new CardReplacer(ClearDataDictionary, true);
        replacer.Replace("%T16CARD001N").ShouldBe("蝶符「ミニットスケールス」");
        replacer.Replace("%T16CARD002N").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestHiddenRank()
    {
        var replacer = new CardReplacer(ClearDataDictionary, true);
        replacer.Replace("%T16CARD001R").ShouldBe("Easy");
        replacer.Replace("%T16CARD002R").ShouldBe("Normal");
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;

        var replacer = new CardReplacer(dictionary, true);
        replacer.Replace("%T16CARD001N").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestEmptyCards()
    {
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.Total);
        _ = clearData.Cards.Returns(ImmutableDictionary<int, ISpellCard>.Empty);
        var dictionary = new[] { clearData }.ToDictionary(data => data.Chara);

        var replacer = new CardReplacer(dictionary, true);
        replacer.Replace("%T16CARD001N").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        replacer.Replace("%T16XXXX001N").ShouldBe("%T16XXXX001N");
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        replacer.Replace("%T16CARD121N").ShouldBe("%T16CARD121N");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        replacer.Replace("%T16CARD001X").ShouldBe("%T16CARD001X");
    }
}

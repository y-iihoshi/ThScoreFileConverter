using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Models.Th12;
using ThScoreFileConverter.Models.Th12;
using IClearData = ThScoreFileConverter.Models.Th10.IClearData<ThScoreFileConverter.Core.Models.Th12.CharaWithTotal>;
using ISpellCard = ThScoreFileConverter.Models.Th10.ISpellCard<ThScoreFileConverter.Core.Models.Level>;

namespace ThScoreFileConverter.Tests.Models.Th12;

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
            return [3, 4];
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
        replacer.Replace("%T12CARD003N").ShouldBe("捜符「レアメタルディテクター」");
        replacer.Replace("%T12CARD004N").ShouldBe("捜符「レアメタルディテクター」");
    }

    [TestMethod]
    public void ReplaceTestRank()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        replacer.Replace("%T12CARD003R").ShouldBe("Easy");
        replacer.Replace("%T12CARD004R").ShouldBe("Normal");
    }

    [TestMethod]
    public void ReplaceTestHiddenName()
    {
        var replacer = new CardReplacer(ClearDataDictionary, true);
        replacer.Replace("%T12CARD003N").ShouldBe("捜符「レアメタルディテクター」");
        replacer.Replace("%T12CARD004N").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestHiddenRank()
    {
        var replacer = new CardReplacer(ClearDataDictionary, true);
        replacer.Replace("%T12CARD003R").ShouldBe("Easy");
        replacer.Replace("%T12CARD004R").ShouldBe("Normal");
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;

        var replacer = new CardReplacer(dictionary, true);
        replacer.Replace("%T12CARD003N").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestEmptyCards()
    {
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.Total);
        _ = clearData.Cards.Returns(ImmutableDictionary<int, ISpellCard>.Empty);
        var dictionary = new[] { clearData }.ToDictionary(data => data.Chara);

        var replacer = new CardReplacer(dictionary, true);
        replacer.Replace("%T12CARD003N").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        replacer.Replace("%T12XXXX003N").ShouldBe("%T12XXXX003N");
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        replacer.Replace("%T12CARD114N").ShouldBe("%T12CARD114N");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        replacer.Replace("%T12CARD003X").ShouldBe("%T12CARD003X");
    }
}

using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Models.Th123;
using ThScoreFileConverter.Models.Th123;
using ICardForDeck = ThScoreFileConverter.Models.Th105.ICardForDeck;
using IClearData = ThScoreFileConverter.Models.Th105.IClearData<ThScoreFileConverter.Core.Models.Th123.Chara>;

namespace ThScoreFileConverter.Tests.Models.Th123;

[TestClass]
public class CardForDeckReplacerTests
{
    private static IClearData MockClearData()
    {
        var cardsForDeck = new[]
        {
            Th105.CardForDeckTests.MockCardForDeck(103, 34),
            Th105.CardForDeckTests.MockCardForDeck(107, 0),
            Th105.CardForDeckTests.MockCardForDeck(208, 56),
            Th105.CardForDeckTests.MockCardForDeck(205, 0),
        }.ToDictionary(card => card.Id);

        var mock = Substitute.For<IClearData>();
        _ = mock.CardsForDeck.Returns(cardsForDeck);
        return mock;
    }

    internal static IReadOnlyDictionary<int, ICardForDeck> SystemCards { get; } = new[]
    {
        Th105.CardForDeckTests.MockCardForDeck(0, 12),
        Th105.CardForDeckTests.MockCardForDeck(1, 0),
    }.ToDictionary(card => card.Id);

    internal static IReadOnlyDictionary<Chara, IClearData> ClearDataDictionary { get; } =
        new[] { (Chara.Marisa, MockClearData()) }.ToDictionary();

    [TestMethod]
    public void CardForDeckReplacerTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void CardForDeckReplacerTestEmptySystemCards()
    {
        var cards = ImmutableDictionary<int, ICardForDeck>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(cards, ClearDataDictionary, formatterMock, false);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void CardForDeckReplacerTestEmptyClearDataDictionary()
    {
        var dictionary = ImmutableDictionary<Chara, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, dictionary, formatterMock, false);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTestSystemCardName()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        replacer.Replace("%T123DCMRY01N").ShouldBe("「霊撃札」");
        replacer.Replace("%T123DCMRY02N").ShouldBe("「マジックポーション」");
    }

    [TestMethod]
    public void ReplaceTestSystemCardCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        replacer.Replace("%T123DCMRY01C").ShouldBe("invoked: 12");
        replacer.Replace("%T123DCMRY02C").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestSkillCardName()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        replacer.Replace("%T123DCMRK01N").ShouldBe("メテオニックデブリ");
        replacer.Replace("%T123DCMRK02N").ShouldBe("ナロースパーク");
    }

    [TestMethod]
    public void ReplaceTestSkillCardCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        replacer.Replace("%T123DCMRK01C").ShouldBe("invoked: 34");
        replacer.Replace("%T123DCMRK02C").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestSpellCardName()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        replacer.Replace("%T123DCMRP01N").ShouldBe("星符「メテオニックシャワー」");
        replacer.Replace("%T123DCMRP02N").ShouldBe("魔符「スターダストレヴァリエ」");
    }

    [TestMethod]
    public void ReplaceTestSpellCardCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        replacer.Replace("%T123DCMRP01C").ShouldBe("invoked: 56");
        replacer.Replace("%T123DCMRP02C").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestUntriedSystemCardName()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, true);
        replacer.Replace("%T123DCMRY01N").ShouldBe("「霊撃札」");
        replacer.Replace("%T123DCMRY02N").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestUntriedSystemCardCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, true);
        replacer.Replace("%T123DCMRY01C").ShouldBe("invoked: 12");
        replacer.Replace("%T123DCMRY02C").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestUntriedSkillCardName()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, true);
        replacer.Replace("%T123DCMRK01N").ShouldBe("メテオニックデブリ");
        replacer.Replace("%T123DCMRK02N").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestUntriedSkillCardCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, true);
        replacer.Replace("%T123DCMRK01C").ShouldBe("invoked: 34");
        replacer.Replace("%T123DCMRK02C").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestUntriedSpellCardName()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, true);
        replacer.Replace("%T123DCMRP01N").ShouldBe("星符「メテオニックシャワー」");
        replacer.Replace("%T123DCMRP02N").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestUntriedSpellCardCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, true);
        replacer.Replace("%T123DCMRP01C").ShouldBe("invoked: 56");
        replacer.Replace("%T123DCMRP02C").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestEmptySystemCards()
    {
        var cards = ImmutableDictionary<int, ICardForDeck>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(cards, ClearDataDictionary, formatterMock, true);
        replacer.Replace("%T123DCMRY01N").ShouldBe("??????????");
        replacer.Replace("%T123DCMRY01C").ShouldBe("invoked: 0");
        replacer.Replace("%T123DCMRK01N").ShouldBe("メテオニックデブリ");
        replacer.Replace("%T123DCMRK01C").ShouldBe("invoked: 34");
        replacer.Replace("%T123DCMRP01N").ShouldBe("星符「メテオニックシャワー」");
        replacer.Replace("%T123DCMRP01C").ShouldBe("invoked: 56");
    }

    [TestMethod]
    public void ReplaceTestEmptyClearDataDictionary()
    {
        var dictionary = ImmutableDictionary<Chara, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, dictionary, formatterMock, true);
        replacer.Replace("%T123DCMRY01N").ShouldBe("「霊撃札」");
        replacer.Replace("%T123DCMRY01C").ShouldBe("invoked: 12");
        replacer.Replace("%T123DCMRK01N").ShouldBe("??????????");
        replacer.Replace("%T123DCMRK01C").ShouldBe("invoked: 0");
        replacer.Replace("%T123DCMRP01N").ShouldBe("??????????");
        replacer.Replace("%T123DCMRP01C").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestOonamazu()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        replacer.Replace("%T123DCNMY01N").ShouldBe("%T123DCNMY01N");
    }

    [TestMethod]
    public void ReplaceTestNonexistentSystemCard()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        replacer.Replace("%T123DCMRY22N").ShouldBe("%T123DCMRY22N");
    }

    [TestMethod]
    public void ReplaceTestNonexistentSkillCard()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        replacer.Replace("%T123DCMRK13N").ShouldBe("%T123DCMRK13N");
    }

    [TestMethod]
    public void ReplaceTestNonexistentSpellCard()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        replacer.Replace("%T123DCMRP15N").ShouldBe("%T123DCMRP15N");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        replacer.Replace("%T123XXMRY01N").ShouldBe("%T123XXMRY01N");
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        replacer.Replace("%T123DCXXY01N").ShouldBe("%T123DCXXY01N");
    }

    [TestMethod]
    public void ReplaceTestInvalidCardType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        replacer.Replace("%T123DCRMX01N").ShouldBe("%T123DCRMX01N");
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        replacer.Replace("%T123DCMRY22N").ShouldBe("%T123DCMRY22N");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        replacer.Replace("%T123DCMRY01X").ShouldBe("%T123DCMRY01X");
    }
}

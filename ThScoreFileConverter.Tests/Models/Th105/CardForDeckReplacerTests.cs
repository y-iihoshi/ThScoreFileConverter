using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Models.Th105;
using ThScoreFileConverter.Models.Th105;

namespace ThScoreFileConverter.Tests.Models.Th105;

[TestClass]
public class CardForDeckReplacerTests
{
    private static IClearData<Chara> MockClearData()
    {
        var cardsForDeck = new[]
        {
            CardForDeckTests.MockCardForDeck(100, 34),
            CardForDeckTests.MockCardForDeck(101, 0),
            CardForDeckTests.MockCardForDeck(200, 56),
            CardForDeckTests.MockCardForDeck(202, 0),
        }.ToDictionary(card => card.Id);

        var mock = Substitute.For<IClearData<Chara>>();
        _ = mock.CardsForDeck.Returns(cardsForDeck);
        return mock;
    }

    internal static IReadOnlyDictionary<int, ICardForDeck> SystemCards { get; } = new[]
    {
        CardForDeckTests.MockCardForDeck(0, 12),
        CardForDeckTests.MockCardForDeck(1, 0),
    }.ToDictionary(card => card.Id);

    internal static IReadOnlyDictionary<Chara, IClearData<Chara>> ClearDataDictionary { get; } =
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
        var dictionary = ImmutableDictionary<Chara, IClearData<Chara>>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, dictionary, formatterMock, false);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTestSystemCardName()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        replacer.Replace("%T105DCMRY01N").ShouldBe("「気質発現」");
        replacer.Replace("%T105DCMRY02N").ShouldBe("「霊撃」");
    }

    [TestMethod]
    public void ReplaceTestSystemCardCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        replacer.Replace("%T105DCMRY01C").ShouldBe("invoked: 12");
        replacer.Replace("%T105DCMRY02C").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestSkillCardName()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        replacer.Replace("%T105DCMRK01N").ShouldBe("ウィッチレイライン");
        replacer.Replace("%T105DCMRK02N").ShouldBe("ミアズマスウィープ");
    }

    [TestMethod]
    public void ReplaceTestSkillCardCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        replacer.Replace("%T105DCMRK01C").ShouldBe("invoked: 34");
        replacer.Replace("%T105DCMRK02C").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestSpellCardName()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        replacer.Replace("%T105DCMRP01N").ShouldBe("恋符「マスタースパーク」");
        replacer.Replace("%T105DCMRP02N").ShouldBe("魔砲「ファイナルスパーク」");
    }

    [TestMethod]
    public void ReplaceTestSpellCardCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        replacer.Replace("%T105DCMRP01C").ShouldBe("invoked: 56");
        replacer.Replace("%T105DCMRP02C").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestUntriedSystemCardName()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, true);
        replacer.Replace("%T105DCMRY01N").ShouldBe("「気質発現」");
        replacer.Replace("%T105DCMRY02N").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestUntriedSystemCardCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, true);
        replacer.Replace("%T105DCMRY01C").ShouldBe("invoked: 12");
        replacer.Replace("%T105DCMRY02C").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestUntriedSkillCardName()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, true);
        replacer.Replace("%T105DCMRK01N").ShouldBe("ウィッチレイライン");
        replacer.Replace("%T105DCMRK02N").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestUntriedSkillCardCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, true);
        replacer.Replace("%T105DCMRK01C").ShouldBe("invoked: 34");
        replacer.Replace("%T105DCMRK02C").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestUntriedSpellCardName()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, true);
        replacer.Replace("%T105DCMRP01N").ShouldBe("恋符「マスタースパーク」");
        replacer.Replace("%T105DCMRP02N").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestUntriedSpellCardCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, true);
        replacer.Replace("%T105DCMRP01C").ShouldBe("invoked: 56");
        replacer.Replace("%T105DCMRP02C").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestEmptySystemCards()
    {
        var cards = ImmutableDictionary<int, ICardForDeck>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(cards, ClearDataDictionary, formatterMock, true);
        replacer.Replace("%T105DCMRY01N").ShouldBe("??????????");
        replacer.Replace("%T105DCMRY01C").ShouldBe("invoked: 0");
        replacer.Replace("%T105DCMRK01N").ShouldBe("ウィッチレイライン");
        replacer.Replace("%T105DCMRK01C").ShouldBe("invoked: 34");
        replacer.Replace("%T105DCMRP01N").ShouldBe("恋符「マスタースパーク」");
        replacer.Replace("%T105DCMRP01C").ShouldBe("invoked: 56");
    }

    [TestMethod]
    public void ReplaceTestEmptyClearDataDictionary()
    {
        var dictionary = ImmutableDictionary<Chara, IClearData<Chara>>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, dictionary, formatterMock, true);
        replacer.Replace("%T105DCMRY01N").ShouldBe("「気質発現」");
        replacer.Replace("%T105DCMRY01C").ShouldBe("invoked: 12");
        replacer.Replace("%T105DCMRK01N").ShouldBe("??????????");
        replacer.Replace("%T105DCMRK01C").ShouldBe("invoked: 0");
        replacer.Replace("%T105DCMRP01N").ShouldBe("??????????");
        replacer.Replace("%T105DCMRP01C").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestNonexistentSystemCard()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        replacer.Replace("%T105DCMRY07N").ShouldBe("%T105DCMRY07N");
    }

    [TestMethod]
    public void ReplaceTestNonexistentSkillCard()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        replacer.Replace("%T105DCMRK12N").ShouldBe("%T105DCMRK12N");
    }

    [TestMethod]
    public void ReplaceTestNonexistentSpellCard()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        replacer.Replace("%T105DCMRP11N").ShouldBe("%T105DCMRP11N");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        replacer.Replace("%T105XXMRY01N").ShouldBe("%T105XXMRY01N");
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        replacer.Replace("%T105DCXXY01N").ShouldBe("%T105DCXXY01N");
    }

    [TestMethod]
    public void ReplaceTestInvalidCardType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        replacer.Replace("%T105DCRMX01N").ShouldBe("%T105DCRMX01N");
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        replacer.Replace("%T105DCMRY12N").ShouldBe("%T105DCMRY12N");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        replacer.Replace("%T105DCMRY01X").ShouldBe("%T105DCMRY01X");
    }
}

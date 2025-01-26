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
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CardForDeckReplacerTestEmptySystemCards()
    {
        var cards = ImmutableDictionary<int, ICardForDeck>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(cards, ClearDataDictionary, formatterMock, false);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CardForDeckReplacerTestEmptyClearDataDictionary()
    {
        var dictionary = ImmutableDictionary<Chara, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, dictionary, formatterMock, false);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestSystemCardName()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        Assert.AreEqual("「霊撃札」", replacer.Replace("%T123DCMRY01N"));
        Assert.AreEqual("「マジックポーション」", replacer.Replace("%T123DCMRY02N"));
    }

    [TestMethod]
    public void ReplaceTestSystemCardCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        Assert.AreEqual("invoked: 12", replacer.Replace("%T123DCMRY01C"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T123DCMRY02C"));
    }

    [TestMethod]
    public void ReplaceTestSkillCardName()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        Assert.AreEqual("メテオニックデブリ", replacer.Replace("%T123DCMRK01N"));
        Assert.AreEqual("ナロースパーク", replacer.Replace("%T123DCMRK02N"));
    }

    [TestMethod]
    public void ReplaceTestSkillCardCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        Assert.AreEqual("invoked: 34", replacer.Replace("%T123DCMRK01C"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T123DCMRK02C"));
    }

    [TestMethod]
    public void ReplaceTestSpellCardName()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        Assert.AreEqual("星符「メテオニックシャワー」", replacer.Replace("%T123DCMRP01N"));
        Assert.AreEqual("魔符「スターダストレヴァリエ」", replacer.Replace("%T123DCMRP02N"));
    }

    [TestMethod]
    public void ReplaceTestSpellCardCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        Assert.AreEqual("invoked: 56", replacer.Replace("%T123DCMRP01C"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T123DCMRP02C"));
    }

    [TestMethod]
    public void ReplaceTestUntriedSystemCardName()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, true);
        Assert.AreEqual("「霊撃札」", replacer.Replace("%T123DCMRY01N"));
        Assert.AreEqual("??????????", replacer.Replace("%T123DCMRY02N"));
    }

    [TestMethod]
    public void ReplaceTestUntriedSystemCardCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, true);
        Assert.AreEqual("invoked: 12", replacer.Replace("%T123DCMRY01C"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T123DCMRY02C"));
    }

    [TestMethod]
    public void ReplaceTestUntriedSkillCardName()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, true);
        Assert.AreEqual("メテオニックデブリ", replacer.Replace("%T123DCMRK01N"));
        Assert.AreEqual("??????????", replacer.Replace("%T123DCMRK02N"));
    }

    [TestMethod]
    public void ReplaceTestUntriedSkillCardCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, true);
        Assert.AreEqual("invoked: 34", replacer.Replace("%T123DCMRK01C"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T123DCMRK02C"));
    }

    [TestMethod]
    public void ReplaceTestUntriedSpellCardName()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, true);
        Assert.AreEqual("星符「メテオニックシャワー」", replacer.Replace("%T123DCMRP01N"));
        Assert.AreEqual("??????????", replacer.Replace("%T123DCMRP02N"));
    }

    [TestMethod]
    public void ReplaceTestUntriedSpellCardCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, true);
        Assert.AreEqual("invoked: 56", replacer.Replace("%T123DCMRP01C"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T123DCMRP02C"));
    }

    [TestMethod]
    public void ReplaceTestEmptySystemCards()
    {
        var cards = ImmutableDictionary<int, ICardForDeck>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(cards, ClearDataDictionary, formatterMock, true);
        Assert.AreEqual("??????????", replacer.Replace("%T123DCMRY01N"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T123DCMRY01C"));
        Assert.AreEqual("メテオニックデブリ", replacer.Replace("%T123DCMRK01N"));
        Assert.AreEqual("invoked: 34", replacer.Replace("%T123DCMRK01C"));
        Assert.AreEqual("星符「メテオニックシャワー」", replacer.Replace("%T123DCMRP01N"));
        Assert.AreEqual("invoked: 56", replacer.Replace("%T123DCMRP01C"));
    }

    [TestMethod]
    public void ReplaceTestEmptyClearDataDictionary()
    {
        var dictionary = ImmutableDictionary<Chara, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, dictionary, formatterMock, true);
        Assert.AreEqual("「霊撃札」", replacer.Replace("%T123DCMRY01N"));
        Assert.AreEqual("invoked: 12", replacer.Replace("%T123DCMRY01C"));
        Assert.AreEqual("??????????", replacer.Replace("%T123DCMRK01N"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T123DCMRK01C"));
        Assert.AreEqual("??????????", replacer.Replace("%T123DCMRP01N"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T123DCMRP01C"));
    }

    [TestMethod]
    public void ReplaceTestOonamazu()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        Assert.AreEqual("%T123DCNMY01N", replacer.Replace("%T123DCNMY01N"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentSystemCard()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        Assert.AreEqual("%T123DCMRY22N", replacer.Replace("%T123DCMRY22N"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentSkillCard()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        Assert.AreEqual("%T123DCMRK13N", replacer.Replace("%T123DCMRK13N"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentSpellCard()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        Assert.AreEqual("%T123DCMRP15N", replacer.Replace("%T123DCMRP15N"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        Assert.AreEqual("%T123XXMRY01N", replacer.Replace("%T123XXMRY01N"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        Assert.AreEqual("%T123DCXXY01N", replacer.Replace("%T123DCXXY01N"));
    }

    [TestMethod]
    public void ReplaceTestInvalidCardType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        Assert.AreEqual("%T123DCRMX01N", replacer.Replace("%T123DCRMX01N"));
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        Assert.AreEqual("%T123DCMRY22N", replacer.Replace("%T123DCMRY22N"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        Assert.AreEqual("%T123DCMRY01X", replacer.Replace("%T123DCMRY01X"));
    }
}

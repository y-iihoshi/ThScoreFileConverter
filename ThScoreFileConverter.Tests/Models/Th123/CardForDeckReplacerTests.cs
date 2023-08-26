using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Models.Th123;
using ThScoreFileConverter.Models;
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

    private static INumberFormatter MockNumberFormatter()
    {
        // NOTE: NSubstitute v5.0.0 has no substitute for Moq's It.IsAny<It.IsValueType>.
        var mock = Substitute.For<INumberFormatter>();
        _ = mock.FormatNumber(Arg.Any<int>()).Returns(callInfo => $"invoked: {(int)callInfo[0]}");
        return mock;
    }

    [TestMethod]
    public void CardForDeckReplacerTest()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CardForDeckReplacerTestEmptySystemCards()
    {
        var cards = ImmutableDictionary<int, ICardForDeck>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CardForDeckReplacer(cards, ClearDataDictionary, formatterMock, false);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CardForDeckReplacerTestEmptyClearDataDictionary()
    {
        var dictionary = ImmutableDictionary<Chara, IClearData>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CardForDeckReplacer(SystemCards, dictionary, formatterMock, false);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestSystemCardName()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        Assert.AreEqual("「霊撃札」", replacer.Replace("%T123DCMRY01N"));
        Assert.AreEqual("「マジックポーション」", replacer.Replace("%T123DCMRY02N"));
    }

    [TestMethod]
    public void ReplaceTestSystemCardCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        Assert.AreEqual("invoked: 12", replacer.Replace("%T123DCMRY01C"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T123DCMRY02C"));
    }

    [TestMethod]
    public void ReplaceTestSkillCardName()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        Assert.AreEqual("メテオニックデブリ", replacer.Replace("%T123DCMRK01N"));
        Assert.AreEqual("ナロースパーク", replacer.Replace("%T123DCMRK02N"));
    }

    [TestMethod]
    public void ReplaceTestSkillCardCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        Assert.AreEqual("invoked: 34", replacer.Replace("%T123DCMRK01C"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T123DCMRK02C"));
    }

    [TestMethod]
    public void ReplaceTestSpellCardName()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        Assert.AreEqual("星符「メテオニックシャワー」", replacer.Replace("%T123DCMRP01N"));
        Assert.AreEqual("魔符「スターダストレヴァリエ」", replacer.Replace("%T123DCMRP02N"));
    }

    [TestMethod]
    public void ReplaceTestSpellCardCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        Assert.AreEqual("invoked: 56", replacer.Replace("%T123DCMRP01C"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T123DCMRP02C"));
    }

    [TestMethod]
    public void ReplaceTestUntriedSystemCardName()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, true);
        Assert.AreEqual("「霊撃札」", replacer.Replace("%T123DCMRY01N"));
        Assert.AreEqual("??????????", replacer.Replace("%T123DCMRY02N"));
    }

    [TestMethod]
    public void ReplaceTestUntriedSystemCardCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, true);
        Assert.AreEqual("invoked: 12", replacer.Replace("%T123DCMRY01C"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T123DCMRY02C"));
    }

    [TestMethod]
    public void ReplaceTestUntriedSkillCardName()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, true);
        Assert.AreEqual("メテオニックデブリ", replacer.Replace("%T123DCMRK01N"));
        Assert.AreEqual("??????????", replacer.Replace("%T123DCMRK02N"));
    }

    [TestMethod]
    public void ReplaceTestUntriedSkillCardCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, true);
        Assert.AreEqual("invoked: 34", replacer.Replace("%T123DCMRK01C"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T123DCMRK02C"));
    }

    [TestMethod]
    public void ReplaceTestUntriedSpellCardName()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, true);
        Assert.AreEqual("星符「メテオニックシャワー」", replacer.Replace("%T123DCMRP01N"));
        Assert.AreEqual("??????????", replacer.Replace("%T123DCMRP02N"));
    }

    [TestMethod]
    public void ReplaceTestUntriedSpellCardCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, true);
        Assert.AreEqual("invoked: 56", replacer.Replace("%T123DCMRP01C"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T123DCMRP02C"));
    }

    [TestMethod]
    public void ReplaceTestEmptySystemCards()
    {
        var cards = ImmutableDictionary<int, ICardForDeck>.Empty;
        var formatterMock = MockNumberFormatter();
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
        var formatterMock = MockNumberFormatter();
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
        var formatterMock = MockNumberFormatter();
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        Assert.AreEqual("%T123DCNMY01N", replacer.Replace("%T123DCNMY01N"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentSystemCard()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        Assert.AreEqual("%T123DCMRY22N", replacer.Replace("%T123DCMRY22N"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentSkillCard()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        Assert.AreEqual("%T123DCMRK13N", replacer.Replace("%T123DCMRK13N"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentSpellCard()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        Assert.AreEqual("%T123DCMRP15N", replacer.Replace("%T123DCMRP15N"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        Assert.AreEqual("%T123XXMRY01N", replacer.Replace("%T123XXMRY01N"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        Assert.AreEqual("%T123DCXXY01N", replacer.Replace("%T123DCXXY01N"));
    }

    [TestMethod]
    public void ReplaceTestInvalidCardType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        Assert.AreEqual("%T123DCRMX01N", replacer.Replace("%T123DCRMX01N"));
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        Assert.AreEqual("%T123DCMRY22N", replacer.Replace("%T123DCMRY22N"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock, false);
        Assert.AreEqual("%T123DCMRY01X", replacer.Replace("%T123DCMRY01X"));
    }
}

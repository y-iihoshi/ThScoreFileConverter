using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th105;

namespace ThScoreFileConverterTests.Models.Th105
{
    [TestClass]
    public class CardForDeckReplacerTests
    {
        internal static IReadOnlyDictionary<int, ICardForDeck> SystemCards { get; } = new[]
        {
            Mock.Of<ICardForDeck>(m => (m.Id == 0) && (m.MaxNumber == 12)),
            Mock.Of<ICardForDeck>(m => (m.Id == 1) && (m.MaxNumber == 0)),
        }.ToDictionary(card => card.Id);

        internal static IReadOnlyDictionary<Chara, IClearData<Chara>> ClearDataDictionary { get; } =
            new Dictionary<Chara, IClearData<Chara>>
            {
                {
                    Chara.Marisa,
                    Mock.Of<IClearData<Chara>>(
                        m => m.CardsForDeck == new[]
                        {
                            Mock.Of<ICardForDeck>(c => (c.Id == 100) && (c.MaxNumber == 34)),
                            Mock.Of<ICardForDeck>(c => (c.Id == 101) && (c.MaxNumber == 0)),
                            Mock.Of<ICardForDeck>(c => (c.Id == 200) && (c.MaxNumber == 56)),
                            Mock.Of<ICardForDeck>(c => (c.Id == 202) && (c.MaxNumber == 0)),
                        }.ToDictionary(card => card.Id))
                },
            };

        private static Mock<INumberFormatter> MockNumberFormatter()
        {
            var mock = new Mock<INumberFormatter>();
            _ = mock.Setup(formatter => formatter.FormatNumber(It.IsAny<It.IsValueType>()))
                .Returns((object value) => "invoked: " + value.ToString());
            return mock;
        }

        [TestMethod]
        public void CardForDeckReplacerTest()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock.Object, false);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void CardForDeckReplacerTestEmptySystemCards()
        {
            var cards = ImmutableDictionary<int, ICardForDeck>.Empty;
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(cards, ClearDataDictionary, formatterMock.Object, false);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void CardForDeckReplacerTestEmptyClearDataDictionary()
        {
            var dictionary = ImmutableDictionary<Chara, IClearData<Chara>>.Empty;
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, dictionary, formatterMock.Object, false);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestSystemCardName()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock.Object, false);
            Assert.AreEqual("「気質発現」", replacer.Replace("%T105DCMRY01N"));
            Assert.AreEqual("「霊撃」", replacer.Replace("%T105DCMRY02N"));
        }

        [TestMethod]
        public void ReplaceTestSystemCardCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock.Object, false);
            Assert.AreEqual("invoked: 12", replacer.Replace("%T105DCMRY01C"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T105DCMRY02C"));
        }

        [TestMethod]
        public void ReplaceTestSkillCardName()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock.Object, false);
            Assert.AreEqual("ウィッチレイライン", replacer.Replace("%T105DCMRK01N"));
            Assert.AreEqual("ミアズマスウィープ", replacer.Replace("%T105DCMRK02N"));
        }

        [TestMethod]
        public void ReplaceTestSkillCardCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock.Object, false);
            Assert.AreEqual("invoked: 34", replacer.Replace("%T105DCMRK01C"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T105DCMRK02C"));
        }

        [TestMethod]
        public void ReplaceTestSpellCardName()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock.Object, false);
            Assert.AreEqual("恋符「マスタースパーク」", replacer.Replace("%T105DCMRP01N"));
            Assert.AreEqual("魔砲「ファイナルスパーク」", replacer.Replace("%T105DCMRP02N"));
        }

        [TestMethod]
        public void ReplaceTestSpellCardCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock.Object, false);
            Assert.AreEqual("invoked: 56", replacer.Replace("%T105DCMRP01C"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T105DCMRP02C"));
        }

        [TestMethod]
        public void ReplaceTestUntriedSystemCardName()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock.Object, true);
            Assert.AreEqual("「気質発現」", replacer.Replace("%T105DCMRY01N"));
            Assert.AreEqual("??????????", replacer.Replace("%T105DCMRY02N"));
        }

        [TestMethod]
        public void ReplaceTestUntriedSystemCardCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock.Object, true);
            Assert.AreEqual("invoked: 12", replacer.Replace("%T105DCMRY01C"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T105DCMRY02C"));
        }

        [TestMethod]
        public void ReplaceTestUntriedSkillCardName()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock.Object, true);
            Assert.AreEqual("ウィッチレイライン", replacer.Replace("%T105DCMRK01N"));
            Assert.AreEqual("??????????", replacer.Replace("%T105DCMRK02N"));
        }

        [TestMethod]
        public void ReplaceTestUntriedSkillCardCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock.Object, true);
            Assert.AreEqual("invoked: 34", replacer.Replace("%T105DCMRK01C"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T105DCMRK02C"));
        }

        [TestMethod]
        public void ReplaceTestUntriedSpellCardName()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock.Object, true);
            Assert.AreEqual("恋符「マスタースパーク」", replacer.Replace("%T105DCMRP01N"));
            Assert.AreEqual("??????????", replacer.Replace("%T105DCMRP02N"));
        }

        [TestMethod]
        public void ReplaceTestUntriedSpellCardCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock.Object, true);
            Assert.AreEqual("invoked: 56", replacer.Replace("%T105DCMRP01C"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T105DCMRP02C"));
        }

        [TestMethod]
        public void ReplaceTestEmptySystemCards()
        {
            var cards = ImmutableDictionary<int, ICardForDeck>.Empty;
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(cards, ClearDataDictionary, formatterMock.Object, true);
            Assert.AreEqual("??????????", replacer.Replace("%T105DCMRY01N"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T105DCMRY01C"));
            Assert.AreEqual("ウィッチレイライン", replacer.Replace("%T105DCMRK01N"));
            Assert.AreEqual("invoked: 34", replacer.Replace("%T105DCMRK01C"));
            Assert.AreEqual("恋符「マスタースパーク」", replacer.Replace("%T105DCMRP01N"));
            Assert.AreEqual("invoked: 56", replacer.Replace("%T105DCMRP01C"));
        }

        [TestMethod]
        public void ReplaceTestEmptyClearDataDictionary()
        {
            var dictionary = ImmutableDictionary<Chara, IClearData<Chara>>.Empty;
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, dictionary, formatterMock.Object, true);
            Assert.AreEqual("「気質発現」", replacer.Replace("%T105DCMRY01N"));
            Assert.AreEqual("invoked: 12", replacer.Replace("%T105DCMRY01C"));
            Assert.AreEqual("??????????", replacer.Replace("%T105DCMRK01N"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T105DCMRK01C"));
            Assert.AreEqual("??????????", replacer.Replace("%T105DCMRP01N"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T105DCMRP01C"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentSystemCard()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock.Object, false);
            Assert.AreEqual("%T105DCMRY07N", replacer.Replace("%T105DCMRY07N"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentSkillCard()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock.Object, false);
            Assert.AreEqual("%T105DCMRK12N", replacer.Replace("%T105DCMRK12N"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentSpellCard()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock.Object, false);
            Assert.AreEqual("%T105DCMRP11N", replacer.Replace("%T105DCMRP11N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock.Object, false);
            Assert.AreEqual("%T105XXMRY01N", replacer.Replace("%T105XXMRY01N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock.Object, false);
            Assert.AreEqual("%T105DCXXY01N", replacer.Replace("%T105DCXXY01N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidCardType()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock.Object, false);
            Assert.AreEqual("%T105DCRMX01N", replacer.Replace("%T105DCRMX01N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidNumber()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock.Object, false);
            Assert.AreEqual("%T105DCMRY12N", replacer.Replace("%T105DCMRY12N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock.Object, false);
            Assert.AreEqual("%T105DCMRY01X", replacer.Replace("%T105DCMRY01X"));
        }
    }
}

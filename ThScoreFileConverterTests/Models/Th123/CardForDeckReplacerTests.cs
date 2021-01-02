using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th123;
using ICardForDeck = ThScoreFileConverter.Models.Th105.ICardForDeck;
using IClearData = ThScoreFileConverter.Models.Th105.IClearData<ThScoreFileConverter.Models.Th123.Chara>;

namespace ThScoreFileConverterTests.Models.Th123
{
    [TestClass]
    public class CardForDeckReplacerTests
    {
        internal static IReadOnlyDictionary<int, ICardForDeck> SystemCards { get; } = new[]
        {
            Mock.Of<ICardForDeck>(m => (m.Id == 0) && (m.MaxNumber == 12)),
            Mock.Of<ICardForDeck>(m => (m.Id == 1) && (m.MaxNumber == 0)),
        }.ToDictionary(card => card.Id);

        internal static IReadOnlyDictionary<Chara, IClearData> ClearDataDictionary { get; } =
            new Dictionary<Chara, IClearData>
            {
                {
                    Chara.Marisa,
                    Mock.Of<IClearData>(
                        m => m.CardsForDeck == new[]
                        {
                            Mock.Of<ICardForDeck>(c => (c.Id == 103) && (c.MaxNumber == 34)),
                            Mock.Of<ICardForDeck>(c => (c.Id == 107) && (c.MaxNumber == 0)),
                            Mock.Of<ICardForDeck>(c => (c.Id == 208) && (c.MaxNumber == 56)),
                            Mock.Of<ICardForDeck>(c => (c.Id == 205) && (c.MaxNumber == 0)),
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
            var cards = new Dictionary<int, ICardForDeck>();
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(cards, ClearDataDictionary, formatterMock.Object, false);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void CardForDeckReplacerTestEmptyClearDataDictionary()
        {
            var dictionary = new Dictionary<Chara, IClearData>();
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, dictionary, formatterMock.Object, false);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestSystemCardName()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock.Object, false);
            Assert.AreEqual("「霊撃札」", replacer.Replace("%T123DCMRY01N"));
            Assert.AreEqual("「マジックポーション」", replacer.Replace("%T123DCMRY02N"));
        }

        [TestMethod]
        public void ReplaceTestSystemCardCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock.Object, false);
            Assert.AreEqual("invoked: 12", replacer.Replace("%T123DCMRY01C"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T123DCMRY02C"));
        }

        [TestMethod]
        public void ReplaceTestSkillCardName()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock.Object, false);
            Assert.AreEqual("メテオニックデブリ", replacer.Replace("%T123DCMRK01N"));
            Assert.AreEqual("ナロースパーク", replacer.Replace("%T123DCMRK02N"));
        }

        [TestMethod]
        public void ReplaceTestSkillCardCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock.Object, false);
            Assert.AreEqual("invoked: 34", replacer.Replace("%T123DCMRK01C"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T123DCMRK02C"));
        }

        [TestMethod]
        public void ReplaceTestSpellCardName()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock.Object, false);
            Assert.AreEqual("星符「メテオニックシャワー」", replacer.Replace("%T123DCMRP01N"));
            Assert.AreEqual("魔符「スターダストレヴァリエ」", replacer.Replace("%T123DCMRP02N"));
        }

        [TestMethod]
        public void ReplaceTestSpellCardCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock.Object, false);
            Assert.AreEqual("invoked: 56", replacer.Replace("%T123DCMRP01C"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T123DCMRP02C"));
        }

        [TestMethod]
        public void ReplaceTestUntriedSystemCardName()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock.Object, true);
            Assert.AreEqual("「霊撃札」", replacer.Replace("%T123DCMRY01N"));
            Assert.AreEqual("??????????", replacer.Replace("%T123DCMRY02N"));
        }

        [TestMethod]
        public void ReplaceTestUntriedSystemCardCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock.Object, true);
            Assert.AreEqual("invoked: 12", replacer.Replace("%T123DCMRY01C"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T123DCMRY02C"));
        }

        [TestMethod]
        public void ReplaceTestUntriedSkillCardName()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock.Object, true);
            Assert.AreEqual("メテオニックデブリ", replacer.Replace("%T123DCMRK01N"));
            Assert.AreEqual("??????????", replacer.Replace("%T123DCMRK02N"));
        }

        [TestMethod]
        public void ReplaceTestUntriedSkillCardCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock.Object, true);
            Assert.AreEqual("invoked: 34", replacer.Replace("%T123DCMRK01C"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T123DCMRK02C"));
        }

        [TestMethod]
        public void ReplaceTestUntriedSpellCardName()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock.Object, true);
            Assert.AreEqual("星符「メテオニックシャワー」", replacer.Replace("%T123DCMRP01N"));
            Assert.AreEqual("??????????", replacer.Replace("%T123DCMRP02N"));
        }

        [TestMethod]
        public void ReplaceTestUntriedSpellCardCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock.Object, true);
            Assert.AreEqual("invoked: 56", replacer.Replace("%T123DCMRP01C"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T123DCMRP02C"));
        }

        [TestMethod]
        public void ReplaceTestEmptySystemCards()
        {
            var cards = new Dictionary<int, ICardForDeck>();
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(cards, ClearDataDictionary, formatterMock.Object, true);
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
            var dictionary = new Dictionary<Chara, IClearData>();
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, dictionary, formatterMock.Object, true);
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
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock.Object, false);
            Assert.AreEqual("%T123DCNMY01N", replacer.Replace("%T123DCNMY01N"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentSystemCard()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock.Object, false);
            Assert.AreEqual("%T123DCMRY22N", replacer.Replace("%T123DCMRY22N"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentSkillCard()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock.Object, false);
            Assert.AreEqual("%T123DCMRK13N", replacer.Replace("%T123DCMRK13N"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentSpellCard()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock.Object, false);
            Assert.AreEqual("%T123DCMRP15N", replacer.Replace("%T123DCMRP15N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock.Object, false);
            Assert.AreEqual("%T123XXMRY01N", replacer.Replace("%T123XXMRY01N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock.Object, false);
            Assert.AreEqual("%T123DCXXY01N", replacer.Replace("%T123DCXXY01N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidCardType()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock.Object, false);
            Assert.AreEqual("%T123DCRMX01N", replacer.Replace("%T123DCRMX01N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidNumber()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock.Object, false);
            Assert.AreEqual("%T123DCMRY22N", replacer.Replace("%T123DCMRY22N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, formatterMock.Object, false);
            Assert.AreEqual("%T123DCMRY01X", replacer.Replace("%T123DCMRY01X"));
        }
    }
}

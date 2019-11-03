using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th123;
using ThScoreFileConverterTests.Models.Th105.Stubs;
using ICardForDeck = ThScoreFileConverter.Models.Th105.ICardForDeck;
using IClearData = ThScoreFileConverter.Models.Th105.IClearData<ThScoreFileConverter.Models.Th123.Chara>;

namespace ThScoreFileConverterTests.Models.Th123
{
    [TestClass]
    public class CardForDeckReplacerTests
    {
        internal static IReadOnlyDictionary<int, ICardForDeck> SystemCards { get; } =
            new List<ICardForDeck>
            {
                new CardForDeckStub { Id = 0, MaxNumber = 12 },
                new CardForDeckStub { Id = 1, MaxNumber = 0 },
            }.ToDictionary(card => card.Id);

        internal static IReadOnlyDictionary<Chara, IClearData> ClearDataDictionary { get; } =
            new Dictionary<Chara, IClearData>
            {
                {
                    Chara.Marisa,
                    new ClearDataStub<Chara>
                    {
                        CardsForDeck = new List<ICardForDeck>
                        {
                            new CardForDeckStub { Id = 103, MaxNumber = 34, },
                            new CardForDeckStub { Id = 107, MaxNumber = 0, },
                            new CardForDeckStub { Id = 208, MaxNumber = 56, },
                            new CardForDeckStub { Id = 205, MaxNumber = 0, },
                        }
                        .ToDictionary(card => card.Id),
                    }
                },
            };

        [TestMethod]
        public void CardForDeckReplacerTest()
        {
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, false);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CardForDeckReplacerTestNullSystemCards()
        {
            _ = new CardForDeckReplacer(null, ClearDataDictionary, false);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CardForDeckReplacerTestNullClearDataDictionary()
        {
            _ = new CardForDeckReplacer(SystemCards, null, false);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void CardForDeckReplacerTestEmptySystemCards()
        {
            var cards = new Dictionary<int, ICardForDeck>();
            var replacer = new CardForDeckReplacer(cards, ClearDataDictionary, false);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void CardForDeckReplacerTestEmptyClearDataDictionary()
        {
            var dictionary = new Dictionary<Chara, IClearData>();
            var replacer = new CardForDeckReplacer(SystemCards, dictionary, false);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestSystemCardName()
        {
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, false);
            Assert.AreEqual("「霊撃札」", replacer.Replace("%T123DCMRY01N"));
            Assert.AreEqual("「マジックポーション」", replacer.Replace("%T123DCMRY02N"));
        }

        [TestMethod]
        public void ReplaceTestSystemCardCount()
        {
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, false);
            Assert.AreEqual("12", replacer.Replace("%T123DCMRY01C"));
            Assert.AreEqual("0", replacer.Replace("%T123DCMRY02C"));
        }

        [TestMethod]
        public void ReplaceTestSkillCardName()
        {
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, false);
            Assert.AreEqual("メテオニックデブリ", replacer.Replace("%T123DCMRK01N"));
            Assert.AreEqual("ナロースパーク", replacer.Replace("%T123DCMRK02N"));
        }

        [TestMethod]
        public void ReplaceTestSkillCardCount()
        {
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, false);
            Assert.AreEqual("34", replacer.Replace("%T123DCMRK01C"));
            Assert.AreEqual("0", replacer.Replace("%T123DCMRK02C"));
        }

        [TestMethod]
        public void ReplaceTestSpellCardName()
        {
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, false);
            Assert.AreEqual("星符「メテオニックシャワー」", replacer.Replace("%T123DCMRP01N"));
            Assert.AreEqual("魔符「スターダストレヴァリエ」", replacer.Replace("%T123DCMRP02N"));
        }

        [TestMethod]
        public void ReplaceTestSpellCardCount()
        {
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, false);
            Assert.AreEqual("56", replacer.Replace("%T123DCMRP01C"));
            Assert.AreEqual("0", replacer.Replace("%T123DCMRP02C"));
        }

        [TestMethod]
        public void ReplaceTestUntriedSystemCardName()
        {
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, true);
            Assert.AreEqual("「霊撃札」", replacer.Replace("%T123DCMRY01N"));
            Assert.AreEqual("??????????", replacer.Replace("%T123DCMRY02N"));
        }

        [TestMethod]
        public void ReplaceTestUntriedSystemCardCount()
        {
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, true);
            Assert.AreEqual("12", replacer.Replace("%T123DCMRY01C"));
            Assert.AreEqual("0", replacer.Replace("%T123DCMRY02C"));
        }

        [TestMethod]
        public void ReplaceTestUntriedSkillCardName()
        {
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, true);
            Assert.AreEqual("メテオニックデブリ", replacer.Replace("%T123DCMRK01N"));
            Assert.AreEqual("??????????", replacer.Replace("%T123DCMRK02N"));
        }

        [TestMethod]
        public void ReplaceTestUntriedSkillCardCount()
        {
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, true);
            Assert.AreEqual("34", replacer.Replace("%T123DCMRK01C"));
            Assert.AreEqual("0", replacer.Replace("%T123DCMRK02C"));
        }

        [TestMethod]
        public void ReplaceTestUntriedSpellCardName()
        {
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, true);
            Assert.AreEqual("星符「メテオニックシャワー」", replacer.Replace("%T123DCMRP01N"));
            Assert.AreEqual("??????????", replacer.Replace("%T123DCMRP02N"));
        }

        [TestMethod]
        public void ReplaceTestUntriedSpellCardCount()
        {
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, true);
            Assert.AreEqual("56", replacer.Replace("%T123DCMRP01C"));
            Assert.AreEqual("0", replacer.Replace("%T123DCMRP02C"));
        }

        [TestMethod]
        public void ReplaceTestEmptySystemCards()
        {
            var cards = new Dictionary<int, ICardForDeck>();
            var replacer = new CardForDeckReplacer(cards, ClearDataDictionary, true);
            Assert.AreEqual("??????????", replacer.Replace("%T123DCMRY01N"));
            Assert.AreEqual("0", replacer.Replace("%T123DCMRY01C"));
            Assert.AreEqual("メテオニックデブリ", replacer.Replace("%T123DCMRK01N"));
            Assert.AreEqual("34", replacer.Replace("%T123DCMRK01C"));
            Assert.AreEqual("星符「メテオニックシャワー」", replacer.Replace("%T123DCMRP01N"));
            Assert.AreEqual("56", replacer.Replace("%T123DCMRP01C"));
        }

        [TestMethod]
        public void ReplaceTestEmptyClearDataDictionary()
        {
            var dictionary = new Dictionary<Chara, IClearData>();
            var replacer = new CardForDeckReplacer(SystemCards, dictionary, true);
            Assert.AreEqual("「霊撃札」", replacer.Replace("%T123DCMRY01N"));
            Assert.AreEqual("12", replacer.Replace("%T123DCMRY01C"));
            Assert.AreEqual("??????????", replacer.Replace("%T123DCMRK01N"));
            Assert.AreEqual("0", replacer.Replace("%T123DCMRK01C"));
            Assert.AreEqual("??????????", replacer.Replace("%T123DCMRP01N"));
            Assert.AreEqual("0", replacer.Replace("%T123DCMRP01C"));
        }

        [TestMethod]
        public void ReplaceTestOonamazu()
        {
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, false);
            Assert.AreEqual("%T123DCNMY01N", replacer.Replace("%T123DCNMY01N"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentSystemCard()
        {
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, false);
            Assert.AreEqual("%T123DCMRY22N", replacer.Replace("%T123DCMRY22N"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentSkillCard()
        {
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, false);
            Assert.AreEqual("%T123DCMRK13N", replacer.Replace("%T123DCMRK13N"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentSpellCard()
        {
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, false);
            Assert.AreEqual("%T123DCMRP15N", replacer.Replace("%T123DCMRP15N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, false);
            Assert.AreEqual("%T123XXMRY01N", replacer.Replace("%T123XXMRY01N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, false);
            Assert.AreEqual("%T123DCXXY01N", replacer.Replace("%T123DCXXY01N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidCardType()
        {
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, false);
            Assert.AreEqual("%T123DCRMX01N", replacer.Replace("%T123DCRMX01N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidNumber()
        {
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, false);
            Assert.AreEqual("%T123DCMRY22N", replacer.Replace("%T123DCMRY22N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, false);
            Assert.AreEqual("%T123DCMRY01X", replacer.Replace("%T123DCMRY01X"));
        }
    }
}

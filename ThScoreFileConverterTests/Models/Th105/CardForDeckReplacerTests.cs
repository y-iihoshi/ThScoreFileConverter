using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th105;
using ThScoreFileConverterTests.Models.Th105.Stubs;

namespace ThScoreFileConverterTests.Models.Th105
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

        internal static IReadOnlyDictionary<Chara, IClearData<Chara, Level>> ClearDataDictionary { get; } =
            new Dictionary<Chara, IClearData<Chara, Level>>
            {
                {
                    Chara.Marisa,
                    new ClearDataStub<Chara, Level>
                    {
                        CardsForDeck = new List<ICardForDeck>
                        {
                            new CardForDeckStub { Id = 100, MaxNumber = 34, },
                            new CardForDeckStub { Id = 101, MaxNumber = 0, },
                            new CardForDeckStub { Id = 200, MaxNumber = 56, },
                            new CardForDeckStub { Id = 202, MaxNumber = 0, },
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
            var dictionary = new Dictionary<Chara, IClearData<Chara, Level>>();
            var replacer = new CardForDeckReplacer(SystemCards, dictionary, false);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestSystemCardName()
        {
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, false);
            Assert.AreEqual("「気質発現」", replacer.Replace("%T105DCMRY01N"));
            Assert.AreEqual("「霊撃」", replacer.Replace("%T105DCMRY02N"));
        }

        [TestMethod]
        public void ReplaceTestSystemCardCount()
        {
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, false);
            Assert.AreEqual("12", replacer.Replace("%T105DCMRY01C"));
            Assert.AreEqual("0", replacer.Replace("%T105DCMRY02C"));
        }

        [TestMethod]
        public void ReplaceTestSkillCardName()
        {
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, false);
            Assert.AreEqual("ウィッチレイライン", replacer.Replace("%T105DCMRK01N"));
            Assert.AreEqual("ミアズマスウィープ", replacer.Replace("%T105DCMRK02N"));
        }

        [TestMethod]
        public void ReplaceTestSkillCardCount()
        {
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, false);
            Assert.AreEqual("34", replacer.Replace("%T105DCMRK01C"));
            Assert.AreEqual("0", replacer.Replace("%T105DCMRK02C"));
        }

        [TestMethod]
        public void ReplaceTestSpellCardName()
        {
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, false);
            Assert.AreEqual("恋符「マスタースパーク」", replacer.Replace("%T105DCMRP01N"));
            Assert.AreEqual("魔砲「ファイナルスパーク」", replacer.Replace("%T105DCMRP02N"));
        }

        [TestMethod]
        public void ReplaceTestSpellCardCount()
        {
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, false);
            Assert.AreEqual("56", replacer.Replace("%T105DCMRP01C"));
            Assert.AreEqual("0", replacer.Replace("%T105DCMRP02C"));
        }

        [TestMethod]
        public void ReplaceTestUntriedSystemCardName()
        {
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, true);
            Assert.AreEqual("「気質発現」", replacer.Replace("%T105DCMRY01N"));
            Assert.AreEqual("??????????", replacer.Replace("%T105DCMRY02N"));
        }

        [TestMethod]
        public void ReplaceTestUntriedSystemCardCount()
        {
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, true);
            Assert.AreEqual("12", replacer.Replace("%T105DCMRY01C"));
            Assert.AreEqual("0", replacer.Replace("%T105DCMRY02C"));
        }

        [TestMethod]
        public void ReplaceTestUntriedSkillCardName()
        {
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, true);
            Assert.AreEqual("ウィッチレイライン", replacer.Replace("%T105DCMRK01N"));
            Assert.AreEqual("??????????", replacer.Replace("%T105DCMRK02N"));
        }

        [TestMethod]
        public void ReplaceTestUntriedSkillCardCount()
        {
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, true);
            Assert.AreEqual("34", replacer.Replace("%T105DCMRK01C"));
            Assert.AreEqual("0", replacer.Replace("%T105DCMRK02C"));
        }

        [TestMethod]
        public void ReplaceTestUntriedSpellCardName()
        {
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, true);
            Assert.AreEqual("恋符「マスタースパーク」", replacer.Replace("%T105DCMRP01N"));
            Assert.AreEqual("??????????", replacer.Replace("%T105DCMRP02N"));
        }

        [TestMethod]
        public void ReplaceTestUntriedSpellCardCount()
        {
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, true);
            Assert.AreEqual("56", replacer.Replace("%T105DCMRP01C"));
            Assert.AreEqual("0", replacer.Replace("%T105DCMRP02C"));
        }

        [TestMethod]
        public void ReplaceTestEmptySystemCards()
        {
            var cards = new Dictionary<int, ICardForDeck>();
            var replacer = new CardForDeckReplacer(cards, ClearDataDictionary, true);
            Assert.AreEqual("??????????", replacer.Replace("%T105DCMRY01N"));
            Assert.AreEqual("0", replacer.Replace("%T105DCMRY01C"));
            Assert.AreEqual("ウィッチレイライン", replacer.Replace("%T105DCMRK01N"));
            Assert.AreEqual("34", replacer.Replace("%T105DCMRK01C"));
            Assert.AreEqual("恋符「マスタースパーク」", replacer.Replace("%T105DCMRP01N"));
            Assert.AreEqual("56", replacer.Replace("%T105DCMRP01C"));
        }

        [TestMethod]
        public void ReplaceTestEmptyClearDataDictionary()
        {
            var dictionary = new Dictionary<Chara, IClearData<Chara, Level>>();
            var replacer = new CardForDeckReplacer(SystemCards, dictionary, true);
            Assert.AreEqual("「気質発現」", replacer.Replace("%T105DCMRY01N"));
            Assert.AreEqual("12", replacer.Replace("%T105DCMRY01C"));
            Assert.AreEqual("??????????", replacer.Replace("%T105DCMRK01N"));
            Assert.AreEqual("0", replacer.Replace("%T105DCMRK01C"));
            Assert.AreEqual("??????????", replacer.Replace("%T105DCMRP01N"));
            Assert.AreEqual("0", replacer.Replace("%T105DCMRP01C"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentSystemCard()
        {
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, false);
            Assert.AreEqual("%T105DCMRY07N", replacer.Replace("%T105DCMRY07N"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentSkillCard()
        {
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, false);
            Assert.AreEqual("%T105DCMRK12N", replacer.Replace("%T105DCMRK12N"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentSpellCard()
        {
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, false);
            Assert.AreEqual("%T105DCMRP11N", replacer.Replace("%T105DCMRP11N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, false);
            Assert.AreEqual("%T105XXMRY01N", replacer.Replace("%T105XXMRY01N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, false);
            Assert.AreEqual("%T105DCXXY01N", replacer.Replace("%T105DCXXY01N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidCardType()
        {
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, false);
            Assert.AreEqual("%T105DCRMX01N", replacer.Replace("%T105DCRMX01N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidNumber()
        {
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, false);
            Assert.AreEqual("%T105DCMRY12N", replacer.Replace("%T105DCMRY12N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CardForDeckReplacer(SystemCards, ClearDataDictionary, false);
            Assert.AreEqual("%T105DCMRY01X", replacer.Replace("%T105DCMRY01X"));
        }
    }
}

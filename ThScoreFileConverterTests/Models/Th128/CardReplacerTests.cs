using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th128;

namespace ThScoreFileConverterTests.Models.Th128
{
    [TestClass]
    public class CardReplacerTests
    {
        internal static IReadOnlyDictionary<int, ISpellCard> SpellCards { get; } = new[]
        {
            Mock.Of<ISpellCard>(
                m => (m.Id == 3) && (m.TrialCount == 1) && (m.Level == Level.Hard) && (m.HasTried == true)),
            Mock.Of<ISpellCard>(
                m => (m.Id == 4) && (m.TrialCount == 0) && (m.Level == Level.Lunatic) && (m.HasTried == false)),
        }.ToDictionary(card => card.Id);

        [TestMethod]
        public void CardReplacerTest()
        {
            var replacer = new CardReplacer(SpellCards, false);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void CardReplacerTestEmpty()
        {
            var cards = ImmutableDictionary<int, ISpellCard>.Empty;
            var replacer = new CardReplacer(cards, false);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestName()
        {
            var replacer = new CardReplacer(SpellCards, false);
            Assert.AreEqual("月符「ルナティックレイン」", replacer.Replace("%T128CARD003N"));
            Assert.AreEqual("月符「ルナティックレイン」", replacer.Replace("%T128CARD004N"));
        }

        [TestMethod]
        public void ReplaceTestRank()
        {
            var replacer = new CardReplacer(SpellCards, false);
            Assert.AreEqual("Hard", replacer.Replace("%T128CARD003R"));
            Assert.AreEqual("Lunatic", replacer.Replace("%T128CARD004R"));
        }

        [TestMethod]
        public void ReplaceTestHiddenName()
        {
            var replacer = new CardReplacer(SpellCards, true);
            Assert.AreEqual("月符「ルナティックレイン」", replacer.Replace("%T128CARD003N"));
            Assert.AreEqual("??????????", replacer.Replace("%T128CARD004N"));
        }

        [TestMethod]
        public void ReplaceTestHiddenRank()
        {
            var replacer = new CardReplacer(SpellCards, true);
            Assert.AreEqual("Hard", replacer.Replace("%T128CARD003R"));
            Assert.AreEqual("Lunatic", replacer.Replace("%T128CARD004R"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = ImmutableDictionary<int, ISpellCard>.Empty;

            var replacer = new CardReplacer(dictionary, true);
            Assert.AreEqual("??????????", replacer.Replace("%T128CARD003N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CardReplacer(SpellCards, false);
            Assert.AreEqual("%T128XXXX003N", replacer.Replace("%T128XXXX003N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidNumber()
        {
            var replacer = new CardReplacer(SpellCards, false);
            Assert.AreEqual("%T128CARD251N", replacer.Replace("%T128CARD251N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CardReplacer(SpellCards, false);
            Assert.AreEqual("%T128CARD003X", replacer.Replace("%T128CARD003X"));
        }
    }
}

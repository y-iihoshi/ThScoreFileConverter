using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th128;
using ThScoreFileConverterTests.Models.Th128.Stubs;

namespace ThScoreFileConverterTests.Models.Th128
{
    [TestClass]
    public class CardReplacerTests
    {
        internal static IReadOnlyDictionary<int, ISpellCard> SpellCards { get; } =
            new List<ISpellCard>
            {
                new SpellCardStub
                {
                    Id = 3,
                    TrialCount = 1,
                    Level = Level.Hard,
                },
                new SpellCardStub
                {
                    Id = 4,
                    TrialCount = 0,
                    Level = Level.Lunatic,
                },
            }.ToDictionary(element => element.Id);

        [TestMethod]
        public void CardReplacerTest()
        {
            var replacer = new CardReplacer(SpellCards, false);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CardReplacerTestNull()
        {
            _ = new CardReplacer(null!, false);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void CardReplacerTestEmpty()
        {
            var cards = new Dictionary<int, ISpellCard>();
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
            var dictionary = new Dictionary<int, ISpellCard>();

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

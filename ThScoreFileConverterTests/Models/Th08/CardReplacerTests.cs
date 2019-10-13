using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th08;
using ThScoreFileConverterTests.Models.Th08.Stubs;

namespace ThScoreFileConverterTests.Models.Th08
{
    [TestClass]
    public class CardReplacerTests
    {
        internal static IReadOnlyDictionary<int, ICardAttack> CardAttacks { get; } = new List<ICardAttack>
        {
            new CardAttackStub(CardAttackTests.ValidStub),
            new CardAttackStub(CardAttackTests.ValidStub)
            {
                CardId = (short)(CardAttackTests.ValidStub.CardId + 1),
                StoryCareer = new CardAttackCareerStub(CardAttackCareerTests.ValidStub)
                {
                    TrialCounts = CardAttackCareerTests.ValidStub.TrialCounts
                        .ToDictionary(pair => pair.Key, pair => 0),
                    ClearCounts = CardAttackCareerTests.ValidStub.ClearCounts
                        .ToDictionary(pair => pair.Key, pair => 0),
                },
                PracticeCareer = new CardAttackCareerStub(CardAttackCareerTests.ValidStub)
                {
                    TrialCounts = CardAttackCareerTests.ValidStub.TrialCounts
                        .ToDictionary(pair => pair.Key, pair => 0),
                    ClearCounts = CardAttackCareerTests.ValidStub.ClearCounts
                        .ToDictionary(pair => pair.Key, pair => 0),
                },
            },
        }.ToDictionary(element => (int)element.CardId);

        [TestMethod]
        public void CardReplacerTest()
        {
            var replacer = new CardReplacer(CardAttacks, true);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CardReplacerTestNull()
        {
            _ = new CardReplacer(null, true);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void CardReplacerTestEmpty()
        {
            var cardAttacks = new Dictionary<int, ICardAttack>();
            var replacer = new CardReplacer(cardAttacks, true);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestName()
        {
            var replacer = new CardReplacer(CardAttacks, false);
            Assert.AreEqual("天丸「壺中の天地」", replacer.Replace("%T08CARD123N"));
            Assert.AreEqual("覚神「神代の記憶」", replacer.Replace("%T08CARD124N"));
        }

        [TestMethod]
        public void ReplaceTestRank()
        {
            var replacer = new CardReplacer(CardAttacks, false);
            Assert.AreEqual("Lunatic", replacer.Replace("%T08CARD123R"));
            Assert.AreEqual("Easy", replacer.Replace("%T08CARD124R"));
        }

        [TestMethod]
        public void ReplaceTestHiddenName()
        {
            var replacer = new CardReplacer(CardAttacks, true);
            Assert.AreEqual("天丸「壺中の天地」", replacer.Replace("%T08CARD123N"));
            Assert.AreEqual("??????????", replacer.Replace("%T08CARD124N"));
        }

        [TestMethod]
        public void ReplaceTestHiddenRank()
        {
            var replacer = new CardReplacer(CardAttacks, true);
            Assert.AreEqual("Lunatic", replacer.Replace("%T08CARD123R"));
            Assert.AreEqual("?????", replacer.Replace("%T08CARD124R"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentName()
        {
            var replacer = new CardReplacer(CardAttacks, true);
            Assert.AreEqual("??????????", replacer.Replace("%T08CARD125N"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentRank()
        {
            var replacer = new CardReplacer(CardAttacks, true);
            Assert.AreEqual("?????", replacer.Replace("%T08CARD125R"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CardReplacer(CardAttacks, true);
            Assert.AreEqual("%T08XXXX123N", replacer.Replace("%T08XXXX123N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidNumber()
        {
            var replacer = new CardReplacer(CardAttacks, true);
            Assert.AreEqual("%T08CARD223N", replacer.Replace("%T08CARD223N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CardReplacer(CardAttacks, true);
            Assert.AreEqual("%T08CARD123X", replacer.Replace("%T08CARD123X"));
        }
    }
}

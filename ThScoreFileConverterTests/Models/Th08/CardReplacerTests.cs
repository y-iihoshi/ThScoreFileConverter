using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th08;

namespace ThScoreFileConverterTests.Models.Th08
{
    [TestClass]
    public class CardReplacerTests
    {
        private static IEnumerable<ICardAttack> CreateCardAttacks()
        {
            var storyCareerMock = CardAttackCareerTests.MockCardAttackCareer();
            var trialCounts = storyCareerMock.Object.TrialCounts;
            var clearCounts = storyCareerMock.Object.ClearCounts;
            var noTrialCounts = trialCounts.ToDictionary(pair => pair.Key, pair => 0);
            var noClearCounts = clearCounts.ToDictionary(pair => pair.Key, pair => 0);
            _ = storyCareerMock.SetupGet(m => m.TrialCounts).Returns(noTrialCounts);
            _ = storyCareerMock.SetupGet(m => m.ClearCounts).Returns(noClearCounts);

            var practiceCareerMock = CardAttackCareerTests.MockCardAttackCareer();
            _ = practiceCareerMock.SetupGet(m => m.TrialCounts).Returns(noTrialCounts);
            _ = practiceCareerMock.SetupGet(m => m.ClearCounts).Returns(noClearCounts);

            var attack1Mock = CardAttackTests.MockCardAttack();

            var attack2Mock = CardAttackTests.MockCardAttack();
            _ = attack2Mock.SetupGet(m => m.CardId).Returns((short)(attack1Mock.Object.CardId + 1));
            _ = attack2Mock.SetupGet(m => m.StoryCareer).Returns(storyCareerMock.Object);
            _ = attack2Mock.SetupGet(m => m.PracticeCareer).Returns(practiceCareerMock.Object);
            CardAttackTests.SetupHasTried(attack2Mock);

            return new[] { attack1Mock.Object, attack2Mock.Object };
        }

        internal static IReadOnlyDictionary<int, ICardAttack> CardAttacks { get; } =
            CreateCardAttacks().ToDictionary(element => (int)element.CardId);

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
            _ = new CardReplacer(null!, true);
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
        public void ReplaceTestRankLastWord()
        {
            var replacer = new CardReplacer(CardAttacks, false);
            Assert.AreEqual("Last Word", replacer.Replace("%T08CARD206R"));
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

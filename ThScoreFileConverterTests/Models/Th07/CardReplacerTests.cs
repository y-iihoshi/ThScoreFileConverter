using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th07;

namespace ThScoreFileConverterTests.Models.Th07
{
    [TestClass]
    public class CardReplacerTests
    {
        private static IEnumerable<ICardAttack> CreateCardAttacks()
        {
            var mock1 = CardAttackTests.MockCardAttack();

            var mock2 = CardAttackTests.MockCardAttack();
            _ = mock2.SetupGet(m => m.MaxBonuses).Returns(
                mock1.Object.MaxBonuses.ToDictionary(pair => pair.Key, pair => pair.Value * 1000));
            _ = mock2.SetupGet(m => m.CardId).Returns((short)(mock1.Object.CardId + 1));
            _ = mock2.SetupGet(m => m.CardName).Returns(TestUtils.MakeRandomArray<byte>(0x30));
            _ = mock2.SetupGet(m => m.TrialCounts).Returns(
                mock1.Object.TrialCounts.ToDictionary(pair => pair.Key, pair => (ushort)0));
            _ = mock2.SetupGet(m => m.ClearCounts).Returns(
                mock1.Object.ClearCounts.ToDictionary(pair => pair.Key, pair => (ushort)0));
            _ = mock2.Setup(m => m.HasTried()).Returns(false);

            return new[] { mock1.Object, mock2.Object };
        }

        internal static IReadOnlyDictionary<int, ICardAttack> CardAttacks { get; } =
            CreateCardAttacks().ToDictionary(attack => (int)attack.CardId);

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
            Assert.AreEqual("式輝「プリンセス天狐 -Illusion-」", replacer.Replace("%T07CARD123N"));
            Assert.AreEqual("式弾「アルティメットブディスト」", replacer.Replace("%T07CARD124N"));
        }

        [TestMethod]
        public void ReplaceTestRank()
        {
            var replacer = new CardReplacer(CardAttacks, false);
            Assert.AreEqual("Extra", replacer.Replace("%T07CARD123R"));
            Assert.AreEqual("Extra", replacer.Replace("%T07CARD124R"));
        }

        [TestMethod]
        public void ReplaceTestHiddenName()
        {
            var replacer = new CardReplacer(CardAttacks, true);
            Assert.AreEqual("式輝「プリンセス天狐 -Illusion-」", replacer.Replace("%T07CARD123N"));
            Assert.AreEqual("??????????", replacer.Replace("%T07CARD124N"));
        }

        [TestMethod]
        public void ReplaceTestHiddenRank()
        {
            var replacer = new CardReplacer(CardAttacks, true);
            Assert.AreEqual("Extra", replacer.Replace("%T07CARD123R"));
            Assert.AreEqual("?????", replacer.Replace("%T07CARD124R"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentName()
        {
            var replacer = new CardReplacer(CardAttacks, true);
            Assert.AreEqual("??????????", replacer.Replace("%T07CARD125N"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentRank()
        {
            var replacer = new CardReplacer(CardAttacks, true);
            Assert.AreEqual("?????", replacer.Replace("%T07CARD125R"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CardReplacer(CardAttacks, true);
            Assert.AreEqual("%T07XXXX123N", replacer.Replace("%T07XXXX123N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidNumber()
        {
            var replacer = new CardReplacer(CardAttacks, true);
            Assert.AreEqual("%T07CARD142N", replacer.Replace("%T07CARD142N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CardReplacer(CardAttacks, true);
            Assert.AreEqual("%T07CARD123X", replacer.Replace("%T07CARD123X"));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th07;

namespace ThScoreFileConverterTests.Models.Th07
{
    [TestClass]
    public class CollectRateReplacerTests
    {
        private static IEnumerable<ICardAttack> CreateCardAttacks()
        {
            var mock1 = CardAttackTests.MockCardAttack();

            var mock2 = CardAttackTests.MockCardAttack();
            _ = mock2.SetupGet(m => m.CardId).Returns(2);
            _ = mock2.SetupGet(m => m.CardName).Returns(TestUtils.MakeRandomArray<byte>(0x30));
            _ = mock2.SetupGet(m => m.TrialCounts).Returns(
                mock1.Object.TrialCounts.ToDictionary(pair => pair.Key, pair => (ushort)(pair.Value * 4)));
            _ = mock2.SetupGet(m => m.ClearCounts).Returns(
                mock1.Object.ClearCounts.ToDictionary(pair => pair.Key, pair => (ushort)(pair.Value * 3)));

            var mock3 = CardAttackTests.MockCardAttack();
            _ = mock3.SetupGet(m => m.CardId).Returns(6);
            _ = mock3.SetupGet(m => m.CardName).Returns(TestUtils.MakeRandomArray<byte>(0x30));
            _ = mock3.SetupGet(m => m.TrialCounts).Returns(
                mock1.Object.TrialCounts.ToDictionary(pair => pair.Key, pair => (ushort)(pair.Value * 5)));
            _ = mock3.SetupGet(m => m.ClearCounts).Returns(
                mock1.Object.ClearCounts.ToDictionary(pair => pair.Key, pair => (ushort)0));

            var mock4 = CardAttackTests.MockCardAttack();
            _ = mock4.SetupGet(m => m.CardId).Returns(129);
            _ = mock4.SetupGet(m => m.CardName).Returns(TestUtils.MakeRandomArray<byte>(0x30));
            _ = mock4.SetupGet(m => m.TrialCounts).Returns(
                mock1.Object.TrialCounts.ToDictionary(pair => pair.Key, pair => (ushort)0));
            _ = mock4.SetupGet(m => m.ClearCounts).Returns(
                mock1.Object.ClearCounts.ToDictionary(pair => pair.Key, pair => (ushort)0));
            _ = mock4.Setup(m => m.HasTried()).Returns(false);

            return new[] { mock1.Object, mock2.Object, mock3.Object, mock4.Object };
        }

        internal static IReadOnlyDictionary<int, ICardAttack> CardAttacks { get; } =
            CreateCardAttacks().ToDictionary(attack => (int)attack.CardId);

        [TestMethod]
        public void CollectRateReplacerTest()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void CollectRateReplacerTestNull()
            => _ = Assert.ThrowsException<ArgumentNullException>(() => _ = new CollectRateReplacer(null!));

        [TestMethod]
        public void CollectRateReplacerTestEmpty()
        {
            var cardAttacks = new Dictionary<int, ICardAttack>();
            var replacer = new CollectRateReplacer(cardAttacks);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestClearCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("1", replacer.Replace("%T07CRGLRB11"));
        }

        [TestMethod]
        public void ReplaceTestTrialCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("2", replacer.Replace("%T07CRGLRB12"));
        }

        [TestMethod]
        public void ReplaceTestExtraClearCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("1", replacer.Replace("%T07CRGXRB11"));
        }

        [TestMethod]
        public void ReplaceTestExtraTrialCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("1", replacer.Replace("%T07CRGXRB12"));
        }

        [TestMethod]
        public void ReplaceTestPhantasmClearCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("0", replacer.Replace("%T07CRGPRB11"));
        }

        [TestMethod]
        public void ReplaceTestPhantasmTrialCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("0", replacer.Replace("%T07CRGPRB12"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalClearCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("1", replacer.Replace("%T07CRGTRB11"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("2", replacer.Replace("%T07CRGTRB12"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotalClearCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("1", replacer.Replace("%T07CRGLTL11"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("2", replacer.Replace("%T07CRGLTL12"));
        }

        [TestMethod]
        public void ReplaceTestStageTotalClearCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("1", replacer.Replace("%T07CRGLRB01"));
        }

        [TestMethod]
        public void ReplaceTestStageTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("2", replacer.Replace("%T07CRGLRB02"));
        }

        [TestMethod]
        public void ReplaceTestTotalClearCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("2", replacer.Replace("%T07CRGTTL01"));
        }

        [TestMethod]
        public void ReplaceTestTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("3", replacer.Replace("%T07CRGTTL02"));
        }

        [TestMethod]
        public void ReplaceTestEmptyClearCount()
        {
            var cardAttacks = new Dictionary<int, ICardAttack>();
            var replacer = new CollectRateReplacer(cardAttacks);
            Assert.AreEqual("0", replacer.Replace("%T07CRGLRB11"));
        }

        [TestMethod]
        public void ReplaceTestEmptyTrialCount()
        {
            var cardAttacks = new Dictionary<int, ICardAttack>();
            var replacer = new CollectRateReplacer(cardAttacks);
            Assert.AreEqual("0", replacer.Replace("%T07CRGLRB12"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("%T07XXXLRB11", replacer.Replace("%T07XXXLRB11"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("%T07CRGYRB11", replacer.Replace("%T07CRGYRB11"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("%T07CRGLXX11", replacer.Replace("%T07CRGLXX11"));
        }

        [TestMethod]
        public void ReplaceTestInvalidStage()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("%T07CRGLRBY1", replacer.Replace("%T07CRGLRBY1"));
            Assert.AreEqual("%T07CRGLRBX1", replacer.Replace("%T07CRGLRBX1"));
            Assert.AreEqual("%T07CRGLRBP1", replacer.Replace("%T07CRGLRBP1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("%T07CRGLRB1X", replacer.Replace("%T07CRGLRB1X"));
        }

        [TestMethod]
        public void ReplaceTestInvalidCardId()
        {
            var mock = CardAttackTests.MockCardAttack();
            _ = mock.SetupGet(m => m.CardId).Returns(142);
            _ = mock.SetupGet(m => m.CardName).Returns(TestUtils.MakeRandomArray<byte>(0x30));
            var cardAttacks = new[] { mock.Object }.ToDictionary(attack => (int)attack.CardId);
            var replacer = new CollectRateReplacer(cardAttacks);
            Assert.AreEqual("0", replacer.Replace("%T07CRGLRB11"));
            Assert.AreEqual("0", replacer.Replace("%T07CRGXRB11"));
            Assert.AreEqual("0", replacer.Replace("%T07CRGPRB11"));
        }
    }
}

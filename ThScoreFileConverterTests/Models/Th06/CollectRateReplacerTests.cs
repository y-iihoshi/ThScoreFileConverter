using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th06;

namespace ThScoreFileConverterTests.Models.Th06
{
    [TestClass]
    public class CollectRateReplacerTests
    {
        private static IEnumerable<ICardAttack> CreateCardAttacks()
        {
            var mock1 = CardAttackTests.MockCardAttack();

            var mock2 = CardAttackTests.MockCardAttack();
            _ = mock2.SetupGet(m => m.CardId).Returns((short)(mock1.Object.CardId + 1));
            _ = mock2.SetupGet(m => m.CardName).Returns(TestUtils.MakeRandomArray<byte>(0x24));
            _ = mock2.SetupGet(m => m.ClearCount).Returns(0);
            _ = mock2.SetupGet(m => m.TrialCount).Returns(123);

            var mock3 = CardAttackTests.MockCardAttack();
            _ = mock3.SetupGet(m => m.CardId).Returns(2);
            _ = mock3.SetupGet(m => m.CardName).Returns(TestUtils.MakeRandomArray<byte>(0x24));
            _ = mock3.SetupGet(m => m.ClearCount).Returns(123);
            _ = mock3.SetupGet(m => m.TrialCount).Returns(123);

            return new[] { mock1.Object, mock2.Object, mock3.Object };
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
        [ExpectedException(typeof(ArgumentNullException))]
        public void CollectRateReplacerTestNull()
        {
            _ = new CollectRateReplacer(null!);
            Assert.Fail(TestUtils.Unreachable);
        }

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
            Assert.AreEqual("1", replacer.Replace("%T06CRG41"));
        }

        [TestMethod]
        public void ReplaceTestTrialCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("2", replacer.Replace("%T06CRG42"));
        }

        [TestMethod]
        public void ReplaceTestTotalClearCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("2", replacer.Replace("%T06CRG01"));
        }

        [TestMethod]
        public void ReplaceTestTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("3", replacer.Replace("%T06CRG02"));
        }

        [TestMethod]
        public void ReplaceTestEmptyClearCount()
        {
            var cardAttacks = new Dictionary<int, ICardAttack>();
            var replacer = new CollectRateReplacer(cardAttacks);
            Assert.AreEqual("0", replacer.Replace("%T06CRG41"));
        }

        [TestMethod]
        public void ReplaceTestEmptyTrialCount()
        {
            var cardAttacks = new Dictionary<int, ICardAttack>();
            var replacer = new CollectRateReplacer(cardAttacks);
            Assert.AreEqual("0", replacer.Replace("%T06CRG42"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("%T06XXX41", replacer.Replace("%T06XXX41"));
        }

        [TestMethod]
        public void ReplaceTestInvalidStage()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("%T06CRGY1", replacer.Replace("%T06CRGY1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("%T06CRG4X", replacer.Replace("%T06CRG4X"));
        }

        [TestMethod]
        public void ReplaceTestInvalidCardId()
        {
            var mock = CardAttackTests.MockCardAttack();
            _ = mock.SetupGet(m => m.CardId).Returns(65);
            var cardAttacks = new[] { mock.Object }.ToDictionary(element => (int)element.CardId);
            var replacer = new CollectRateReplacer(cardAttacks);
            Assert.AreEqual("0", replacer.Replace("%T06CRG41"));
        }
    }
}

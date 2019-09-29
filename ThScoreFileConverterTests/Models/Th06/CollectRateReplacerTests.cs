using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th06;
using ThScoreFileConverterTests.Models.Th06.Stubs;

namespace ThScoreFileConverterTests.Models.Th06
{
    [TestClass]
    public class CollectRateReplacerTests
    {
        internal static IReadOnlyDictionary<int, ICardAttack> CardAttacks { get; } =
            new List<ICardAttack>
            {
                new CardAttackStub(CardAttackTests.ValidStub),
                new CardAttackStub(CardAttackTests.ValidStub)
                {
                    CardId = (short)(CardAttackTests.ValidStub.CardId + 1),
                    CardName = TestUtils.MakeRandomArray<byte>(0x24),
                    ClearCount = 0,
                    TrialCount = 123,
                },
                new CardAttackStub(CardAttackTests.ValidStub)
                {
                    CardId = 2,
                    CardName = TestUtils.MakeRandomArray<byte>(0x24),
                    ClearCount = 123,
                    TrialCount = 123,
                },
            }.ToDictionary(element => (int)element.CardId);

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
            _ = new CollectRateReplacer(null);
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
            var cardAttacks = new List<ICardAttack>
            {
                new CardAttackStub(CardAttackTests.ValidStub)
                {
                    CardId = 65,
                },
            }.ToDictionary(element => (int)element.CardId);
            var replacer = new CollectRateReplacer(cardAttacks);
            Assert.AreEqual("0", replacer.Replace("%T06CRG41"));
        }
    }
}

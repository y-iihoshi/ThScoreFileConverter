using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th06;
using ThScoreFileConverterTests.Models.Th06.Wrappers;

namespace ThScoreFileConverterTests.Models.Th06
{
    [TestClass]
    public class CollectRateReplacerTests
    {
        internal static IReadOnlyDictionary<int, CardAttack> CardAttacks { get; } =
            new Dictionary<int, CardAttack>
            {
                {
                    CardAttackTests.ValidProperties.cardId,
                    new CardAttack(ChapterWrapper.Create(
                        CardAttackTests.MakeByteArray(CardAttackTests.ValidProperties)).Target)
                },
                {
                    CardAttackTests.ValidProperties.cardId + 1,
                    new CardAttack(ChapterWrapper.Create(CardAttackTests.MakeByteArray(
                        new CardAttackTests.Properties
                        {
                            signature = CardAttackTests.ValidProperties.signature,
                            size1 = CardAttackTests.ValidProperties.size1,
                            size2 = CardAttackTests.ValidProperties.size2,
                            cardId = (short)(CardAttackTests.ValidProperties.cardId + 1),
                            cardName = TestUtils.MakeRandomArray<byte>(0x24),
                            clearCount = 0,
                            trialCount = 123,
                        })).Target)
                },
                {
                    2,
                    new CardAttack(ChapterWrapper.Create(CardAttackTests.MakeByteArray(
                        new CardAttackTests.Properties
                        {
                            signature = CardAttackTests.ValidProperties.signature,
                            size1 = CardAttackTests.ValidProperties.size1,
                            size2 = CardAttackTests.ValidProperties.size2,
                            cardId = 2,
                            cardName = TestUtils.MakeRandomArray<byte>(0x24),
                            clearCount = 123,
                            trialCount = 123,
                        })).Target)
                },
            };

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
            var cardAttacks = new Dictionary<int, CardAttack>();
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
            var cardAttacks = new Dictionary<int, CardAttack>();
            var replacer = new CollectRateReplacer(cardAttacks);
            Assert.AreEqual("0", replacer.Replace("%T06CRG41"));
        }

        [TestMethod]
        public void ReplaceTestEmptyTrialCount()
        {
            var cardAttacks = new Dictionary<int, CardAttack>();
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
            var cardAttacks = new Dictionary<int, CardAttack>
            {
                {
                    65,
                    new CardAttack(ChapterWrapper.Create(CardAttackTests.MakeByteArray(
                        new CardAttackTests.Properties
                        {
                            signature = CardAttackTests.ValidProperties.signature,
                            size1 = CardAttackTests.ValidProperties.size1,
                            size2 = CardAttackTests.ValidProperties.size2,
                            cardId = 65,
                            cardName = CardAttackTests.ValidProperties.cardName,
                            clearCount = CardAttackTests.ValidProperties.clearCount,
                            trialCount = CardAttackTests.ValidProperties.trialCount,
                        })).Target)
                },
            };
            var replacer = new CollectRateReplacer(cardAttacks);
            Assert.AreEqual("0", replacer.Replace("%T06CRG41"));
        }
    }
}

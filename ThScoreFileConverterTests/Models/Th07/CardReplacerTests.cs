using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th07;
using ThScoreFileConverterTests.Models.Th06.Wrappers;

namespace ThScoreFileConverterTests.Models.Th07
{
    [TestClass]
    public class CardReplacerTests
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
                            maxBonuses = CardAttackTests.ValidProperties.maxBonuses
                                .ToDictionary(pair => pair.Key, pair => pair.Value * 1000),
                            cardId = (short)(CardAttackTests.ValidProperties.cardId + 1),
                            cardName = TestUtils.MakeRandomArray<byte>(0x30),
                            trialCounts = CardAttackTests.ValidProperties.trialCounts
                                .ToDictionary(pair => pair.Key, pair => (ushort)0),
                            clearCounts = CardAttackTests.ValidProperties.clearCounts
                                .ToDictionary(pair => pair.Key, pair => (ushort)0),
                        })).Target)
                },
            };

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
            var cardAttacks = new Dictionary<int, CardAttack>();
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

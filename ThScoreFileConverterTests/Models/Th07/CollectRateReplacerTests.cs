using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th07;
using ThScoreFileConverterTests.Models.Th07.Stubs;

namespace ThScoreFileConverterTests.Models.Th07
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
                    CardId = 2,
                    CardName = TestUtils.MakeRandomArray<byte>(0x30),
                    TrialCounts = CardAttackTests.ValidStub.TrialCounts
                        .ToDictionary(pair => pair.Key, pair => (ushort)(pair.Value * 4)),
                    ClearCounts = CardAttackTests.ValidStub.ClearCounts
                        .ToDictionary(pair => pair.Key, pair => (ushort)(pair.Value * 3)),
                },
                new CardAttackStub(CardAttackTests.ValidStub)
                {
                    CardId = 6,
                    CardName = TestUtils.MakeRandomArray<byte>(0x30),
                    TrialCounts = CardAttackTests.ValidStub.TrialCounts
                        .ToDictionary(pair => pair.Key, pair => (ushort)(pair.Value * 5)),
                    ClearCounts = CardAttackTests.ValidStub.ClearCounts
                        .ToDictionary(pair => pair.Key, pair => (ushort)0),
                },
                new CardAttackStub(CardAttackTests.ValidStub)
                {
                    CardId = 129,
                    CardName = TestUtils.MakeRandomArray<byte>(0x30),
                    TrialCounts = CardAttackTests.ValidStub.TrialCounts
                        .ToDictionary(pair => pair.Key, pair => (ushort)0),
                    ClearCounts = CardAttackTests.ValidStub.ClearCounts
                        .ToDictionary(pair => pair.Key, pair => (ushort)0),
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
            var cardAttacks = new List<ICardAttack>
            {
                new CardAttackStub(CardAttackTests.ValidStub)
                {
                    CardId = 142,
                    CardName = TestUtils.MakeRandomArray<byte>(0x30),
                },
            }.ToDictionary(element => (int)element.CardId);
            var replacer = new CollectRateReplacer(cardAttacks);
            Assert.AreEqual("0", replacer.Replace("%T07CRGLRB11"));
            Assert.AreEqual("0", replacer.Replace("%T07CRGXRB11"));
            Assert.AreEqual("0", replacer.Replace("%T07CRGPRB11"));
        }
    }
}

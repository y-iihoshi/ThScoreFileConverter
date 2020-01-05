using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th06;
using ThScoreFileConverterTests.Models.Th06.Stubs;

namespace ThScoreFileConverterTests.Models.Th06
{
    [TestClass]
    public class CardReplacerTests
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
                    TrialCount = 0,
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
            Assert.AreEqual("火符「アグニレイディアンス」", replacer.Replace("%T06CARD23N"));
            Assert.AreEqual("水符「ベリーインレイク」", replacer.Replace("%T06CARD24N"));
        }

        [TestMethod]
        public void ReplaceTestRank()
        {
            var replacer = new CardReplacer(CardAttacks, false);
            Assert.AreEqual("Hard, Lunatic", replacer.Replace("%T06CARD23R"));
            Assert.AreEqual("Hard, Lunatic", replacer.Replace("%T06CARD24R"));
        }

        [TestMethod]
        public void ReplaceTestHiddenName()
        {
            var replacer = new CardReplacer(CardAttacks, true);
            Assert.AreEqual("火符「アグニレイディアンス」", replacer.Replace("%T06CARD23N"));
            Assert.AreEqual("??????????", replacer.Replace("%T06CARD24N"));
        }

        [TestMethod]
        public void ReplaceTestHiddenRank()
        {
            var replacer = new CardReplacer(CardAttacks, true);
            Assert.AreEqual("Hard, Lunatic", replacer.Replace("%T06CARD23R"));
            Assert.AreEqual("?????", replacer.Replace("%T06CARD24R"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentName()
        {
            var replacer = new CardReplacer(CardAttacks, true);
            Assert.AreEqual("??????????", replacer.Replace("%T06CARD25N"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentRank()
        {
            var replacer = new CardReplacer(CardAttacks, true);
            Assert.AreEqual("?????", replacer.Replace("%T06CARD25R"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CardReplacer(CardAttacks, true);
            Assert.AreEqual("%T06XXXX23N", replacer.Replace("%T06XXXX23N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidNumber()
        {
            var replacer = new CardReplacer(CardAttacks, true);
            Assert.AreEqual("%T06CARD65N", replacer.Replace("%T06CARD65N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CardReplacer(CardAttacks, true);
            Assert.AreEqual("%T06CARD23X", replacer.Replace("%T06CARD23X"));
        }
    }
}

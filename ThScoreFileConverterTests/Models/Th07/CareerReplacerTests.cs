using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter;
using ThScoreFileConverter.Models.Th07;
using ThScoreFileConverterTests.Models.Th07.Stubs;

namespace ThScoreFileConverterTests.Models.Th07
{
    [TestClass]
    public class CareerReplacerTests
    {
        internal static IReadOnlyDictionary<int, ICardAttack> CardAttacks { get; } =
            new List<ICardAttack>
            {
                new CardAttackStub(CardAttackTests.ValidStub),
                new CardAttackStub(CardAttackTests.ValidStub)
                {
                    MaxBonuses = CardAttackTests.ValidStub.MaxBonuses
                        .ToDictionary(pair => pair.Key, pair => pair.Value * 1000),
                    CardId = (short)(CardAttackTests.ValidStub.CardId + 1),
                    CardName = TestUtils.MakeRandomArray<byte>(0x30),
                    TrialCounts = CardAttackTests.ValidStub.TrialCounts
                        .ToDictionary(pair => pair.Key, pair => (ushort)(pair.Value * 3)),
                    ClearCounts = CardAttackTests.ValidStub.ClearCounts
                        .ToDictionary(pair => pair.Key, pair => (ushort)(pair.Value * 2)),
                },
            }.ToDictionary(element => (int)element.CardId);

        [TestMethod]
        public void CareerReplacerTest()
        {
            var replacer = new CareerReplacer(CardAttacks);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CareerReplacerTestNull()
        {
            _ = new CareerReplacer(null);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void CareerReplacerTestEmpty()
        {
            var cardAttacks = new Dictionary<int, ICardAttack>();
            var replacer = new CareerReplacer(cardAttacks);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestMaxBonus()
        {
            var replacer = new CareerReplacer(CardAttacks);
            Assert.AreEqual("1", replacer.Replace("%T07C123RB1"));
        }

        [TestMethod]
        public void ReplaceTestClearCount()
        {
            var replacer = new CareerReplacer(CardAttacks);
            Assert.AreEqual("9", replacer.Replace("%T07C123RB2"));
        }

        [TestMethod]
        public void ReplaceTestTrialCount()
        {
            var replacer = new CareerReplacer(CardAttacks);
            Assert.AreEqual("11", replacer.Replace("%T07C123RB3"));
        }

        [TestMethod]
        public void ReplaceTestTotalMaxBonus()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new CareerReplacer(CardAttacks);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("1,001", replacer.Replace("%T07C000RB1"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("1001", replacer.Replace("%T07C000RB1"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestTotalClearCount()
        {
            var replacer = new CareerReplacer(CardAttacks);
            Assert.AreEqual("27", replacer.Replace("%T07C000RB2"));
        }

        [TestMethod]
        public void ReplaceTestTotalTrialCount()
        {
            var replacer = new CareerReplacer(CardAttacks);
            Assert.AreEqual("44", replacer.Replace("%T07C000RB3"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentMaxBonus()
        {
            var replacer = new CareerReplacer(CardAttacks);
            Assert.AreEqual("0", replacer.Replace("%T07C001RB1"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentClearCount()
        {
            var replacer = new CareerReplacer(CardAttacks);
            Assert.AreEqual("0", replacer.Replace("%T07C001RB2"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentTrialCount()
        {
            var replacer = new CareerReplacer(CardAttacks);
            Assert.AreEqual("0", replacer.Replace("%T07C001RB3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CareerReplacer(CardAttacks);
            Assert.AreEqual("%T07X123RB1", replacer.Replace("%T07X123RB1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidNumber()
        {
            var replacer = new CareerReplacer(CardAttacks);
            Assert.AreEqual("%T07C142RB1", replacer.Replace("%T07C142RB1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CareerReplacer(CardAttacks);
            Assert.AreEqual("%T07C123RB4", replacer.Replace("%T07C123RB4"));
        }
    }
}

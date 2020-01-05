using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter;
using ThScoreFileConverter.Models.Th06;
using ThScoreFileConverterTests.Models.Th06.Stubs;

namespace ThScoreFileConverterTests.Models.Th06
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
                    CardId = (short)(CardAttackTests.ValidStub.CardId + 1),
                    CardName = TestUtils.MakeRandomArray<byte>(0x24),
                    ClearCount = (ushort)(CardAttackTests.ValidStub.ClearCount + 2),
                    TrialCount = (ushort)(CardAttackTests.ValidStub.TrialCount + 3),
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
            _ = new CareerReplacer(null!);
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
        public void ReplaceTestClearCount()
        {
            var replacer = new CareerReplacer(CardAttacks);
            Assert.AreEqual("456", replacer.Replace("%T06C231"));
        }

        [TestMethod]
        public void ReplaceTestTrialCount()
        {
            var replacer = new CareerReplacer(CardAttacks);
            Assert.AreEqual("789", replacer.Replace("%T06C232"));
        }

        [TestMethod]
        public void ReplaceTestTotalClearCount()
        {
            var replacer = new CareerReplacer(CardAttacks);
            Assert.AreEqual("914", replacer.Replace("%T06C001"));
        }

        [TestMethod]
        public void ReplaceTestTotalTrialCount()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new CareerReplacer(CardAttacks);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("1,581", replacer.Replace("%T06C002"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("1581", replacer.Replace("%T06C002"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestNonexistentClearCount()
        {
            var replacer = new CareerReplacer(CardAttacks);
            Assert.AreEqual("0", replacer.Replace("%T06C011"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentTrialCount()
        {
            var replacer = new CareerReplacer(CardAttacks);
            Assert.AreEqual("0", replacer.Replace("%T06C012"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CareerReplacer(CardAttacks);
            Assert.AreEqual("%T06X231", replacer.Replace("%T06X231"));
        }

        [TestMethod]
        public void ReplaceTestInvalidNumber()
        {
            var replacer = new CareerReplacer(CardAttacks);
            Assert.AreEqual("%T06C651", replacer.Replace("%T06C651"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CareerReplacer(CardAttacks);
            Assert.AreEqual("%T06C233", replacer.Replace("%T06C233"));
        }
    }
}

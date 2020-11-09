using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter;
using ThScoreFileConverter.Models.Th06;

namespace ThScoreFileConverterTests.Models.Th06
{
    [TestClass]
    public class CareerReplacerTests
    {
        private static IEnumerable<ICardAttack> CreateCardAttacks()
        {
            var mock1 = CardAttackTests.MockCardAttack();

            var mock2 = CardAttackTests.MockCardAttack();
            mock2.SetupGet(m => m.CardId).Returns((short)(mock1.Object.CardId + 1));
            mock2.SetupGet(m => m.CardName).Returns(TestUtils.MakeRandomArray<byte>(0x24));
            mock2.SetupGet(m => m.ClearCount).Returns((ushort)(mock1.Object.ClearCount + 2));
            mock2.SetupGet(m => m.TrialCount).Returns((ushort)(mock1.Object.TrialCount + 3));

            return new[] { mock1.Object, mock2.Object };
        }

        internal static IReadOnlyDictionary<int, ICardAttack> CardAttacks { get; } =
            CreateCardAttacks().ToDictionary(attack => (int)attack.CardId);

        [TestMethod]
        public void CareerReplacerTest()
        {
            var replacer = new CareerReplacer(CardAttacks);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void CareerReplacerTestNull()
            => _ = Assert.ThrowsException<ArgumentNullException>(() => _ = new CareerReplacer(null!));

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

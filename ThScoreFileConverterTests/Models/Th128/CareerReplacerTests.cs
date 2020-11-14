using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th128;

namespace ThScoreFileConverterTests.Models.Th128
{
    [TestClass]
    public class CareerReplacerTests
    {
        private static IEnumerable<ISpellCard> CreateSpellCards()
        {
            var mock1 = SpellCardTests.MockSpellCard();

            var mock2 = SpellCardTests.MockSpellCard();
            _ = mock2.SetupGet(m => m.Id).Returns(mock1.Object.Id + 1);

            return new[] { mock1.Object, mock2.Object };
        }

        internal static IReadOnlyDictionary<int, ISpellCard> SpellCards { get; } =
            CreateSpellCards().ToDictionary(card => card.Id);

        [TestMethod]
        public void CareerReplacerTest()
        {
            var replacer = new CareerReplacer(SpellCards);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void CareerReplacerTestNull()
            => _ = Assert.ThrowsException<ArgumentNullException>(() => _ = new CareerReplacer(null!));

        [TestMethod]
        public void CareerReplacerTestEmpty()
        {
            var cards = new Dictionary<int, ISpellCard>();
            var replacer = new CareerReplacer(cards);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestNoIceCount()
        {
            var replacer = new CareerReplacer(SpellCards);
            Assert.AreEqual("34", replacer.Replace("%T128C0781"));
        }

        [TestMethod]
        public void ReplaceTestNoMissCount()
        {
            var replacer = new CareerReplacer(SpellCards);
            Assert.AreEqual("12", replacer.Replace("%T128C0782"));
        }

        [TestMethod]
        public void ReplaceTestTrialCount()
        {
            var replacer = new CareerReplacer(SpellCards);
            Assert.AreEqual("56", replacer.Replace("%T128C0783"));
        }

        [TestMethod]
        public void ReplaceTestTotalNoIceCount()
        {
            var replacer = new CareerReplacer(SpellCards);
            Assert.AreEqual("68", replacer.Replace("%T128C0001"));
        }

        [TestMethod]
        public void ReplaceTestTotalNoMissCount()
        {
            var replacer = new CareerReplacer(SpellCards);
            Assert.AreEqual("24", replacer.Replace("%T128C0002"));
        }

        [TestMethod]
        public void ReplaceTestTotalTrialCount()
        {
            var replacer = new CareerReplacer(SpellCards);
            Assert.AreEqual("112", replacer.Replace("%T128C0003"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var cards = new Dictionary<int, ISpellCard>();
            var replacer = new CareerReplacer(cards);
            Assert.AreEqual("0", replacer.Replace("%T128C0781"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CareerReplacer(SpellCards);
            Assert.AreEqual("%T128X0781", replacer.Replace("%T128X0781"));
        }

        [TestMethod]
        public void ReplaceTestInvalidNumber()
        {
            var replacer = new CareerReplacer(SpellCards);
            Assert.AreEqual("%T128C2511", replacer.Replace("%T128C2511"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CareerReplacer(SpellCards);
            Assert.AreEqual("%T128C078X", replacer.Replace("%T128C078X"));
        }
    }
}

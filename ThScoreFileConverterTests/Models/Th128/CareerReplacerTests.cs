using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th128;
using ThScoreFileConverterTests.Models.Th128.Stubs;

namespace ThScoreFileConverterTests.Models.Th128
{
    [TestClass]
    public class CareerReplacerTests
    {
        internal static IReadOnlyDictionary<int, ISpellCard> SpellCards { get; } =
            new List<ISpellCard>
            {
                SpellCardTests.ValidStub,
                new SpellCardStub(SpellCardTests.ValidStub)
                {
                    Id = SpellCardTests.ValidStub.Id + 1,
                }
            }.ToDictionary(element => element.Id);

        [TestMethod]
        public void CareerReplacerTest()
        {
            var replacer = new CareerReplacer(SpellCards);
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

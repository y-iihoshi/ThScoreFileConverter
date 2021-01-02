using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models;
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

        private static Mock<INumberFormatter> MockNumberFormatter()
        {
            var mock = new Mock<INumberFormatter>();
            _ = mock.Setup(formatter => formatter.FormatNumber(It.IsAny<It.IsValueType>()))
                .Returns((object value) => "invoked: " + value.ToString());
            return mock;
        }

        [TestMethod]
        public void CareerReplacerTest()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(SpellCards, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void CareerReplacerTestEmpty()
        {
            var cards = new Dictionary<int, ISpellCard>();
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(cards, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestNoIceCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(SpellCards, formatterMock.Object);
            Assert.AreEqual("invoked: 34", replacer.Replace("%T128C0781"));
        }

        [TestMethod]
        public void ReplaceTestNoMissCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(SpellCards, formatterMock.Object);
            Assert.AreEqual("invoked: 12", replacer.Replace("%T128C0782"));
        }

        [TestMethod]
        public void ReplaceTestTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(SpellCards, formatterMock.Object);
            Assert.AreEqual("invoked: 56", replacer.Replace("%T128C0783"));
        }

        [TestMethod]
        public void ReplaceTestTotalNoIceCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(SpellCards, formatterMock.Object);
            Assert.AreEqual("invoked: 68", replacer.Replace("%T128C0001"));
        }

        [TestMethod]
        public void ReplaceTestTotalNoMissCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(SpellCards, formatterMock.Object);
            Assert.AreEqual("invoked: 24", replacer.Replace("%T128C0002"));
        }

        [TestMethod]
        public void ReplaceTestTotalTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(SpellCards, formatterMock.Object);
            Assert.AreEqual("invoked: 112", replacer.Replace("%T128C0003"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var cards = new Dictionary<int, ISpellCard>();
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(cards, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T128C0781"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(SpellCards, formatterMock.Object);
            Assert.AreEqual("%T128X0781", replacer.Replace("%T128X0781"));
        }

        [TestMethod]
        public void ReplaceTestInvalidNumber()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(SpellCards, formatterMock.Object);
            Assert.AreEqual("%T128C2511", replacer.Replace("%T128C2511"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(SpellCards, formatterMock.Object);
            Assert.AreEqual("%T128C078X", replacer.Replace("%T128C078X"));
        }
    }
}

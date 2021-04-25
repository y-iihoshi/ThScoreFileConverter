using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models.Th105;
using ThScoreFileConverterTests.UnitTesting;

namespace ThScoreFileConverterTests.Models.Th105
{
    [TestClass]
    public class CardForDeckTests
    {
        internal static Mock<ICardForDeck> MockCardForDeck()
        {
            var mock = new Mock<ICardForDeck>();
            _ = mock.SetupGet(m => m.Id).Returns(1);
            _ = mock.SetupGet(m => m.MaxNumber).Returns(2);
            return mock;
        }

        internal static byte[] MakeByteArray(ICardForDeck cardForDeck)
        {
            return TestUtils.MakeByteArray(cardForDeck.Id, cardForDeck.MaxNumber);
        }

        internal static void Validate(ICardForDeck expected, ICardForDeck actual)
        {
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.MaxNumber, actual.MaxNumber);
        }

        [TestMethod]
        public void CardForDeckTest()
        {
            var mock = new Mock<ICardForDeck>();
            var cardForDeck = new CardForDeck();

            Validate(mock.Object, cardForDeck);
        }

        [TestMethod]
        public void ReadFromTest()
        {
            var mock = MockCardForDeck();
            var cardForDeck = TestUtils.Create<CardForDeck>(MakeByteArray(mock.Object));

            Validate(mock.Object, cardForDeck);
        }

        [TestMethod]
        public void ReadFromTestShortened()
        {
            var mock = MockCardForDeck();
            var array = MakeByteArray(mock.Object).SkipLast(1).ToArray();

            _ = Assert.ThrowsException<EndOfStreamException>(() => _ = TestUtils.Create<CardForDeck>(array));
        }

        [TestMethod]
        public void ReadFromTestExceeded()
        {
            var mock = MockCardForDeck();
            var array = MakeByteArray(mock.Object).Concat(new byte[1] { 1 }).ToArray();

            var cardForDeck = TestUtils.Create<CardForDeck>(array);

            Validate(mock.Object, cardForDeck);
        }
    }
}

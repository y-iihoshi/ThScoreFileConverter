using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models.Th105;
using ThScoreFileConverterTests.Models.Th105.Stubs;

namespace ThScoreFileConverterTests.Models.Th105
{
    [TestClass]
    public class CardForDeckTests
    {
        internal static CardForDeckStub ValidStub { get; } = new CardForDeckStub
        {
            Id = 1,
            MaxNumber = 2,
        };

        internal static byte[] MakeByteArray(ICardForDeck cardForDeck)
            => TestUtils.MakeByteArray(cardForDeck.Id, cardForDeck.MaxNumber);

        internal static void Validate(ICardForDeck expected, ICardForDeck actual)
        {
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.MaxNumber, actual.MaxNumber);
        }

        [TestMethod]
        public void CardForDeckTest()
        {
            var stub = new CardForDeckStub();

            var cardForDeck = new CardForDeck();

            Validate(stub, cardForDeck);
        }

        [TestMethod]
        public void ReadFromTest()
        {
            var stub = ValidStub;

            var cardForDeck = TestUtils.Create<CardForDeck>(MakeByteArray(stub));

            Validate(stub, cardForDeck);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull()
        {
            var cardForDeck = new CardForDeck();
            cardForDeck.ReadFrom(null!);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestShortened()
        {
            var stub = ValidStub;
            var array = MakeByteArray(stub).SkipLast(1).ToArray();

            _ = TestUtils.Create<CardForDeck>(array);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void ReadFromTestExceeded()
        {
            var stub = ValidStub;
            var array = MakeByteArray(stub).Concat(new byte[1] { 1 }).ToArray();

            var cardForDeck = TestUtils.Create<CardForDeck>(array);

            Validate(stub, cardForDeck);
        }
    }
}

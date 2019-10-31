using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models.Th105;

namespace ThScoreFileConverterTests.Models.Th105
{
    [TestClass]
    public class CardForDeckTests
    {
        internal struct Properties
        {
            public int id;
            public int maxNumber;
        };

        internal static Properties ValidProperties { get; } = new Properties()
        {
            id = 1,
            maxNumber = 2
        };

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(properties.id, properties.maxNumber);

        internal static void Validate(in Properties expected, in CardForDeck actual)
        {
            Assert.AreEqual(expected.id, actual.Id);
            Assert.AreEqual(expected.maxNumber, actual.MaxNumber);
        }

        [TestMethod]
        public void CardForDeckTest() => TestUtils.Wrap(() =>
        {
            var properties = new Properties();

            var cardForDeck = new CardForDeck();

            Validate(properties, cardForDeck);
        });

        [TestMethod]
        public void ReadFromTest() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var cardForDeck = TestUtils.Create<CardForDeck>(MakeByteArray(properties));

            Validate(properties, cardForDeck);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var cardForDeck = new CardForDeck();
            cardForDeck.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestShortened() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            var array = MakeByteArray(properties).SkipLast(1).ToArray();

            _ = TestUtils.Create<CardForDeck>(array);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void ReadFromTestExceeded() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            var array = MakeByteArray(properties).Concat(new byte[1] { 1 }).ToArray();

            var cardForDeck = TestUtils.Create<CardForDeck>(array);

            Validate(properties, cardForDeck);
        });
    }
}

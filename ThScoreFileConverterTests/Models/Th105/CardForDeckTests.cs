using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
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

        internal static Properties ValidProperties => new Properties()
        {
            id = 1,
            maxNumber = 2
        };

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(properties.id, properties.maxNumber);

        internal static CardForDeck Create(byte[] array)
        {
            var cardForDeck = new CardForDeck();

            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(array);
                using (var reader = new BinaryReader(stream))
                {
                    stream = null;
                    cardForDeck.ReadFrom(reader);
                }
            }
            finally
            {
                stream?.Dispose();
            }

            return cardForDeck;
        }

        internal static void Validate(in CardForDeck cardForDeck, in Properties properties)
        {
            Assert.AreEqual(properties.id, cardForDeck.Id);
            Assert.AreEqual(properties.maxNumber, cardForDeck.MaxNumber);
        }

        [TestMethod]
        public void CardForDeckTest()
            => TestUtils.Wrap(() =>
            {
                var properties = new Properties();

                var cardForDeck = new CardForDeck();

                Validate(cardForDeck, properties);
            });

        [TestMethod]
        public void ReadFromTest()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;

                var cardForDeck = Create(MakeByteArray(properties));

                Validate(cardForDeck, properties);
            });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull()
            => TestUtils.Wrap(() =>
            {
                var cardForDeck = new CardForDeck();
                cardForDeck.ReadFrom(null);

                Assert.Fail(TestUtils.Unreachable);
            });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestShortened()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                var array = MakeByteArray(properties);
                array = array.Take(array.Length - 1).ToArray();

                Create(array);

                Assert.Fail(TestUtils.Unreachable);
            });

        [TestMethod]
        public void ReadFromTestExceeded()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                var array = MakeByteArray(properties).Concat(new byte[1] { 1 }).ToArray();

                var cardForDeck = Create(array);

                Validate(cardForDeck, properties);
            });
    }
}

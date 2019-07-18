using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using ThScoreFileConverterTests.Models.Wrappers;
using CardForDeck = ThScoreFileConverter.Models.Th105.CardForDeck;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th105CardForDeckTests
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

        internal static void Validate(in Th105CardForDeckWrapper cardForDeck, in Properties properties)
            => Validate(cardForDeck.Target as CardForDeck, properties);

        internal static void Validate(in CardForDeck cardForDeck, in Properties properties)
        {
            Assert.AreEqual(properties.id, cardForDeck.Id);
            Assert.AreEqual(properties.maxNumber, cardForDeck.MaxNumber);
        }

        [TestMethod]
        public void Th105CardForDeckTest()
            => TestUtils.Wrap(() =>
            {
                var properties = new Properties();

                var cardForDeck = new Th105CardForDeckWrapper();

                Validate(cardForDeck, properties);
            });

        [TestMethod]
        public void Th105CardForDeckReadFromTest()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;

                var cardForDeck = Th105CardForDeckWrapper.Create(MakeByteArray(properties));

                Validate(cardForDeck, properties);
            });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th105CardForDeckReadFromTestNull()
            => TestUtils.Wrap(() =>
            {
                var cardForDeck = new Th105CardForDeckWrapper();
                cardForDeck.ReadFrom(null);

                Assert.Fail(TestUtils.Unreachable);
            });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th105CardForDeckReadFromTestShortened()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                var array = MakeByteArray(properties);
                array = array.Take(array.Length - 1).ToArray();

                Th105CardForDeckWrapper.Create(array);

                Assert.Fail(TestUtils.Unreachable);
            });

        [TestMethod]
        public void Th105CardForDeckReadFromTestExceeded()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                var array = MakeByteArray(properties).Concat(new byte[1] { 1 }).ToArray();

                var cardForDeck = Th105CardForDeckWrapper.Create(array);

                Validate(cardForDeck, properties);
            });
    }
}

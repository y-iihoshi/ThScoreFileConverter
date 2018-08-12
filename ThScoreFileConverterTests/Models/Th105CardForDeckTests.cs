using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;

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

        internal static void Validate<TParent>(
            in Th105CardForDeckWrapper<TParent> cardForDeck, in Properties properties)
            where TParent : ThConverter
        {
            Assert.AreEqual(properties.id, cardForDeck.Id);
            Assert.AreEqual(properties.maxNumber, cardForDeck.MaxNumber);
        }

        internal static void CardForDeckTestHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = new Properties();

                var cardForDeck = new Th105CardForDeckWrapper<TParent>();

                Validate(cardForDeck, properties);
            });

        internal static void ReadFromTestHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;

                var cardForDeck = Th105CardForDeckWrapper<TParent>.Create(MakeByteArray(properties));

                Validate(cardForDeck, properties);
            });

        internal static void ReadFromTestNullHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var cardForDeck = new Th105CardForDeckWrapper<TParent>();
                cardForDeck.ReadFrom(null);

                Assert.Fail(TestUtils.Unreachable);
            });

        internal static void ReadFromTestShortenedHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                var array = MakeByteArray(properties);
                array = array.Take(array.Length - 1).ToArray();

                Th105CardForDeckWrapper<TParent>.Create(array);

                Assert.Fail(TestUtils.Unreachable);
            });

        internal static void ReadFromTestExceededHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                var array = MakeByteArray(properties).Concat(new byte[1] { 1 }).ToArray();

                var cardForDeck = Th105CardForDeckWrapper<TParent>.Create(array);

                Validate(cardForDeck, properties);
            });

        #region Th105

        [TestMethod]
        public void Th105CardForDeckTest()
            => CardForDeckTestHelper<Th105Converter>();

        [TestMethod]
        public void Th105CardForDeckReadFromTest()
            => ReadFromTestHelper<Th105Converter>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th105CardForDeckReadFromTestNull()
            => ReadFromTestNullHelper<Th105Converter>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th105CardForDeckReadFromTestShortened()
            => ReadFromTestShortenedHelper<Th105Converter>();

        [TestMethod]
        public void Th105CardForDeckReadFromTestExceeded()
            => ReadFromTestExceededHelper<Th105Converter>();

        #endregion

        #region Th123

        [TestMethod]
        public void Th123CardForDeckTest()
            => CardForDeckTestHelper<Th123Converter>();

        [TestMethod]
        public void Th123CardForDeckReadFromTest()
            => ReadFromTestHelper<Th123Converter>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th123CardForDeckReadFromTestNull()
            => ReadFromTestNullHelper<Th123Converter>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th123CardForDeckReadFromTestShortened()
            => ReadFromTestShortenedHelper<Th123Converter>();

        [TestMethod]
        public void Th123CardForDeckReadFromTestExceeded()
            => ReadFromTestExceededHelper<Th123Converter>();

        #endregion
    }
}

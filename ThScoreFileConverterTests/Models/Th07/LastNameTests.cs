using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th07;
using ThScoreFileConverterTests.Extensions;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;

namespace ThScoreFileConverterTests.Models.Th07
{
    [TestClass]
    public class LastNameTests
    {
        internal struct Properties
        {
            public string signature;
            public short size1;
            public short size2;
            public byte[] name;
        }

        internal static Properties ValidProperties { get; } = new Properties()
        {
            signature = "LSNM",
            size1 = 0x18,
            size2 = 0x18,
            name = TestUtils.MakeRandomArray<byte>(12),
        };

        internal static byte[] MakeData(in Properties properties)
            => TestUtils.MakeByteArray(0u, properties.name);

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(), properties.size1, properties.size2, MakeData(properties));

        internal static void Validate(in Properties expected, in LastName actual)
        {
            var data = MakeData(expected);

            Assert.AreEqual(expected.signature, actual.Signature);
            Assert.AreEqual(expected.size1, actual.Size1);
            Assert.AreEqual(expected.size2, actual.Size2);
            Assert.AreEqual(data[0], actual.FirstByteOfData);
            CollectionAssert.That.AreEqual(expected.name, actual.Name);
        }

        [TestMethod]
        public void LastNameTestChapter()
        {
            var properties = ValidProperties;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(properties));
            var lastName = new LastName(chapter);

            Validate(properties, lastName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LastNameTestNullChapter()
        {
            _ = new LastName(null!);

            Assert.Fail(TestUtils.Unreachable);
        }

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void LastNameTestInvalidSignature()
        {
            var properties = ValidProperties;
            properties.signature = properties.signature.ToLowerInvariant();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(properties));
            _ = new LastName(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void LastNameTestInvalidSize1()
        {
            var properties = ValidProperties;
            --properties.size1;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(properties));
            _ = new LastName(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }
    }
}

using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th10;
using ThScoreFileConverterTests.Models.Th10.Wrappers;
using ThScoreFileConverterTests.UnitTesting;
using IChapter = ThScoreFileConverter.Models.Th095.IChapter;

namespace ThScoreFileConverterTests.Models.Th10
{
    [TestClass]
    public class ChapterTests
    {
        internal struct Properties
        {
            public string signature;
            public ushort version;
            public uint checksum;
            public int size;
            public byte[] data;
        }

        internal static Properties DefaultProperties { get; } = new Properties()
        {
            signature = string.Empty,
            version = default,
            checksum = default,
            size = default,
            data = Array.Empty<byte>(),
        };

        internal static Properties ValidProperties { get; } = new Properties()
        {
            signature = "AB",
            version = 1234,
            checksum = 0xE0u,
            size = 16,
            data = new byte[] { 0x01, 0x23, 0x45, 0x67 },
        };

        internal static byte[] MakeByteArray(in Properties properties)
        {
            return TestUtils.MakeByteArray(
                properties.signature.ToCharArray(),
                properties.version,
                properties.checksum,
                properties.size,
                properties.data);
        }

        internal static void Validate(in Properties expected, IChapter actual)
        {
            Assert.AreEqual(expected.signature, actual.Signature);
            Assert.AreEqual(expected.version, actual.Version);
            Assert.AreEqual(expected.checksum, actual.Checksum);
            Assert.AreEqual(expected.size, actual.Size);
        }

        internal static void Validate(in Properties expected, in ChapterWrapper actual)
        {
            Validate(expected, actual as IChapter);
            CollectionAssert.That.AreEqual(expected.data, actual.Data);
        }

        [TestMethod]
        public void ChapterTest()
        {
            var chapter = new ChapterWrapper();

            Validate(DefaultProperties, chapter);
            Assert.IsTrue(chapter.IsValid);
        }

        [TestMethod]
        public void ChapterTestCopy()
        {
            var chapter1 = new Chapter();
            var chapter2 = new ChapterWrapper(chapter1);

            Validate(DefaultProperties, chapter2);
            Assert.IsTrue(chapter2.IsValid);
        }

        [TestMethod]
        public void ChapterTestCopyWithExpected()
        {
            var chapter1 = TestUtils.Create<Chapter>(MakeByteArray(ValidProperties));
            var chapter2 = new ChapterWrapper(chapter1, chapter1.Signature, chapter1.Version, chapter1.Size);

            Validate(ValidProperties, chapter2);
            Assert.IsTrue(chapter2.IsValid);
        }

        [TestMethod]
        public void ChapterTestInvalidSignature()
        {
            var chapter = TestUtils.Create<Chapter>(MakeByteArray(ValidProperties));
            _ = Assert.ThrowsException<InvalidDataException>(
                () => _ = new ChapterWrapper(
                    chapter, chapter.Signature.ToLowerInvariant(), chapter.Version, chapter.Size));
        }

        [TestMethod]
        public void ChapterTestInvalidVersion()
        {
            var chapter = TestUtils.Create<Chapter>(MakeByteArray(ValidProperties));
            _ = Assert.ThrowsException<InvalidDataException>(
                () => _ = new ChapterWrapper(chapter, chapter.Signature, (ushort)(chapter.Version - 1), chapter.Size));
        }

        [TestMethod]
        public void ChapterTestInvalidSize()
        {
            var chapter = TestUtils.Create<Chapter>(MakeByteArray(ValidProperties));
            _ = Assert.ThrowsException<InvalidDataException>(
                () => _ = new ChapterWrapper(chapter, chapter.Signature, chapter.Version, chapter.Size - 1));
        }

        [TestMethod]
        public void ReadFromTest()
        {
            var chapter = TestUtils.Create<ChapterWrapper>(MakeByteArray(ValidProperties));

            Validate(ValidProperties, chapter);
            Assert.IsTrue(chapter.IsValid);
        }

        [TestMethod]
        public void ReadFromTestEmptySignature()
        {
            var properties = ValidProperties;
            properties.signature = string.Empty;

            // <sig> <ver> < chksum -> <- size --> <- data -->
            // __ __ d2 04 E0 00 00 00 10 00 00 00 01 23 45 67
            //       <sig> <ver> < chksum -> <- size --> <dat>

            // The actual value of the Size property becomes too large,
            // so EndOfStreamException will be thrown.
            _ = Assert.ThrowsException<EndOfStreamException>(
                () => _ = TestUtils.Create<Chapter>(MakeByteArray(properties)));
        }

        [TestMethod]
        public void ReadFromTestShortenedSignature()
        {
            var properties = ValidProperties;
#if NETFRAMEWORK
            properties.signature = properties.signature.Substring(0, properties.signature.Length - 1);
#else
            properties.signature = properties.signature[0..^1];
#endif

            // <sig> <ver> < chksum -> <- size --> <- data -->
            // __ 41 d2 04 E0 00 00 00 10 00 00 00 01 23 45 67
            //    <sig> <ver> < chksum -> <- size --> < data >

            // The actual value of the Size property becomes too large,
            // so EndOfStreamException will be thrown.
            _ = Assert.ThrowsException<EndOfStreamException>(
                () => _ = TestUtils.Create<Chapter>(MakeByteArray(properties)));
        }

        [TestMethod]
        public void ReadFromTestExceededSignature()
        {
            var properties = ValidProperties;
            properties.signature += "C";

            // < sig -> <ver> < chksum -> <- size --> <- data -->
            // 41 42 43 d2 04 E0 00 00 00 10 00 00 00 01 23 45 67
            // <sig> <ver> < chksum -> <- size --> <---- data ---->

            // The actual value of the Size property becomes large,
            // so EndOfStreamException will be thrown.
            _ = Assert.ThrowsException<EndOfStreamException>(
                () => _ = TestUtils.Create<Chapter>(MakeByteArray(properties)));
        }

        [TestMethod]
        public void ReadFromTestNegativeSize()
        {
            var properties = ValidProperties;
            properties.size = -1;

            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => _ = TestUtils.Create<Chapter>(MakeByteArray(properties)));
        }

        [TestMethod]
        public void ReadFromTestZeroSize()
        {
            var properties = ValidProperties;
            properties.size = 0;

            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => _ = TestUtils.Create<Chapter>(MakeByteArray(properties)));
        }

        [TestMethod]
        public void ReadFromTestShortenedSize()
        {
            var properties = ValidProperties;
            --properties.size;

            var chapter = TestUtils.Create<ChapterWrapper>(MakeByteArray(properties));

            Validate(properties, chapter as IChapter);
            CollectionAssert.That.AreNotEqual(properties.data, chapter.Data);
            Assert.IsFalse(chapter.IsValid);
        }

        [TestMethod]
        public void ReadFromTestExceededSize()
        {
            var properties = ValidProperties;
            ++properties.size;

            _ = Assert.ThrowsException<EndOfStreamException>(
                () => _ = TestUtils.Create<Chapter>(MakeByteArray(properties)));
        }

        [TestMethod]
        public void ReadFromTestInvalidChecksum()
        {
            var properties = ValidProperties;
            --properties.checksum;

            var chapter = TestUtils.Create<ChapterWrapper>(MakeByteArray(properties));

            Validate(properties, chapter);
            Assert.IsFalse(chapter.IsValid);
        }

        [TestMethod]
        public void ReadFromTestEmptyData()
        {
            var properties = ValidProperties;
            properties.data = Array.Empty<byte>();

            _ = Assert.ThrowsException<EndOfStreamException>(
                () => _ = TestUtils.Create<Chapter>(MakeByteArray(properties)));
        }

        [TestMethod]
        public void ReadFromTestMisalignedData()
        {
            var properties = ValidProperties;
            --properties.size;
            properties.data = properties.data.Take(properties.data.Length - 1).ToArray();

            var chapter = TestUtils.Create<ChapterWrapper>(MakeByteArray(properties));

            Validate(properties, chapter);
            Assert.IsFalse(chapter.IsValid);
        }
    }
}

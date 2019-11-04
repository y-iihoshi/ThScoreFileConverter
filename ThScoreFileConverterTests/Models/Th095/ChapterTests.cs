using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th095.Wrappers;

namespace ThScoreFileConverterTests.Models.Th095
{
    [TestClass]
    public class ChapterTests
    {
        internal struct Properties
        {
            public string signature;
            public ushort version;
            public int size;
            public uint checksum;
            public byte[] data;
        };

        internal static Properties DefaultProperties { get; } = new Properties()
        {
            signature = string.Empty,
            version = default,
            size = default,
            checksum = default,
            data = new byte[] { }
        };

        internal static Properties ValidProperties { get; } = new Properties()
        {
            signature = "AB",
            version = 1234,
            size = 16,
            checksum = 0xC16CBAA7u,
            data = new byte[] { 0x56, 0x78, 0x9A, 0xBC }
        };

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(),
                properties.version,
                properties.size,
                properties.checksum,
                properties.data);

        internal static void Validate(in Properties expected, in ChapterWrapper actual)
        {
            Assert.AreEqual(expected.signature, actual.Signature);
            Assert.AreEqual(expected.version, actual.Version);
            Assert.AreEqual(expected.size, actual.Size);
            Assert.AreEqual(expected.checksum, actual.Checksum);
            CollectionAssert.That.AreEqual(expected.data, actual.Data);
        }

        [TestMethod]
        public void ChapterTest() => TestUtils.Wrap(() =>
        {
            var chapter = new ChapterWrapper();

            Validate(DefaultProperties, chapter);
            Assert.IsFalse(chapter.IsValid.Value);
        });

        [TestMethod]
        public void ChapterTestCopy() => TestUtils.Wrap(() =>
        {
            var chapter1 = new ChapterWrapper();
            var chapter2 = new ChapterWrapper(chapter1);

            Validate(DefaultProperties, chapter2);
            Assert.IsFalse(chapter2.IsValid.Value);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ChapterTestNull() => TestUtils.Wrap(() =>
        {
            _ = new ChapterWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void ReadFromTest() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(MakeByteArray(ValidProperties));

            Validate(ValidProperties, chapter);
            Assert.IsTrue(chapter.IsValid.Value);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var chapter = new ChapterWrapper();
            chapter.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ReadFromTestEmptySignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = string.Empty;

            // <sig> <ver> <- size --> < chksum -> <- data -->
            // __ __ d2 04 10 00 00 00 a7 ba 6c c1 56 78 9a bc
            //       <sig> <ver> <- size --> < chksum -> <dat>

            // The actual value of the Size property becomes negative,
            // so ArgumentOutOfRangeException will be thrown.
            ChapterWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ReadFromTestShortenedSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = properties.signature.Substring(0, properties.signature.Length - 1);

            // <sig> <ver> <- size --> < chksum -> <- data -->
            // __ 41 d2 04 10 00 00 00 a7 ba 6c c1 56 78 9a bc
            //    <sig> <ver> <- size --> < chksum -> < data >

            // The actual value of the Size property becomes negative,
            // so ArgumentOutOfRangeException will be thrown.
            ChapterWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestExceededSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature += "C";

            // < sig -> <ver> <- size --> < chksum -> <- data -->
            // 41 42 43 d2 04 10 00 00 00 a7 ba 6c c1 56 78 9a bc
            // <sig> <ver> <- size --> < chksum -> <---- data ---->

            // The actual value of the Size property becomes too large,
            // so EndOfStreamException will be thrown.
            ChapterWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ReadFromTestNegativeSize() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.size = -1;

            ChapterWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ReadFromTestZeroSize() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.size = 0;

            ChapterWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void ReadFromTestShortenedSize() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            --properties.size;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));

            Assert.AreEqual(properties.signature, chapter.Signature);
            Assert.AreEqual(properties.version, chapter.Version);
            Assert.AreEqual(properties.size, chapter.Size);
            Assert.AreEqual(properties.checksum, chapter.Checksum);
            CollectionAssert.That.AreNotEqual(properties.data, chapter.Data);
            Assert.IsFalse(chapter.IsValid.Value);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestExceededSize() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            ++properties.size;

            ChapterWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void ReadFromTestInvalidChecksum() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            --properties.checksum;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));

            Validate(properties, chapter);
            Assert.IsFalse(chapter.IsValid.Value);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestEmptyData() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.data = new byte[] { };

            ChapterWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void ReadFromTestMisalignedData() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            --properties.size;
            properties.data = properties.data.Take(properties.data.Length - 1).ToArray();

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));

            Validate(properties, chapter);
            Assert.IsFalse(chapter.IsValid.Value);
        });
    }
}

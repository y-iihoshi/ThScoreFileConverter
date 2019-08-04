using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverterTests.Models.Th10.Wrappers;

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
        };

        internal static Properties DefaultProperties => new Properties()
        {
            signature = string.Empty,
            version = default,
            checksum = default,
            size = default,
            data = new byte[] { }
        };

        internal static Properties ValidProperties => new Properties()
        {
            signature = "AB",
            version = 1234,
            checksum = 0x234u,
            size = 16,
            data = new byte[] { 0x56, 0x78, 0x9A, 0xBC }
        };

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(),
                properties.version,
                properties.checksum,
                properties.size,
                properties.data);

        internal static void Validate(in ChapterWrapper chapter, in Properties properties)
        {
            Assert.AreEqual(properties.signature, chapter.Signature);
            Assert.AreEqual(properties.version, chapter.Version);
            Assert.AreEqual(properties.checksum, chapter.Checksum);
            Assert.AreEqual(properties.size, chapter.Size);
            CollectionAssert.AreEqual(properties.data, chapter.Data.ToArray());
        }

        [TestMethod]
        public void Th10ChapterTest()
            => TestUtils.Wrap(() =>
            {
                var chapter = new ChapterWrapper();

                Validate(chapter, DefaultProperties);
                Assert.IsTrue(chapter.IsValid.Value);
            });

        [TestMethod]
        public void Th10ChapterTestCopy()
            => TestUtils.Wrap(() =>
            {
                var chapter1 = new ChapterWrapper();
                var chapter2 = new ChapterWrapper(chapter1);

                Validate(chapter2, DefaultProperties);
                Assert.IsTrue(chapter2.IsValid.Value);
            });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "chapter")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th10ChapterTestNull()
            => TestUtils.Wrap(() =>
            {
                var chapter = new ChapterWrapper(null);

                Assert.Fail(TestUtils.Unreachable);
            });

        [TestMethod]
        public void Th10ChapterReadFromTest()
            => TestUtils.Wrap(() =>
            {
                var chapter = ChapterWrapper.Create(MakeByteArray(ValidProperties));

                Validate(chapter, ValidProperties);
                Assert.IsTrue(chapter.IsValid.Value);
            });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th10ChapterReadFromTestNull()
            => TestUtils.Wrap(() =>
            {
                var chapter = new ChapterWrapper();
                chapter.ReadFrom(null);

                Assert.Fail(TestUtils.Unreachable);
            });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th10ChapterReadFromTestEmptySignature()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.signature = string.Empty;

                // <sig> <ver> < chksum -> <- size --> <- data -->
                // __ __ d2 04 34 02 00 00 10 00 00 00 56 78 9a bc
                //       <sig> <ver> < chksum -> <- size --> <dat>

                // The actual value of the Size property becomes too large,
                // so EndOfStreamException will be thrown.
                ChapterWrapper.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th10ChapterReadFromTestShortenedSignature()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.signature = properties.signature.Substring(0, properties.signature.Length - 1);

                // <sig> <ver> < chksum -> <- size --> <- data -->
                // __ 41 d2 04 34 02 00 00 10 00 00 00 56 78 9a bc
                //    <sig> <ver> < chksum -> <- size --> < data >

                // The actual value of the Size property becomes too large,
                // so EndOfStreamException will be thrown.
                ChapterWrapper.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th10ChapterReadFromTestExceededSignature()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.signature += "C";

                // < sig -> <ver> < chksum -> <- size --> <- data -->
                // 41 42 43 d2 04 34 02 00 00 10 00 00 00 56 78 9a bc
                // <sig> <ver> < chksum -> <- size --> <---- data ---->

                // The actual value of the Size property becomes large,
                // so EndOfStreamException will be thrown.
                ChapterWrapper.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th10ChapterReadFromTestNegativeSize()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.size = -1;

                ChapterWrapper.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th10ChapterReadFromTestZeroSize()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.size = 0;

                ChapterWrapper.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });


        [TestMethod]
        public void Th10ChapterReadFromTestShortenedSize()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                --properties.size;

                var chapter = ChapterWrapper.Create(MakeByteArray(properties));

                Assert.AreEqual(properties.signature, chapter.Signature);
                Assert.AreEqual(properties.version, chapter.Version);
                Assert.AreEqual(properties.checksum, chapter.Checksum);
                Assert.AreEqual(properties.size, chapter.Size);
                CollectionAssert.AreNotEqual(properties.data, chapter.Data.ToArray());
                Assert.IsFalse(chapter.IsValid.Value);
            });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th10ChapterReadFromTestExceededSize()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                ++properties.size;

                ChapterWrapper.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [TestMethod]
        public void Th10ChapterReadFromTestInvalidChecksum()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                --properties.checksum;

                var chapter = ChapterWrapper.Create(MakeByteArray(properties));

                Validate(chapter, properties);
                Assert.IsFalse(chapter.IsValid.Value);
            });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th10ChapterReadFromTestEmptyData()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.data = new byte[] { };

                ChapterWrapper.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [TestMethod]
        public void Th10ChapterReadFromTestMisalignedData()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                --properties.size;
                properties.data = properties.data.Take(properties.data.Length - 1).ToArray();

                var chapter = ChapterWrapper.Create(MakeByteArray(properties));

                Validate(chapter, properties);
                Assert.IsFalse(chapter.IsValid.Value);
            });
    }
}

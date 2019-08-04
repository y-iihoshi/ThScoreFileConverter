using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th095ChapterTests
    {
        internal struct Properties
        {
            public string signature;
            public ushort version;
            public int size;
            public uint checksum;
            public byte[] data;
        };

        internal static Properties DefaultProperties => new Properties()
        {
            signature = string.Empty,
            version = default,
            size = default,
            checksum = default,
            data = new byte[] { }
        };

        internal static Properties ValidProperties => new Properties()
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

        internal static void Validate(in Th095ChapterWrapper chapter, in Properties properties)
        {
            Assert.AreEqual(properties.signature, chapter.Signature);
            Assert.AreEqual(properties.version, chapter.Version);
            Assert.AreEqual(properties.size, chapter.Size);
            Assert.AreEqual(properties.checksum, chapter.Checksum);
            CollectionAssert.AreEqual(properties.data, chapter.Data.ToArray());
        }

        [TestMethod]
        public void Th095ChapterTest()
            => TestUtils.Wrap(() =>
            {
                var chapter = new Th095ChapterWrapper();

                Validate(chapter, DefaultProperties);
                Assert.IsFalse(chapter.IsValid.Value);
            });

        [TestMethod]
        public void Th095ChapterTestCopy()
            => TestUtils.Wrap(() =>
            {
                var chapter1 = new Th095ChapterWrapper();
                var chapter2 = new Th095ChapterWrapper(chapter1);

                Validate(chapter2, DefaultProperties);
                Assert.IsFalse(chapter2.IsValid.Value);
            });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "chapter")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th095ChapterTestNull()
            => TestUtils.Wrap(() =>
            {
                var chapter = new Th095ChapterWrapper(null);

                Assert.Fail(TestUtils.Unreachable);
            });

        [TestMethod]
        public void Th095ChapterReadFromTest()
            => TestUtils.Wrap(() =>
            {
                var chapter = Th095ChapterWrapper.Create(MakeByteArray(ValidProperties));

                Validate(chapter, ValidProperties);
                Assert.IsTrue(chapter.IsValid.Value);
            });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th095ChapterReadFromTestNull()
            => TestUtils.Wrap(() =>
            {
                var chapter = new Th095ChapterWrapper();
                chapter.ReadFrom(null);

                Assert.Fail(TestUtils.Unreachable);
            });

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th095ChapterReadFromTestEmptySignature()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.signature = string.Empty;

                // <sig> <ver> <- size --> < chksum -> <- data -->
                // __ __ d2 04 10 00 00 00 a7 ba 6c c1 56 78 9a bc
                //       <sig> <ver> <- size --> < chksum -> <dat>

                // The actual value of the Size property becomes negative,
                // so ArgumentOutOfRangeException will be thrown.
                Th095ChapterWrapper.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th095ChapterReadFromTestShortenedSignature()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.signature = properties.signature.Substring(0, properties.signature.Length - 1);

                // <sig> <ver> <- size --> < chksum -> <- data -->
                // __ 41 d2 04 10 00 00 00 a7 ba 6c c1 56 78 9a bc
                //    <sig> <ver> <- size --> < chksum -> < data >

                // The actual value of the Size property becomes negative,
                // so ArgumentOutOfRangeException will be thrown.
                Th095ChapterWrapper.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th095ChapterReadFromTestExceededSignature()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.signature += "C";

                // < sig -> <ver> <- size --> < chksum -> <- data -->
                // 41 42 43 d2 04 10 00 00 00 a7 ba 6c c1 56 78 9a bc
                // <sig> <ver> <- size --> < chksum -> <---- data ---->

                // The actual value of the Size property becomes too large,
                // so EndOfStreamException will be thrown.
                Th095ChapterWrapper.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th095ChapterReadFromTestNegativeSize()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.size = -1;

                Th095ChapterWrapper.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th095ChapterReadFromTestZeroSize()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.size = 0;

                Th095ChapterWrapper.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [TestMethod]
        public void Th095ChapterReadFromTestShortenedSize()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                --properties.size;

                var chapter = Th095ChapterWrapper.Create(MakeByteArray(properties));

                Assert.AreEqual(properties.signature, chapter.Signature);
                Assert.AreEqual(properties.version, chapter.Version);
                Assert.AreEqual(properties.size, chapter.Size);
                Assert.AreEqual(properties.checksum, chapter.Checksum);
                CollectionAssert.AreNotEqual(properties.data, chapter.Data.ToArray());
                Assert.IsFalse(chapter.IsValid.Value);
            });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th095ChapterReadFromTestExceededSize()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                ++properties.size;

                Th095ChapterWrapper.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [TestMethod]
        public void Th095ChapterReadFromTestInvalidChecksum()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                --properties.checksum;

                var chapter = Th095ChapterWrapper.Create(MakeByteArray(properties));

                Validate(chapter, properties);
                Assert.IsFalse(chapter.IsValid.Value);
            });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th095ChapterReadFromTestEmptyData()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.data = new byte[] { };

                Th095ChapterWrapper.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [TestMethod]
        public void Th095ChapterReadFromTestMisalignedData()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                --properties.size;
                properties.data = properties.data.Take(properties.data.Length - 1).ToArray();

                var chapter = Th095ChapterWrapper.Create(MakeByteArray(properties));

                Validate(chapter, properties);
                Assert.IsFalse(chapter.IsValid.Value);
            });
    }
}

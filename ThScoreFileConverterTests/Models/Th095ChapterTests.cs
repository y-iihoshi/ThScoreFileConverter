using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass()]
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

        internal static void Validate<TParent>(in Th095ChapterWrapper<TParent> chapter, in Properties properties)
            where TParent : ThConverter
        {
            Assert.AreEqual(properties.signature, chapter.Signature);
            Assert.AreEqual(properties.version, chapter.Version);
            Assert.AreEqual(properties.size, chapter.Size);
            Assert.AreEqual(properties.checksum, chapter.Checksum);
            CollectionAssert.AreEqual(properties.data, chapter.Data.ToArray());
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ChapterTestHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var chapter = new Th095ChapterWrapper<TParent>();

                Validate(chapter, DefaultProperties);
                Assert.IsFalse(chapter.IsValid.Value);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ChapterTestCopyHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var chapter1 = new Th095ChapterWrapper<TParent>();
                var chapter2 = new Th095ChapterWrapper<TParent>(chapter1);

                Validate(chapter2, DefaultProperties);
                Assert.IsFalse(chapter2.IsValid.Value);
            });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "chapter")]
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ChapterTestNullHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var chapter = new Th095ChapterWrapper<TParent>(null);

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var chapter = Th095ChapterWrapper<TParent>.Create(MakeByteArray(ValidProperties));

                Validate(chapter, ValidProperties);
                Assert.IsTrue(chapter.IsValid.Value);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestNullHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var chapter = new Th095ChapterWrapper<TParent>();
                chapter.ReadFrom(null);

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestEmptySignatureHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.signature = string.Empty;

                // <sig> <ver> <- size --> < chksum -> <- data -->
                // __ __ d2 04 10 00 00 00 a7 ba 6c c1 56 78 9a bc
                //       <sig> <ver> <- size --> < chksum -> <dat>

                // The actual value of the Size property becomes negative,
                // so ArgumentOutOfRangeException will be thrown.
                Th095ChapterWrapper<TParent>.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestShortenedSignatureHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.signature = properties.signature.Substring(0, properties.signature.Length - 1);

                // <sig> <ver> <- size --> < chksum -> <- data -->
                // __ 41 d2 04 10 00 00 00 a7 ba 6c c1 56 78 9a bc
                //    <sig> <ver> <- size --> < chksum -> < data >

                // The actual value of the Size property becomes negative,
                // so ArgumentOutOfRangeException will be thrown.
                Th095ChapterWrapper<TParent>.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestExceededSignatureHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.signature += "C";

                // < sig -> <ver> <- size --> < chksum -> <- data -->
                // 41 42 43 d2 04 10 00 00 00 a7 ba 6c c1 56 78 9a bc
                // <sig> <ver> <- size --> < chksum -> <---- data ---->

                // The actual value of the Size property becomes too large,
                // so EndOfStreamException will be thrown.
                Th095ChapterWrapper<TParent>.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestNegativeSizeHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.size = -1;

                Th095ChapterWrapper<TParent>.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestZeroSizeHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.size = 0;

                Th095ChapterWrapper<TParent>.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestShortenedSizeHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                --properties.size;

                var chapter = Th095ChapterWrapper<TParent>.Create(MakeByteArray(properties));

                Assert.AreEqual(properties.signature, chapter.Signature);
                Assert.AreEqual(properties.version, chapter.Version);
                Assert.AreEqual(properties.size, chapter.Size);
                Assert.AreEqual(properties.checksum, chapter.Checksum);
                CollectionAssert.AreNotEqual(properties.data, chapter.Data.ToArray());
                Assert.IsFalse(chapter.IsValid.Value);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestExceededSizeHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                ++properties.size;

                Th095ChapterWrapper<TParent>.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestInvalidChecksumHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                --properties.checksum;

                var chapter = Th095ChapterWrapper<TParent>.Create(MakeByteArray(properties));

                Validate(chapter, properties);
                Assert.IsFalse(chapter.IsValid.Value);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestEmptyDataHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.data = new byte[] { };

                Th095ChapterWrapper<TParent>.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestMisalignedDataHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                --properties.size;
                properties.data = properties.data.Take(properties.data.Length - 1).ToArray();

                var chapter = Th095ChapterWrapper<TParent>.Create(MakeByteArray(properties));

                Validate(chapter, properties);
                Assert.IsFalse(chapter.IsValid.Value);
            });

        #region Th095

        [TestMethod()]
        public void Th095ChapterTest()
            => ChapterTestHelper<Th095Converter>();

        [TestMethod()]
        public void Th095ChapterTestCopy()
            => ChapterTestCopyHelper<Th095Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th095ChapterTestNull()
            => ChapterTestNullHelper<Th095Converter>();

        [TestMethod()]
        public void Th095ChapterReadFromTest()
            => ReadFromTestHelper<Th095Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th095ChapterReadFromTestNull()
            => ReadFromTestNullHelper<Th095Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th095ChapterReadFromTestEmptySignature()
            => ReadFromTestEmptySignatureHelper<Th095Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th095ChapterReadFromTestShortenedSignature()
            => ReadFromTestShortenedSignatureHelper<Th095Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th095ChapterReadFromTestExceededSignature()
            => ReadFromTestExceededSignatureHelper<Th095Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th095ChapterReadFromTestNegativeSize()
            => ReadFromTestNegativeSizeHelper<Th095Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th095ChapterReadFromTestZeroSize()
            => ReadFromTestZeroSizeHelper<Th095Converter>();

        [TestMethod()]
        public void Th095ChapterReadFromTestShortenedSize()
            => ReadFromTestShortenedSizeHelper<Th095Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th095ChapterReadFromTestExceededSize()
            => ReadFromTestExceededSizeHelper<Th095Converter>();

        [TestMethod()]
        public void Th095ChapterReadFromTestInvalidChecksum()
            => ReadFromTestInvalidChecksumHelper<Th095Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th095ChapterReadFromTestEmptyData()
            => ReadFromTestEmptyDataHelper<Th095Converter>();

        [TestMethod()]
        public void Th095ChapterReadFromTestMisalignedData()
            => ReadFromTestMisalignedDataHelper<Th095Converter>();

        #endregion

        #region Th125

        [TestMethod()]
        public void Th125ChapterTest()
            => ChapterTestHelper<Th125Converter>();

        [TestMethod()]
        public void Th125ChapterTestCopy()
            => ChapterTestCopyHelper<Th125Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th125ChapterTestNull()
            => ChapterTestNullHelper<Th125Converter>();

        [TestMethod()]
        public void Th125ChapterReadFromTest()
            => ReadFromTestHelper<Th125Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th125ChapterReadFromTestNull()
            => ReadFromTestNullHelper<Th125Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th125ChapterReadFromTestEmptySignature()
            => ReadFromTestEmptySignatureHelper<Th125Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th125ChapterReadFromTestShortenedSignature()
            => ReadFromTestShortenedSignatureHelper<Th125Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th125ChapterReadFromTestExceededSignature()
            => ReadFromTestExceededSignatureHelper<Th125Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th125ChapterReadFromTestNegativeSize()
            => ReadFromTestNegativeSizeHelper<Th125Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th125ChapterReadFromTestZeroSize()
            => ReadFromTestZeroSizeHelper<Th125Converter>();

        [TestMethod()]
        public void Th125ChapterReadFromTestShortenedSize()
            => ReadFromTestShortenedSizeHelper<Th125Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th125ChapterReadFromTestExceededSize()
            => ReadFromTestExceededSizeHelper<Th125Converter>();

        [TestMethod()]
        public void Th125ChapterReadFromTestInvalidChecksum()
            => ReadFromTestInvalidChecksumHelper<Th125Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th125ChapterReadFromTestEmptyData()
            => ReadFromTestEmptyDataHelper<Th125Converter>();

        [TestMethod()]
        public void Th125ChapterReadFromTestMisalignedData()
            => ReadFromTestMisalignedDataHelper<Th125Converter>();

        #endregion
    }
}

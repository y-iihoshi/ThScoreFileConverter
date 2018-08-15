﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th06ChapterTests
    {
        internal struct Properties
        {
            public string signature;
            public short size1;
            public short size2;
            public byte[] data;
        };

        internal static Properties DefaultProperties => new Properties()
        {
            signature = string.Empty,
            size1 = default,
            size2 = default,
            data = new byte[] { }
        };

        internal static Properties ValidProperties => new Properties()
        {
            signature = "ABCD",
            size1 = 12,
            size2 = 34,
            data = new byte[] { 0x56, 0x78, 0x9A, 0xBC }
        };

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(),
                properties.size1,
                properties.size2,
                properties.data);

        internal static void Validate<TParent>(in Th06ChapterWrapper<TParent> chapter, in Properties properties)
            where TParent : ThConverter
        {
            Assert.AreEqual(properties.signature, chapter.Signature);
            Assert.AreEqual(properties.size1, chapter.Size1);
            Assert.AreEqual(properties.size2, chapter.Size2);
            CollectionAssert.AreEqual(properties.data, chapter.Data.ToArray());
            Assert.AreEqual((properties.data?.Length > 0 ? properties.data[0] : default), chapter.FirstByteOfData);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ChapterTestHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var chapter = new Th06ChapterWrapper<TParent>();

                Validate(chapter, DefaultProperties);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ChapterTestCopyHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var chapter1 = new Th06ChapterWrapper<TParent>();
                var chapter2 = new Th06ChapterWrapper<TParent>(chapter1);

                Validate(chapter2, DefaultProperties);
            });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "chapter")]
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ChapterTestNullHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var chapter = new Th06ChapterWrapper<TParent>(null);

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var chapter = Th06ChapterWrapper<TParent>.Create(MakeByteArray(ValidProperties));

                Validate(chapter, ValidProperties);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestNullHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var chapter = new Th06ChapterWrapper<TParent>();
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

                // <-- sig --> size1 size2 <- data -->
                // __ __ __ __ 0c 00 22 00 56 78 9a bc
                //             <-- sig --> size1 size2 <dat>

                // The actual value of the Size1 property becomes too large and
                // the Data property becomes empty,
                // so EndOfStreamException will be thrown.
                Th06ChapterWrapper<TParent>.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestShortenedSignatureHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.signature = properties.signature.Substring(0, properties.signature.Length - 1);

                // <-- sig --> size1 size2 <- data -->
                // __ 41 42 43 0c 00 22 00 56 78 9a bc
                //    <-- sig --> size1 size2 < dat ->

                // The actual value of the Size1 property becomes too large,
                // so EndOfStreamException will be thrown.
                Th06ChapterWrapper<TParent>.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestExceededSignatureHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.signature += "E";

                // <--- sig ----> size1 size2 <- data -->
                // 41 42 43 44 45 0c 00 22 00 56 78 9a bc
                // <-- sig --> size1 size2 <---- data ---->

                // The actual value of the Size1 property becomes too large,
                // so EndOfStreamException will be thrown.
                Th06ChapterWrapper<TParent>.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestNegativeSize1Helper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.size1 = -1;

                Th06ChapterWrapper<TParent>.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestZeroSize1Helper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.size1 = 0;

                Th06ChapterWrapper<TParent>.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestShortenedSize1Helper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                --properties.size1;

                var chapter = Th06ChapterWrapper<TParent>.Create(MakeByteArray(properties));

                Assert.AreEqual(properties.signature, chapter.Signature);
                Assert.AreEqual(properties.size1, chapter.Size1);
                Assert.AreEqual(properties.size2, chapter.Size2);
                CollectionAssert.AreNotEqual(properties.data, chapter.Data.ToArray());
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestExceededSize1Helper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                ++properties.size1;

                Th06ChapterWrapper<TParent>.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestNegativeSize2Helper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.size2 = -1;

                var chapter = Th06ChapterWrapper<TParent>.Create(MakeByteArray(properties));

                Validate(chapter, properties);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestZeroSize2Helper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.size2 = 0;

                var chapter = Th06ChapterWrapper<TParent>.Create(MakeByteArray(properties));

                Validate(chapter, properties);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestShortenedSize2Helper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                --properties.size2;

                var chapter = Th06ChapterWrapper<TParent>.Create(MakeByteArray(properties));

                Validate(chapter, properties);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestExceededSize2Helper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                ++properties.size2;

                var chapter = Th06ChapterWrapper<TParent>.Create(MakeByteArray(properties));

                Validate(chapter, properties);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestEmptyDataHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.data = new byte[] { };

                Th06ChapterWrapper<TParent>.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestMisalignedDataHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                --properties.size1;
                properties.data = properties.data.Take(properties.data.Length - 1).ToArray();

                var chapter = Th06ChapterWrapper<TParent>.Create(MakeByteArray(properties));

                Validate(chapter, properties);
            });

        #region Th06

        [TestMethod]
        public void Th06ChapterTest()
            => ChapterTestHelper<Th06Converter>();

        [TestMethod]
        public void Th06ChapterTestCopy()
            => ChapterTestCopyHelper<Th06Converter>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th06ChapterTestNull()
            => ChapterTestNullHelper<Th06Converter>();

        [TestMethod]
        public void Th06ChapterReadFromTest()
            => ReadFromTestHelper<Th06Converter>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th06ChapterReadFromTestNull()
            => ReadFromTestNullHelper<Th06Converter>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th06ChapterReadFromTestEmptySignature()
            => ReadFromTestEmptySignatureHelper<Th06Converter>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th06ChapterReadFromTestShortenedSignature()
            => ReadFromTestShortenedSignatureHelper<Th06Converter>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th06ChapterReadFromTestExceededSignature()
            => ReadFromTestExceededSignatureHelper<Th06Converter>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th06ChapterReadFromTestNegativeSize1()
            => ReadFromTestNegativeSize1Helper<Th06Converter>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th06ChapterReadFromTestZeroSize1()
            => ReadFromTestZeroSize1Helper<Th06Converter>();

        [TestMethod]
        public void Th06ChapterReadFromTestShortenedSize1()
            => ReadFromTestShortenedSize1Helper<Th06Converter>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th06ChapterReadFromTestExceededSize1()
            => ReadFromTestExceededSize1Helper<Th06Converter>();

        [TestMethod]
        public void Th06ChapterReadFromTestNegativeSize2()
            => ReadFromTestNegativeSize2Helper<Th06Converter>();

        [TestMethod]
        public void Th06ChapterReadFromTestZeroSize2()
            => ReadFromTestZeroSize2Helper<Th06Converter>();

        [TestMethod]
        public void Th06ChapterReadFromTestShortenedSize2()
            => ReadFromTestShortenedSize2Helper<Th06Converter>();

        [TestMethod]
        public void Th06ChapterReadFromTestExceededSize2()
            => ReadFromTestExceededSize2Helper<Th06Converter>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th06ChapterReadFromTestEmptyData()
            => ReadFromTestEmptyDataHelper<Th06Converter>();

        [TestMethod]
        public void Th06ChapterReadFromTestMisalignedData()
            => ReadFromTestMisalignedDataHelper<Th06Converter>();

        #endregion

        #region Th07

        [TestMethod]
        public void Th07ChapterTest()
            => ChapterTestHelper<Th07Converter>();

        [TestMethod]
        public void Th07ChapterTestCopy()
            => ChapterTestCopyHelper<Th07Converter>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th07ChapterTestNull()
            => ChapterTestNullHelper<Th07Converter>();

        [TestMethod]
        public void Th07ChapterReadFromTest()
            => ReadFromTestHelper<Th07Converter>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th07ChapterReadFromTestNull()
            => ReadFromTestNullHelper<Th07Converter>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th07ChapterReadFromTestEmptySignature()
            => ReadFromTestEmptySignatureHelper<Th07Converter>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th07ChapterReadFromTestShortenedSignature()
            => ReadFromTestShortenedSignatureHelper<Th07Converter>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th07ChapterReadFromTestExceededSignature()
            => ReadFromTestExceededSignatureHelper<Th07Converter>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th07ChapterReadFromTestNegativeSize1()
            => ReadFromTestNegativeSize1Helper<Th07Converter>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th07ChapterReadFromTestZeroSize1()
            => ReadFromTestZeroSize1Helper<Th07Converter>();

        [TestMethod]
        public void Th07ChapterReadFromTestShortenedSize1()
            => ReadFromTestShortenedSize1Helper<Th07Converter>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th07ChapterReadFromTestExceededSize1()
            => ReadFromTestExceededSize1Helper<Th07Converter>();

        [TestMethod]
        public void Th07ChapterReadFromTestNegativeSize2()
            => ReadFromTestNegativeSize2Helper<Th07Converter>();

        [TestMethod]
        public void Th07ChapterReadFromTestZeroSize2()
            => ReadFromTestZeroSize2Helper<Th07Converter>();

        [TestMethod]
        public void Th07ChapterReadFromTestShortenedSize2()
            => ReadFromTestShortenedSize2Helper<Th07Converter>();

        [TestMethod]
        public void Th07ChapterReadFromTestExceededSize2()
            => ReadFromTestExceededSize2Helper<Th07Converter>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th07ChapterReadFromTestEmptyData()
            => ReadFromTestEmptyDataHelper<Th07Converter>();

        [TestMethod]
        public void Th07ChapterReadFromTestMisalignedData()
            => ReadFromTestMisalignedDataHelper<Th07Converter>();

        #endregion

        #region Th08

        [TestMethod]
        public void Th08ChapterTest()
            => ChapterTestHelper<Th08Converter>();

        [TestMethod]
        public void Th08ChapterTestCopy()
            => ChapterTestCopyHelper<Th08Converter>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th08ChapterTestNull()
            => ChapterTestNullHelper<Th08Converter>();

        [TestMethod]
        public void Th08ChapterReadFromTest()
            => ReadFromTestHelper<Th08Converter>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th08ChapterReadFromTestNull()
            => ReadFromTestNullHelper<Th08Converter>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th08ChapterReadFromTestEmptySignature()
            => ReadFromTestEmptySignatureHelper<Th08Converter>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th08ChapterReadFromTestShortenedSignature()
            => ReadFromTestShortenedSignatureHelper<Th08Converter>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th08ChapterReadFromTestExceededSignature()
            => ReadFromTestExceededSignatureHelper<Th08Converter>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th08ChapterReadFromTestNegativeSize1()
            => ReadFromTestNegativeSize1Helper<Th08Converter>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th08ChapterReadFromTestZeroSize1()
            => ReadFromTestZeroSize1Helper<Th08Converter>();

        [TestMethod]
        public void Th08ChapterReadFromTestShortenedSize1()
            => ReadFromTestShortenedSize1Helper<Th08Converter>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th08ChapterReadFromTestExceededSize1()
            => ReadFromTestExceededSize1Helper<Th08Converter>();

        [TestMethod]
        public void Th08ChapterReadFromTestNegativeSize2()
            => ReadFromTestNegativeSize2Helper<Th08Converter>();

        [TestMethod]
        public void Th08ChapterReadFromTestZeroSize2()
            => ReadFromTestZeroSize2Helper<Th08Converter>();

        [TestMethod]
        public void Th08ChapterReadFromTestShortenedSize2()
            => ReadFromTestShortenedSize2Helper<Th08Converter>();

        [TestMethod]
        public void Th08ChapterReadFromTestExceededSize2()
            => ReadFromTestExceededSize2Helper<Th08Converter>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th08ChapterReadFromTestEmptyData()
            => ReadFromTestEmptyDataHelper<Th08Converter>();

        [TestMethod]
        public void Th08ChapterReadFromTestMisalignedData()
            => ReadFromTestMisalignedDataHelper<Th08Converter>();

        #endregion

        #region Th09

        [TestMethod]
        public void Th09ChapterTest()
            => ChapterTestHelper<Th09Converter>();

        [TestMethod]
        public void Th09ChapterTestCopy()
            => ChapterTestCopyHelper<Th09Converter>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th09ChapterTestNull()
            => ChapterTestNullHelper<Th09Converter>();

        [TestMethod]
        public void Th09ChapterReadFromTest()
            => ReadFromTestHelper<Th09Converter>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th09ChapterReadFromTestNull()
            => ReadFromTestNullHelper<Th09Converter>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th09ChapterReadFromTestEmptySignature()
            => ReadFromTestEmptySignatureHelper<Th09Converter>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th09ChapterReadFromTestShortenedSignature()
            => ReadFromTestShortenedSignatureHelper<Th09Converter>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th09ChapterReadFromTestExceededSignature()
            => ReadFromTestExceededSignatureHelper<Th09Converter>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th09ChapterReadFromTestNegativeSize1()
            => ReadFromTestNegativeSize1Helper<Th09Converter>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th09ChapterReadFromTestZeroSize1()
            => ReadFromTestZeroSize1Helper<Th09Converter>();

        [TestMethod]
        public void Th09ChapterReadFromTestShortenedSize1()
            => ReadFromTestShortenedSize1Helper<Th09Converter>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th09ChapterReadFromTestExceededSize1()
            => ReadFromTestExceededSize1Helper<Th09Converter>();

        [TestMethod]
        public void Th09ChapterReadFromTestNegativeSize2()
            => ReadFromTestNegativeSize2Helper<Th09Converter>();

        [TestMethod]
        public void Th09ChapterReadFromTestZeroSize2()
            => ReadFromTestZeroSize2Helper<Th09Converter>();

        [TestMethod]
        public void Th09ChapterReadFromTestShortenedSize2()
            => ReadFromTestShortenedSize2Helper<Th09Converter>();

        [TestMethod]
        public void Th09ChapterReadFromTestExceededSize2()
            => ReadFromTestExceededSize2Helper<Th09Converter>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th09ChapterReadFromTestEmptyData()
            => ReadFromTestEmptyDataHelper<Th09Converter>();

        [TestMethod]
        public void Th09ChapterReadFromTestMisalignedData()
            => ReadFromTestMisalignedDataHelper<Th09Converter>();

        #endregion
    }
}

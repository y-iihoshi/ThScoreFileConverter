using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass()]
    public class Th10ChapterTests
    {
        internal static void Validate<TParent>(
            Th10ChapterWrapper<TParent> chapter,
            string signature,
            ushort version,
            uint checksum,
            int size,
            byte[] data,
            bool isValid)
            where TParent : ThConverter
        {
            if (chapter == null)
                throw new ArgumentNullException(nameof(chapter));

            Assert.AreEqual(signature, chapter.Signature);
            Assert.AreEqual(version, chapter.Version);
            Assert.AreEqual(checksum, chapter.Checksum);
            Assert.AreEqual(size, chapter.Size);
            CollectionAssert.AreEqual(data, chapter.Data.ToArray());
            Assert.AreEqual(isValid, chapter.IsValid);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ChapterTestHelper<TParent>()
            where TParent : ThConverter
        {
            try
            {
                var chapter = new Th10ChapterWrapper<TParent>();
                Validate(chapter, string.Empty, 0, 0, 0, new byte[] { }, true);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ChapterTestCopyHelper<TParent>()
            where TParent : ThConverter
        {
            try
            {
                var chapter1 = new Th10ChapterWrapper<TParent>();
                var chapter2 = new Th10ChapterWrapper<TParent>(chapter1);
                Validate(chapter2, string.Empty, 0, 0, 0, new byte[] { }, true);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "chapter")]
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ChapterTestNullHelper<TParent>()
            where TParent : ThConverter
        {
            try
            {
                var chapter = new Th10ChapterWrapper<TParent>(null);
                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestHelper<TParent>()
            where TParent : ThConverter
        {
            try
            {
                var signature = "AB";
                var version = (ushort)1234;
                var checksum = 0x234u;
                var size = 16;
                var data = new byte[] { 0x56, 0x78, 0x9A, 0xBC };

                var chapter = Th10ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

                Validate(chapter, signature, version, checksum, size, data, true);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestNullHelper<TParent>()
            where TParent : ThConverter
        {
            try
            {
                var chapter = new Th10ChapterWrapper<TParent>();
                chapter.ReadFrom(null);
                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestEmptySignatureHelper<TParent>()
            where TParent : ThConverter
        {
            try
            {
                var signature = string.Empty;
                var version = (ushort)1234;
                var checksum = 0x234u;
                var size = 16;
                var data = new byte[] { 0x56, 0x78, 0x9A, 0xBC };

                // <sig> <ver> < chksum -> <- size --> <- data -->
                // __ __ d2 04 34 02 00 00 10 00 00 00 56 78 9a bc
                //       <sig> <ver> < chksum -> <- size --> <dat>

                // The actual value of the Size property becomes too large,
                // so EndOfStreamException will be thrown.
                Th10ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestShortenedSignatureHelper<TParent>()
            where TParent : ThConverter
        {
            try
            {
                var signature = "A";
                var version = (ushort)1234;
                var checksum = 0x234u;
                var size = 16;
                var data = new byte[] { 0x56, 0x78, 0x9A, 0xBC };

                // <sig> <ver> < chksum -> <- size --> <- data -->
                // __ 41 d2 04 34 02 00 00 10 00 00 00 56 78 9a bc
                //    <sig> <ver> < chksum -> <- size --> < data >

                // The actual value of the Size property becomes too large,
                // so EndOfStreamException will be thrown.
                Th10ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestExceededSignatureHelper<TParent>()
            where TParent : ThConverter
        {
            try
            {
                var signature = "ABC";
                var version = (ushort)1234;
                var checksum = 0x234u;
                var size = 16;
                var data = new byte[] { 0x56, 0x78, 0x9A, 0xBC };

                // < sig -> <ver> < chksum -> <- size --> <- data -->
                // 41 42 43 d2 04 34 02 00 00 10 00 00 00 56 78 9a bc
                // <sig> <ver> < chksum -> <- size --> <---- data ---->

                // The actual value of the Size property becomes too large,
                // so EndOfStreamException will be thrown.
                Th10ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestNegativeSizeHelper<TParent>()
            where TParent : ThConverter
        {
            try
            {
                var signature = "AB";
                var version = (ushort)1234;
                var checksum = 0x234u;
                var size = -1;
                var data = new byte[] { 0x56, 0x78, 0x9A, 0xBC };

                Th10ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestZeroSizeHelper<TParent>()
            where TParent : ThConverter
        {
            try
            {
                var signature = "AB";
                var version = (ushort)1234;
                var checksum = 0x234u;
                var size = 0;
                var data = new byte[] { 0x56, 0x78, 0x9A, 0xBC };

                Th10ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestShortenedSizeHelper<TParent>()
            where TParent : ThConverter
        {
            try
            {
                var signature = "AB";
                var version = (ushort)1234;
                var checksum = 0x234u;
                var size = 15;
                var data = new byte[] { 0x56, 0x78, 0x9A, 0xBC };

                var chapter = Th10ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

                Assert.AreEqual(signature, chapter.Signature);
                Assert.AreEqual(version, chapter.Version);
                Assert.AreEqual(checksum, chapter.Checksum);
                Assert.AreEqual(size, chapter.Size);
                CollectionAssert.AreNotEqual(data, chapter.Data.ToArray());
                Assert.IsFalse(chapter.IsValid.Value);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestExceededSizeHelper<TParent>()
            where TParent : ThConverter
        {
            try
            {
                var signature = "AB";
                var version = (ushort)1234;
                var checksum = 0x234u;
                var size = 17;
                var data = new byte[] { 0x56, 0x78, 0x9A, 0xBC };

                Th10ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestInvalidChecksumHelper<TParent>()
            where TParent : ThConverter
        {
            try
            {
                var signature = "AB";
                var version = (ushort)1234;
                var checksum = 0x233u;
                var size = 16;
                var data = new byte[] { 0x56, 0x78, 0x9A, 0xBC };

                var chapter = Th10ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

                Assert.AreEqual(signature, chapter.Signature);
                Assert.AreEqual(version, chapter.Version);
                Assert.AreEqual(checksum, chapter.Checksum);
                Assert.AreEqual(size, chapter.Size);
                CollectionAssert.AreEqual(data, chapter.Data.ToArray());
                Assert.IsFalse(chapter.IsValid.Value);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestEmptyDataHelper<TParent>()
            where TParent : ThConverter
        {
            try
            {
                var signature = "AB";
                var version = (ushort)1234;
                var checksum = 0x0Cu;
                var size = 12;
                var data = new byte[] { };

                var chapter = Th10ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

                Assert.AreEqual(signature, chapter.Signature);
                Assert.AreEqual(version, chapter.Version);
                Assert.AreEqual(checksum, chapter.Checksum);
                Assert.AreEqual(size, chapter.Size);
                CollectionAssert.AreEqual(data, chapter.Data.ToArray());
                Assert.IsTrue(chapter.IsValid.Value);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestMisalignedDataHelper<TParent>()
            where TParent : ThConverter
        {
            try
            {
                var signature = "AB";
                var version = (ushort)1234;
                var checksum = 0x17Du;
                var size = 15;
                var data = new byte[] { 0x56, 0x78, 0x9A };

                var chapter = Th10ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

                Assert.AreEqual(signature, chapter.Signature);
                Assert.AreEqual(version, chapter.Version);
                Assert.AreEqual(checksum, chapter.Checksum);
                Assert.AreEqual(size, chapter.Size);
                CollectionAssert.AreEqual(data, chapter.Data.ToArray());
                Assert.IsFalse(chapter.IsValid.Value);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        #region Th10

        [TestMethod()]
        public void Th10ChapterTest()
            => ChapterTestHelper<Th10Converter>();

        [TestMethod()]
        public void Th10ChapterTestCopy()
            => ChapterTestCopyHelper<Th10Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th10ChapterTestNull()
            => ChapterTestNullHelper<Th10Converter>();

        [TestMethod()]
        public void Th10ChapterReadFromTest()
            => ReadFromTestHelper<Th10Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th10ChapterReadFromTestNull()
            => ReadFromTestNullHelper<Th10Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th10ChapterReadFromTestEmptySignature()
            => ReadFromTestEmptySignatureHelper<Th10Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th10ChapterReadFromTestShortenedSignature()
            => ReadFromTestShortenedSignatureHelper<Th10Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th10ChapterReadFromTestExceededSignature()
            => ReadFromTestExceededSignatureHelper<Th10Converter>();

        [TestMethod()]
        public void Th10ChapterReadFromTestInvalidChecksum()
            => ReadFromTestInvalidChecksumHelper<Th10Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th10ChapterReadFromTestNegativeSize()
            => ReadFromTestNegativeSizeHelper<Th10Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th10ChapterReadFromTestZeroSize()
            => ReadFromTestZeroSizeHelper<Th10Converter>();

        [TestMethod()]
        public void Th10ChapterReadFromTestShortenedSize()
            => ReadFromTestShortenedSizeHelper<Th10Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th10ChapterReadFromTestExceededSize()
            => ReadFromTestExceededSizeHelper<Th10Converter>();

        [TestMethod()]
        public void Th10ChapterReadFromTestEmptyData()
            => ReadFromTestEmptyDataHelper<Th10Converter>();

        [TestMethod()]
        public void Th10ChapterReadFromTestMisalignedData()
            => ReadFromTestMisalignedDataHelper<Th10Converter>();

        #endregion

        #region Th11

        [TestMethod()]
        public void Th11ChapterTest()
            => ChapterTestHelper<Th11Converter>();

        [TestMethod()]
        public void Th11ChapterTestCopy()
            => ChapterTestCopyHelper<Th11Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th11ChapterTestNull()
            => ChapterTestNullHelper<Th11Converter>();

        [TestMethod()]
        public void Th11ChapterReadFromTest()
            => ReadFromTestHelper<Th11Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th11ChapterReadFromTestNull()
            => ReadFromTestNullHelper<Th11Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th11ChapterReadFromTestEmptySignature()
            => ReadFromTestEmptySignatureHelper<Th11Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th11ChapterReadFromTestShortenedSignature()
            => ReadFromTestShortenedSignatureHelper<Th11Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th11ChapterReadFromTestExceededSignature()
            => ReadFromTestExceededSignatureHelper<Th11Converter>();

        [TestMethod()]
        public void Th11ChapterReadFromTestInvalidChecksum()
            => ReadFromTestInvalidChecksumHelper<Th11Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th11ChapterReadFromTestNegativeSize()
            => ReadFromTestNegativeSizeHelper<Th11Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th11ChapterReadFromTestZeroSize()
            => ReadFromTestZeroSizeHelper<Th11Converter>();

        [TestMethod()]
        public void Th11ChapterReadFromTestShortenedSize()
            => ReadFromTestShortenedSizeHelper<Th11Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th11ChapterReadFromTestExceededSize()
            => ReadFromTestExceededSizeHelper<Th11Converter>();

        [TestMethod()]
        public void Th11ChapterReadFromTestEmptyData()
            => ReadFromTestEmptyDataHelper<Th11Converter>();

        [TestMethod()]
        public void Th11ChapterReadFromTestMisalignedData()
            => ReadFromTestMisalignedDataHelper<Th11Converter>();

        #endregion

        #region Th12

        [TestMethod()]
        public void Th12ChapterTest()
            => ChapterTestHelper<Th12Converter>();

        [TestMethod()]
        public void Th12ChapterTestCopy()
            => ChapterTestCopyHelper<Th12Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th12ChapterTestNull()
            => ChapterTestNullHelper<Th12Converter>();

        [TestMethod()]
        public void Th12ChapterReadFromTest()
            => ReadFromTestHelper<Th12Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th12ChapterReadFromTestNull()
            => ReadFromTestNullHelper<Th12Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th12ChapterReadFromTestEmptySignature()
            => ReadFromTestEmptySignatureHelper<Th12Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th12ChapterReadFromTestShortenedSignature()
            => ReadFromTestShortenedSignatureHelper<Th12Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th12ChapterReadFromTestExceededSignature()
            => ReadFromTestExceededSignatureHelper<Th12Converter>();

        [TestMethod()]
        public void Th12ChapterReadFromTestInvalidChecksum()
            => ReadFromTestInvalidChecksumHelper<Th12Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th12ChapterReadFromTestNegativeSize()
            => ReadFromTestNegativeSizeHelper<Th12Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th12ChapterReadFromTestZeroSize()
            => ReadFromTestZeroSizeHelper<Th12Converter>();

        [TestMethod()]
        public void Th12ChapterReadFromTestShortenedSize()
            => ReadFromTestShortenedSizeHelper<Th12Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th12ChapterReadFromTestExceededSize()
            => ReadFromTestExceededSizeHelper<Th12Converter>();

        [TestMethod()]
        public void Th12ChapterReadFromTestEmptyData()
            => ReadFromTestEmptyDataHelper<Th12Converter>();

        [TestMethod()]
        public void Th12ChapterReadFromTestMisalignedData()
            => ReadFromTestMisalignedDataHelper<Th12Converter>();

        #endregion

        #region Th128

        [TestMethod()]
        public void Th128ChapterTest()
            => ChapterTestHelper<Th128Converter>();

        [TestMethod()]
        public void Th128ChapterTestCopy()
            => ChapterTestCopyHelper<Th128Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th128ChapterTestNull()
            => ChapterTestNullHelper<Th128Converter>();

        [TestMethod()]
        public void Th128ChapterReadFromTest()
            => ReadFromTestHelper<Th128Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th128ChapterReadFromTestNull()
            => ReadFromTestNullHelper<Th128Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th128ChapterReadFromTestEmptySignature()
            => ReadFromTestEmptySignatureHelper<Th128Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th128ChapterReadFromTestShortenedSignature()
            => ReadFromTestShortenedSignatureHelper<Th128Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th128ChapterReadFromTestExceededSignature()
            => ReadFromTestExceededSignatureHelper<Th128Converter>();

        [TestMethod()]
        public void Th128ChapterReadFromTestInvalidChecksum()
            => ReadFromTestInvalidChecksumHelper<Th128Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th128ChapterReadFromTestNegativeSize()
            => ReadFromTestNegativeSizeHelper<Th128Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th128ChapterReadFromTestZeroSize()
            => ReadFromTestZeroSizeHelper<Th128Converter>();

        [TestMethod()]
        public void Th128ChapterReadFromTestShortenedSize()
            => ReadFromTestShortenedSizeHelper<Th128Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th128ChapterReadFromTestExceededSize()
            => ReadFromTestExceededSizeHelper<Th128Converter>();

        [TestMethod()]
        public void Th128ChapterReadFromTestEmptyData()
            => ReadFromTestEmptyDataHelper<Th128Converter>();

        [TestMethod()]
        public void Th128ChapterReadFromTestMisalignedData()
            => ReadFromTestMisalignedDataHelper<Th128Converter>();

        #endregion

        #region Th13

        [TestMethod()]
        public void Th13ChapterTest()
            => ChapterTestHelper<Th13Converter>();

        [TestMethod()]
        public void Th13ChapterTestCopy()
            => ChapterTestCopyHelper<Th13Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th13ChapterTestNull()
            => ChapterTestNullHelper<Th13Converter>();

        [TestMethod()]
        public void Th13ChapterReadFromTest()
            => ReadFromTestHelper<Th13Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th13ChapterReadFromTestNull()
            => ReadFromTestNullHelper<Th13Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th13ChapterReadFromTestEmptySignature()
            => ReadFromTestEmptySignatureHelper<Th13Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th13ChapterReadFromTestShortenedSignature()
            => ReadFromTestShortenedSignatureHelper<Th13Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th13ChapterReadFromTestExceededSignature()
            => ReadFromTestExceededSignatureHelper<Th13Converter>();

        [TestMethod()]
        public void Th13ChapterReadFromTestInvalidChecksum()
            => ReadFromTestInvalidChecksumHelper<Th13Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th13ChapterReadFromTestNegativeSize()
            => ReadFromTestNegativeSizeHelper<Th13Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th13ChapterReadFromTestZeroSize()
            => ReadFromTestZeroSizeHelper<Th13Converter>();

        [TestMethod()]
        public void Th13ChapterReadFromTestShortenedSize()
            => ReadFromTestShortenedSizeHelper<Th13Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th13ChapterReadFromTestExceededSize()
            => ReadFromTestExceededSizeHelper<Th13Converter>();

        [TestMethod()]
        public void Th13ChapterReadFromTestEmptyData()
            => ReadFromTestEmptyDataHelper<Th13Converter>();

        [TestMethod()]
        public void Th13ChapterReadFromTestMisalignedData()
            => ReadFromTestMisalignedDataHelper<Th13Converter>();

        #endregion

        #region Th14

        [TestMethod()]
        public void Th14ChapterTest()
            => ChapterTestHelper<Th14Converter>();

        [TestMethod()]
        public void Th14ChapterTestCopy()
            => ChapterTestCopyHelper<Th14Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th14ChapterTestNull()
            => ChapterTestNullHelper<Th14Converter>();

        [TestMethod()]
        public void Th14ChapterReadFromTest()
            => ReadFromTestHelper<Th14Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th14ChapterReadFromTestNull()
            => ReadFromTestNullHelper<Th14Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th14ChapterReadFromTestEmptySignature()
            => ReadFromTestEmptySignatureHelper<Th14Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th14ChapterReadFromTestShortenedSignature()
            => ReadFromTestShortenedSignatureHelper<Th14Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th14ChapterReadFromTestExceededSignature()
            => ReadFromTestExceededSignatureHelper<Th14Converter>();

        [TestMethod()]
        public void Th14ChapterReadFromTestInvalidChecksum()
            => ReadFromTestInvalidChecksumHelper<Th14Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th14ChapterReadFromTestNegativeSize()
            => ReadFromTestNegativeSizeHelper<Th14Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th14ChapterReadFromTestZeroSize()
            => ReadFromTestZeroSizeHelper<Th14Converter>();

        [TestMethod()]
        public void Th14ChapterReadFromTestShortenedSize()
            => ReadFromTestShortenedSizeHelper<Th14Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th14ChapterReadFromTestExceededSize()
            => ReadFromTestExceededSizeHelper<Th14Converter>();

        [TestMethod()]
        public void Th14ChapterReadFromTestEmptyData()
            => ReadFromTestEmptyDataHelper<Th14Converter>();

        [TestMethod()]
        public void Th14ChapterReadFromTestMisalignedData()
            => ReadFromTestMisalignedDataHelper<Th14Converter>();

        #endregion

        #region Th143

        [TestMethod()]
        public void Th143ChapterTest()
            => ChapterTestHelper<Th143Converter>();

        [TestMethod()]
        public void Th143ChapterTestCopy()
            => ChapterTestCopyHelper<Th143Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th143ChapterTestNull()
            => ChapterTestNullHelper<Th143Converter>();

        [TestMethod()]
        public void Th143ChapterReadFromTest()
            => ReadFromTestHelper<Th143Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th143ChapterReadFromTestNull()
            => ReadFromTestNullHelper<Th143Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th143ChapterReadFromTestEmptySignature()
            => ReadFromTestEmptySignatureHelper<Th143Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th143ChapterReadFromTestShortenedSignature()
            => ReadFromTestShortenedSignatureHelper<Th143Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th143ChapterReadFromTestExceededSignature()
            => ReadFromTestExceededSignatureHelper<Th143Converter>();

        [TestMethod()]
        public void Th143ChapterReadFromTestInvalidChecksum()
            => ReadFromTestInvalidChecksumHelper<Th143Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th143ChapterReadFromTestNegativeSize()
            => ReadFromTestNegativeSizeHelper<Th143Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th143ChapterReadFromTestZeroSize()
            => ReadFromTestZeroSizeHelper<Th143Converter>();

        [TestMethod()]
        public void Th143ChapterReadFromTestShortenedSize()
            => ReadFromTestShortenedSizeHelper<Th143Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th143ChapterReadFromTestExceededSize()
            => ReadFromTestExceededSizeHelper<Th143Converter>();

        [TestMethod()]
        public void Th143ChapterReadFromTestEmptyData()
            => ReadFromTestEmptyDataHelper<Th143Converter>();

        [TestMethod()]
        public void Th143ChapterReadFromTestMisalignedData()
            => ReadFromTestMisalignedDataHelper<Th143Converter>();

        #endregion

        #region Th15

        [TestMethod()]
        public void Th15ChapterTest()
            => ChapterTestHelper<Th15Converter>();

        [TestMethod()]
        public void Th15ChapterTestCopy()
            => ChapterTestCopyHelper<Th15Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th15ChapterTestNull()
            => ChapterTestNullHelper<Th15Converter>();

        [TestMethod()]
        public void Th15ChapterReadFromTest()
            => ReadFromTestHelper<Th15Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th15ChapterReadFromTestNull()
            => ReadFromTestNullHelper<Th15Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th15ChapterReadFromTestEmptySignature()
            => ReadFromTestEmptySignatureHelper<Th15Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th15ChapterReadFromTestShortenedSignature()
            => ReadFromTestShortenedSignatureHelper<Th15Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th15ChapterReadFromTestExceededSignature()
            => ReadFromTestExceededSignatureHelper<Th15Converter>();

        [TestMethod()]
        public void Th15ChapterReadFromTestInvalidChecksum()
            => ReadFromTestInvalidChecksumHelper<Th15Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th15ChapterReadFromTestNegativeSize()
            => ReadFromTestNegativeSizeHelper<Th15Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th15ChapterReadFromTestZeroSize()
            => ReadFromTestZeroSizeHelper<Th15Converter>();

        [TestMethod()]
        public void Th15ChapterReadFromTestShortenedSize()
            => ReadFromTestShortenedSizeHelper<Th15Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th15ChapterReadFromTestExceededSize()
            => ReadFromTestExceededSizeHelper<Th15Converter>();

        [TestMethod()]
        public void Th15ChapterReadFromTestEmptyData()
            => ReadFromTestEmptyDataHelper<Th15Converter>();

        [TestMethod()]
        public void Th15ChapterReadFromTestMisalignedData()
            => ReadFromTestMisalignedDataHelper<Th15Converter>();

        #endregion

        #region Th16

        [TestMethod()]
        public void Th16ChapterTest()
            => ChapterTestHelper<Th16Converter>();

        [TestMethod()]
        public void Th16ChapterTestCopy()
            => ChapterTestCopyHelper<Th16Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th16ChapterTestNull()
            => ChapterTestNullHelper<Th16Converter>();

        [TestMethod()]
        public void Th16ChapterReadFromTest()
            => ReadFromTestHelper<Th16Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th16ChapterReadFromTestNull()
            => ReadFromTestNullHelper<Th16Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th16ChapterReadFromTestEmptySignature()
            => ReadFromTestEmptySignatureHelper<Th16Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th16ChapterReadFromTestShortenedSignature()
            => ReadFromTestShortenedSignatureHelper<Th16Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th16ChapterReadFromTestExceededSignature()
            => ReadFromTestExceededSignatureHelper<Th16Converter>();

        [TestMethod()]
        public void Th16ChapterReadFromTestInvalidChecksum()
            => ReadFromTestInvalidChecksumHelper<Th16Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th16ChapterReadFromTestNegativeSize()
            => ReadFromTestNegativeSizeHelper<Th16Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th16ChapterReadFromTestZeroSize()
            => ReadFromTestZeroSizeHelper<Th16Converter>();

        [TestMethod()]
        public void Th16ChapterReadFromTestShortenedSize()
            => ReadFromTestShortenedSizeHelper<Th16Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th16ChapterReadFromTestExceededSize()
            => ReadFromTestExceededSizeHelper<Th16Converter>();

        [TestMethod()]
        public void Th16ChapterReadFromTestEmptyData()
            => ReadFromTestEmptyDataHelper<Th16Converter>();

        [TestMethod()]
        public void Th16ChapterReadFromTestMisalignedData()
            => ReadFromTestMisalignedDataHelper<Th16Converter>();

        #endregion
    }
}

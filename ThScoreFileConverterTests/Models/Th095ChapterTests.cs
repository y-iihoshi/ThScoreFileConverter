using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace ThScoreFileConverter.Models.Tests
{
    [TestClass()]
    public class Th095ChapterTests
    {
        public static void Validate<TParent>(
            Th095ChapterWrapper<TParent> chapter,
            string signature,
            ushort version,
            int size,
            uint checksum,
            byte[] data,
            bool isValid)
        {
            if (chapter == null)
                throw new ArgumentNullException(nameof(chapter));

            Assert.AreEqual(signature, chapter.Signature);
            Assert.AreEqual(version, chapter.Version);
            Assert.AreEqual(size, chapter.Size);
            Assert.AreEqual(checksum, chapter.Checksum);
            CollectionAssert.AreEqual(data, chapter.Data.ToArray());
            Assert.AreEqual(isValid, chapter.IsValid);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void ChapterTestHelper<TParent>()
        {
            try
            {
                var chapter = new Th095ChapterWrapper<TParent>();
                Validate(chapter, string.Empty, 0, 0, 0, new byte[] { }, false);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void ChapterTestCopyHelper<TParent>()
        {
            try
            {
                var chapter1 = new Th095ChapterWrapper<TParent>();
                var chapter2 = new Th095ChapterWrapper<TParent>(chapter1);
                Validate(chapter2, string.Empty, 0, 0, 0, new byte[] { }, false);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "chapter")]
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void ChapterTestNullHelper<TParent>()
        {
            try
            {
                var chapter = new Th095ChapterWrapper<TParent>(null);
                Assert.Fail("Unreachable");
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void ReadFromTestHelper<TParent>()
        {
            try
            {
                var signature = "AB";
                var version = (ushort)1234;
                var size = 16;
                var checksum = 0xC16CBAA7u;
                var data = new byte[] { 0x56, 0x78, 0x9A, 0xBC };

                var chapter = Th095ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, size, checksum, data));

                Validate(chapter, signature, version, size, checksum, data, true);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void ReadFromTestNullHelper<TParent>()
        {
            try
            {
                var chapter = new Th095ChapterWrapper<TParent>();
                chapter.ReadFrom(null);
                Assert.Fail("Unreachable");
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void ReadFromTestEmptySignatureHelper<TParent>()
        {
            try
            {
                var signature = string.Empty;
                var version = (ushort)1234;
                var size = 16;
                var checksum = 0xC16CBAA7u;
                var data = new byte[] { 0x56, 0x78, 0x9A, 0xBC };

                // <sig> <ver> <- size --> < chksum -> <- data -->
                // __ __ d2 04 10 00 00 00 a7 ba 6c c1 56 78 9a bc
                //       <sig> <ver> <- size --> < chksum -> <dat>

                // The actual value of the Size property becomes negative,
                // so ArgumentOutOfRangeException will be thrown.
                Th095ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, size, checksum, data));

                Assert.Fail("Unreachable");
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void ReadFromTestShortenedSignatureHelper<TParent>()
        {
            try
            {
                var signature = "A";
                var version = (ushort)1234;
                var size = 16;
                var checksum = 0xC16CBAA7u;
                var data = new byte[] { 0x56, 0x78, 0x9A, 0xBC };

                // <sig> <ver> <- size --> < chksum -> <- data -->
                // __ 41 d2 04 10 00 00 00 a7 ba 6c c1 56 78 9a bc
                //    <sig> <ver> <- size --> < chksum -> < data >

                // The actual value of the Size property becomes negative,
                // so ArgumentOutOfRangeException will be thrown.
                Th095ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, size, checksum, data));

                Assert.Fail("Unreachable");
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void ReadFromTestExceededSignatureHelper<TParent>()
        {
            try
            {
                var signature = "ABC";
                var version = (ushort)1234;
                var size = 16;
                var checksum = 0xC16CBAA7u;
                var data = new byte[] { 0x56, 0x78, 0x9A, 0xBC };

                // < sig -> <ver> <- size --> < chksum -> <- data -->
                // 41 42 43 d2 04 10 00 00 00 a7 ba 6c c1 56 78 9a bc
                // <sig> <ver> <- size --> < chksum -> <---- data ---->

                // The actual value of the Size property becomes too large,
                // so EndOfStreamException will be thrown.
                Th095ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, size, checksum, data));

                Assert.Fail("Unreachable");
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void ReadFromTestNegativeSizeHelper<TParent>()
        {
            try
            {
                var signature = "AB";
                var version = (ushort)1234;
                var size = -1;
                var checksum = 0xC16CBAA7u;
                var data = new byte[] { 0x56, 0x78, 0x9A, 0xBC };

                Th095ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, size, checksum, data));

                Assert.Fail("Unreached");
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void ReadFromTestZeroSizeHelper<TParent>()
        {
            try
            {
                var signature = "AB";
                var version = (ushort)1234;
                var size = 0;
                var checksum = 0xC16CBAA7u;
                var data = new byte[] { 0x56, 0x78, 0x9A, 0xBC };

                Th095ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, size, checksum, data));

                Assert.Fail("Unreached");
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void ReadFromTestShortenedSizeHelper<TParent>()
        {
            try
            {
                var signature = "AB";
                var version = (ushort)1234;
                var size = 15;
                var checksum = 0xC16CBAA7u;
                var data = new byte[] { 0x56, 0x78, 0x9A, 0xBC };

                var chapter = Th095ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, size, checksum, data));

                Assert.AreEqual(signature, chapter.Signature);
                Assert.AreEqual(version, chapter.Version);
                Assert.AreEqual(size, chapter.Size);
                Assert.AreEqual(checksum, chapter.Checksum);
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
        public static void ReadFromTestExceededSizeHelper<TParent>()
        {
            try
            {
                var signature = "AB";
                var version = (ushort)1234;
                var size = 17;
                var checksum = 0xC16CBAA7u;
                var data = new byte[] { 0x56, 0x78, 0x9A, 0xBC };

                Th095ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, size, checksum, data));

                Assert.Fail("Unreachable");
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void ReadFromTestInvalidChecksumHelper<TParent>()
        {
            try
            {
                var signature = "AB";
                var version = (ushort)1234;
                var size = 16;
                var checksum = 0xC16CBAA6u;
                var data = new byte[] { 0x56, 0x78, 0x9A, 0xBC };

                var chapter = Th095ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, size, checksum, data));

                Assert.AreEqual(signature, chapter.Signature);
                Assert.AreEqual(version, chapter.Version);
                Assert.AreEqual(size, chapter.Size);
                Assert.AreEqual(checksum, chapter.Checksum);
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
        public static void ReadFromTestEmptyDataHelper<TParent>()
        {
            try
            {
                var signature = "AB";
                var version = (ushort)1234;
                var size = 12;
                var checksum = 0x04D2424Du;
                var data = new byte[] { };

                var chapter = Th095ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, size, checksum, data));

                Assert.AreEqual(signature, chapter.Signature);
                Assert.AreEqual(version, chapter.Version);
                Assert.AreEqual(size, chapter.Size);
                Assert.AreEqual(checksum, chapter.Checksum);
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
        public static void ReadFromTestMisalignedDataHelper<TParent>()
        {
            try
            {
                var signature = "AB";
                var version = (ushort)1234;
                var size = 15;
                var checksum = 0x056CBAA7u;
                var data = new byte[] { 0x56, 0x78, 0x9A };

                var chapter = Th095ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, size, checksum, data));

                Assert.AreEqual(signature, chapter.Signature);
                Assert.AreEqual(version, chapter.Version);
                Assert.AreEqual(size, chapter.Size);
                Assert.AreEqual(checksum, chapter.Checksum);
                CollectionAssert.AreEqual(data, chapter.Data.ToArray());
                Assert.IsFalse(chapter.IsValid.Value);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        #region Th095

        [TestMethod()]
        public void Th095ChapterTest()
        {
            ChapterTestHelper<Th095Converter>();
        }

        [TestMethod()]
        public void Th095ChapterTestCopy()
        {
            ChapterTestCopyHelper<Th095Converter>();
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th095ChapterTestNull()
        {
            ChapterTestNullHelper<Th095Converter>();
        }

        [TestMethod()]
        public void Th095ChapterReadFromTest()
        {
            ReadFromTestHelper<Th095Converter>();
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th095ChapterReadFromTestNull()
        {
            ReadFromTestNullHelper<Th095Converter>();
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th095ChapterReadFromTestEmptySignature()
        {
            ReadFromTestEmptySignatureHelper<Th095Converter>();
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th095ChapterReadFromTestShortenedSignature()
        {
            ReadFromTestShortenedSignatureHelper<Th095Converter>();
        }

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th095ChapterReadFromTestExceededSignature()
        {
            ReadFromTestExceededSignatureHelper<Th095Converter>();
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th095ChapterReadFromTestNegativeSize()
        {
            ReadFromTestNegativeSizeHelper<Th095Converter>();
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th095ChapterReadFromTestZeroSize()
        {
            ReadFromTestZeroSizeHelper<Th095Converter>();
        }

        [TestMethod()]
        public void Th095ChapterReadFromTestShortenedSize()
        {
            ReadFromTestShortenedSizeHelper<Th095Converter>();
        }

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th095ChapterReadFromTestExceededSize()
        {
            ReadFromTestExceededSizeHelper<Th095Converter>();
        }

        [TestMethod()]
        public void Th095ChapterReadFromTestInvalidChecksum()
        {
            ReadFromTestInvalidChecksumHelper<Th095Converter>();
        }

        [TestMethod()]
        public void Th095ChapterReadFromTestEmptyData()
        {
            ReadFromTestEmptyDataHelper<Th095Converter>();
        }

        [TestMethod()]
        public void Th095ChapterReadFromTestMisalignedData()
        {
            ReadFromTestMisalignedDataHelper<Th095Converter>();
        }

        #endregion

        #region Th125

        [TestMethod()]
        public void Th125ChapterTest()
        {
            ChapterTestHelper<Th125Converter>();
        }

        [TestMethod()]
        public void Th125ChapterTestCopy()
        {
            ChapterTestCopyHelper<Th125Converter>();
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th125ChapterTestNull()
        {
            ChapterTestNullHelper<Th125Converter>();
        }

        [TestMethod()]
        public void Th125ChapterReadFromTest()
        {
            ReadFromTestHelper<Th125Converter>();
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th125ChapterReadFromTestNull()
        {
            ReadFromTestNullHelper<Th125Converter>();
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th125ChapterReadFromTestEmptySignature()
        {
            ReadFromTestEmptySignatureHelper<Th125Converter>();
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th125ChapterReadFromTestShortenedSignature()
        {
            ReadFromTestShortenedSignatureHelper<Th125Converter>();
        }

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th125ChapterReadFromTestExceededSignature()
        {
            ReadFromTestExceededSignatureHelper<Th125Converter>();
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th125ChapterReadFromTestNegativeSize()
        {
            ReadFromTestNegativeSizeHelper<Th125Converter>();
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th125ChapterReadFromTestZeroSize()
        {
            ReadFromTestZeroSizeHelper<Th125Converter>();
        }

        [TestMethod()]
        public void Th125ChapterReadFromTestShortenedSize()
        {
            ReadFromTestShortenedSizeHelper<Th125Converter>();
        }

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th125ChapterReadFromTestExceededSize()
        {
            ReadFromTestExceededSizeHelper<Th125Converter>();
        }

        [TestMethod()]
        public void Th125ChapterReadFromTestInvalidChecksum()
        {
            ReadFromTestInvalidChecksumHelper<Th125Converter>();
        }

        [TestMethod()]
        public void Th125ChapterReadFromTestEmptyData()
        {
            ReadFromTestEmptyDataHelper<Th125Converter>();
        }

        [TestMethod()]
        public void Th125ChapterReadFromTestMisalignedData()
        {
            ReadFromTestMisalignedDataHelper<Th125Converter>();
        }

        #endregion
    }
}

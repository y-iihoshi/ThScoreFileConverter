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
    public class Th06ChapterTests
    {
        internal static void Validate<TParent>(
            Th06ChapterWrapper<TParent> chapter,
            string signature,
            short size1,
            short size2,
            byte[] data)
            where TParent : ThConverter
        {
            if (chapter == null)
                throw new ArgumentNullException(nameof(chapter));

            Assert.AreEqual(signature, chapter.Signature);
            Assert.AreEqual(size1, chapter.Size1);
            Assert.AreEqual(size2, chapter.Size2);
            CollectionAssert.AreEqual(data, chapter.Data.ToArray());
            Assert.AreEqual((data?.Length > 0 ? data[0] : (byte)0), chapter.FirstByteOfData);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ChapterTestHelper<TParent>()
            where TParent : ThConverter
        {
            try
            {
                var chapter = new Th06ChapterWrapper<TParent>();
                Validate(chapter, string.Empty, 0, 0, new byte[] { });
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
                var chapter1 = new Th06ChapterWrapper<TParent>();
                var chapter2 = new Th06ChapterWrapper<TParent>(chapter1);
                Validate(chapter2, string.Empty, 0, 0, new byte[] { });
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
                var chapter = new Th06ChapterWrapper<TParent>(null);
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
                var signature = "ABCD";
                short size1 = 12;
                short size2 = 34;
                var data = new byte[] { 0x56, 0x78, 0x9A, 0xBC };

                var chapter = Th06ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));

                Validate(chapter, signature, size1, size2, data);
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
                var chapter = new Th06ChapterWrapper<TParent>();
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
                short size1 = 12;
                short size2 = 34;
                var data = new byte[] { 0x56, 0x78, 0x9A, 0xBC };

                // <-- sig --> size1 size2 <- data -->
                // __ __ __ __ 0c 00 22 00 56 78 9a bc
                //             <-- sig --> size1 size2 <dat>

                // The actual value of the Size1 property becomes too large and
                // the Data property becomes empty,
                // so EndOfStreamException will be thrown.
                Th06ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));

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
                var signature = "ABC";
                short size1 = 12;
                short size2 = 34;
                var data = new byte[] { 0x56, 0x78, 0x9A, 0xBC };

                // <-- sig --> size1 size2 <- data -->
                // __ 41 42 43 0c 00 22 00 56 78 9a bc
                //    <-- sig --> size1 size2 < dat ->

                // The actual value of the Size property becomes too large,
                // so EndOfStreamException will be thrown.
                Th06ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));

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
                var signature = "ABCDE";
                short size1 = 12;
                short size2 = 34;
                var data = new byte[] { 0x56, 0x78, 0x9A, 0xBC };

                // <--- sig ----> size1 size2 <- data -->
                // 41 42 43 44 45 0c 00 22 00 56 78 9a bc
                // <-- sig --> size1 size2 <---- data ---->

                // The actual value of the Size property becomes too large,
                // so EndOfStreamException will be thrown.
                Th06ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestNegativeSize1Helper<TParent>()
            where TParent : ThConverter
        {
            try
            {
                var signature = "ABCD";
                short size1 = -1;
                short size2 = 34;
                var data = new byte[] { 0x56, 0x78, 0x9A, 0xBC };

                Th06ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestZeroSize1Helper<TParent>()
            where TParent : ThConverter
        {
            try
            {
                var signature = "ABCD";
                short size1 = 0;
                short size2 = 34;
                var data = new byte[] { 0x56, 0x78, 0x9A, 0xBC };

                Th06ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestShortenedSize1Helper<TParent>()
            where TParent : ThConverter
        {
            try
            {
                var signature = "ABCD";
                short size1 = 11;
                short size2 = 34;
                var data = new byte[] { 0x56, 0x78, 0x9A, 0xBC };

                var chapter = Th06ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));

                Assert.AreEqual(signature, chapter.Signature);
                Assert.AreEqual(size1, chapter.Size1);
                Assert.AreEqual(size2, chapter.Size2);
                CollectionAssert.AreNotEqual(data, chapter.Data.ToArray());
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestExceededSize1Helper<TParent>()
            where TParent : ThConverter
        {
            try
            {
                var signature = "ABCD";
                short size1 = 13;
                short size2 = 34;
                var data = new byte[] { 0x56, 0x78, 0x9A, 0xBC };

                var chapter = Th06ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));

                Validate(chapter, signature, size1, size2, data);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestNegativeSize2Helper<TParent>()
            where TParent : ThConverter
        {
            try
            {
                var signature = "ABCD";
                short size1 = 12;
                short size2 = -1;
                var data = new byte[] { 0x56, 0x78, 0x9A, 0xBC };

                var chapter = Th06ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));

                Validate(chapter, signature, size1, size2, data);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestZeroSize2Helper<TParent>()
            where TParent : ThConverter
        {
            try
            {
                var signature = "ABCD";
                short size1 = 12;
                short size2 = 0;
                var data = new byte[] { 0x56, 0x78, 0x9A, 0xBC };

                var chapter = Th06ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));

                Validate(chapter, signature, size1, size2, data);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestShortenedSize2Helper<TParent>()
            where TParent : ThConverter
        {
            try
            {
                var signature = "ABCD";
                short size1 = 12;
                short size2 = 33;
                var data = new byte[] { 0x56, 0x78, 0x9A, 0xBC };

                var chapter = Th06ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));

                Validate(chapter, signature, size1, size2, data);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestExceededSize2Helper<TParent>()
            where TParent : ThConverter
        {
            try
            {
                var signature = "ABCD";
                short size1 = 12;
                short size2 = 35;
                var data = new byte[] { 0x56, 0x78, 0x9A, 0xBC };

                var chapter = Th06ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));

                Validate(chapter, signature, size1, size2, data);
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
                var signature = "ABCD";
                short size1 = 12;
                short size2 = 34;
                var data = new byte[] { };

                var chapter = Th06ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));

                Validate(chapter, signature, size1, size2, data);
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
                var signature = "ABCD";
                short size1 = 11;
                short size2 = 34;
                var data = new byte[] { 0x56, 0x78, 0x9A };

                var chapter = Th06ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));

                Validate(chapter, signature, size1, size2, data);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        #region Th06

        [TestMethod()]
        public void Th06ChapterTest()
            => ChapterTestHelper<Th06Converter>();

        [TestMethod()]
        public void Th06ChapterTestCopy()
            => ChapterTestCopyHelper<Th06Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th06ChapterTestNull()
            => ChapterTestNullHelper<Th06Converter>();

        [TestMethod()]
        public void Th06ChapterReadFromTest()
            => ReadFromTestHelper<Th06Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th06ChapterReadFromTestNull()
            => ReadFromTestNullHelper<Th06Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th06ChapterReadFromTestEmptySignature()
            => ReadFromTestEmptySignatureHelper<Th06Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th06ChapterReadFromTestShortenedSignature()
            => ReadFromTestShortenedSignatureHelper<Th06Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th06ChapterReadFromTestExceededSignature()
            => ReadFromTestExceededSignatureHelper<Th06Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th06ChapterReadFromTestNegativeSize1()
            => ReadFromTestNegativeSize1Helper<Th06Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th06ChapterReadFromTestZeroSize1()
            => ReadFromTestZeroSize1Helper<Th06Converter>();

        [TestMethod()]
        public void Th06ChapterReadFromTestShortenedSize1()
            => ReadFromTestShortenedSize1Helper<Th06Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th06ChapterReadFromTestExceededSize1()
            => ReadFromTestExceededSize1Helper<Th06Converter>();

        [TestMethod()]
        public void Th06ChapterReadFromTestNegativeSize2()
            => ReadFromTestNegativeSize2Helper<Th06Converter>();

        [TestMethod()]
        public void Th06ChapterReadFromTestZeroSize2()
            => ReadFromTestZeroSize2Helper<Th06Converter>();

        [TestMethod()]
        public void Th06ChapterReadFromTestShortenedSize2()
            => ReadFromTestShortenedSize2Helper<Th06Converter>();

        [TestMethod()]
        public void Th06ChapterReadFromTestExceededSize2()
            => ReadFromTestExceededSize2Helper<Th06Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th06ChapterReadFromTestEmptyData()
            => ReadFromTestEmptyDataHelper<Th06Converter>();

        [TestMethod()]
        public void Th06ChapterReadFromTestMisalignedData()
            => ReadFromTestMisalignedDataHelper<Th06Converter>();

        #endregion

        #region Th07

        [TestMethod()]
        public void Th07ChapterTest()
            => ChapterTestHelper<Th07Converter>();

        [TestMethod()]
        public void Th07ChapterTestCopy()
            => ChapterTestCopyHelper<Th07Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th07ChapterTestNull()
            => ChapterTestNullHelper<Th07Converter>();

        [TestMethod()]
        public void Th07ChapterReadFromTest()
            => ReadFromTestHelper<Th07Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th07ChapterReadFromTestNull()
            => ReadFromTestNullHelper<Th07Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th07ChapterReadFromTestEmptySignature()
            => ReadFromTestEmptySignatureHelper<Th07Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th07ChapterReadFromTestShortenedSignature()
            => ReadFromTestShortenedSignatureHelper<Th07Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th07ChapterReadFromTestExceededSignature()
            => ReadFromTestExceededSignatureHelper<Th07Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th07ChapterReadFromTestNegativeSize1()
            => ReadFromTestNegativeSize1Helper<Th07Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th07ChapterReadFromTestZeroSize1()
            => ReadFromTestZeroSize1Helper<Th07Converter>();

        [TestMethod()]
        public void Th07ChapterReadFromTestShortenedSize1()
            => ReadFromTestShortenedSize1Helper<Th07Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th07ChapterReadFromTestExceededSize1()
            => ReadFromTestExceededSize1Helper<Th07Converter>();

        [TestMethod()]
        public void Th07ChapterReadFromTestNegativeSize2()
            => ReadFromTestNegativeSize2Helper<Th07Converter>();

        [TestMethod()]
        public void Th07ChapterReadFromTestZeroSize2()
            => ReadFromTestZeroSize2Helper<Th07Converter>();

        [TestMethod()]
        public void Th07ChapterReadFromTestShortenedSize2()
            => ReadFromTestShortenedSize2Helper<Th07Converter>();

        [TestMethod()]
        public void Th07ChapterReadFromTestExceededSize2()
            => ReadFromTestExceededSize2Helper<Th07Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th07ChapterReadFromTestEmptyData()
            => ReadFromTestEmptyDataHelper<Th07Converter>();

        [TestMethod()]
        public void Th07ChapterReadFromTestMisalignedData()
            => ReadFromTestMisalignedDataHelper<Th07Converter>();

        #endregion

        #region Th08

        [TestMethod()]
        public void Th08ChapterTest()
            => ChapterTestHelper<Th08Converter>();

        [TestMethod()]
        public void Th08ChapterTestCopy()
            => ChapterTestCopyHelper<Th08Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th08ChapterTestNull()
            => ChapterTestNullHelper<Th08Converter>();

        [TestMethod()]
        public void Th08ChapterReadFromTest()
            => ReadFromTestHelper<Th08Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th08ChapterReadFromTestNull()
            => ReadFromTestNullHelper<Th08Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th08ChapterReadFromTestEmptySignature()
            => ReadFromTestEmptySignatureHelper<Th08Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th08ChapterReadFromTestShortenedSignature()
            => ReadFromTestShortenedSignatureHelper<Th08Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th08ChapterReadFromTestExceededSignature()
            => ReadFromTestExceededSignatureHelper<Th08Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th08ChapterReadFromTestNegativeSize1()
            => ReadFromTestNegativeSize1Helper<Th08Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th08ChapterReadFromTestZeroSize1()
            => ReadFromTestZeroSize1Helper<Th08Converter>();

        [TestMethod()]
        public void Th08ChapterReadFromTestShortenedSize1()
            => ReadFromTestShortenedSize1Helper<Th08Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th08ChapterReadFromTestExceededSize1()
            => ReadFromTestExceededSize1Helper<Th08Converter>();

        [TestMethod()]
        public void Th08ChapterReadFromTestNegativeSize2()
            => ReadFromTestNegativeSize2Helper<Th08Converter>();

        [TestMethod()]
        public void Th08ChapterReadFromTestZeroSize2()
            => ReadFromTestZeroSize2Helper<Th08Converter>();

        [TestMethod()]
        public void Th08ChapterReadFromTestShortenedSize2()
            => ReadFromTestShortenedSize2Helper<Th08Converter>();

        [TestMethod()]
        public void Th08ChapterReadFromTestExceededSize2()
            => ReadFromTestExceededSize2Helper<Th08Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th08ChapterReadFromTestEmptyData()
            => ReadFromTestEmptyDataHelper<Th08Converter>();

        [TestMethod()]
        public void Th08ChapterReadFromTestMisalignedData()
            => ReadFromTestMisalignedDataHelper<Th08Converter>();

        #endregion

        #region Th09

        [TestMethod()]
        public void Th09ChapterTest()
            => ChapterTestHelper<Th09Converter>();

        [TestMethod()]
        public void Th09ChapterTestCopy()
            => ChapterTestCopyHelper<Th09Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th09ChapterTestNull()
            => ChapterTestNullHelper<Th09Converter>();

        [TestMethod()]
        public void Th09ChapterReadFromTest()
            => ReadFromTestHelper<Th09Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th09ChapterReadFromTestNull()
            => ReadFromTestNullHelper<Th09Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th09ChapterReadFromTestEmptySignature()
            => ReadFromTestEmptySignatureHelper<Th09Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th09ChapterReadFromTestShortenedSignature()
            => ReadFromTestShortenedSignatureHelper<Th09Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th09ChapterReadFromTestExceededSignature()
            => ReadFromTestExceededSignatureHelper<Th09Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th09ChapterReadFromTestNegativeSize1()
            => ReadFromTestNegativeSize1Helper<Th09Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th09ChapterReadFromTestZeroSize1()
            => ReadFromTestZeroSize1Helper<Th09Converter>();

        [TestMethod()]
        public void Th09ChapterReadFromTestShortenedSize1()
            => ReadFromTestShortenedSize1Helper<Th09Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th09ChapterReadFromTestExceededSize1()
            => ReadFromTestExceededSize1Helper<Th09Converter>();

        [TestMethod()]
        public void Th09ChapterReadFromTestNegativeSize2()
            => ReadFromTestNegativeSize2Helper<Th09Converter>();

        [TestMethod()]
        public void Th09ChapterReadFromTestZeroSize2()
            => ReadFromTestZeroSize2Helper<Th09Converter>();

        [TestMethod()]
        public void Th09ChapterReadFromTestShortenedSize2()
            => ReadFromTestShortenedSize2Helper<Th09Converter>();

        [TestMethod()]
        public void Th09ChapterReadFromTestExceededSize2()
            => ReadFromTestExceededSize2Helper<Th09Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th09ChapterReadFromTestEmptyData()
            => ReadFromTestEmptyDataHelper<Th09Converter>();

        [TestMethod()]
        public void Th09ChapterReadFromTestMisalignedData()
            => ReadFromTestMisalignedDataHelper<Th09Converter>();

        #endregion
    }
}

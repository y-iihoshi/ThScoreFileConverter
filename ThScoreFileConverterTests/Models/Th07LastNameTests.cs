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
    public class Th07LastNameTests
    {
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void LastNameTestChapterHelper<TParent>()
        {
            try
            {
                var signature = "LSNM";
                short size1 = 0x18;
                short size2 = 0x18;
                var unknown1 = 1u;
                var name = TestUtils.MakeRandomArray<byte>(12);
                var data = TestUtils.MakeByteArray(unknown1, name);

                var chapter = Th06ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var lastName = new Th07LastNameWrapper<TParent>(chapter);

                Assert.AreEqual(signature, lastName.Signature);
                Assert.AreEqual(size1, lastName.Size1);
                Assert.AreEqual(size2, lastName.Size2);
                CollectionAssert.AreEqual(data, lastName.Data.ToArray());
                Assert.AreEqual(data[0], lastName.FirstByteOfData);
                CollectionAssert.AreEqual(name, lastName.Name.ToArray());
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "lastName")]
        public static void LastNameTestNullChapterHelper<TParent>()
        {
            try
            {
                var lastName = new Th07LastNameWrapper<TParent>(null);

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "lastName")]
        public static void LastNameTestInvalidSignatureHelper<TParent>()
        {
            try
            {
                var signature = "lsnm";
                short size1 = 0x18;
                short size2 = 0x18;
                var unknown1 = 1u;
                var name = TestUtils.MakeRandomArray<byte>(12);
                var data = TestUtils.MakeByteArray(unknown1, name);

                var chapter = Th06ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var lastName = new Th07LastNameWrapper<TParent>(chapter);

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "lastName")]
        public static void LastNameTestInvalidSize1Helper<TParent>()
        {
            try
            {
                var signature = "LSNM";
                short size1 = 0x19;
                short size2 = 0x18;
                var unknown1 = 1u;
                var name = TestUtils.MakeRandomArray<byte>(12);
                var data = TestUtils.MakeByteArray(unknown1, name);

                var chapter = Th06ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var lastName = new Th07LastNameWrapper<TParent>(chapter);

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        #region Th07

        [TestMethod()]
        public void Th07LastNameTestChapter()
            => LastNameTestChapterHelper<Th07Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th07LastNameTestNullChapter()
            => LastNameTestNullChapterHelper<Th07Converter>();

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th07LastNameTestInvalidSignature()
            => LastNameTestInvalidSignatureHelper<Th07Converter>();

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th07LastNameTestInvalidSize1()
            => LastNameTestInvalidSize1Helper<Th07Converter>();

        #endregion

        #region Th08

        [TestMethod()]
        public void Th08LastNameTestChapter()
            => LastNameTestChapterHelper<Th08Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th08LastNameTestNullChapter()
            => LastNameTestNullChapterHelper<Th08Converter>();

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08LastNameTestInvalidSignature()
            => LastNameTestInvalidSignatureHelper<Th08Converter>();

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08LastNameTestInvalidSize1()
            => LastNameTestInvalidSize1Helper<Th08Converter>();

        #endregion

        #region Th09

        [TestMethod()]
        public void Th09LastNameTestChapter()
            => LastNameTestChapterHelper<Th09Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th09LastNameTestNullChapter()
            => LastNameTestNullChapterHelper<Th09Converter>();

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th09LastNameTestInvalidSignature()
            => LastNameTestInvalidSignatureHelper<Th09Converter>();

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th09LastNameTestInvalidSize1()
            => LastNameTestInvalidSize1Helper<Th09Converter>();

        #endregion
    }
}

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
    public class Th07VersionInfoTests
    {
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void VersionInfoTestChapterHelper<TParent>()
        {
            try
            {
                var signature = "VRSM";
                short size1 = 0x1C;
                short size2 = 0x1C;
                var unknown1 = 1u;
                var version = TestUtils.MakeRandomArray<byte>(6);
                var unknown2 = TestUtils.MakeRandomArray<byte>(3);
                var unknown3 = TestUtils.MakeRandomArray<byte>(3);
                var unknown4 = 4u;
                var data = TestUtils.MakeByteArray(unknown1, version, unknown2, unknown3, unknown4);

                var chapter = Th06ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var versionInfo = new Th07VersionInfoWrapper<TParent>(chapter);

                Assert.AreEqual(signature, versionInfo.Signature);
                Assert.AreEqual(size1, versionInfo.Size1);
                Assert.AreEqual(size2, versionInfo.Size2);
                CollectionAssert.AreEqual(data, versionInfo.Data.ToArray());
                Assert.AreEqual(data[0], versionInfo.FirstByteOfData);
                CollectionAssert.AreEqual(version, versionInfo.Version.ToArray());
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "versionInfo")]
        public static void VersionInfoTestNullChapterHelper<TParent>()
        {
            try
            {
                var versionInfo = new Th07VersionInfoWrapper<TParent>(null);

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "versionInfo")]
        public static void VersionInfoTestInvalidSignatureHelper<TParent>()
        {
            try
            {
                var signature = "vrsm";
                short size1 = 0x1C;
                short size2 = 0x1C;
                var unknown1 = 1u;
                var version = TestUtils.MakeRandomArray<byte>(6);
                var unknown2 = TestUtils.MakeRandomArray<byte>(3);
                var unknown3 = TestUtils.MakeRandomArray<byte>(3);
                var unknown4 = 4u;
                var data = TestUtils.MakeByteArray(unknown1, version, unknown2, unknown3, unknown4);

                var chapter = Th06ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var versionInfo = new Th07VersionInfoWrapper<TParent>(chapter);

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "versionInfo")]
        public static void VersionInfoTestInvalidSize1Helper<TParent>()
        {
            try
            {
                var signature = "VRSM";
                short size1 = 0x1D;
                short size2 = 0x1C;
                var unknown1 = 1u;
                var version = TestUtils.MakeRandomArray<byte>(6);
                var unknown2 = TestUtils.MakeRandomArray<byte>(3);
                var unknown3 = TestUtils.MakeRandomArray<byte>(3);
                var unknown4 = 4u;
                var data = TestUtils.MakeByteArray(unknown1, version, unknown2, unknown3, unknown4);

                var chapter = Th06ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var versionInfo = new Th07VersionInfoWrapper<TParent>(chapter);

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
        public void Th07VersionInfoTestChapter()
            => VersionInfoTestChapterHelper<Th07Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th07VersionInfoTestNullChapter()
            => VersionInfoTestNullChapterHelper<Th07Converter>();

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th07VersionInfoTestInvalidSignature()
            => VersionInfoTestInvalidSignatureHelper<Th07Converter>();

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th07VersionInfoTestInvalidSize1()
            => VersionInfoTestInvalidSize1Helper<Th07Converter>();

        #endregion

        #region Th08

        [TestMethod()]
        public void Th08VersionInfoTestChapter()
            => VersionInfoTestChapterHelper<Th08Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th08VersionInfoTestNullChapter()
            => VersionInfoTestNullChapterHelper<Th08Converter>();

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08VersionInfoTestInvalidSignature()
            => VersionInfoTestInvalidSignatureHelper<Th08Converter>();

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08VersionInfoTestInvalidSize1()
            => VersionInfoTestInvalidSize1Helper<Th08Converter>();

        #endregion

        #region Th09

        [TestMethod()]
        public void Th09VersionInfoTestChapter()
            => VersionInfoTestChapterHelper<Th09Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th09VersionInfoTestNullChapter()
            => VersionInfoTestNullChapterHelper<Th09Converter>();

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th09VersionInfoTestInvalidSignature()
            => VersionInfoTestInvalidSignatureHelper<Th09Converter>();

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th09VersionInfoTestInvalidSize1()
            => VersionInfoTestInvalidSize1Helper<Th09Converter>();

        #endregion
    }
}

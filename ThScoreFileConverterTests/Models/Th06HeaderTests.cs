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
    public class Th06HeaderTests
    {
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void Th06HeaderTestHelper<TParent>(string signature)
        {
            if (signature == null)
                throw new ArgumentNullException(nameof(signature));

            try
            {
                short size1 = 12;
                short size2 = 12;
                var data = new byte[] { 0x10, 0x00, 0x00, 0x00 };

                var chapter = Th06ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var header = new Th06HeaderWrapper<TParent>(chapter);

                Assert.AreEqual(signature, header.Signature);
                Assert.AreEqual(size1, header.Size1);
                Assert.AreEqual(size2, header.Size2);
                CollectionAssert.AreEqual(data, header.Data.ToArray());
                Assert.AreEqual(data[0], header.FirstByteOfData);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "header")]
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void Th06HeaderTestNullHelper<TParent>()
        {
            try
            {
                var header = new Th06HeaderWrapper<TParent>(null);

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "header")]
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void Th06HeaderTestInvalidSignatureHelper<TParent>(string signature)
        {
            if (signature == null)
                throw new ArgumentNullException(nameof(signature));

            try
            {
                short size1 = 12;
                short size2 = 12;
                var data = new byte[] { 0x10, 0x00, 0x00, 0x00 };

                var chapter = Th06ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var header = new Th06HeaderWrapper<TParent>(chapter);

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "header")]
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void Th06HeaderTestInvalidSize1Helper<TParent>(string signature)
        {
            if (signature == null)
                throw new ArgumentNullException(nameof(signature));

            try
            {
                short size1 = 13;
                short size2 = 12;
                var data = new byte[] { 0x10, 0x00, 0x00, 0x00, 0x00 };

                var chapter = Th06ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var header = new Th06HeaderWrapper<TParent>(chapter);

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        #region Th06

        [TestMethod()]
        public void Th06HeaderTest()
            => Th06HeaderTestHelper<Th06Converter>("TH6K");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th06HeaderTestNull()
            => Th06HeaderTestNullHelper<Th06Converter>();

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th06HeaderTestInvalidSignature()
            => Th06HeaderTestInvalidSignatureHelper<Th06Converter>("TH6k");

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th06HeaderTestInvalidSize1()
            => Th06HeaderTestInvalidSize1Helper<Th06Converter>("TH6K");

        #endregion

        #region Th07

        [TestMethod()]
        public void Th07HeaderTest()
            => Th06HeaderTestHelper<Th07Converter>("TH7K");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th07HeaderTestNull()
            => Th06HeaderTestNullHelper<Th07Converter>();

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th07HeaderTestInvalidSignature()
            => Th06HeaderTestInvalidSignatureHelper<Th07Converter>("TH7k");

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th07HeaderTestInvalidSize1()
            => Th06HeaderTestInvalidSize1Helper<Th07Converter>("TH7K");

        #endregion

        #region Th08

        [TestMethod()]
        public void Th08HeaderTest()
            => Th06HeaderTestHelper<Th08Converter>("TH8K");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th08HeaderTestNull()
            => Th06HeaderTestNullHelper<Th08Converter>();

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08HeaderTestInvalidSignature()
            => Th06HeaderTestInvalidSignatureHelper<Th08Converter>("TH8k");

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08HeaderTestInvalidSize1()
            => Th06HeaderTestInvalidSize1Helper<Th08Converter>("TH8K");

        #endregion

        #region Th09

        [TestMethod()]
        public void Th09HeaderTest()
            => Th06HeaderTestHelper<Th09Converter>("TH9K");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th09HeaderTestNull()
            => Th06HeaderTestNullHelper<Th09Converter>();

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th09HeaderTestInvalidSignature()
            => Th06HeaderTestInvalidSignatureHelper<Th09Converter>("TH9k");

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th09HeaderTestInvalidSize1()
            => Th06HeaderTestInvalidSize1Helper<Th09Converter>("TH9K");

        #endregion
    }
}

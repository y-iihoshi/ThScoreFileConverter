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
    public class Th08FlspTests
    {
        [TestMethod()]
        public void Th08FlspTestChapter()
        {
            try
            {
                var signature = "FLSP";
                short size1 = 0x20;
                short size2 = 0x20;
                var data = TestUtils.MakeRandomArray<byte>(0x18);

                var chapter = Th06ChapterWrapper<Th08Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var flsp = new Th08FlspWrapper<Th08Converter>(chapter);

                Assert.AreEqual(signature, flsp.Signature);
                Assert.AreEqual(size1, flsp.Size1);
                Assert.AreEqual(size2, flsp.Size2);
                CollectionAssert.AreEqual(data, flsp.Data.ToArray());
                Assert.AreEqual(data[0], flsp.FirstByteOfData);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "flsp")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th08FlspTestNullChapter()
        {
            try
            {
                var flsp = new Th08FlspWrapper<Th08Converter>(null);

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "flsp")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08FlspTestInvalidSignature()
        {
            try
            {
                var signature = "flsp";
                short size1 = 0x20;
                short size2 = 0x20;
                var data = TestUtils.MakeRandomArray<byte>(0x18);

                var chapter = Th06ChapterWrapper<Th08Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var flsp = new Th08FlspWrapper<Th08Converter>(chapter);

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "flsp")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08FlspTestInvalidSize1()
        {
            try
            {
                var signature = "FLSP";
                short size1 = 0x21;
                short size2 = 0x20;
                var data = TestUtils.MakeRandomArray<byte>(0x18);

                var chapter = Th06ChapterWrapper<Th08Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var flsp = new Th08FlspWrapper<Th08Converter>(chapter);

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }
    }
}

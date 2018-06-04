using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass()]
    public class Th095StatusTests
    {
        [TestMethod()]
        public void Th095StatusTestChapter()
        {
            try
            {
                var signature = "ST";
                var version = (ushort)0;
                var size = 0x458;
                var checksum = 0u;
                var lastName = "Player1\0\0\0";
                var unknown = TestUtils.MakeRandomArray<byte>(0x442);
                var data = TestUtils.MakeByteArray(lastName.ToCharArray(), unknown);

                var chapter = Th095ChapterWrapper<Th095Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, size, checksum, data));
                var status = new Th095StatusWrapper(chapter);

                Assert.AreEqual(signature, status.Signature);
                Assert.AreEqual(version, status.Version);
                Assert.AreEqual(size, status.Size);
                Assert.AreEqual(checksum, status.Checksum);
                Assert.IsFalse(status.IsValid.Value);
                CollectionAssert.AreEqual(data, status.Data.ToArray());
                CollectionAssert.AreEqual(Encoding.Default.GetBytes(lastName), status.LastName.ToArray());
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th095StatusTestNullChapter()
        {
            try
            {
                var status = new Th095StatusWrapper(null);

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th095StatusTestInvalidSignature()
        {
            try
            {
                var signature = "st";
                var version = (ushort)0;
                var size = 0x458;
                var checksum = 0u;
                var lastName = "Player1\0\0\0";
                var unknown = TestUtils.MakeRandomArray<byte>(0x442);
                var data = TestUtils.MakeByteArray(lastName.ToCharArray(), unknown);

                var chapter = Th095ChapterWrapper<Th095Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, size, checksum, data));
                var status = new Th095StatusWrapper(chapter);

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th095StatusTestInvalidVersion()
        {
            try
            {
                var signature = "ST";
                var version = (ushort)1;
                var size = 0x458;
                var checksum = 0u;
                var lastName = "Player1\0\0\0";
                var unknown = TestUtils.MakeRandomArray<byte>(0x442);
                var data = TestUtils.MakeByteArray(lastName.ToCharArray(), unknown);

                var chapter = Th095ChapterWrapper<Th095Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, size, checksum, data));
                var status = new Th095StatusWrapper(chapter);

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th095StatusTestInvalidSize()
        {
            try
            {
                var signature = "ST";
                var version = (ushort)0;
                var size = 0x459;
                var checksum = 0u;
                var lastName = "Player1\0\0\0";
                var unknown = TestUtils.MakeRandomArray<byte>(0x442);
                var data = TestUtils.MakeByteArray(lastName.ToCharArray(), unknown);

                var chapter = Th095ChapterWrapper<Th095Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, size, checksum, data));
                var status = new Th095StatusWrapper(chapter);

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        internal static void CanInitializeTestHelper(string signature, ushort version, int size, bool expected)
        {
            try
            {
                var checksum = 0u;
                var data = new byte[size];

                var chapter = Th095ChapterWrapper<Th095Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, size, checksum, data));

                Assert.AreEqual(expected, Th095StatusWrapper.CanInitialize(chapter));
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [TestMethod()]
        public void Th095StatusCanInitializeTest()
            => CanInitializeTestHelper("ST", 0, 0x458, true);

        [TestMethod()]
        public void Th095StatusCanInitializeTestInvalidSignature()
            => CanInitializeTestHelper("st", 0, 0x458, false);

        [TestMethod()]
        public void Th095StatusCanInitializeTestInvalidVersion()
            => CanInitializeTestHelper("ST", 1, 0x458, false);

        [TestMethod()]
        public void Th095StatusCanInitializeTestInvalidSize()
            => CanInitializeTestHelper("ST", 0, 0x459, false);
    }
}

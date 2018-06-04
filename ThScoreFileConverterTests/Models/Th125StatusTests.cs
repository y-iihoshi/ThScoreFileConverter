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
    public class Th125StatusTests
    {
        [TestMethod()]
        public void Th125StatusTestChapter()
        {
            try
            {
                var signature = "ST";
                var version = (ushort)1;
                var size = 0x474;
                var checksum = 0u;
                var lastName = "Player1\0\0\0";
                var unknown1 = TestUtils.MakeRandomArray<byte>(0x2);
                var bgmFlags = TestUtils.MakeRandomArray<byte>(6);
                var unknown2 = TestUtils.MakeRandomArray<byte>(0x2E);
                var totalPlayTime = 12345678;
                var unknown3 = TestUtils.MakeRandomArray<byte>(0x424);
                var data = TestUtils.MakeByteArray(
                    lastName.ToCharArray(), unknown1, bgmFlags, unknown2, totalPlayTime, unknown3);

                var chapter = Th095ChapterWrapper<Th125Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, size, checksum, data));
                var status = new Th125StatusWrapper(chapter);

                Assert.AreEqual(signature, status.Signature);
                Assert.AreEqual(version, status.Version);
                Assert.AreEqual(size, status.Size);
                Assert.AreEqual(checksum, status.Checksum);
                Assert.IsFalse(status.IsValid.Value);
                CollectionAssert.AreEqual(data, status.Data.ToArray());
                CollectionAssert.AreEqual(Encoding.Default.GetBytes(lastName), status.LastName.ToArray());
                CollectionAssert.AreEqual(bgmFlags, status.BgmFlags.ToArray());
                Assert.AreEqual(totalPlayTime, status.TotalPlayTime);
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
        public void Th125StatusTestNullChapter()
        {
            try
            {
                var status = new Th125StatusWrapper(null);

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
        public void Th125StatusTestInvalidSignature()
        {
            try
            {
                var signature = "st";
                var version = (ushort)1;
                var size = 0x474;
                var checksum = 0u;
                var lastName = "Player1\0\0\0";
                var unknown1 = TestUtils.MakeRandomArray<byte>(0x2);
                var bgmFlags = TestUtils.MakeRandomArray<byte>(6);
                var unknown2 = TestUtils.MakeRandomArray<byte>(0x2E);
                var totalPlayTime = 12345678;
                var unknown3 = TestUtils.MakeRandomArray<byte>(0x424);
                var data = TestUtils.MakeByteArray(
                    lastName.ToCharArray(), unknown1, bgmFlags, unknown2, totalPlayTime, unknown3);

                var chapter = Th095ChapterWrapper<Th125Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, size, checksum, data));
                var status = new Th125StatusWrapper(chapter);

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
        public void Th125StatusTestInvalidVersion()
        {
            try
            {
                var signature = "ST";
                var version = (ushort)0;
                var size = 0x474;
                var checksum = 0u;
                var lastName = "Player1\0\0\0";
                var unknown1 = TestUtils.MakeRandomArray<byte>(0x2);
                var bgmFlags = TestUtils.MakeRandomArray<byte>(6);
                var unknown2 = TestUtils.MakeRandomArray<byte>(0x2E);
                var totalPlayTime = 12345678;
                var unknown3 = TestUtils.MakeRandomArray<byte>(0x424);
                var data = TestUtils.MakeByteArray(
                    lastName.ToCharArray(), unknown1, bgmFlags, unknown2, totalPlayTime, unknown3);

                var chapter = Th095ChapterWrapper<Th125Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, size, checksum, data));
                var status = new Th125StatusWrapper(chapter);

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
        public void Th125StatusTestInvalidSize()
        {
            try
            {
                var signature = "ST";
                var version = (ushort)1;
                var size = 0x475;
                var checksum = 0u;
                var lastName = "Player1\0\0\0";
                var unknown1 = TestUtils.MakeRandomArray<byte>(0x2);
                var bgmFlags = TestUtils.MakeRandomArray<byte>(6);
                var unknown2 = TestUtils.MakeRandomArray<byte>(0x2E);
                var totalPlayTime = 12345678;
                var unknown3 = TestUtils.MakeRandomArray<byte>(0x424);
                var data = TestUtils.MakeByteArray(
                    lastName.ToCharArray(), unknown1, bgmFlags, unknown2, totalPlayTime, unknown3);

                var chapter = Th095ChapterWrapper<Th125Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, size, checksum, data));
                var status = new Th125StatusWrapper(chapter);

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

                var chapter = Th095ChapterWrapper<Th125Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, size, checksum, data));

                Assert.AreEqual(expected, Th125StatusWrapper.CanInitialize(chapter));
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [TestMethod()]
        public void Th125StatusCanInitializeTest()
            => CanInitializeTestHelper("ST", 1, 0x474, true);

        [TestMethod()]
        public void Th125StatusCanInitializeTestInvalidSignature()
            => CanInitializeTestHelper("st", 1, 0x474, false);

        [TestMethod()]
        public void Th125StatusCanInitializeTestInvalidVersion()
            => CanInitializeTestHelper("ST", 0, 0x474, false);

        [TestMethod()]
        public void Th125StatusCanInitializeTestInvalidSize()
            => CanInitializeTestHelper("ST", 1, 0x475, false);
    }
}

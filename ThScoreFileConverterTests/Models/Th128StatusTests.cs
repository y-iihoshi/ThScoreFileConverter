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
    public class Th128StatusTests
    {
        // NOTE: header == (signature, version, size, checksum)
        internal static Func<string, int> getHeaderSize =
            (signature) => Encoding.Default.GetByteCount(signature) + sizeof(ushort) + sizeof(uint) + sizeof(int);

        // NOTE: data == (lastName, gap1, bgms, gap2, totalPlayTime, gap3)
        internal static Func<int, string, int, int, int, int> getGap3Size =
            (dataSize, lastName, numBgms, gap1Size, gap2Size)
                => dataSize - Encoding.Default.GetByteCount(lastName) - gap1Size - numBgms - gap2Size - sizeof(int);

        internal static void StatusTestChapterHelper<TParent>(
            ushort version, int size, int numBgms, int gap1Size, int gap2Size)
            where TParent : ThConverter
        {
            try
            {
                var signature = "ST";
                var checksum = 0u;
                var lastName = "Player1\0\0\0";
                var unknown1 = TestUtils.MakeRandomArray<byte>(gap1Size);
                var bgmFlags = TestUtils.MakeRandomArray<byte>(numBgms);
                var unknown2 = TestUtils.MakeRandomArray<byte>(gap2Size);
                var totalPlayTime = 12345678;
                var unknown3 = TestUtils.MakeRandomArray<byte>(
                    getGap3Size(size - getHeaderSize(signature), lastName, numBgms, gap1Size, gap2Size));
                var data = TestUtils.MakeByteArray(
                    lastName.ToCharArray(), unknown1, bgmFlags, unknown2, totalPlayTime, unknown3);

                var chapter = Th10ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));
                var status = new Th128StatusWrapper<TParent>(chapter);

                Assert.AreEqual(signature, status.Signature);
                Assert.AreEqual(checksum, status.Checksum);
                Assert.AreEqual(version, status.Version);
                Assert.AreEqual(size, status.Size);
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
        internal static void StatusTestNullChapterHelper<TParent>()
            where TParent : ThConverter
        {
            try
            {
                var status = new Th128StatusWrapper<TParent>(null);

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        internal static void StatusTestInvalidSignatureHelper<TParent>(
            ushort version, int size, int numBgms, int gap1Size, int gap2Size)
            where TParent : ThConverter
        {
            try
            {
                var signature = "st";
                var checksum = 0u;
                var lastName = "Player1\0\0\0";
                var unknown1 = TestUtils.MakeRandomArray<byte>(gap1Size);
                var bgmFlags = TestUtils.MakeRandomArray<byte>(numBgms);
                var unknown2 = TestUtils.MakeRandomArray<byte>(gap2Size);
                var totalPlayTime = 12345678;
                var unknown3 = TestUtils.MakeRandomArray<byte>(
                    getGap3Size(size - getHeaderSize(signature), lastName, numBgms, gap1Size, gap2Size));
                var data = TestUtils.MakeByteArray(
                    lastName.ToCharArray(), unknown1, bgmFlags, unknown2, totalPlayTime, unknown3);

                var chapter = Th10ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));
                var status = new Th128StatusWrapper<TParent>(chapter);

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        internal static void StatusTestInvalidVersionHelper<TParent>(
            ushort version, int size, int numBgms, int gap1Size, int gap2Size)
            where TParent : ThConverter
        {
            try
            {
                var signature = "ST";
                var checksum = 0u;
                var lastName = "Player1\0\0\0";
                var unknown1 = TestUtils.MakeRandomArray<byte>(gap1Size);
                var bgmFlags = TestUtils.MakeRandomArray<byte>(numBgms);
                var unknown2 = TestUtils.MakeRandomArray<byte>(gap2Size);
                var totalPlayTime = 12345678;
                var unknown3 = TestUtils.MakeRandomArray<byte>(
                    getGap3Size(size - getHeaderSize(signature), lastName, numBgms, gap1Size, gap2Size));
                var data = TestUtils.MakeByteArray(
                    lastName.ToCharArray(), unknown1, bgmFlags, unknown2, totalPlayTime, unknown3);

                var chapter = Th10ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));
                var status = new Th128StatusWrapper<TParent>(chapter);

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        internal static void StatusTestInvalidSizeHelper<TParent>(
            ushort version, int size, int numBgms, int gap1Size, int gap2Size)
            where TParent : ThConverter
        {
            try
            {
                var signature = "ST";
                var checksum = 0u;
                var lastName = "Player1\0\0\0";
                var unknown1 = TestUtils.MakeRandomArray<byte>(gap1Size);
                var bgmFlags = TestUtils.MakeRandomArray<byte>(numBgms);
                var unknown2 = TestUtils.MakeRandomArray<byte>(gap2Size);
                var totalPlayTime = 12345678;
                var unknown3 = TestUtils.MakeRandomArray<byte>(
                    getGap3Size(size - getHeaderSize(signature), lastName, numBgms, gap1Size, gap2Size));
                var data = TestUtils.MakeByteArray(
                    lastName.ToCharArray(), unknown1, bgmFlags, unknown2, totalPlayTime, unknown3);

                var chapter = Th10ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));
                var status = new Th128StatusWrapper<TParent>(chapter);

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        internal static void CanInitializeTestHelper<TParent>(string signature, ushort version, int size, bool expected)
            where TParent : ThConverter
        {
            try
            {
                var checksum = 0u;
                var data = new byte[size];

                var chapter = Th10ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

                Assert.AreEqual(expected, Th128StatusWrapper<TParent>.CanInitialize(chapter));
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        #region Th128

        [TestMethod()]
        public void Th128StatusTestChapter()
            => StatusTestChapterHelper<Th128Converter>(2, 0x42C, 10, 0x10, 0x18);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th128StatusTestNullChapter()
            => StatusTestNullChapterHelper<Th128Converter>();

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th128StatusTestInvalidSignature()
            => StatusTestInvalidSignatureHelper<Th128Converter>(2, 0x42C, 10, 0x10, 0x18);

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th128StatusTestInvalidVersion()
            => StatusTestInvalidVersionHelper<Th128Converter>(1, 0x42C, 10, 0x10, 0x18);

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th128StatusTestInvalidSize()
            => StatusTestInvalidSizeHelper<Th128Converter>(2, 0x42D, 10, 0x10, 0x18);

        [TestMethod()]
        public void Th128StatusCanInitializeTest()
            => CanInitializeTestHelper<Th128Converter>("ST", 2, 0x42C, true);

        [TestMethod()]
        public void Th128StatusCanInitializeTestInvalidSignature()
            => CanInitializeTestHelper<Th128Converter>("st", 2, 0x42C, false);

        [TestMethod()]
        public void Th128StatusCanInitializeTestInvalidVersion()
            => CanInitializeTestHelper<Th128Converter>("ST", 1, 0x42C, false);

        [TestMethod()]
        public void Th128StatusCanInitializeTestInvalidSize()
            => CanInitializeTestHelper<Th128Converter>("ST", 2, 0x42D, false);

        #endregion

        #region Th13

        [TestMethod()]
        public void Th13StatusTestChapter()
            => StatusTestChapterHelper<Th13Converter>(1, 0x42C, 17, 0x10, 0x11);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th13StatusTestNullChapter()
            => StatusTestNullChapterHelper<Th13Converter>();

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th13StatusTestInvalidSignature()
            => StatusTestInvalidSignatureHelper<Th13Converter>(1, 0x42C, 17, 0x10, 0x11);

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th13StatusTestInvalidVersion()
            => StatusTestInvalidVersionHelper<Th13Converter>(0, 0x42C, 17, 0x10, 0x11);

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th13StatusTestInvalidSize()
            => StatusTestInvalidSizeHelper<Th13Converter>(1, 0x42D, 17, 0x10, 0x11);

        [TestMethod()]
        public void Th13StatusCanInitializeTest()
            => CanInitializeTestHelper<Th13Converter>("ST", 1, 0x42C, true);

        [TestMethod()]
        public void Th13StatusCanInitializeTestInvalidSignature()
            => CanInitializeTestHelper<Th13Converter>("st", 1, 0x42C, false);

        [TestMethod()]
        public void Th13StatusCanInitializeTestInvalidVersion()
            => CanInitializeTestHelper<Th13Converter>("ST", 0, 0x42C, false);

        [TestMethod()]
        public void Th13StatusCanInitializeTestInvalidSize()
            => CanInitializeTestHelper<Th13Converter>("ST", 1, 0x42D, false);

        #endregion

        #region Th14

        [TestMethod()]
        public void Th14StatusTestChapter()
            => StatusTestChapterHelper<Th14Converter>(1, 0x42C, 17, 0x10, 0x11);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th14StatusTestNullChapter()
            => StatusTestNullChapterHelper<Th14Converter>();

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th14StatusTestInvalidSignature()
            => StatusTestInvalidSignatureHelper<Th14Converter>(1, 0x42C, 17, 0x10, 0x11);

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th14StatusTestInvalidVersion()
            => StatusTestInvalidVersionHelper<Th14Converter>(0, 0x42C, 17, 0x10, 0x11);

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th14StatusTestInvalidSize()
            => StatusTestInvalidSizeHelper<Th14Converter>(1, 0x42D, 17, 0x10, 0x11);

        [TestMethod()]
        public void Th14StatusCanInitializeTest()
            => CanInitializeTestHelper<Th14Converter>("ST", 1, 0x42C, true);

        [TestMethod()]
        public void Th14StatusCanInitializeTestInvalidSignature()
            => CanInitializeTestHelper<Th14Converter>("st", 1, 0x42C, false);

        [TestMethod()]
        public void Th14StatusCanInitializeTestInvalidVersion()
            => CanInitializeTestHelper<Th14Converter>("ST", 0, 0x42C, false);

        [TestMethod()]
        public void Th14StatusCanInitializeTestInvalidSize()
            => CanInitializeTestHelper<Th14Converter>("ST", 1, 0x42D, false);

        #endregion

        #region Th15

        [TestMethod()]
        public void Th15StatusTestChapter()
            => StatusTestChapterHelper<Th15Converter>(1, 0x42C, 17, 0x10, 0x11);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th15StatusTestNullChapter()
            => StatusTestNullChapterHelper<Th15Converter>();

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th15StatusTestInvalidSignature()
            => StatusTestInvalidSignatureHelper<Th15Converter>(1, 0x42C, 17, 0x10, 0x11);

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th15StatusTestInvalidVersion()
            => StatusTestInvalidVersionHelper<Th15Converter>(0, 0x42C, 17, 0x10, 0x11);

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th15StatusTestInvalidSize()
            => StatusTestInvalidSizeHelper<Th15Converter>(1, 0x42D, 17, 0x10, 0x11);

        [TestMethod()]
        public void Th15StatusCanInitializeTest()
            => CanInitializeTestHelper<Th15Converter>("ST", 1, 0x42C, true);

        [TestMethod()]
        public void Th15StatusCanInitializeTestInvalidSignature()
            => CanInitializeTestHelper<Th15Converter>("st", 1, 0x42C, false);

        [TestMethod()]
        public void Th15StatusCanInitializeTestInvalidVersion()
            => CanInitializeTestHelper<Th15Converter>("ST", 0, 0x42C, false);

        [TestMethod()]
        public void Th15StatusCanInitializeTestInvalidSize()
            => CanInitializeTestHelper<Th15Converter>("ST", 1, 0x42D, false);

        #endregion

        #region Th16

        [TestMethod()]
        public void Th16StatusTestChapter()
            => StatusTestChapterHelper<Th16Converter>(1, 0x42C, 17, 0x10, 0x11);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th16StatusTestNullChapter()
            => StatusTestNullChapterHelper<Th16Converter>();

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th16StatusTestInvalidSignature()
            => StatusTestInvalidSignatureHelper<Th16Converter>(1, 0x42C, 17, 0x10, 0x11);

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th16StatusTestInvalidVersion()
            => StatusTestInvalidVersionHelper<Th16Converter>(0, 0x42C, 17, 0x10, 0x11);

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th16StatusTestInvalidSize()
            => StatusTestInvalidSizeHelper<Th16Converter>(1, 0x42D, 17, 0x10, 0x11);

        [TestMethod()]
        public void Th16StatusCanInitializeTest()
            => CanInitializeTestHelper<Th16Converter>("ST", 1, 0x42C, true);

        [TestMethod()]
        public void Th16StatusCanInitializeTestInvalidSignature()
            => CanInitializeTestHelper<Th16Converter>("st", 1, 0x42C, false);

        [TestMethod()]
        public void Th16StatusCanInitializeTestInvalidVersion()
            => CanInitializeTestHelper<Th16Converter>("ST", 0, 0x42C, false);

        [TestMethod()]
        public void Th16StatusCanInitializeTestInvalidSize()
            => CanInitializeTestHelper<Th16Converter>("ST", 1, 0x42D, false);

        #endregion
    }
}

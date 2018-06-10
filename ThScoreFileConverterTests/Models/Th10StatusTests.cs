using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass()]
    public class Th10StatusTests
    {
        internal static void StatusTestChapterHelper<TParent>(ushort version, int size, int numBgms)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var signature = "ST";
                var checksum = 0u;
                // NOTE: header == (signature, version, checksum, size)
                var headerSize = Encoding.Default.GetByteCount(signature) + sizeof(ushort) + sizeof(uint) + sizeof(int);
                var lastName = "Player1\0\0\0";
                var unknown1 = TestUtils.MakeRandomArray<byte>(0x10);
                var bgmFlags = TestUtils.MakeRandomArray<byte>(numBgms);
                var unknown2 = TestUtils.MakeRandomArray<byte>(
                    size - headerSize - Encoding.Default.GetByteCount(lastName) - unknown1.Length - numBgms);
                var data = TestUtils.MakeByteArray(lastName.ToCharArray(), unknown1, bgmFlags, unknown2);

                var chapter = Th10ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));
                var status = new Th10StatusWrapper<TParent>(chapter);

                Assert.AreEqual(signature, status.Signature);
                Assert.AreEqual(version, status.Version);
                Assert.AreEqual(checksum, status.Checksum);
                Assert.AreEqual(size, status.Size);
                CollectionAssert.AreEqual(data, status.Data.ToArray());
                CollectionAssert.AreEqual(Encoding.Default.GetBytes(lastName), status.LastName.ToArray());
                CollectionAssert.AreEqual(bgmFlags, status.BgmFlags.ToArray());
            });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        internal static void StatusTestNullChapterHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var status = new Th10StatusWrapper<TParent>(null);

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        internal static void StatusTestInvalidSignatureHelper<TParent>(ushort version, int size, int numBgms)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var signature = "st";
                var checksum = 0u;
                // NOTE: header == (signature, version, checksum, size)
                var headerSize = Encoding.Default.GetByteCount(signature) + sizeof(ushort) + sizeof(uint) + sizeof(int);
                var lastName = "Player1\0\0\0";
                var unknown1 = TestUtils.MakeRandomArray<byte>(0x10);
                var bgmFlags = TestUtils.MakeRandomArray<byte>(numBgms);
                var unknown2 = TestUtils.MakeRandomArray<byte>(
                    size - headerSize - Encoding.Default.GetByteCount(lastName) - unknown1.Length - numBgms);
                var data = TestUtils.MakeByteArray(lastName.ToCharArray(), unknown1, bgmFlags, unknown2);

                var chapter = Th10ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));
                var status = new Th10StatusWrapper<TParent>(chapter);

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        internal static void StatusTestInvalidVersionHelper<TParent>(ushort version, int size, int numBgms)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var signature = "ST";
                var checksum = 0u;
                // NOTE: header == (signature, version, checksum, size)
                var headerSize = Encoding.Default.GetByteCount(signature) + sizeof(ushort) + sizeof(uint) + sizeof(int);
                var lastName = "Player1\0\0\0";
                var unknown1 = TestUtils.MakeRandomArray<byte>(0x10);
                var bgmFlags = TestUtils.MakeRandomArray<byte>(numBgms);
                var unknown2 = TestUtils.MakeRandomArray<byte>(
                    size - headerSize - Encoding.Default.GetByteCount(lastName) - unknown1.Length - numBgms);
                var data = TestUtils.MakeByteArray(lastName.ToCharArray(), unknown1, bgmFlags, unknown2);

                var chapter = Th10ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));
                var status = new Th10StatusWrapper<TParent>(chapter);

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        internal static void StatusTestInvalidSizeHelper<TParent>(ushort version, int size, int numBgms)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var signature = "ST";
                var checksum = 0u;
                // NOTE: header == (signature, version, checksum, size)
                var headerSize = Encoding.Default.GetByteCount(signature) + sizeof(ushort) + sizeof(uint) + sizeof(int);
                var lastName = "Player1\0\0\0";
                var unknown1 = TestUtils.MakeRandomArray<byte>(0x10);
                var bgmFlags = TestUtils.MakeRandomArray<byte>(numBgms);
                var unknown2 = TestUtils.MakeRandomArray<byte>(
                    size - headerSize - Encoding.Default.GetByteCount(lastName) - unknown1.Length - numBgms);
                var data = TestUtils.MakeByteArray(lastName.ToCharArray(), unknown1, bgmFlags, unknown2);

                var chapter = Th10ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));
                var status = new Th10StatusWrapper<TParent>(chapter);

                Assert.Fail(TestUtils.Unreachable);
            });

        internal static void CanInitializeTestHelper<TParent>(string signature, ushort version, int size, bool expected)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var checksum = 0u;
                var data = new byte[size];

                var chapter = Th10ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

                Assert.AreEqual(expected, Th10StatusWrapper<TParent>.CanInitialize(chapter));
            });

        #region Th10

        [TestMethod()]
        public void Th10StatusTestChapter()
            => StatusTestChapterHelper<Th10Converter>(0, 0x448, 18);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th10StatusTestNullChapter()
            => StatusTestNullChapterHelper<Th10Converter>();

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th10StatusTestInvalidSignature()
            => StatusTestInvalidSignatureHelper<Th10Converter>(0, 0x448, 18);

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th10StatusTestInvalidVersion()
            => StatusTestInvalidVersionHelper<Th10Converter>(1, 0x448, 18);

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th10StatusTestInvalidSize()
            => StatusTestInvalidSizeHelper<Th10Converter>(0, 0x449, 18);

        [TestMethod()]
        public void Th10StatusCanInitializeTest()
            => CanInitializeTestHelper<Th10Converter>("ST", 0, 0x448, true);

        [TestMethod()]
        public void Th10StatusCanInitializeTestInvalidSignature()
            => CanInitializeTestHelper<Th10Converter>("st", 0, 0x448, false);

        [TestMethod()]
        public void Th10StatusCanInitializeTestInvalidVersion()
            => CanInitializeTestHelper<Th10Converter>("ST", 1, 0x448, false);

        [TestMethod()]
        public void Th10StatusCanInitializeTestInvalidSize()
            => CanInitializeTestHelper<Th10Converter>("ST", 0, 0x449, false);

        #endregion

        #region Th11

        [TestMethod()]
        public void Th11StatusTestChapter()
            => StatusTestChapterHelper<Th11Converter>(0, 0x448, 17);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th11StatusTestNullChapter()
            => StatusTestNullChapterHelper<Th11Converter>();

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th11StatusTestInvalidSignature()
            => StatusTestInvalidSignatureHelper<Th11Converter>(0, 0x448, 17);

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th11StatusTestInvalidVersion()
            => StatusTestInvalidVersionHelper<Th11Converter>(1, 0x448, 17);

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th11StatusTestInvalidSize()
            => StatusTestInvalidSizeHelper<Th11Converter>(0, 0x449, 17);

        [TestMethod()]
        public void Th11StatusCanInitializeTest()
            => CanInitializeTestHelper<Th11Converter>("ST", 0, 0x448, true);

        [TestMethod()]
        public void Th11StatusCanInitializeTestInvalidSignature()
            => CanInitializeTestHelper<Th11Converter>("st", 0, 0x448, false);

        [TestMethod()]
        public void Th11StatusCanInitializeTestInvalidVersion()
            => CanInitializeTestHelper<Th11Converter>("ST", 1, 0x448, false);

        [TestMethod()]
        public void Th11StatusCanInitializeTestInvalidSize()
            => CanInitializeTestHelper<Th11Converter>("ST", 0, 0x449, false);

        #endregion

        #region Th12

        [TestMethod()]
        public void Th12StatusTestChapter()
            => StatusTestChapterHelper<Th12Converter>(2, 0x448, 17);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th12StatusTestNullChapter()
            => StatusTestNullChapterHelper<Th12Converter>();

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th12StatusTestInvalidSignature()
            => StatusTestInvalidSignatureHelper<Th12Converter>(2, 0x448, 17);

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th12StatusTestInvalidVersion()
            => StatusTestInvalidVersionHelper<Th12Converter>(1, 0x448, 17);

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th12StatusTestInvalidSize()
            => StatusTestInvalidSizeHelper<Th12Converter>(2, 0x449, 17);

        [TestMethod()]
        public void Th12StatusCanInitializeTest()
            => CanInitializeTestHelper<Th12Converter>("ST", 2, 0x448, true);

        [TestMethod()]
        public void Th12StatusCanInitializeTestInvalidSignature()
            => CanInitializeTestHelper<Th12Converter>("st", 2, 0x448, false);

        [TestMethod()]
        public void Th12StatusCanInitializeTestInvalidVersion()
            => CanInitializeTestHelper<Th12Converter>("ST", 1, 0x448, false);

        [TestMethod()]
        public void Th12StatusCanInitializeTestInvalidSize()
            => CanInitializeTestHelper<Th12Converter>("ST", 2, 0x449, false);

        #endregion
    }
}

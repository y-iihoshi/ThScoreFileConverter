using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th10;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th10.Stubs;
using ThScoreFileConverterTests.Models.Th10.Wrappers;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th10StatusTests
    {
        internal static StatusStub GetValidStub(ushort version, int size, int numBgms) => new StatusStub()
        {
            Signature = "ST",
            Version = version,
            Checksum = 0u,
            Size = size,
            LastName = TestUtils.CP932Encoding.GetBytes("Player1\0\0\0"),
            BgmFlags = TestUtils.MakeRandomArray<byte>(numBgms)
        };

        internal static byte[] MakeData(IStatus status)
        {
            // NOTE: header == (signature, version, checksum, size)
            var headerSize =
                TestUtils.CP932Encoding.GetByteCount(status.Signature) + sizeof(ushort) + sizeof(uint) + sizeof(int);
            // NOTE: data == (lastName, gap1, bgms, gap2)
            var dataSize = status.Size - headerSize;
            var gap1Size = 0x10;
            var gap2Size = dataSize - status.LastName.Count() - gap1Size - status.BgmFlags.Count();

            return TestUtils.MakeByteArray(
                status.LastName, new byte[gap1Size], status.BgmFlags, new byte[gap2Size]);
        }

        internal static byte[] MakeByteArray(IStatus status)
            => TestUtils.MakeByteArray(
                status.Signature.ToCharArray(),
                status.Version,
                status.Checksum,
                status.Size,
                MakeData(status));

        internal static void Validate<TParent>(IStatus expected, in Th10StatusWrapper<TParent> actual)
            where TParent : ThConverter
        {
            var data = MakeData(expected);

            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Version, actual.Version);
            Assert.AreEqual(expected.Checksum, actual.Checksum);
            Assert.AreEqual(expected.Size, actual.Size);
            CollectionAssert.That.AreEqual(data, actual.Data);
            CollectionAssert.That.AreEqual(expected.LastName, actual.LastName);
            CollectionAssert.That.AreEqual(expected.BgmFlags, actual.BgmFlags);
        }

        internal static void StatusTestChapterHelper<TParent>(ushort version, int size, int numBgms)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var stub = GetValidStub(version, size, numBgms);

                var chapter = ChapterWrapper.Create(MakeByteArray(stub));
                var status = new Th10StatusWrapper<TParent>(chapter);

                Validate(stub, status);
                Assert.IsFalse(status.IsValid.Value);
            });

        internal static void StatusTestNullChapterHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                _ = new Th10StatusWrapper<TParent>(null);

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        internal static void StatusTestInvalidSignatureHelper<TParent>(ushort version, int size, int numBgms)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var stub = GetValidStub(version, size, numBgms);
                stub.Signature = stub.Signature.ToLowerInvariant();

                var chapter = ChapterWrapper.Create(MakeByteArray(stub));
                _ = new Th10StatusWrapper<TParent>(chapter);

                Assert.Fail(TestUtils.Unreachable);
            });

        internal static void StatusTestInvalidVersionHelper<TParent>(ushort version, int size, int numBgms)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var stub = GetValidStub(version, size, numBgms);
                ++stub.Version;

                var chapter = ChapterWrapper.Create(MakeByteArray(stub));
                _ = new Th10StatusWrapper<TParent>(chapter);

                Assert.Fail(TestUtils.Unreachable);
            });

        internal static void StatusTestInvalidSizeHelper<TParent>(ushort version, int size, int numBgms)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var stub = GetValidStub(version, size, numBgms);
                ++stub.Size;

                var chapter = ChapterWrapper.Create(MakeByteArray(stub));
                _ = new Th10StatusWrapper<TParent>(chapter);

                Assert.Fail(TestUtils.Unreachable);
            });

        internal static void CanInitializeTestHelper<TParent>(string signature, ushort version, int size, bool expected)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var checksum = 0u;
                var data = new byte[size];

                var chapter = ChapterWrapper.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

                Assert.AreEqual(expected, Th10StatusWrapper<TParent>.CanInitialize(chapter));
            });

        #region Th10

        [TestMethod]
        public void Th10StatusTestChapter()
            => StatusTestChapterHelper<Th10Converter>(0, 0x448, 18);

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th10StatusTestNullChapter()
            => StatusTestNullChapterHelper<Th10Converter>();

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th10StatusTestInvalidSignature()
            => StatusTestInvalidSignatureHelper<Th10Converter>(0, 0x448, 18);

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th10StatusTestInvalidVersion()
            => StatusTestInvalidVersionHelper<Th10Converter>(0, 0x448, 18);

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th10StatusTestInvalidSize()
            => StatusTestInvalidSizeHelper<Th10Converter>(0, 0x448, 18);

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("ST", (ushort)0, 0x448, true)]
        [DataRow("st", (ushort)0, 0x448, false)]
        [DataRow("ST", (ushort)1, 0x448, false)]
        [DataRow("ST", (ushort)0, 0x449, false)]
        public void Th10StatusCanInitializeTest(string signature, ushort version, int size, bool expected)
            => CanInitializeTestHelper<Th10Converter>(signature, version, size, expected);

        #endregion

        #region Th11

        [TestMethod]
        public void Th11StatusTestChapter()
            => StatusTestChapterHelper<Th11Converter>(0, 0x448, 17);

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th11StatusTestNullChapter()
            => StatusTestNullChapterHelper<Th11Converter>();

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th11StatusTestInvalidSignature()
            => StatusTestInvalidSignatureHelper<Th11Converter>(0, 0x448, 17);

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th11StatusTestInvalidVersion()
            => StatusTestInvalidVersionHelper<Th11Converter>(0, 0x448, 17);

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th11StatusTestInvalidSize()
            => StatusTestInvalidSizeHelper<Th11Converter>(0, 0x448, 17);

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("ST", (ushort)0, 0x448, true)]
        [DataRow("st", (ushort)0, 0x448, false)]
        [DataRow("ST", (ushort)1, 0x448, false)]
        [DataRow("ST", (ushort)0, 0x449, false)]
        public void Th11StatusCanInitializeTest(string signature, ushort version, int size, bool expected)
            => CanInitializeTestHelper<Th11Converter>(signature, version, size, expected);

        #endregion

        #region Th12

        [TestMethod]
        public void Th12StatusTestChapter()
            => StatusTestChapterHelper<Th12Converter>(2, 0x448, 17);

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th12StatusTestNullChapter()
            => StatusTestNullChapterHelper<Th12Converter>();

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th12StatusTestInvalidSignature()
            => StatusTestInvalidSignatureHelper<Th12Converter>(2, 0x448, 17);

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th12StatusTestInvalidVersion()
            => StatusTestInvalidVersionHelper<Th12Converter>(2, 0x448, 17);

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th12StatusTestInvalidSize()
            => StatusTestInvalidSizeHelper<Th12Converter>(2, 0x448, 17);

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("ST", (ushort)2, 0x448, true)]
        [DataRow("st", (ushort)2, 0x448, false)]
        [DataRow("ST", (ushort)1, 0x448, false)]
        [DataRow("ST", (ushort)2, 0x449, false)]
        public void Th12StatusCanInitializeTest(string signature, ushort version, int size, bool expected)
            => CanInitializeTestHelper<Th12Converter>(signature, version, size, expected);

        #endregion
    }
}

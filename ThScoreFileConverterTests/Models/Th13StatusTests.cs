using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th125;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th10.Wrappers;
using ThScoreFileConverterTests.Models.Th125.Stubs;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th13StatusTests
    {
        internal static StatusStub GetValidStub(ushort version, int size, int numBgms) => new StatusStub()
        {
            Signature = "ST",
            Version = version,
            Checksum = 0u,
            Size = size,
            LastName = TestUtils.CP932Encoding.GetBytes("Player1\0\0\0"),
            BgmFlags = TestUtils.MakeRandomArray<byte>(numBgms),
            TotalPlayTime = 12345678
        };

        internal static byte[] MakeData(IStatus status, int gap1Size, int gap2Size)
        {
            // NOTE: header == (signature, version, size, checksum)
            var headerSize =
                TestUtils.CP932Encoding.GetByteCount(status.Signature) + sizeof(ushort) + sizeof(uint) + sizeof(int);
            // NOTE: data == (lastName, gap1, bgms, gap2, totalPlayTime, gap3)
            var dataSize = status.Size - headerSize;
            var gap3Size =
                dataSize - status.LastName.Count() - gap1Size - status.BgmFlags.Count() - gap2Size - sizeof(int);

            return TestUtils.MakeByteArray(
                status.LastName,
                new byte[gap1Size],
                status.BgmFlags,
                new byte[gap2Size],
                status.TotalPlayTime,
                new byte[gap3Size]);
        }

        internal static byte[] MakeByteArray(IStatus status, int gap1Size, int gap2Size)
            => TestUtils.MakeByteArray(
                status.Signature.ToCharArray(),
                status.Version,
                status.Checksum,
                status.Size,
                MakeData(status, gap1Size, gap2Size));

        internal static void Validate<TParent>(
            IStatus expected, in Th128StatusWrapper<TParent> actual, int gap1Size, int gap2Size)
            where TParent : ThConverter
        {
            var data = MakeData(expected, gap1Size, gap2Size);

            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Checksum, actual.Checksum);
            Assert.AreEqual(expected.Version, actual.Version);
            Assert.AreEqual(expected.Size, actual.Size);
            CollectionAssert.That.AreEqual(data, actual.Data);
            CollectionAssert.That.AreEqual(expected.LastName, actual.LastName);
            CollectionAssert.That.AreEqual(expected.BgmFlags, actual.BgmFlags);
            Assert.AreEqual(expected.TotalPlayTime, actual.TotalPlayTime);
        }

        internal static void StatusTestChapterHelper<TParent>(
            ushort version, int size, int numBgms, int gap1Size, int gap2Size)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var stub = GetValidStub(version, size, numBgms);

                var chapter = ChapterWrapper.Create(MakeByteArray(stub, gap1Size, gap2Size));
                var status = new Th128StatusWrapper<TParent>(chapter);

                Validate(stub, status, gap1Size, gap2Size);
                Assert.IsFalse(status.IsValid.Value);
            });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        internal static void StatusTestNullChapterHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var status = new Th128StatusWrapper<TParent>(null);

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        internal static void StatusTestInvalidSignatureHelper<TParent>(
            ushort version, int size, int numBgms, int gap1Size, int gap2Size)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var stub = GetValidStub(version, size, numBgms);
                stub.Signature = stub.Signature.ToLowerInvariant();

                var chapter = ChapterWrapper.Create(MakeByteArray(stub, gap1Size, gap2Size));
                var status = new Th128StatusWrapper<TParent>(chapter);

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        internal static void StatusTestInvalidVersionHelper<TParent>(
            ushort version, int size, int numBgms, int gap1Size, int gap2Size)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var stub = GetValidStub(version, size, numBgms);
                ++stub.Version;

                var chapter = ChapterWrapper.Create(MakeByteArray(stub, gap1Size, gap2Size));
                var status = new Th128StatusWrapper<TParent>(chapter);

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        internal static void StatusTestInvalidSizeHelper<TParent>(
            ushort version, int size, int numBgms, int gap1Size, int gap2Size)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var stub = GetValidStub(version, size, numBgms);
                ++stub.Size;

                var chapter = ChapterWrapper.Create(MakeByteArray(stub, gap1Size, gap2Size));
                var status = new Th128StatusWrapper<TParent>(chapter);

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

                Assert.AreEqual(expected, Th128StatusWrapper<TParent>.CanInitialize(chapter));
            });

        #region Th13

        [TestMethod]
        public void Th13StatusTestChapter()
            => StatusTestChapterHelper<Th13Converter>(1, 0x42C, 17, 0x10, 0x11);

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th13StatusTestNullChapter()
            => StatusTestNullChapterHelper<Th13Converter>();

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th13StatusTestInvalidSignature()
            => StatusTestInvalidSignatureHelper<Th13Converter>(1, 0x42C, 17, 0x10, 0x11);

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th13StatusTestInvalidVersion()
            => StatusTestInvalidVersionHelper<Th13Converter>(1, 0x42C, 17, 0x10, 0x11);

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th13StatusTestInvalidSize()
            => StatusTestInvalidSizeHelper<Th13Converter>(1, 0x42C, 17, 0x10, 0x11);

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("ST", (ushort)1, 0x42C, true)]
        [DataRow("st", (ushort)1, 0x42C, false)]
        [DataRow("ST", (ushort)0, 0x42C, false)]
        [DataRow("ST", (ushort)1, 0x42D, false)]
        public void Th13StatusCanInitializeTest(string signature, ushort version, int size, bool expected)
            => CanInitializeTestHelper<Th13Converter>(signature, version, size, expected);

        #endregion

        #region Th14

        [TestMethod]
        public void Th14StatusTestChapter()
            => StatusTestChapterHelper<Th14Converter>(1, 0x42C, 17, 0x10, 0x11);

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th14StatusTestNullChapter()
            => StatusTestNullChapterHelper<Th14Converter>();

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th14StatusTestInvalidSignature()
            => StatusTestInvalidSignatureHelper<Th14Converter>(1, 0x42C, 17, 0x10, 0x11);

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th14StatusTestInvalidVersion()
            => StatusTestInvalidVersionHelper<Th14Converter>(1, 0x42C, 17, 0x10, 0x11);

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th14StatusTestInvalidSize()
            => StatusTestInvalidSizeHelper<Th14Converter>(1, 0x42C, 17, 0x10, 0x11);

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("ST", (ushort)1, 0x42C, true)]
        [DataRow("st", (ushort)1, 0x42C, false)]
        [DataRow("ST", (ushort)0, 0x42C, false)]
        [DataRow("ST", (ushort)1, 0x42D, false)]
        public void Th14StatusCanInitializeTest(string signature, ushort version, int size, bool expected)
            => CanInitializeTestHelper<Th14Converter>(signature, version, size, expected);

        #endregion

        #region Th15

        [TestMethod]
        public void Th15StatusTestChapter()
            => StatusTestChapterHelper<Th15Converter>(1, 0x42C, 17, 0x10, 0x11);

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th15StatusTestNullChapter()
            => StatusTestNullChapterHelper<Th15Converter>();

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th15StatusTestInvalidSignature()
            => StatusTestInvalidSignatureHelper<Th15Converter>(1, 0x42C, 17, 0x10, 0x11);

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th15StatusTestInvalidVersion()
            => StatusTestInvalidVersionHelper<Th15Converter>(1, 0x42C, 17, 0x10, 0x11);

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th15StatusTestInvalidSize()
            => StatusTestInvalidSizeHelper<Th15Converter>(1, 0x42C, 17, 0x10, 0x11);

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("ST", (ushort)1, 0x42C, true)]
        [DataRow("st", (ushort)1, 0x42C, false)]
        [DataRow("ST", (ushort)0, 0x42C, false)]
        [DataRow("ST", (ushort)1, 0x42D, false)]
        public void Th15StatusCanInitializeTest(string signature, ushort version, int size, bool expected)
            => CanInitializeTestHelper<Th15Converter>(signature, version, size, expected);

        #endregion

        #region Th16

        [TestMethod]
        public void Th16StatusTestChapter()
            => StatusTestChapterHelper<Th16Converter>(1, 0x42C, 17, 0x10, 0x11);

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th16StatusTestNullChapter()
            => StatusTestNullChapterHelper<Th16Converter>();

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th16StatusTestInvalidSignature()
            => StatusTestInvalidSignatureHelper<Th16Converter>(1, 0x42C, 17, 0x10, 0x11);

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th16StatusTestInvalidVersion()
            => StatusTestInvalidVersionHelper<Th16Converter>(1, 0x42C, 17, 0x10, 0x11);

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th16StatusTestInvalidSize()
            => StatusTestInvalidSizeHelper<Th16Converter>(1, 0x42C, 17, 0x10, 0x11);

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("ST", (ushort)1, 0x42C, true)]
        [DataRow("st", (ushort)1, 0x42C, false)]
        [DataRow("ST", (ushort)0, 0x42C, false)]
        [DataRow("ST", (ushort)1, 0x42D, false)]
        public void Th16StatusCanInitializeTest(string signature, ushort version, int size, bool expected)
            => CanInitializeTestHelper<Th16Converter>(signature, version, size, expected);

        #endregion
    }
}

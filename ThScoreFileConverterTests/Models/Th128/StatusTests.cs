using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models.Th128;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th125.Stubs;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;
using IStatus = ThScoreFileConverter.Models.Th125.IStatus;

namespace ThScoreFileConverterTests.Models.Th128
{
    [TestClass]
    public class StatusTests
    {
        internal static StatusStub ValidStub { get; } = new StatusStub()
        {
            Signature = "ST",
            Version = 0x0002,
            Checksum = 0u,
            Size = 0x42C,
            LastName = TestUtils.CP932Encoding.GetBytes("Player1\0\0\0"),
            BgmFlags = TestUtils.MakeRandomArray<byte>(10),
            TotalPlayTime = 12345678
        };

        internal static byte[] MakeData(IStatus status)
        {
            // NOTE: header == (signature, version, size, checksum)
            var headerSize =
                TestUtils.CP932Encoding.GetByteCount(status.Signature) + sizeof(ushort) + sizeof(uint) + sizeof(int);
            // NOTE: data == (lastName, gap1, bgms, gap2, totalPlayTime, gap3)
            var dataSize = status.Size - headerSize;
            var gap1Size = 0x10;
            var gap2Size = 0x18;
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

        internal static byte[] MakeByteArray(IStatus status)
            => TestUtils.MakeByteArray(
                status.Signature.ToCharArray(),
                status.Version,
                status.Checksum,
                status.Size,
                MakeData(status));

        internal static void Validate(IStatus expected, IStatus actual)
        {
            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Checksum, actual.Checksum);
            Assert.AreEqual(expected.Version, actual.Version);
            Assert.AreEqual(expected.Size, actual.Size);
            CollectionAssert.That.AreEqual(expected.LastName, actual.LastName);
            CollectionAssert.That.AreEqual(expected.BgmFlags, actual.BgmFlags);
            Assert.AreEqual(expected.TotalPlayTime, actual.TotalPlayTime);
        }

        [TestMethod]
        public void StatusTestChapter() => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            var status = new Status(chapter);

            Validate(stub, status);
            Assert.IsFalse(status.IsValid);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StatusTestNullChapter() => TestUtils.Wrap(() =>
        {
            _ = new Status(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void StatusTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;
            stub.Signature = stub.Signature.ToLowerInvariant();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new Status(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void StatusTestInvalidVersion() => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;
            ++stub.Version;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new Status(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void StatusTestInvalidSize() => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;
            ++stub.Size;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new Status(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [DataTestMethod]
        [DataRow("ST", (ushort)2, 0x42C, true)]
        [DataRow("st", (ushort)2, 0x42C, false)]
        [DataRow("ST", (ushort)1, 0x42C, false)]
        [DataRow("ST", (ushort)2, 0x42D, false)]
        public void CanInitializeTest(string signature, ushort version, int size, bool expected) => TestUtils.Wrap(() =>
        {
            var checksum = 0u;
            var data = new byte[size];

            var chapter = TestUtils.Create<Chapter>(
                TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

            Assert.AreEqual(expected, Status.CanInitialize(chapter));
        });
    }
}

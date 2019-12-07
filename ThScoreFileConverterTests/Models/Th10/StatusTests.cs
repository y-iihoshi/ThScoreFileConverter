using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models.Th10;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th10.Stubs;

namespace ThScoreFileConverterTests.Models.Th10
{
    [TestClass]
    public class StatusTests
    {
        internal static StatusStub ValidStub { get; } = MakeValidStub(0x0000, 18);

        internal static StatusStub MakeValidStub(ushort version, int numBgms) => new StatusStub()
        {
            Signature = "ST",
            Version = version,
            Checksum = 0u,
            Size = 0x448,
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

        internal static void Validate(IStatus expected, IStatus actual)
        {
            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Version, actual.Version);
            Assert.AreEqual(expected.Checksum, actual.Checksum);
            Assert.AreEqual(expected.Size, actual.Size);
            CollectionAssert.That.AreEqual(expected.LastName, actual.LastName);
            CollectionAssert.That.AreEqual(expected.BgmFlags, actual.BgmFlags);
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
            var stub = new StatusStub(ValidStub);
            stub.Signature = stub.Signature.ToLowerInvariant();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new Status(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void StatusTestInvalidVersion() => TestUtils.Wrap(() =>
        {
            var stub = new StatusStub(ValidStub);
            ++stub.Version;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new Status(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void StatusTestInvalidSize() => TestUtils.Wrap(() =>
        {
            var stub = new StatusStub(ValidStub);
            ++stub.Size;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new Status(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [DataTestMethod]
        [DataRow("ST", (ushort)0, 0x448, true)]
        [DataRow("st", (ushort)0, 0x448, false)]
        [DataRow("ST", (ushort)1, 0x448, false)]
        [DataRow("ST", (ushort)0, 0x449, false)]
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

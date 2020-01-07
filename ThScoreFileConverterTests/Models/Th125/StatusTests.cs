using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using ThScoreFileConverter.Models.Th125;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th125.Stubs;
using Chapter = ThScoreFileConverter.Models.Th095.Chapter;

namespace ThScoreFileConverterTests.Models.Th125
{
    [TestClass]
    public class StatusTests
    {
        internal static StatusStub ValidStub { get; } = new StatusStub()
        {
            Signature = "ST",
            Version = 1,
            Size = 0x474,
            Checksum = 0u,
            LastName = TestUtils.CP932Encoding.GetBytes("Player1\0\0\0"),
            BgmFlags = TestUtils.MakeRandomArray<byte>(6),
            TotalPlayTime = 12345678,
        };

        internal static byte[] MakeData(IStatus status)
            => TestUtils.MakeByteArray(
                status.LastName,
                new byte[0x2],
                status.BgmFlags,
                new byte[0x2E],
                status.TotalPlayTime,
                new byte[0x424]);

        internal static byte[] MakeByteArray(IStatus status)
            => TestUtils.MakeByteArray(
                status.Signature.ToCharArray(),
                status.Version,
                status.Size,
                status.Checksum,
                MakeData(status));

        internal static void Validate(IStatus expected, IStatus actual)
        {
            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Version, actual.Version);
            Assert.AreEqual(expected.Size, actual.Size);
            Assert.AreEqual(expected.Checksum, actual.Checksum);
            CollectionAssert.That.AreEqual(expected.LastName, actual.LastName);
            CollectionAssert.That.AreEqual(expected.BgmFlags, actual.BgmFlags);
            Assert.AreEqual(expected.TotalPlayTime, actual.TotalPlayTime);
        }

        [TestMethod]
        public void StatusTestChapter()
        {
            var stub = ValidStub;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            var status = new Status(chapter);

            Validate(stub, status);
            Assert.IsFalse(status.IsValid);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StatusTestNullChapter()
        {
            _ = new Status(null!);

            Assert.Fail(TestUtils.Unreachable);
        }

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void StatusTestInvalidSignature()
        {
            var stub = new StatusStub(ValidStub);
            stub.Signature = stub.Signature.ToLowerInvariant();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new Status(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void StatusTestInvalidVersion()
        {
            var stub = new StatusStub(ValidStub);
            ++stub.Version;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new Status(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void StatusTestInvalidSize()
        {
            var stub = new StatusStub(ValidStub);
            --stub.Size;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new Status(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        [DataTestMethod]
        [DataRow("ST", (ushort)1, 0x474, true)]
        [DataRow("st", (ushort)1, 0x474, false)]
        [DataRow("ST", (ushort)0, 0x474, false)]
        [DataRow("ST", (ushort)1, 0x475, false)]
        public void CanInitializeTest(string signature, ushort version, int size, bool expected)
        {
            var checksum = 0u;
            var data = new byte[size];

            var chapter = TestUtils.Create<Chapter>(
                TestUtils.MakeByteArray(signature.ToCharArray(), version, size, checksum, data));

            Assert.AreEqual(expected, Status.CanInitialize(chapter));
        }
    }
}

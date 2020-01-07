using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th165;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th165.Stubs;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;

namespace ThScoreFileConverterTests.Models.Th165
{
    [TestClass]
    public class StatusTests
    {
        internal static StatusStub ValidStub { get; } = new StatusStub()
        {
            Signature = "ST",
            Version = 2,
            Checksum = 0u,
            Size = 0x224,
            LastName = TestUtils.CP932Encoding.GetBytes("Player1\0\0\0\0\0\0\0"),
            BgmFlags = TestUtils.MakeRandomArray<byte>(8),
            TotalPlayTime = 12345678,
            NicknameFlags = Enumerable.Range(0, 51).Select(num => (byte)(num % 3)).ToArray(),
        };

        internal static byte[] MakeData(IStatus status)
            => TestUtils.MakeByteArray(
                status.LastName,
                new byte[0x12],
                status.BgmFlags,
                new byte[0x18],
                status.TotalPlayTime,
                new byte[0x4C],
                status.NicknameFlags,
                new byte[0x155]);

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
            Assert.AreEqual(expected.TotalPlayTime, actual.TotalPlayTime);
            CollectionAssert.That.AreEqual(expected.NicknameFlags, actual.NicknameFlags);
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
        [DataRow("ST", (ushort)2, 0x224, true)]
        [DataRow("st", (ushort)2, 0x224, false)]
        [DataRow("ST", (ushort)1, 0x224, false)]
        [DataRow("ST", (ushort)2, 0x225, false)]
        public void CanInitializeTest(string signature, ushort version, int size, bool expected)
        {
            var checksum = 0u;
            var data = new byte[size];

            var chapter = TestUtils.Create<Chapter>(
                TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

            Assert.AreEqual(expected, Status.CanInitialize(chapter));
        }
    }
}

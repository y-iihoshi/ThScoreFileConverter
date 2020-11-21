using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models.Th125;
using ThScoreFileConverterTests.Extensions;
using Chapter = ThScoreFileConverter.Models.Th095.Chapter;

namespace ThScoreFileConverterTests.Models.Th125
{
    [TestClass]
    public class StatusTests
    {
        internal static Mock<IStatus> MockStatus()
        {
            var mock = new Mock<IStatus>();
            _ = mock.SetupGet(m => m.Signature).Returns("ST");
            _ = mock.SetupGet(m => m.Version).Returns(1);
            _ = mock.SetupGet(m => m.Size).Returns(0x474);
            _ = mock.SetupGet(m => m.Checksum).Returns(0u);
            _ = mock.SetupGet(m => m.LastName).Returns(TestUtils.CP932Encoding.GetBytes("Player1\0\0\0"));
            _ = mock.SetupGet(m => m.BgmFlags).Returns(TestUtils.MakeRandomArray<byte>(6));
            _ = mock.SetupGet(m => m.TotalPlayTime).Returns(12345678);
            return mock;
        }

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
            var mock = MockStatus();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            var status = new Status(chapter);

            Validate(mock.Object, status);
            Assert.IsFalse(status.IsValid);
        }

        [TestMethod]
        public void StatusTestNullChapter()
            => _ = Assert.ThrowsException<ArgumentNullException>(() => _ = new Status(null!));

        [TestMethod]
        public void StatusTestInvalidSignature()
        {
            var mock = MockStatus();
            var signature = mock.Object.Signature;
            _ = mock.SetupGet(m => m.Signature).Returns(signature.ToLowerInvariant());

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidDataException>(() => _ = new Status(chapter));
        }

        [TestMethod]
        public void StatusTestInvalidVersion()
        {
            var mock = MockStatus();
            var version = mock.Object.Version;
            _ = mock.SetupGet(m => m.Version).Returns(++version);

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidDataException>(() => _ = new Status(chapter));
        }

        [TestMethod]
        public void StatusTestInvalidSize()
        {
            var mock = MockStatus();
            var size = mock.Object.Size;
            _ = mock.SetupGet(m => m.Size).Returns(--size);

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidDataException>(() => _ = new Status(chapter));
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

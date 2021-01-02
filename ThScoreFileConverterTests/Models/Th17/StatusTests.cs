using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models.Th17;
using ThScoreFileConverterTests.Extensions;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;

namespace ThScoreFileConverterTests.Models.Th17
{
    [TestClass]
    public class StatusTests
    {
        internal static Mock<IStatus> MockStatus()
        {
            var mock = new Mock<IStatus>();
            _ = mock.SetupGet(m => m.Signature).Returns("ST");
            _ = mock.SetupGet(m => m.Version).Returns(0x0002);
            _ = mock.SetupGet(m => m.Checksum).Returns(0u);
            _ = mock.SetupGet(m => m.Size).Returns(0x04B0);
            _ = mock.SetupGet(m => m.LastName).Returns(TestUtils.CP932Encoding.GetBytes("Player1\0\0\0"));
            _ = mock.SetupGet(m => m.BgmFlags).Returns(TestUtils.MakeRandomArray<byte>(17));
            _ = mock.SetupGet(m => m.TotalPlayTime).Returns(12345678);
            _ = mock.SetupGet(m => m.Achievements).Returns(
                Enumerable.Range(0, 40).Select(number => (byte)(number % 3)));
            return mock;
        }

        internal static byte[] MakeByteArray(IStatus status)
            => TestUtils.MakeByteArray(
                status.Signature.ToCharArray(),
                status.Version,
                status.Checksum,
                status.Size,
                status.LastName.ToArray(),
                TestUtils.MakeRandomArray<byte>(0x10),
                status.BgmFlags.ToArray(),
                TestUtils.MakeRandomArray<byte>(0x11),
                status.TotalPlayTime,
                TestUtils.MakeRandomArray<byte>(4),
                status.Achievements.ToArray(),
                TestUtils.MakeRandomArray<byte>(0x438));

        internal static void Validate(IStatus expected, IStatus actual)
        {
            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Checksum, actual.Checksum);
            Assert.AreEqual(expected.Version, actual.Version);
            Assert.AreEqual(expected.Size, actual.Size);
            CollectionAssert.That.AreEqual(expected.LastName, actual.LastName);
            CollectionAssert.That.AreEqual(expected.BgmFlags, actual.BgmFlags);
            Assert.AreEqual(expected.TotalPlayTime, actual.TotalPlayTime);
            CollectionAssert.That.AreEqual(expected.Achievements, actual.Achievements);
        }

        [TestMethod]
        public void StatusTest()
        {
            var mock = MockStatus();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            var status = new Status(chapter);

            Validate(mock.Object, status);
            Assert.IsFalse(status.IsValid);
        }

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
        [DataRow("ST", (ushort)2, 0x4B0, true)]
        [DataRow("st", (ushort)2, 0x4B0, false)]
        [DataRow("ST", (ushort)1, 0x4B0, false)]
        [DataRow("ST", (ushort)2, 0x4B1, false)]
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

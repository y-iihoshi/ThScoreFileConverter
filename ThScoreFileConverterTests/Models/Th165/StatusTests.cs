using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models.Th165;
using ThScoreFileConverterTests.UnitTesting;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;

namespace ThScoreFileConverterTests.Models.Th165
{
    [TestClass]
    public class StatusTests
    {
        internal static Mock<IStatus> MockStatus()
        {
            var mock = new Mock<IStatus>();
            _ = mock.SetupGet(m => m.Signature).Returns("ST");
            _ = mock.SetupGet(m => m.Version).Returns(2);
            _ = mock.SetupGet(m => m.Checksum).Returns(0u);
            _ = mock.SetupGet(m => m.Size).Returns(0x224);
            _ = mock.SetupGet(m => m.LastName).Returns(TestUtils.CP932Encoding.GetBytes("Player1\0\0\0\0\0\0\0"));
            _ = mock.SetupGet(m => m.BgmFlags).Returns(TestUtils.MakeRandomArray<byte>(8));
            _ = mock.SetupGet(m => m.TotalPlayTime).Returns(12345678);
            _ = mock.SetupGet(m => m.NicknameFlags).Returns(
                Enumerable.Range(0, 51).Select(num => (byte)(num % 3)).ToArray());
            return mock;
        }

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

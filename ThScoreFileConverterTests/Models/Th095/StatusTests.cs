using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models.Th095;
using ThScoreFileConverterTests.Extensions;

namespace ThScoreFileConverterTests.Models.Th095
{
    [TestClass]
    public class StatusTests
    {
        internal static Mock<IStatus> MockStatus()
        {
            var mock = new Mock<IStatus>();
            _ = mock.SetupGet(m => m.Signature).Returns("ST");
            _ = mock.SetupGet(m => m.Version).Returns(0);
            _ = mock.SetupGet(m => m.Size).Returns(0x458);
            _ = mock.SetupGet(m => m.Checksum).Returns(0u);
            _ = mock.SetupGet(m => m.LastName).Returns(TestUtils.CP932Encoding.GetBytes("Player1\0\0\0"));
            return mock;
        }

        internal static byte[] MakeByteArray(IStatus status)
            => TestUtils.MakeByteArray(
                status.Signature.ToCharArray(),
                status.Version,
                status.Size,
                status.Checksum,
                status.LastName,
                new byte[0x442]);

        internal static void Validate(IStatus expected, IStatus actual)
        {
            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Version, actual.Version);
            Assert.AreEqual(expected.Size, actual.Size);
            Assert.AreEqual(expected.Checksum, actual.Checksum);
            CollectionAssert.That.AreEqual(expected.LastName, actual.LastName);
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
        [DataRow("ST", (ushort)0, 0x458, true)]
        [DataRow("st", (ushort)0, 0x458, false)]
        [DataRow("ST", (ushort)1, 0x458, false)]
        [DataRow("ST", (ushort)0, 0x459, false)]
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

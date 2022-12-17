using System.IO;
using System.Linq;
using Moq;
using ThScoreFileConverter.Core.Tests.UnitTesting;
using ThScoreFileConverter.Models.Th10;
using ThScoreFileConverter.Tests.UnitTesting;

namespace ThScoreFileConverter.Tests.Models.Th10;

[TestClass]
public class StatusTests
{
    internal static Mock<IStatus> MockStatus()
    {
        return MockStatus(0x0000, 18);
    }

    internal static Mock<IStatus> MockStatus(ushort version, int numBgms)
    {
        var mock = new Mock<IStatus>();
        _ = mock.SetupGet(m => m.Signature).Returns("ST");
        _ = mock.SetupGet(m => m.Version).Returns(version);
        _ = mock.SetupGet(m => m.Checksum).Returns(0u);
        _ = mock.SetupGet(m => m.Size).Returns(0x448);
        _ = mock.SetupGet(m => m.LastName).Returns(TestUtils.CP932Encoding.GetBytes("Player1\0\0\0"));
        _ = mock.SetupGet(m => m.BgmFlags).Returns(TestUtils.MakeRandomArray<byte>(numBgms));
        return mock;
    }

    internal static byte[] MakeByteArray(IStatus status)
    {
        // NOTE: header == (signature, version, checksum, size)
        var headerSize =
            TestUtils.CP932Encoding.GetByteCount(status.Signature) + sizeof(ushort) + sizeof(uint) + sizeof(int);
        // NOTE: data == (lastName, gap1, bgms, gap2)
        var dataSize = status.Size - headerSize;
        var gap1Size = 0x10;
        var gap2Size = dataSize - status.LastName.Count() - gap1Size - status.BgmFlags.Count();

        return TestUtils.MakeByteArray(
            status.Signature.ToCharArray(),
            status.Version,
            status.Checksum,
            status.Size,
            status.LastName,
            new byte[gap1Size],
            status.BgmFlags,
            new byte[gap2Size]);
    }

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
        _ = Assert.ThrowsException<InvalidDataException>(() => new Status(chapter));
    }

    [TestMethod]
    public void StatusTestInvalidVersion()
    {
        var mock = MockStatus();
        var version = mock.Object.Version;
        _ = mock.SetupGet(m => m.Version).Returns(++version);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
        _ = Assert.ThrowsException<InvalidDataException>(() => new Status(chapter));
    }

    [TestMethod]
    public void StatusTestInvalidSize()
    {
        var mock = MockStatus();
        var size = mock.Object.Size;
        _ = mock.SetupGet(m => m.Size).Returns(++size);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
        _ = Assert.ThrowsException<InvalidDataException>(() => new Status(chapter));
    }

    [DataTestMethod]
    [DataRow("ST", (ushort)0, 0x448, true)]
    [DataRow("st", (ushort)0, 0x448, false)]
    [DataRow("ST", (ushort)1, 0x448, false)]
    [DataRow("ST", (ushort)0, 0x449, false)]
    public void CanInitializeTest(string signature, ushort version, int size, bool expected)
    {
        var checksum = 0u;
        var data = new byte[size];

        var chapter = TestUtils.Create<Chapter>(
            TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

        Assert.AreEqual(expected, Status.CanInitialize(chapter));
    }
}

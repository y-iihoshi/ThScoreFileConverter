using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models.Th128;
using ThScoreFileConverterTests.UnitTesting;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;
using IStatus = ThScoreFileConverter.Models.Th125.IStatus;

namespace ThScoreFileConverterTests.Models.Th128;

[TestClass]
public class StatusTests
{
    internal static Mock<IStatus> MockStatus()
    {
        var mock = new Mock<IStatus>();
        _ = mock.SetupGet(m => m.Signature).Returns("ST");
        _ = mock.SetupGet(m => m.Version).Returns(0x0002);
        _ = mock.SetupGet(m => m.Checksum).Returns(0u);
        _ = mock.SetupGet(m => m.Size).Returns(0x42C);
        _ = mock.SetupGet(m => m.LastName).Returns(TestUtils.CP932Encoding.GetBytes("Player1\0\0\0"));
        _ = mock.SetupGet(m => m.BgmFlags).Returns(TestUtils.MakeRandomArray<byte>(10));
        _ = mock.SetupGet(m => m.TotalPlayTime).Returns(12345678);
        return mock;
    }

    internal static byte[] MakeByteArray(IStatus status)
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
            status.Signature.ToCharArray(),
            status.Version,
            status.Checksum,
            status.Size,
            status.LastName,
            new byte[gap1Size],
            status.BgmFlags,
            new byte[gap2Size],
            status.TotalPlayTime,
            new byte[gap3Size]);
    }

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
    [DataRow("ST", (ushort)2, 0x42C, true)]
    [DataRow("st", (ushort)2, 0x42C, false)]
    [DataRow("ST", (ushort)1, 0x42C, false)]
    [DataRow("ST", (ushort)2, 0x42D, false)]
    public void CanInitializeTest(string signature, ushort version, int size, bool expected)
    {
        var checksum = 0u;
        var data = new byte[size];

        var chapter = TestUtils.Create<Chapter>(
            TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

        Assert.AreEqual(expected, Status.CanInitialize(chapter));
    }
}

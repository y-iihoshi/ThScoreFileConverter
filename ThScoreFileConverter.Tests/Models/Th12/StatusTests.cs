using NSubstitute;
using ThScoreFileConverter.Models.Th12;
using ThScoreFileConverter.Tests.UnitTesting;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;
using IStatus = ThScoreFileConverter.Models.Th10.IStatus;

namespace ThScoreFileConverter.Tests.Models.Th12;

[TestClass]
public class StatusTests
{
    internal static IStatus MockStatus()
    {
        return Th10.StatusTests.MockStatus(0x0002, 17);
    }

    [TestMethod]
    public void StatusTestChapter()
    {
        var mock = MockStatus();

        var chapter = TestUtils.Create<Chapter>(Th10.StatusTests.MakeByteArray(mock));
        var status = new Status(chapter);

        Th10.StatusTests.Validate(mock, status);
        Assert.IsFalse(status.IsValid);
    }

    [TestMethod]
    public void StatusTestInvalidSignature()
    {
        var mock = MockStatus();
        var signature = mock.Signature;
        _ = mock.Signature.Returns(signature.ToLowerInvariant());

        var chapter = TestUtils.Create<Chapter>(Th10.StatusTests.MakeByteArray(mock));
        _ = Assert.ThrowsException<InvalidDataException>(() => new Status(chapter));
    }

    [TestMethod]
    public void StatusTestInvalidVersion()
    {
        var mock = MockStatus();
        var version = mock.Version;
        _ = mock.Version.Returns(++version);

        var chapter = TestUtils.Create<Chapter>(Th10.StatusTests.MakeByteArray(mock));
        _ = Assert.ThrowsException<InvalidDataException>(() => new Status(chapter));
    }

    [TestMethod]
    public void StatusTestInvalidSize()
    {
        var mock = MockStatus();
        var size = mock.Size;
        _ = mock.Size.Returns(++size);

        var chapter = TestUtils.Create<Chapter>(Th10.StatusTests.MakeByteArray(mock));
        _ = Assert.ThrowsException<InvalidDataException>(() => new Status(chapter));
    }

    [DataTestMethod]
    [DataRow("ST", (ushort)2, 0x448, true)]
    [DataRow("st", (ushort)2, 0x448, false)]
    [DataRow("ST", (ushort)1, 0x448, false)]
    [DataRow("ST", (ushort)2, 0x449, false)]
    public void CanInitializeTest(string signature, ushort version, int size, bool expected)
    {
        var checksum = 0u;
        var data = new byte[size];

        var chapter = TestUtils.Create<Chapter>(
            TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

        Assert.AreEqual(expected, Status.CanInitialize(chapter));
    }
}

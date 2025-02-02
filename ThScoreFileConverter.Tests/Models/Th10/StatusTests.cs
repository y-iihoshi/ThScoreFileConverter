using NSubstitute;
using ThScoreFileConverter.Models.Th10;

namespace ThScoreFileConverter.Tests.Models.Th10;

internal static class StatusExtensions
{
    internal static void ShouldBe(this IStatus actual, IStatus expected)
    {
        actual.Signature.ShouldBe(expected.Signature);
        actual.Version.ShouldBe(expected.Version);
        actual.Checksum.ShouldBe(expected.Checksum);
        actual.Size.ShouldBe(expected.Size);
        actual.LastName.ShouldBe(expected.LastName);
        actual.BgmFlags.ShouldBe(expected.BgmFlags);
    }
}

[TestClass]
public class StatusTests
{
    internal static IStatus MockStatus()
    {
        return MockStatus(0x0000, 18);
    }

    internal static IStatus MockStatus(ushort version, int numBgms)
    {
        var mock = Substitute.For<IStatus>();
        _ = mock.Signature.Returns("ST");
        _ = mock.Version.Returns(version);
        _ = mock.Checksum.Returns(0u);
        _ = mock.Size.Returns(0x448);
        _ = mock.LastName.Returns(TestUtils.CP932Encoding.GetBytes("Player1\0\0\0"));
        _ = mock.BgmFlags.Returns(TestUtils.MakeRandomArray(numBgms));
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

    [TestMethod]
    public void StatusTestChapter()
    {
        var mock = MockStatus();

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        var status = new Status(chapter);

        status.ShouldBe(mock);
        status.IsValid.ShouldBeFalse();
    }

    [TestMethod]
    public void StatusTestInvalidSignature()
    {
        var mock = MockStatus();
        var signature = mock.Signature;
        _ = mock.Signature.Returns(signature.ToLowerInvariant());

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidDataException>(() => new Status(chapter));
    }

    [TestMethod]
    public void StatusTestInvalidVersion()
    {
        var mock = MockStatus();
        var version = mock.Version;
        _ = mock.Version.Returns(++version);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidDataException>(() => new Status(chapter));
    }

    [TestMethod]
    public void StatusTestInvalidSize()
    {
        var mock = MockStatus();
        var size = mock.Size;
        _ = mock.Size.Returns(++size);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidDataException>(() => new Status(chapter));
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

        Status.CanInitialize(chapter).ShouldBe(expected);
    }
}

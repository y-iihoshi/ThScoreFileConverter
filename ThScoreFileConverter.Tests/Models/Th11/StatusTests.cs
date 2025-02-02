using NSubstitute;
using ThScoreFileConverter.Models.Th11;
using static ThScoreFileConverter.Tests.Models.Th10.StatusExtensions;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;
using IStatus = ThScoreFileConverter.Models.Th10.IStatus;

namespace ThScoreFileConverter.Tests.Models.Th11;

[TestClass]
public class StatusTests
{
    internal static IStatus MockStatus()
    {
        return Th10.StatusTests.MockStatus(0x0000, 17);
    }

    [TestMethod]
    public void StatusTestChapter()
    {
        var mock = MockStatus();

        var chapter = TestUtils.Create<Chapter>(Th10.StatusTests.MakeByteArray(mock));
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

        var chapter = TestUtils.Create<Chapter>(Th10.StatusTests.MakeByteArray(mock));
        _ = Should.Throw<InvalidDataException>(() => new Status(chapter));
    }

    [TestMethod]
    public void StatusTestInvalidVersion()
    {
        var mock = MockStatus();
        var version = mock.Version;
        _ = mock.Version.Returns(++version);

        var chapter = TestUtils.Create<Chapter>(Th10.StatusTests.MakeByteArray(mock));
        _ = Should.Throw<InvalidDataException>(() => new Status(chapter));
    }

    [TestMethod]
    public void StatusTestInvalidSize()
    {
        var mock = MockStatus();
        var size = mock.Size;
        _ = mock.Size.Returns(++size);

        var chapter = TestUtils.Create<Chapter>(Th10.StatusTests.MakeByteArray(mock));
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

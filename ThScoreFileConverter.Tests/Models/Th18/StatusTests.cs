using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models.Th18;
using ThScoreFileConverter.Tests.UnitTesting;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;

namespace ThScoreFileConverter.Tests.Models.Th18;

[TestClass]
public class StatusTests
{
    internal static Mock<IStatus> MockStatus()
    {
        var mock = new Mock<IStatus>();
        _ = mock.SetupGet(m => m.Signature).Returns("ST");
        _ = mock.SetupGet(m => m.Version).Returns(0x0006);
        _ = mock.SetupGet(m => m.Checksum).Returns(0u);
        _ = mock.SetupGet(m => m.Size).Returns(0x03D0);
        _ = mock.SetupGet(m => m.LastName).Returns(TestUtils.CP932Encoding.GetBytes("Player1\0\0\0"));
        _ = mock.SetupGet(m => m.BgmFlags).Returns(TestUtils.MakeRandomArray<byte>(17));
        _ = mock.SetupGet(m => m.TotalPlayTime).Returns(12345678);
        _ = mock.SetupGet(m => m.Achievements).Returns(
            Enumerable.Range(0, 30).Select(number => (byte)(number % 3)));
        _ = mock.SetupGet(m => m.AbilityCards).Returns(
            Enumerable.Range(0, 56).Select(number => (byte)(number % 4)));
        _ = mock.SetupGet(m => m.InitialHoldAbilityCards).Returns(
            Enumerable.Range(0, 0x30).Select(number => (byte)(number % 5)));
        return mock;
    }

    internal static byte[] MakeByteArray(IStatus status)
    {
        return TestUtils.MakeByteArray(
            status.Signature.ToCharArray(),
            status.Version,
            status.Checksum,
            status.Size,
            status.LastName,
            TestUtils.MakeRandomArray<byte>(0x10),
            status.BgmFlags,
            TestUtils.MakeRandomArray<byte>(0x11),
            status.TotalPlayTime,
            TestUtils.MakeRandomArray<byte>(4),
            status.Achievements,
            TestUtils.MakeRandomArray<byte>(0x62),
            status.AbilityCards,
            TestUtils.MakeRandomArray<byte>(0x48),
            status.InitialHoldAbilityCards,
            TestUtils.MakeRandomArray<byte>(0x250));
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
        CollectionAssert.That.AreEqual(expected.Achievements, actual.Achievements);
        CollectionAssert.That.AreEqual(expected.AbilityCards, actual.AbilityCards);
        CollectionAssert.That.AreEqual(expected.InitialHoldAbilityCards, actual.InitialHoldAbilityCards);
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
        _ = mock.SetupGet(m => m.Size).Returns(--size);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));

        _ = Assert.ThrowsException<InvalidDataException>(() => new Status(chapter));
    }

    [DataTestMethod]
    [DataRow("ST", (ushort)0x06, 0x3D0, true)]
    [DataRow("st", (ushort)0x06, 0x3D0, false)]
    [DataRow("ST", (ushort)0x07, 0x3D0, false)]
    [DataRow("ST", (ushort)0x06, 0x3D1, false)]
    public void CanInitializeTest(string signature, ushort version, int size, bool expected)
    {
        var checksum = 0u;
        var data = new byte[size];

        var chapter = TestUtils.Create<Chapter>(
            TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

        Assert.AreEqual(expected, Status.CanInitialize(chapter));
    }
}

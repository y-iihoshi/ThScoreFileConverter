﻿using System.IO;
using NSubstitute;
using ThScoreFileConverter.Models.Th18;
using ThScoreFileConverter.Tests.UnitTesting;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;

namespace ThScoreFileConverter.Tests.Models.Th18;

[TestClass]
public class StatusTests
{
    internal static IStatus MockStatus()
    {
        var mock = Substitute.For<IStatus>();
        _ = mock.Signature.Returns("ST");
        _ = mock.Version.Returns((ushort)0x0006);
        _ = mock.Checksum.Returns(0u);
        _ = mock.Size.Returns(0x03D0);
        _ = mock.LastName.Returns(TestUtils.CP932Encoding.GetBytes("Player1\0\0\0"));
        _ = mock.BgmFlags.Returns(TestUtils.MakeRandomArray(17));
        _ = mock.TotalPlayTime.Returns(12345678);
        _ = mock.Achievements.Returns(TestUtils.MakeRandomArray(30));
        _ = mock.AbilityCards.Returns(TestUtils.MakeRandomArray(56));
        _ = mock.InitialHoldAbilityCards.Returns(TestUtils.MakeRandomArray(0x30));
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
            TestUtils.MakeRandomArray(0x10),
            status.BgmFlags,
            TestUtils.MakeRandomArray(0x11),
            status.TotalPlayTime,
            TestUtils.MakeRandomArray(4),
            status.Achievements,
            TestUtils.MakeRandomArray(0x62),
            status.AbilityCards,
            TestUtils.MakeRandomArray(0x48),
            status.InitialHoldAbilityCards,
            TestUtils.MakeRandomArray(0x250));
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

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        var status = new Status(chapter);

        Validate(mock, status);
        Assert.IsFalse(status.IsValid);
    }

    [TestMethod]
    public void StatusTestInvalidSignature()
    {
        var mock = MockStatus();
        var signature = mock.Signature;
        _ = mock.Signature.Returns(signature.ToLowerInvariant());

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));

        _ = Assert.ThrowsException<InvalidDataException>(() => new Status(chapter));
    }

    [TestMethod]
    public void StatusTestInvalidVersion()
    {
        var mock = MockStatus();
        var version = mock.Version;
        _ = mock.Version.Returns(++version);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));

        _ = Assert.ThrowsException<InvalidDataException>(() => new Status(chapter));
    }

    [TestMethod]
    public void StatusTestInvalidSize()
    {
        var mock = MockStatus();
        var size = mock.Size;
        _ = mock.Size.Returns(--size);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));

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

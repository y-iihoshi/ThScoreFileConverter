﻿using System.IO;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Tests.UnitTesting;
using ThScoreFileConverter.Models.Th165;
using ThScoreFileConverter.Tests.UnitTesting;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;

namespace ThScoreFileConverter.Tests.Models.Th165;

[TestClass]
public class StatusTests
{
    internal static IStatus MockStatus()
    {
        var mock = Substitute.For<IStatus>();
        _ = mock.Signature.Returns("ST");
        _ = mock.Version.Returns((ushort)2);
        _ = mock.Checksum.Returns(0u);
        _ = mock.Size.Returns(0x224);
        _ = mock.LastName.Returns(TestUtils.CP932Encoding.GetBytes("Player1\0\0\0\0\0\0\0"));
        _ = mock.BgmFlags.Returns(TestUtils.MakeRandomArray(8));
        _ = mock.TotalPlayTime.Returns(12345678);
        _ = mock.NicknameFlags.Returns(
            Enumerable.Range(0, 51).Select(num => (byte)(num % 3)).ToArray());
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
            new byte[0x12],
            status.BgmFlags,
            new byte[0x18],
            status.TotalPlayTime,
            new byte[0x4C],
            status.NicknameFlags,
            new byte[0x155]);
    }

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

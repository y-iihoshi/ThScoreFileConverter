﻿using NSubstitute;
using ThScoreFileConverter.Models.Th17;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;

namespace ThScoreFileConverter.Tests.Models.Th17;

internal static class StatusExtensions
{
    internal static void ShouldBe(this IStatus actual, IStatus expected)
    {
        actual.Signature.ShouldBe(expected.Signature);
        actual.Checksum.ShouldBe(expected.Checksum);
        actual.Version.ShouldBe(expected.Version);
        actual.Size.ShouldBe(expected.Size);
        actual.LastName.ShouldBe(expected.LastName);
        actual.BgmFlags.ShouldBe(expected.BgmFlags);
        actual.TotalPlayTime.ShouldBe(expected.TotalPlayTime);
        actual.Achievements.ShouldBe(expected.Achievements);
    }
}

[TestClass]
public class StatusTests
{
    internal static IStatus MockStatus()
    {
        var mock = Substitute.For<IStatus>();
        _ = mock.Signature.Returns("ST");
        _ = mock.Version.Returns((ushort)0x0002);
        _ = mock.Checksum.Returns(0u);
        _ = mock.Size.Returns(0x04B0);
        _ = mock.LastName.Returns(TestUtils.CP932Encoding.GetBytes("Player1\0\0\0"));
        _ = mock.BgmFlags.Returns(TestUtils.MakeRandomArray(17));
        _ = mock.TotalPlayTime.Returns(12345678);
        _ = mock.Achievements.Returns(TestUtils.MakeRandomArray(40));
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
            TestUtils.MakeRandomArray(0x438));
    }

    [TestMethod]
    public void StatusTest()
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
        _ = mock.Size.Returns(--size);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));

        _ = Should.Throw<InvalidDataException>(() => new Status(chapter));
    }

    [DataTestMethod]
    [DataRow("ST", (ushort)2, 0x4B0, true)]
    [DataRow("st", (ushort)2, 0x4B0, false)]
    [DataRow("ST", (ushort)1, 0x4B0, false)]
    [DataRow("ST", (ushort)2, 0x4B1, false)]
    public void CanInitializeTest(string signature, ushort version, int size, bool expected)
    {
        var checksum = 0u;
        var data = new byte[size];

        var chapter = TestUtils.Create<Chapter>(
            TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

        Status.CanInitialize(chapter).ShouldBe(expected);
    }
}

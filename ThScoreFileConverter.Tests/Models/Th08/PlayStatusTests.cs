using System.IO;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Tests.UnitTesting;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th08;
using ThScoreFileConverter.Tests.UnitTesting;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;

namespace ThScoreFileConverter.Tests.Models.Th08;

[TestClass]
public class PlayStatusTests
{
    internal static IPlayStatus MockPlayStatus()
    {
        var playCounts = EnumHelper<Level>.Enumerable.ToDictionary(level => level, level => PlayCountTests.MockPlayCount());
        var totalPlayCount = PlayCountTests.MockPlayCount();

        var mock = Substitute.For<IPlayStatus>();
        _ = mock.Signature.Returns("PLST");
        _ = mock.Size1.Returns((short)0x228);
        _ = mock.Size2.Returns((short)0x228);
        _ = mock.TotalRunningTime.Returns(new Time(12, 34, 56, 789, false));
        _ = mock.TotalPlayTime.Returns(new Time(23, 45, 19, 876, false));
        _ = mock.PlayCounts.Returns(playCounts);
        _ = mock.TotalPlayCount.Returns(totalPlayCount);
        _ = mock.BgmFlags.Returns(TestUtils.MakeRandomArray(21));
        return mock;
    }

    internal static byte[] MakeByteArray(IPlayStatus status)
    {
        return TestUtils.MakeByteArray(
            status.Signature.ToCharArray(),
            status.Size1,
            status.Size2,
            0u,
            (int)status.TotalRunningTime.Hours,
            status.TotalRunningTime.Minutes,
            status.TotalRunningTime.Seconds,
            status.TotalRunningTime.Milliseconds,
            (int)status.TotalPlayTime.Hours,
            status.TotalPlayTime.Minutes,
            status.TotalPlayTime.Seconds,
            status.TotalPlayTime.Milliseconds,
            status.PlayCounts.Select(pair => PlayCountTests.MakeByteArray(pair.Value)),
            PlayCountTests.MakeByteArray(PlayCountTests.MockPlayCount()),
            PlayCountTests.MakeByteArray(status.TotalPlayCount),
            status.BgmFlags,
            new byte[11]);
    }

    internal static void Validate(IPlayStatus expected, IPlayStatus actual)
    {
        Assert.AreEqual(expected.Signature, actual.Signature);
        Assert.AreEqual(expected.Size1, actual.Size1);
        Assert.AreEqual(expected.Size2, actual.Size2);
        Assert.AreEqual(expected.FirstByteOfData, actual.FirstByteOfData);
        Assert.AreEqual(expected.TotalRunningTime.Hours, actual.TotalRunningTime.Hours);
        Assert.AreEqual(expected.TotalRunningTime.Minutes, actual.TotalRunningTime.Minutes);
        Assert.AreEqual(expected.TotalRunningTime.Seconds, actual.TotalRunningTime.Seconds);
        Assert.AreEqual(expected.TotalRunningTime.Milliseconds, actual.TotalRunningTime.Milliseconds);
        Assert.IsFalse(actual.TotalRunningTime.IsFrames);
        Assert.AreEqual(expected.TotalPlayTime.Hours, actual.TotalPlayTime.Hours);
        Assert.AreEqual(expected.TotalPlayTime.Minutes, actual.TotalPlayTime.Minutes);
        Assert.AreEqual(expected.TotalPlayTime.Seconds, actual.TotalPlayTime.Seconds);
        Assert.AreEqual(expected.TotalPlayTime.Milliseconds, actual.TotalPlayTime.Milliseconds);
        Assert.IsFalse(actual.TotalPlayTime.IsFrames);

        foreach (var key in expected.PlayCounts.Keys)
        {
            PlayCountTests.Validate(expected.PlayCounts[key], actual.PlayCounts[key]);
        }

        PlayCountTests.Validate(expected.TotalPlayCount, actual.TotalPlayCount);
        CollectionAssert.That.AreEqual(expected.BgmFlags, actual.BgmFlags);
    }

    [TestMethod]
    public void PlayStatusTestChapter()
    {
        var mock = MockPlayStatus();

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        var playStatus = new PlayStatus(chapter);

        Validate(mock, playStatus);
    }

    [TestMethod]
    public void PlayStatusTestInvalidSignature()
    {
        var mock = MockPlayStatus();
        var signature = mock.Signature;
        _ = mock.Signature.Returns(signature.ToLowerInvariant());

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Assert.ThrowsException<InvalidDataException>(() => new PlayStatus(chapter));
    }

    [TestMethod]
    public void PlayStatusTestInvalidSize1()
    {
        var mock = MockPlayStatus();
        var size = mock.Size1;
        _ = mock.Size1.Returns(--size);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Assert.ThrowsException<InvalidDataException>(() => new PlayStatus(chapter));
    }
}

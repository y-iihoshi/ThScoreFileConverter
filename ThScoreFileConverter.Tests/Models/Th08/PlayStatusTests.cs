using NSubstitute;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th08;
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
        actual.Signature.ShouldBe(expected.Signature);
        actual.Size1.ShouldBe(expected.Size1);
        actual.Size2.ShouldBe(expected.Size2);
        actual.FirstByteOfData.ShouldBe(expected.FirstByteOfData);
        actual.TotalRunningTime.Hours.ShouldBe(expected.TotalRunningTime.Hours);
        actual.TotalRunningTime.Minutes.ShouldBe(expected.TotalRunningTime.Minutes);
        actual.TotalRunningTime.Seconds.ShouldBe(expected.TotalRunningTime.Seconds);
        actual.TotalRunningTime.Milliseconds.ShouldBe(expected.TotalRunningTime.Milliseconds);
        actual.TotalRunningTime.IsFrames.ShouldBeFalse();
        actual.TotalPlayTime.Hours.ShouldBe(expected.TotalPlayTime.Hours);
        actual.TotalPlayTime.Minutes.ShouldBe(expected.TotalPlayTime.Minutes);
        actual.TotalPlayTime.Seconds.ShouldBe(expected.TotalPlayTime.Seconds);
        actual.TotalPlayTime.Milliseconds.ShouldBe(expected.TotalPlayTime.Milliseconds);
        actual.TotalPlayTime.IsFrames.ShouldBeFalse();

        foreach (var key in expected.PlayCounts.Keys)
        {
            PlayCountTests.Validate(expected.PlayCounts[key], actual.PlayCounts[key]);
        }

        PlayCountTests.Validate(expected.TotalPlayCount, actual.TotalPlayCount);
        actual.BgmFlags.ShouldBe(expected.BgmFlags);
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
        _ = Should.Throw<InvalidDataException>(() => new PlayStatus(chapter));
    }

    [TestMethod]
    public void PlayStatusTestInvalidSize1()
    {
        var mock = MockPlayStatus();
        var size = mock.Size1;
        _ = mock.Size1.Returns(--size);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidDataException>(() => new PlayStatus(chapter));
    }
}

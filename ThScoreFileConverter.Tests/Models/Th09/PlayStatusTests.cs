using NSubstitute;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th09;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th09;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;

namespace ThScoreFileConverter.Tests.Models.Th09;

internal static class PlayStatusExtensions
{
    internal static void ShouldBe(this IPlayStatus actual, IPlayStatus expected)
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
        actual.BgmFlags.ShouldBe(expected.BgmFlags);
        actual.MatchFlags.ShouldBe(expected.MatchFlags);
        actual.StoryFlags.ShouldBe(expected.StoryFlags);
        actual.ExtraFlags.ShouldBe(expected.ExtraFlags);

        foreach (var key in expected.ClearCounts.Keys)
        {
            actual.ClearCounts[key].ShouldBe(expected.ClearCounts[key]);
        }
    }
}

[TestClass]
public class PlayStatusTests
{
    internal static IPlayStatus MockPlayStatus()
    {
        var characters = EnumHelper<Chara>.Enumerable;
        var pairs = characters.Select((chara, index) => (chara, index)).ToArray();
        var clearCounts = characters.ToDictionary(chara => chara, _ => ClearCountTests.MockClearCount());

        var mock = Substitute.For<IPlayStatus>();
        _ = mock.Signature.Returns("PLST");
        _ = mock.Size1.Returns((short)0x1FC);
        _ = mock.Size2.Returns((short)0x1FC);
        _ = mock.TotalRunningTime.Returns(new Time(12, 34, 56, 789, false));
        _ = mock.TotalPlayTime.Returns(new Time(23, 45, 19, 876, false));
        _ = mock.BgmFlags.Returns(TestUtils.MakeRandomArray(19));
        _ = mock.MatchFlags.Returns(pairs.ToDictionary(pair => pair.chara, pair => (byte)pair.index));
        _ = mock.StoryFlags.Returns(pairs.ToDictionary(pair => pair.chara, pair => (byte)(20 + pair.index)));
        _ = mock.ExtraFlags.Returns(pairs.ToDictionary(pair => pair.chara, pair => (byte)(40 + pair.index)));
        _ = mock.ClearCounts.Returns(clearCounts);
        return mock;
    }

    internal static byte[] MakeByteArray(IPlayStatus playStatus)
    {
        return TestUtils.MakeByteArray(
            playStatus.Signature.ToCharArray(),
            playStatus.Size1,
            playStatus.Size2,
            0u,
            (int)playStatus.TotalRunningTime.Hours,
            playStatus.TotalRunningTime.Minutes,
            playStatus.TotalRunningTime.Seconds,
            playStatus.TotalRunningTime.Milliseconds,
            (int)playStatus.TotalPlayTime.Hours,
            playStatus.TotalPlayTime.Minutes,
            playStatus.TotalPlayTime.Seconds,
            playStatus.TotalPlayTime.Milliseconds,
            playStatus.BgmFlags,
            new byte[13],
            playStatus.MatchFlags.Values,
            playStatus.StoryFlags.Values,
            playStatus.ExtraFlags.Values,
            playStatus.ClearCounts.Select(pair => ClearCountTests.MakeByteArray(pair.Value)));
    }

    [TestMethod]
    public void PlayStatusTestChapter()
    {
        var mock = MockPlayStatus();

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        var playStatus = new PlayStatus(chapter);

        playStatus.ShouldBe(mock);
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

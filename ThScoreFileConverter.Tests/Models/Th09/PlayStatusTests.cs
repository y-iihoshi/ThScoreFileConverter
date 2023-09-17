using System.IO;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th09;
using ThScoreFileConverter.Core.Tests.UnitTesting;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th09;
using ThScoreFileConverter.Tests.UnitTesting;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;

namespace ThScoreFileConverter.Tests.Models.Th09;

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
        CollectionAssert.That.AreEqual(expected.BgmFlags, actual.BgmFlags);
        CollectionAssert.That.AreEqual(expected.MatchFlags.Values, actual.MatchFlags.Values);
        CollectionAssert.That.AreEqual(expected.StoryFlags.Values, actual.StoryFlags.Values);
        CollectionAssert.That.AreEqual(expected.ExtraFlags.Values, actual.ExtraFlags.Values);

        foreach (var key in expected.ClearCounts.Keys)
        {
            ClearCountTests.Validate(expected.ClearCounts[key], actual.ClearCounts[key]);
        }
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

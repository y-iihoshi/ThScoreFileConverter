using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th07;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;
using LevelWithTotal = ThScoreFileConverter.Core.Models.Th07.LevelWithTotal;

namespace ThScoreFileConverter.Tests.Models.Th07;

[TestClass]
public class PlayStatusTests
{
    internal struct Properties
    {
        public string signature;
        public short size1;
        public short size2;
        public Time totalRunningTime;
        public Time totalPlayTime;
        public Dictionary<LevelWithTotal, PlayCountTests.Properties> playCounts;
    }

    internal static Properties ValidProperties { get; } = new Properties()
    {
        signature = "PLST",
        size1 = 0x160,
        size2 = 0x160,
        totalRunningTime = new Time(12, 34, 56, 789, false),
        totalPlayTime = new Time(23, 45, 19, 876, false),
        playCounts = EnumHelper<LevelWithTotal>.Enumerable.ToDictionary(
            level => level,
            level => new PlayCountTests.Properties(PlayCountTests.ValidProperties)),
    };

    internal static byte[] MakeData(in Properties properties)
    {
        return TestUtils.MakeByteArray(
            0u,
            (int)properties.totalRunningTime.Hours,
            properties.totalRunningTime.Minutes,
            properties.totalRunningTime.Seconds,
            properties.totalRunningTime.Milliseconds,
            (int)properties.totalPlayTime.Hours,
            properties.totalPlayTime.Minutes,
            properties.totalPlayTime.Seconds,
            properties.totalPlayTime.Milliseconds,
            properties.playCounts.Select(pair => PlayCountTests.MakeByteArray(pair.Value)));
    }

    internal static byte[] MakeByteArray(in Properties properties)
    {
        return TestUtils.MakeByteArray(
            properties.signature.ToCharArray(), properties.size1, properties.size2, MakeData(properties));
    }

    internal static void Validate(in Properties expected, in PlayStatus actual)
    {
        var data = MakeData(expected);

        actual.Signature.ShouldBe(expected.signature);
        actual.Size1.ShouldBe(expected.size1);
        actual.Size2.ShouldBe(expected.size2);
        actual.FirstByteOfData.ShouldBe(data[0]);
        actual.TotalRunningTime.Hours.ShouldBe(expected.totalRunningTime.Hours);
        actual.TotalRunningTime.Minutes.ShouldBe(expected.totalRunningTime.Minutes);
        actual.TotalRunningTime.Seconds.ShouldBe(expected.totalRunningTime.Seconds);
        actual.TotalRunningTime.Milliseconds.ShouldBe(expected.totalRunningTime.Milliseconds);
        actual.TotalRunningTime.IsFrames.ShouldBeFalse();
        actual.TotalPlayTime.Hours.ShouldBe(expected.totalPlayTime.Hours);
        actual.TotalPlayTime.Minutes.ShouldBe(expected.totalPlayTime.Minutes);
        actual.TotalPlayTime.Seconds.ShouldBe(expected.totalPlayTime.Seconds);
        actual.TotalPlayTime.Milliseconds.ShouldBe(expected.totalPlayTime.Milliseconds);
        actual.TotalPlayTime.IsFrames.ShouldBeFalse();

        foreach (var key in expected.playCounts.Keys)
        {
            PlayCountTests.Validate(actual.PlayCounts[key], expected.playCounts[key]);
        }
    }

    [TestMethod]
    public void PlayStatusTestChapter()
    {
        var properties = ValidProperties;

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(properties));
        var playStatus = new PlayStatus(chapter);

        Validate(properties, playStatus);
    }

    [TestMethod]
    public void PlayStatusTestInvalidSignature()
    {
        var properties = ValidProperties;
        properties.signature = properties.signature.ToLowerInvariant();

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(properties));
        _ = Should.Throw<InvalidDataException>(() => new PlayStatus(chapter));
    }

    [TestMethod]
    public void PlayStatusTestInvalidSize1()
    {
        var properties = ValidProperties;
        --properties.size1;

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(properties));
        _ = Should.Throw<InvalidDataException>(() => new PlayStatus(chapter));
    }
}

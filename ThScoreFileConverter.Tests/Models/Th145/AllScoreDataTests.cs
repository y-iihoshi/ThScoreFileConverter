using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th145;
using ThScoreFileConverter.Models.Th145;
using SQOT = ThScoreFileConverter.Squirrel.SQObjectType;

namespace ThScoreFileConverter.Tests.Models.Th145;

internal static class AllScoreDataExtensions
{
    internal static void ShouldBe(this AllScoreData actual, AllScoreDataTests.Properties expected)
    {
        actual.StoryProgress.ShouldBe(expected.storyProgress);
        actual.StoryClearFlags.ShouldBe(expected.storyClearFlags);
        actual.EndingCount.ShouldBe(expected.endingCount);
        actual.Ending2Count.ShouldBe(expected.ending2Count);
        actual.IsEnabledStageTanuki1.ShouldBe(expected.isEnabledStageTanuki1);
        actual.IsEnabledStageTanuki2.ShouldBe(expected.isEnabledStageTanuki2);
        actual.IsEnabledStageKokoro.ShouldBe(expected.isEnabledStageKokoro);
        actual.IsEnabledSt27.ShouldBe(expected.isEnabledSt27);
        actual.IsEnabledSt28.ShouldBe(expected.isEnabledSt28);
        actual.IsPlayableMamizou.ShouldBe(expected.isPlayableMamizou);
        actual.IsPlayableKokoro.ShouldBe(expected.isPlayableKokoro);
        actual.BgmFlags.ShouldBe(expected.bgmFlags);
        actual.ClearRanks.Keys.ShouldBe(expected.clearRanks.Keys);

        foreach (var pair in expected.clearRanks)
        {
            actual.ClearRanks[pair.Key].ShouldBe(pair.Value);
        }

        actual.ClearTimes.Keys.ShouldBe(expected.clearTimes.Keys);

        foreach (var pair in expected.clearTimes)
        {
            actual.ClearTimes[pair.Key].ShouldBe(pair.Value);
        }
    }
}

[TestClass]
public class AllScoreDataTests
{
    internal struct Properties
    {
        public int storyProgress;
        public Dictionary<Chara, Levels> storyClearFlags;
        public int endingCount;
        public int ending2Count;
        public bool isEnabledStageTanuki1;
        public bool isEnabledStageTanuki2;
        public bool isEnabledStageKokoro;
        public bool isEnabledSt27;
        public bool isEnabledSt28;
        public bool isPlayableMamizou;
        public bool isPlayableKokoro;
        public Dictionary<int, bool> bgmFlags;
        public Dictionary<Level, Dictionary<Chara, int>> clearRanks;
        public Dictionary<Level, Dictionary<Chara, int>> clearTimes;
    }

    internal static Properties MakeValidProperties()
    {
        var charas = EnumHelper<Chara>.Enumerable;
        var levels = EnumHelper<Level>.Enumerable;

        return new Properties()
        {
            storyProgress = 1,
            storyClearFlags = charas.ToDictionary(
                chara => chara, chara => (Levels)(30 - (int)chara)),
            endingCount = 2,
            ending2Count = 3,
            isEnabledStageTanuki1 = true,
            isEnabledStageTanuki2 = true,
            isEnabledStageKokoro = false,
            isEnabledSt27 = false,
            isEnabledSt28 = false,
            isPlayableMamizou = true,
            isPlayableKokoro = false,
            bgmFlags = Enumerable.Range(1, 10).ToDictionary(id => id, id => id % 2 == 0),
            clearRanks = levels.ToDictionary(
                level => level, level => charas.ToDictionary(
                    chara => chara, chara => ((int)level * 100) + (int)chara)),
            clearTimes = levels.ToDictionary(
                level => level, level => charas.ToDictionary(
                    chara => chara, chara => ((int)chara * 100) + (int)level)),
        };
    }

    internal static byte[] MakeByteArray(in Properties properties)
    {
        return
        [
            // .. TestUtils.MakeByteArray((int)SQOT.Table),
            .. SquirrelHelper.MakeByteArray(
                "story_progress", properties.storyProgress,
                "story_clear", properties.storyClearFlags.Select(pair => (int)pair.Value).ToArray(),
                "ed_count", properties.endingCount,
                "ed2_count", properties.ending2Count,
                "enable_stage_tanuki1", properties.isEnabledStageTanuki1,
                "enable_stage_tanuki2", properties.isEnabledStageTanuki2,
                "enable_stage_kokoro", properties.isEnabledStageKokoro,
                "enable_st27", properties.isEnabledSt27,
                "enable_st28", properties.isEnabledSt28,
                "enable_mamizou", properties.isPlayableMamizou,
                "enable_kokoro", properties.isPlayableKokoro,
                "enable_bgm", properties.bgmFlags,
                "clear_rank", properties.clearRanks.Select(
                    perLevelPair => perLevelPair.Value.Select(
                        perCharaPair => perCharaPair.Value).ToArray()).ToArray(),
                "clear_time", properties.clearTimes.Select(
                    perLevelPair => perLevelPair.Value.Select(
                        perCharaPair => perCharaPair.Value).ToArray()).ToArray()),
            .. TestUtils.MakeByteArray((int)SQOT.Null),
        ];
    }

    [TestMethod]
    public void AllScoreDataTest()
    {
        var allScoreData = new AllScoreData();

        allScoreData.StoryProgress.ShouldBe(default);
        allScoreData.StoryClearFlags.ShouldBeEmpty();
        allScoreData.EndingCount.ShouldBe(default);
        allScoreData.Ending2Count.ShouldBe(default);
        allScoreData.IsEnabledStageTanuki1.ShouldBe(default);
        allScoreData.IsEnabledStageTanuki2.ShouldBe(default);
        allScoreData.IsEnabledStageKokoro.ShouldBe(default);
        allScoreData.IsPlayableMamizou.ShouldBe(default);
        allScoreData.IsPlayableKokoro.ShouldBe(default);
        allScoreData.BgmFlags.ShouldBeEmpty();
        allScoreData.ClearRanks.ShouldBeEmpty();
        allScoreData.ClearTimes.ShouldBeEmpty();
    }

    [TestMethod]
    public void ReadFromTest()
    {
        var properties = MakeValidProperties();

        var allScoreData = TestUtils.Create<AllScoreData>(MakeByteArray(properties));

        allScoreData.ShouldBe(properties);
    }

    [TestMethod]
    public void ReadFromTestEmpty()
    {
        _ = Should.Throw<EndOfStreamException>(() => TestUtils.Create<AllScoreData>([]));
    }

    [TestMethod]
    public void ReadFromTestNoKey()
    {
        var allScoreData = TestUtils.Create<AllScoreData>(TestUtils.MakeByteArray((int)SQOT.Null));

        allScoreData.StoryProgress.ShouldBe(default);
        allScoreData.StoryClearFlags.ShouldBeEmpty();
        allScoreData.EndingCount.ShouldBe(default);
        allScoreData.Ending2Count.ShouldBe(default);
        allScoreData.IsEnabledStageTanuki1.ShouldBe(default);
        allScoreData.IsEnabledStageTanuki2.ShouldBe(default);
        allScoreData.IsEnabledStageKokoro.ShouldBe(default);
        allScoreData.IsEnabledSt27.ShouldBe(default);
        allScoreData.IsEnabledSt28.ShouldBe(default);
        allScoreData.IsPlayableMamizou.ShouldBe(default);
        allScoreData.IsPlayableKokoro.ShouldBe(default);
        allScoreData.BgmFlags.ShouldBeEmpty();
        allScoreData.ClearRanks.ShouldBeEmpty();
        allScoreData.ClearTimes.ShouldBeEmpty();
    }

    [TestMethod]
    public void ReadFromTestNoTables()
    {
        var storyProgressValue = 1;

        var allScoreData = TestUtils.Create<AllScoreData>([
            // .. TestUtils.MakeByteArray((int)SQOT.Table),
            .. SquirrelHelper.MakeByteArray("story_progress", storyProgressValue),
            .. TestUtils.MakeByteArray((int)SQOT.Null),
        ]);

        allScoreData.StoryProgress.ShouldBe(storyProgressValue);
        allScoreData.StoryClearFlags.ShouldBeEmpty();
        allScoreData.BgmFlags.ShouldBeEmpty();
        allScoreData.ClearRanks.ShouldBeEmpty();
        allScoreData.ClearTimes.ShouldBeEmpty();
    }

    [TestMethod]
    public void ReadFromTestInvalidStoryClear()
    {
        var allScoreData = TestUtils.Create<AllScoreData>([
            // .. TestUtils.MakeByteArray((int)SQOT.Table),
            .. SquirrelHelper.MakeByteArray("story_clear", 1),
            .. TestUtils.MakeByteArray((int)SQOT.Null),
        ]);

        allScoreData.StoryClearFlags.ShouldBeEmpty();
    }

    [TestMethod]
    public void ReadFromTestInvalidStoryClearValue()
    {
        static float[] GetInvalidValue()
        {
            return [123f];
        }

        var allScoreData = TestUtils.Create<AllScoreData>([
            // .. TestUtils.MakeByteArray((int)SQOT.Table),
            .. SquirrelHelper.MakeByteArray("story_clear", GetInvalidValue()),
            .. TestUtils.MakeByteArray((int)SQOT.Null),
        ]);

        allScoreData.StoryClearFlags.ShouldBeEmpty();
    }

    [TestMethod]
    public void ReadFromTestInvalidEnableBgm()
    {
        var allScoreData = TestUtils.Create<AllScoreData>([
            // .. TestUtils.MakeByteArray((int)SQOT.Table),
            .. SquirrelHelper.MakeByteArray("enable_bgm", 1),
            .. TestUtils.MakeByteArray((int)SQOT.Null),
        ]);

        allScoreData.BgmFlags.ShouldBeEmpty();
    }

    [TestMethod]
    public void ReadFromTestInvalidClearRank()
    {
        var allScoreData = TestUtils.Create<AllScoreData>([
            // .. TestUtils.MakeByteArray((int)SQOT.Table),
            .. SquirrelHelper.MakeByteArray("clear_rank", 1),
            .. TestUtils.MakeByteArray((int)SQOT.Null),
        ]);

        allScoreData.ClearRanks.ShouldBeEmpty();
    }

    [TestMethod]
    public void ReadFromTestInvalidClearRankValuePerLevel()
    {
        static float[] GetInvalidValue()
        {
            return [123f];
        }

        var allScoreData = TestUtils.Create<AllScoreData>([
            // .. TestUtils.MakeByteArray((int)SQOT.Table),
            .. SquirrelHelper.MakeByteArray("clear_rank", GetInvalidValue()),
            .. TestUtils.MakeByteArray((int)SQOT.Null),
        ]);

        allScoreData.ClearRanks.ShouldBeEmpty();
    }

    [TestMethod]
    public void ReadFromTestInvalidClearRankValuePerChara()
    {
        static float[][] GetInvalidValue()
        {
            return [[123f]];
        }

        var allScoreData = TestUtils.Create<AllScoreData>([
            // .. TestUtils.MakeByteArray((int)SQOT.Table),
            .. SquirrelHelper.MakeByteArray("clear_rank", GetInvalidValue()),
            .. TestUtils.MakeByteArray((int)SQOT.Null),
        ]);

        allScoreData.ClearRanks.Count.ShouldBe(1);
        allScoreData.ClearRanks.First().Value.ShouldBeEmpty();
    }

    [TestMethod]
    public void ReadFromTestInvalidClearTime()
    {
        var allScoreData = TestUtils.Create<AllScoreData>([
            // .. TestUtils.MakeByteArray((int)SQOT.Table),
            .. SquirrelHelper.MakeByteArray("clear_time", 1),
            .. TestUtils.MakeByteArray((int)SQOT.Null),
        ]);

        allScoreData.ClearTimes.ShouldBeEmpty();
    }

    [TestMethod]
    public void ReadFromTestInvalidClearTimeValuePerLevel()
    {
        static float[] GetInvalidValue()
        {
            return [123f];
        }

        var allScoreData = TestUtils.Create<AllScoreData>([
            // .. TestUtils.MakeByteArray((int)SQOT.Table),
            .. SquirrelHelper.MakeByteArray("clear_time", GetInvalidValue()),
            .. TestUtils.MakeByteArray((int)SQOT.Null),
        ]);

        allScoreData.ClearTimes.ShouldBeEmpty();
    }

    [TestMethod]
    public void ReadFromTestInvalidClearTimeValuePerChara()
    {
        static float[][] GetInvalidValue()
        {
            return [[123f]];
        }

        var allScoreData = TestUtils.Create<AllScoreData>([
            // .. TestUtils.MakeByteArray((int)SQOT.Table),
            .. SquirrelHelper.MakeByteArray("clear_time", GetInvalidValue()),
            .. TestUtils.MakeByteArray((int)SQOT.Null),
        ]);

        allScoreData.ClearTimes.Count.ShouldBe(1);
        allScoreData.ClearTimes.First().Value.ShouldBeEmpty();
    }
}

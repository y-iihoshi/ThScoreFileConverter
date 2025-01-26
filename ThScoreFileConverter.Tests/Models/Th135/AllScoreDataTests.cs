using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th135;
using ThScoreFileConverter.Models.Th135;
using ThScoreFileConverter.Tests.UnitTesting;
using SQOT = ThScoreFileConverter.Squirrel.SQObjectType;

namespace ThScoreFileConverter.Tests.Models.Th135;

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
        public bool isPlayableMamizou;
        public bool isPlayableKokoro;
        public Dictionary<int, bool> bgmFlags;
    }

    internal static Properties GetValidProperties()
    {
        return new()
        {
            storyProgress = 1,
            storyClearFlags = EnumHelper<Chara>.Enumerable.ToDictionary(
                chara => chara, chara => (Levels)(30 - (int)chara)),
            endingCount = 2,
            ending2Count = 3,
            isEnabledStageTanuki1 = true,
            isEnabledStageTanuki2 = true,
            isEnabledStageKokoro = false,
            isPlayableMamizou = true,
            isPlayableKokoro = false,
            bgmFlags = Enumerable.Range(1, 10).ToDictionary(id => id, id => id % 2 == 0),
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
                "enable_mamizou", properties.isPlayableMamizou,
                "enable_kokoro", properties.isPlayableKokoro,
                "enable_bgm", properties.bgmFlags),
            .. TestUtils.MakeByteArray((int)SQOT.Null),
        ];
    }

    internal static void Validate(in Properties expected, in AllScoreData actual)
    {
        Assert.AreEqual(expected.storyProgress, actual.StoryProgress);
        CollectionAssert.That.AreEqual(expected.storyClearFlags.Keys, actual.StoryClearFlags.Keys);
        CollectionAssert.That.AreEqual(expected.storyClearFlags.Values, actual.StoryClearFlags.Values);
        Assert.AreEqual(expected.endingCount, actual.EndingCount);
        Assert.AreEqual(expected.ending2Count, actual.Ending2Count);
        Assert.AreEqual(expected.isEnabledStageTanuki1, actual.IsEnabledStageTanuki1);
        Assert.AreEqual(expected.isEnabledStageTanuki2, actual.IsEnabledStageTanuki2);
        Assert.AreEqual(expected.isEnabledStageKokoro, actual.IsEnabledStageKokoro);
        Assert.AreEqual(expected.isPlayableMamizou, actual.IsPlayableMamizou);
        Assert.AreEqual(expected.isPlayableKokoro, actual.IsPlayableKokoro);
        CollectionAssert.That.AreEqual(expected.bgmFlags.Keys, actual.BgmFlags.Keys);
        CollectionAssert.That.AreEqual(expected.bgmFlags.Values, actual.BgmFlags.Values);
    }

    [TestMethod]
    public void AllScoreDataTest()
    {
        var allScoreData = new AllScoreData();

        Assert.AreEqual(default, allScoreData.StoryProgress);
        Assert.AreEqual(0, allScoreData.StoryClearFlags.Count);
        Assert.AreEqual(default, allScoreData.EndingCount);
        Assert.AreEqual(default, allScoreData.Ending2Count);
        Assert.AreEqual(default, allScoreData.IsEnabledStageTanuki1);
        Assert.AreEqual(default, allScoreData.IsEnabledStageTanuki2);
        Assert.AreEqual(default, allScoreData.IsEnabledStageKokoro);
        Assert.AreEqual(default, allScoreData.IsPlayableMamizou);
        Assert.AreEqual(default, allScoreData.IsPlayableKokoro);
        Assert.AreEqual(0, allScoreData.BgmFlags.Count);
    }

    [TestMethod]
    public void ReadFromTest()
    {
        var properties = GetValidProperties();

        var allScoreData = TestUtils.Create<AllScoreData>(MakeByteArray(properties));

        Validate(properties, allScoreData);
    }

    [TestMethod]
    public void ReadFromTestEmpty()
    {
        _ = Assert.ThrowsException<EndOfStreamException>(() => TestUtils.Create<AllScoreData>([]));
    }

    [TestMethod]
    public void ReadFromTestNoKey()
    {
        var allScoreData = TestUtils.Create<AllScoreData>(TestUtils.MakeByteArray((int)SQOT.Null));

        Assert.AreEqual(default, allScoreData.StoryProgress);
        Assert.AreEqual(0, allScoreData.StoryClearFlags.Count);
        Assert.AreEqual(default, allScoreData.EndingCount);
        Assert.AreEqual(default, allScoreData.Ending2Count);
        Assert.AreEqual(default, allScoreData.IsEnabledStageTanuki1);
        Assert.AreEqual(default, allScoreData.IsEnabledStageTanuki2);
        Assert.AreEqual(default, allScoreData.IsEnabledStageKokoro);
        Assert.AreEqual(default, allScoreData.IsPlayableMamizou);
        Assert.AreEqual(default, allScoreData.IsPlayableKokoro);
        Assert.AreEqual(0, allScoreData.BgmFlags.Count);
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

        Assert.AreEqual(storyProgressValue, allScoreData.StoryProgress);
        Assert.AreEqual(0, allScoreData.StoryClearFlags.Count);
        Assert.AreEqual(0, allScoreData.BgmFlags.Count);
    }

    [TestMethod]
    public void ReadFromTestInvalidStoryClear()
    {
        var allScoreData = TestUtils.Create<AllScoreData>([
            // .. TestUtils.MakeByteArray((int)SQOT.Table),
            .. SquirrelHelper.MakeByteArray("story_clear", 1),
            .. TestUtils.MakeByteArray((int)SQOT.Null),
        ]);

        Assert.AreEqual(0, allScoreData.StoryClearFlags.Count);
    }

    [TestMethod]
    public void ReadFromTestInvalidStoryClearValue()
    {
        static float[] GetInvalidStoryClear()
        {
            return [123f];
        }

        var allScoreData = TestUtils.Create<AllScoreData>([
            // .. TestUtils.MakeByteArray((int)SQOT.Table),
            .. SquirrelHelper.MakeByteArray("story_clear", GetInvalidStoryClear()),
            .. TestUtils.MakeByteArray((int)SQOT.Null),
        ]);

        Assert.AreEqual(0, allScoreData.StoryClearFlags.Count);
    }

    [TestMethod]
    public void ReadFromTestInvalidEnableBgm()
    {
        var allScoreData = TestUtils.Create<AllScoreData>([
            // .. TestUtils.MakeByteArray((int)SQOT.Table),
            .. SquirrelHelper.MakeByteArray("enable_bgm", 1),
            .. TestUtils.MakeByteArray((int)SQOT.Null),
        ]);

        Assert.AreEqual(0, allScoreData.BgmFlags.Count);
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th155;
using ThScoreFileConverter.Core.Tests.UnitTesting;
using ThScoreFileConverter.Models.Th155;
using ThScoreFileConverter.Tests.UnitTesting;
using SQOT = ThScoreFileConverter.Squirrel.SQObjectType;

namespace ThScoreFileConverter.Tests.Models.Th155;

[TestClass]
public class AllScoreDataTests
{
    internal struct Properties
    {
        public Dictionary<StoryChara, AllScoreData.Story> storyDictionary;
        public Dictionary<string, int> characterDictionary;
        public Dictionary<int, bool> bgmDictionary;
        public Dictionary<string, int> endingDictionary;
        public Dictionary<int, int> stageDictionary;
        public int version;
    }

    internal static Properties GetValidProperties()
    {
        return new Properties()
        {
            storyDictionary = EnumHelper<StoryChara>.Enumerable.ToDictionary(
                chara => chara,
                chara => new AllScoreData.Story
                {
                    Stage = 1,
                    Ed = Levels.Normal,
                    Available = true,
                    OverDrive = 2,
                    StageOverDrive = 3,
                }),
            characterDictionary = new Dictionary<string, int>
            {
                { "reimu", 1 },
                { "marisa", 2 },
            },
            bgmDictionary = new Dictionary<int, bool>
            {
                { 101, true },
                { 102, false },
            },
            endingDictionary = new Dictionary<string, int>
            {
                { "ed1", 3 },
                { "ed2", 4 },
            },
            stageDictionary = new Dictionary<int, int>
            {
                { 201, 5 },
                { 202, 6 },
            },
            version = 7,
        };
    }

    internal static string ToString(StoryChara chara)
    {
        var table = new Dictionary<StoryChara, string>
        {
            { StoryChara.ReimuKasen,         "reimu" },
            { StoryChara.MarisaKoishi,       "marisa" },
            { StoryChara.NitoriKokoro,       "nitori" },
            { StoryChara.SumirekoDoremy,     "usami" },
            { StoryChara.TenshiShinmyoumaru, "tenshi" },
            { StoryChara.MikoByakuren,       "miko" },
            { StoryChara.YukariReimu,        "yukari" },
            { StoryChara.MamizouMokou,       "mamizou" },
            { StoryChara.ReisenDoremy,       "udonge" },
            { StoryChara.FutoIchirin,        "futo" },
            { StoryChara.JoonShion,          "jyoon" },
        };
        return table[chara];
    }

    internal static IEnumerable<byte> MakeSQByteArray(in AllScoreData.Story story)
    {
        return TestUtils.MakeByteArray((int)SQOT.Table)
            .Concat(SquirrelHelper.MakeByteArray(
                "stage", story.Stage,
                "ed", (int)story.Ed,
                "available", story.Available,
                "overdrive", story.OverDrive,
                "stage_overdrive", story.StageOverDrive))
            .Concat(TestUtils.MakeByteArray((int)SQOT.Null));
    }

    internal static byte[] MakeByteArray(in Properties properties)
    {
        var sqTable = TestUtils.MakeByteArray((int)SQOT.Table);
        var sqNull = TestUtils.MakeByteArray((int)SQOT.Null);

        return Array.Empty<byte>()
            // .Concat(sqTable)
            .Concat(SquirrelHelper.MakeByteArray("story"))
            .Concat(sqTable)
            .Concat(properties.storyDictionary.SelectMany(
                pair => SquirrelHelper.MakeByteArray(ToString(pair.Key)).Concat(MakeSQByteArray(pair.Value))))
            .Concat(sqNull)
            .Concat(SquirrelHelper.MakeByteArray(
                "character", properties.characterDictionary,
                "bgm", properties.bgmDictionary,
                "ed", properties.endingDictionary,
                "stage", properties.stageDictionary,
                "version", properties.version))
            .Concat(sqNull)
            .ToArray();
    }

    internal static void Validate(in Properties expected, in AllScoreData actual)
    {
        Assert.AreEqual(expected.storyDictionary.Count, actual.StoryDictionary.Count);

        foreach (var pair in expected.storyDictionary)
        {
            var story = actual.StoryDictionary[pair.Key];
            Assert.AreEqual(pair.Value.Stage, story.Stage);
            Assert.AreEqual(pair.Value.Ed, story.Ed);
            Assert.AreEqual(pair.Value.Available, story.Available);
            Assert.AreEqual(pair.Value.OverDrive, story.OverDrive);
            Assert.AreEqual(pair.Value.StageOverDrive, story.StageOverDrive);
        }

        CollectionAssert.That.AreEqual(expected.characterDictionary.Keys, actual.CharacterDictionary.Keys);
        CollectionAssert.That.AreEqual(
            expected.characterDictionary.Values, actual.CharacterDictionary.Values);
        CollectionAssert.That.AreEqual(expected.bgmDictionary.Keys, actual.BgmDictionary.Keys);
        CollectionAssert.That.AreEqual(expected.bgmDictionary.Values, actual.BgmDictionary.Values);
        CollectionAssert.That.AreEqual(expected.endingDictionary.Keys, actual.EndingDictionary.Keys);
        CollectionAssert.That.AreEqual(expected.endingDictionary.Values, actual.EndingDictionary.Values);
        CollectionAssert.That.AreEqual(expected.stageDictionary.Keys, actual.StageDictionary.Keys);
        CollectionAssert.That.AreEqual(expected.stageDictionary.Values, actual.StageDictionary.Values);
        Assert.AreEqual(expected.version, actual.Version);
    }

    [TestMethod]
    public void AllScoreDataTest()
    {
        var allScoreData = new AllScoreData();

        Assert.AreEqual(0, allScoreData.StoryDictionary.Count);
        Assert.AreEqual(0, allScoreData.CharacterDictionary.Count);
        Assert.AreEqual(0, allScoreData.BgmDictionary.Count);
        Assert.AreEqual(0, allScoreData.EndingDictionary.Count);
        Assert.AreEqual(0, allScoreData.StageDictionary.Count);
        Assert.AreEqual(default, allScoreData.Version);
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
        _ = Assert.ThrowsException<EndOfStreamException>(
            () => TestUtils.Create<AllScoreData>(Array.Empty<byte>()));
    }

    [TestMethod]
    public void ReadFromTestNoKey()
    {
        var allScoreData = TestUtils.Create<AllScoreData>(TestUtils.MakeByteArray((int)SQOT.Null));

        Assert.AreEqual(0, allScoreData.StoryDictionary.Count);
        Assert.AreEqual(0, allScoreData.CharacterDictionary.Count);
        Assert.AreEqual(0, allScoreData.BgmDictionary.Count);
        Assert.AreEqual(0, allScoreData.EndingDictionary.Count);
        Assert.AreEqual(0, allScoreData.StageDictionary.Count);
        Assert.AreEqual(default, allScoreData.Version);
    }

    [TestMethod]
    public void ReadFromTestNoTables()
    {
        var version = 1;

        var allScoreData = TestUtils.Create<AllScoreData>(TestUtils.MakeByteArray(
            // (int)SQOT.Table,
            SquirrelHelper.MakeByteArray("version", version),
            (int)SQOT.Null));

        Assert.AreEqual(0, allScoreData.StoryDictionary.Count);
        Assert.AreEqual(0, allScoreData.CharacterDictionary.Count);
        Assert.AreEqual(0, allScoreData.BgmDictionary.Count);
        Assert.AreEqual(0, allScoreData.EndingDictionary.Count);
        Assert.AreEqual(0, allScoreData.StageDictionary.Count);
        Assert.AreEqual(version, allScoreData.Version);
    }

    [TestMethod]
    public void ReadFromTestInvalidStoryDictionary()
    {
        var allScoreData = TestUtils.Create<AllScoreData>(Array.Empty<byte>()
            // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
            .Concat(SquirrelHelper.MakeByteArray("story", 123))
            .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
            .ToArray());

        Assert.AreEqual(0, allScoreData.StoryDictionary.Count);
    }

    [TestMethod]
    public void ReadFromTestInvalidStoryKey()
    {
        var allScoreData = TestUtils.Create<AllScoreData>(Array.Empty<byte>()
            // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
            .Concat(SquirrelHelper.MakeByteArray("story", new Dictionary<int, int> { { 123, 456 } }))
            .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
            .ToArray());

        Assert.AreEqual(0, allScoreData.StoryDictionary.Count);
    }

    [TestMethod]
    public void ReadFromTestInvalidStoryChara()
    {
        var allScoreData = TestUtils.Create<AllScoreData>(Array.Empty<byte>()
            // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
            .Concat(SquirrelHelper.MakeByteArray("story", new Dictionary<string, int> { { "abc", 123 } }))
            .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
            .ToArray());

        Assert.AreEqual(0, allScoreData.StoryDictionary.Count);
    }

    [TestMethod]
    public void ReadFromTestInvalidStory()
    {
        var allScoreData = TestUtils.Create<AllScoreData>(Array.Empty<byte>()
            // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
            .Concat(SquirrelHelper.MakeByteArray(
                "story",
                new Dictionary<string, int>
                {
                    { ToString(StoryChara.ReimuKasen), 123 },
                }))
            .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
            .ToArray());

        Assert.AreEqual(1, allScoreData.StoryDictionary.Count);
        var story = allScoreData.StoryDictionary[StoryChara.ReimuKasen];
        Assert.AreEqual(default, story.Stage);
        Assert.AreEqual(default, story.Ed);
        Assert.AreEqual(default, story.Available);
        Assert.AreEqual(default, story.OverDrive);
        Assert.AreEqual(default, story.StageOverDrive);
    }

    [TestMethod]
    public void ReadFromTestInvalidStoryFieldName()
    {
        var allScoreData = TestUtils.Create<AllScoreData>(Array.Empty<byte>()
            // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
            .Concat(SquirrelHelper.MakeByteArray(
                "story",
                new Dictionary<string, Dictionary<int, int>>
                {
                    {
                        ToString(StoryChara.ReimuKasen),
                        new Dictionary<int, int> { { 123, 456 } }
                    },
                }))
            .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
            .ToArray());

        Assert.AreEqual(1, allScoreData.StoryDictionary.Count);
        var story = allScoreData.StoryDictionary[StoryChara.ReimuKasen];
        Assert.AreEqual(default, story.Stage);
        Assert.AreEqual(default, story.Ed);
        Assert.AreEqual(default, story.Available);
        Assert.AreEqual(default, story.OverDrive);
        Assert.AreEqual(default, story.StageOverDrive);
    }

    [TestMethod]
    public void ReadFromTestInvalidStoryFieldValue()
    {
        var allScoreData = TestUtils.Create<AllScoreData>(Array.Empty<byte>()
            // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
            .Concat(SquirrelHelper.MakeByteArray(
                "story",
                new Dictionary<string, Dictionary<string, float>>
                {
                    {
                        ToString(StoryChara.ReimuKasen),
                        new Dictionary<string, float>
                        {
                            { "stage", 12f },
                            { "ed", 34f },
                            { "available", 56f },
                            { "overdrive", 78f },
                            { "stage_overdrive", 90f },
                        }
                    },
                }))
            .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
            .ToArray());

        Assert.AreEqual(1, allScoreData.StoryDictionary.Count);
        var story = allScoreData.StoryDictionary[StoryChara.ReimuKasen];
        Assert.AreEqual(default, story.Stage);
        Assert.AreEqual(default, story.Ed);
        Assert.AreEqual(default, story.Available);
        Assert.AreEqual(default, story.OverDrive);
        Assert.AreEqual(default, story.StageOverDrive);
    }

    [TestMethod]
    public void ReadFromTestInvalidCharacterDictionary()
    {
        var allScoreData = TestUtils.Create<AllScoreData>(Array.Empty<byte>()
            // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
            .Concat(SquirrelHelper.MakeByteArray("character", 123))
            .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
            .ToArray());

        Assert.AreEqual(0, allScoreData.CharacterDictionary.Count);
    }

    [TestMethod]
    public void ReadFromTestInvalidCharacterKey()
    {
        var allScoreData = TestUtils.Create<AllScoreData>(Array.Empty<byte>()
            // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
            .Concat(SquirrelHelper.MakeByteArray("character", new Dictionary<int, int> { { 123, 456 } }))
            .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
            .ToArray());

        Assert.AreEqual(0, allScoreData.CharacterDictionary.Count);
    }

    [TestMethod]
    public void ReadFromTestInvalidCharacterValue()
    {
        var allScoreData = TestUtils.Create<AllScoreData>(Array.Empty<byte>()
            // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
            .Concat(SquirrelHelper.MakeByteArray("character", new Dictionary<string, string> { { "abc", "def" } }))
            .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
            .ToArray());

        Assert.AreEqual(0, allScoreData.CharacterDictionary.Count);
    }

    [TestMethod]
    public void ReadFromTestInvalidBgmDictionary()
    {
        var allScoreData = TestUtils.Create<AllScoreData>(Array.Empty<byte>()
            // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
            .Concat(SquirrelHelper.MakeByteArray("bgm", 123))
            .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
            .ToArray());

        Assert.AreEqual(0, allScoreData.BgmDictionary.Count);
    }

    [TestMethod]
    public void ReadFromTestInvalidBgmKey()
    {
        var allScoreData = TestUtils.Create<AllScoreData>(Array.Empty<byte>()
            // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
            .Concat(SquirrelHelper.MakeByteArray("bgm", new Dictionary<int, int> { { 123, 456 } }))
            .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
            .ToArray());

        Assert.AreEqual(0, allScoreData.BgmDictionary.Count);
    }

    [TestMethod]
    public void ReadFromTestInvalidBgmValue()
    {
        var allScoreData = TestUtils.Create<AllScoreData>(Array.Empty<byte>()
            // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
            .Concat(SquirrelHelper.MakeByteArray("bgm", new Dictionary<bool, bool> { { true, false } }))
            .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
            .ToArray());

        Assert.AreEqual(0, allScoreData.BgmDictionary.Count);
    }

    [TestMethod]
    public void ReadFromTestInvalidEndingDictionary()
    {
        var allScoreData = TestUtils.Create<AllScoreData>(Array.Empty<byte>()
            // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
            .Concat(SquirrelHelper.MakeByteArray("ed", 123))
            .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
            .ToArray());

        Assert.AreEqual(0, allScoreData.EndingDictionary.Count);
    }

    [TestMethod]
    public void ReadFromTestInvalidEndingKey()
    {
        var allScoreData = TestUtils.Create<AllScoreData>(Array.Empty<byte>()
            // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
            .Concat(SquirrelHelper.MakeByteArray("ed", new Dictionary<int, int> { { 123, 456 } }))
            .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
            .ToArray());

        Assert.AreEqual(0, allScoreData.EndingDictionary.Count);
    }

    [TestMethod]
    public void ReadFromTestInvalidEndingValue()
    {
        var allScoreData = TestUtils.Create<AllScoreData>(Array.Empty<byte>()
            // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
            .Concat(SquirrelHelper.MakeByteArray("ed", new Dictionary<string, string> { { "abc", "def" } }))
            .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
            .ToArray());

        Assert.AreEqual(0, allScoreData.EndingDictionary.Count);
    }

    [TestMethod]
    public void ReadFromTestInvalidStageDictionary()
    {
        var allScoreData = TestUtils.Create<AllScoreData>(Array.Empty<byte>()
            // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
            .Concat(SquirrelHelper.MakeByteArray("stage", 123))
            .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
            .ToArray());

        Assert.AreEqual(0, allScoreData.StageDictionary.Count);
    }

    [TestMethod]
    public void ReadFromTestInvalidStageKey()
    {
        var allScoreData = TestUtils.Create<AllScoreData>(Array.Empty<byte>()
            // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
            .Concat(SquirrelHelper.MakeByteArray("stage", new Dictionary<string, int> { { "abc", 123 } }))
            .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
            .ToArray());

        Assert.AreEqual(0, allScoreData.StageDictionary.Count);
    }

    [TestMethod]
    public void ReadFromTestInvalidStageValue()
    {
        var allScoreData = TestUtils.Create<AllScoreData>(Array.Empty<byte>()
            // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
            .Concat(SquirrelHelper.MakeByteArray("stage", new Dictionary<int, string> { { 123, "abc" } }))
            .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
            .ToArray());

        Assert.AreEqual(0, allScoreData.StageDictionary.Count);
    }

    [TestMethod]
    public void ReadFromTestInvalidVersion()
    {
        var allScoreData = TestUtils.Create<AllScoreData>(Array.Empty<byte>()
            // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
            .Concat(SquirrelHelper.MakeByteArray("version", 123f))
            .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
            .ToArray());

        Assert.AreEqual(default, allScoreData.Version);
    }
}

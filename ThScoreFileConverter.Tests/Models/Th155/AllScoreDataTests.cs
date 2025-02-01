using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th155;
using ThScoreFileConverter.Models.Th155;
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

        return
        [
            // .. sqTable,
            .. SquirrelHelper.MakeByteArray("story"),
            .. sqTable,
            .. properties.storyDictionary.SelectMany(
                    pair => SquirrelHelper.MakeByteArray(ToString(pair.Key)).Concat(MakeSQByteArray(pair.Value))),
            .. sqNull,
            .. SquirrelHelper.MakeByteArray(
                "character", properties.characterDictionary,
                "bgm", properties.bgmDictionary,
                "ed", properties.endingDictionary,
                "stage", properties.stageDictionary,
                "version", properties.version),
            .. sqNull,
        ];
    }

    internal static void Validate(in Properties expected, in AllScoreData actual)
    {
        actual.StoryDictionary.Count.ShouldBe(expected.storyDictionary.Count);

        foreach (var pair in expected.storyDictionary)
        {
            var story = actual.StoryDictionary[pair.Key];
            story.Stage.ShouldBe(pair.Value.Stage);
            story.Ed.ShouldBe(pair.Value.Ed);
            story.Available.ShouldBe(pair.Value.Available);
            story.OverDrive.ShouldBe(pair.Value.OverDrive);
            story.StageOverDrive.ShouldBe(pair.Value.StageOverDrive);
        }

        actual.CharacterDictionary.ShouldBe(expected.characterDictionary);
        actual.BgmDictionary.ShouldBe(expected.bgmDictionary);
        actual.EndingDictionary.ShouldBe(expected.endingDictionary);
        actual.StageDictionary.ShouldBe(expected.stageDictionary);
        actual.Version.ShouldBe(expected.version);
    }

    [TestMethod]
    public void AllScoreDataTest()
    {
        var allScoreData = new AllScoreData();

        allScoreData.StoryDictionary.ShouldBeEmpty();
        allScoreData.CharacterDictionary.ShouldBeEmpty();
        allScoreData.BgmDictionary.ShouldBeEmpty();
        allScoreData.EndingDictionary.ShouldBeEmpty();
        allScoreData.StageDictionary.ShouldBeEmpty();
        allScoreData.Version.ShouldBe(default);
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
        _ = Should.Throw<EndOfStreamException>(() => TestUtils.Create<AllScoreData>([]));
    }

    [TestMethod]
    public void ReadFromTestNoKey()
    {
        var allScoreData = TestUtils.Create<AllScoreData>(TestUtils.MakeByteArray((int)SQOT.Null));

        allScoreData.StoryDictionary.ShouldBeEmpty();
        allScoreData.CharacterDictionary.ShouldBeEmpty();
        allScoreData.BgmDictionary.ShouldBeEmpty();
        allScoreData.EndingDictionary.ShouldBeEmpty();
        allScoreData.StageDictionary.ShouldBeEmpty();
        allScoreData.Version.ShouldBe(default);
    }

    [TestMethod]
    public void ReadFromTestNoTables()
    {
        var version = 1;

        var allScoreData = TestUtils.Create<AllScoreData>(TestUtils.MakeByteArray(
            // (int)SQOT.Table,
            SquirrelHelper.MakeByteArray("version", version),
            (int)SQOT.Null));

        allScoreData.StoryDictionary.ShouldBeEmpty();
        allScoreData.CharacterDictionary.ShouldBeEmpty();
        allScoreData.BgmDictionary.ShouldBeEmpty();
        allScoreData.EndingDictionary.ShouldBeEmpty();
        allScoreData.StageDictionary.ShouldBeEmpty();
        allScoreData.Version.ShouldBe(version);
    }

    [TestMethod]
    public void ReadFromTestInvalidStoryDictionary()
    {
        var allScoreData = TestUtils.Create<AllScoreData>([
            // .. TestUtils.MakeByteArray((int)SQOT.Table),
            .. SquirrelHelper.MakeByteArray("story", 123),
            .. TestUtils.MakeByteArray((int)SQOT.Null),
        ]);

        allScoreData.StoryDictionary.ShouldBeEmpty();
    }

    [TestMethod]
    public void ReadFromTestInvalidStoryKey()
    {
        var allScoreData = TestUtils.Create<AllScoreData>([
            // .. TestUtils.MakeByteArray((int)SQOT.Table),
            .. SquirrelHelper.MakeByteArray("story", new Dictionary<int, int> { { 123, 456 } }),
            .. TestUtils.MakeByteArray((int)SQOT.Null),
        ]);

        allScoreData.StoryDictionary.ShouldBeEmpty();
    }

    [TestMethod]
    public void ReadFromTestInvalidStoryChara()
    {
        var allScoreData = TestUtils.Create<AllScoreData>([
            // .. TestUtils.MakeByteArray((int)SQOT.Table),
            .. SquirrelHelper.MakeByteArray("story", new Dictionary<string, int> { { "abc", 123 } }),
            .. TestUtils.MakeByteArray((int)SQOT.Null),
        ]);

        allScoreData.StoryDictionary.ShouldBeEmpty();
    }

    [TestMethod]
    public void ReadFromTestInvalidStory()
    {
        var allScoreData = TestUtils.Create<AllScoreData>([
            // .. TestUtils.MakeByteArray((int)SQOT.Table),
            .. SquirrelHelper.MakeByteArray(
                "story",
                new Dictionary<string, int> { { ToString(StoryChara.ReimuKasen), 123 } }),
            .. TestUtils.MakeByteArray((int)SQOT.Null),
        ]);

        allScoreData.StoryDictionary.Count.ShouldBe(1);
        var story = allScoreData.StoryDictionary[StoryChara.ReimuKasen];
        story.Stage.ShouldBe(default);
        story.Ed.ShouldBe(default);
        story.Available.ShouldBe(default);
        story.OverDrive.ShouldBe(default);
        story.StageOverDrive.ShouldBe(default);
    }

    [TestMethod]
    public void ReadFromTestInvalidStoryFieldName()
    {
        var allScoreData = TestUtils.Create<AllScoreData>([
            // .. TestUtils.MakeByteArray((int)SQOT.Table),
            .. SquirrelHelper.MakeByteArray(
                "story",
                new Dictionary<string, Dictionary<int, int>>
                {
                    {
                        ToString(StoryChara.ReimuKasen),
                        new Dictionary<int, int> { { 123, 456 } }
                    },
                }),
            .. TestUtils.MakeByteArray((int)SQOT.Null),
        ]);

        allScoreData.StoryDictionary.Count.ShouldBe(1);
        var story = allScoreData.StoryDictionary[StoryChara.ReimuKasen];
        story.Stage.ShouldBe(default);
        story.Ed.ShouldBe(default);
        story.Available.ShouldBe(default);
        story.OverDrive.ShouldBe(default);
        story.StageOverDrive.ShouldBe(default);
    }

    [TestMethod]
    public void ReadFromTestInvalidStoryFieldValue()
    {
        var allScoreData = TestUtils.Create<AllScoreData>([
            // .. TestUtils.MakeByteArray((int)SQOT.Table),
            .. SquirrelHelper.MakeByteArray(
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
                }),
            .. TestUtils.MakeByteArray((int)SQOT.Null),
        ]);

        allScoreData.StoryDictionary.Count.ShouldBe(1);
        var story = allScoreData.StoryDictionary[StoryChara.ReimuKasen];
        story.Stage.ShouldBe(default);
        story.Ed.ShouldBe(default);
        story.Available.ShouldBe(default);
        story.OverDrive.ShouldBe(default);
        story.StageOverDrive.ShouldBe(default);
    }

    [TestMethod]
    public void ReadFromTestInvalidCharacterDictionary()
    {
        var allScoreData = TestUtils.Create<AllScoreData>([
            // .. TestUtils.MakeByteArray((int)SQOT.Table),
            .. SquirrelHelper.MakeByteArray("character", 123),
            .. TestUtils.MakeByteArray((int)SQOT.Null),
        ]);

        allScoreData.CharacterDictionary.ShouldBeEmpty();
    }

    [TestMethod]
    public void ReadFromTestInvalidCharacterKey()
    {
        // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
        var allScoreData = TestUtils.Create<AllScoreData>([
            // .. TestUtils.MakeByteArray((int)SQOT.Table),
            .. SquirrelHelper.MakeByteArray("character", new Dictionary<int, int> { { 123, 456 } }),
            .. TestUtils.MakeByteArray((int)SQOT.Null),
        ]);

        allScoreData.CharacterDictionary.ShouldBeEmpty();
    }

    [TestMethod]
    public void ReadFromTestInvalidCharacterValue()
    {
        var allScoreData = TestUtils.Create<AllScoreData>([
            // .. TestUtils.MakeByteArray((int)SQOT.Table),
            .. SquirrelHelper.MakeByteArray("character", new Dictionary<string, string> { { "abc", "def" } }),
            .. TestUtils.MakeByteArray((int)SQOT.Null),
        ]);

        allScoreData.CharacterDictionary.ShouldBeEmpty();
    }

    [TestMethod]
    public void ReadFromTestInvalidBgmDictionary()
    {
        var allScoreData = TestUtils.Create<AllScoreData>([
            // .. TestUtils.MakeByteArray((int)SQOT.Table),
            .. SquirrelHelper.MakeByteArray("bgm", 123),
            .. TestUtils.MakeByteArray((int)SQOT.Null),
        ]);

        allScoreData.BgmDictionary.ShouldBeEmpty();
    }

    [TestMethod]
    public void ReadFromTestInvalidBgmKey()
    {
        var allScoreData = TestUtils.Create<AllScoreData>([
            // .. TestUtils.MakeByteArray((int)SQOT.Table),
            .. SquirrelHelper.MakeByteArray("bgm", new Dictionary<int, int> { { 123, 456 } }),
            .. TestUtils.MakeByteArray((int)SQOT.Null),
        ]);

        allScoreData.BgmDictionary.ShouldBeEmpty();
    }

    [TestMethod]
    public void ReadFromTestInvalidBgmValue()
    {
        var allScoreData = TestUtils.Create<AllScoreData>([
            // .. TestUtils.MakeByteArray((int)SQOT.Table),
            .. SquirrelHelper.MakeByteArray("bgm", new Dictionary<bool, bool> { { true, false } }),
            .. TestUtils.MakeByteArray((int)SQOT.Null),
        ]);

        allScoreData.BgmDictionary.ShouldBeEmpty();
    }

    [TestMethod]
    public void ReadFromTestInvalidEndingDictionary()
    {
        var allScoreData = TestUtils.Create<AllScoreData>([
            // .. TestUtils.MakeByteArray((int)SQOT.Table),
            .. SquirrelHelper.MakeByteArray("ed", 123),
            .. TestUtils.MakeByteArray((int)SQOT.Null),
        ]);

        allScoreData.EndingDictionary.ShouldBeEmpty();
    }

    [TestMethod]
    public void ReadFromTestInvalidEndingKey()
    {
        var allScoreData = TestUtils.Create<AllScoreData>([
            // .. TestUtils.MakeByteArray((int)SQOT.Table),
            .. SquirrelHelper.MakeByteArray("ed", new Dictionary<int, int> { { 123, 456 } }),
            .. TestUtils.MakeByteArray((int)SQOT.Null),
        ]);

        allScoreData.EndingDictionary.ShouldBeEmpty();
    }

    [TestMethod]
    public void ReadFromTestInvalidEndingValue()
    {
        var allScoreData = TestUtils.Create<AllScoreData>([
            // .. TestUtils.MakeByteArray((int)SQOT.Table),
            .. SquirrelHelper.MakeByteArray("ed", new Dictionary<string, string> { { "abc", "def" } }),
            .. TestUtils.MakeByteArray((int)SQOT.Null),
        ]);

        allScoreData.EndingDictionary.ShouldBeEmpty();
    }

    [TestMethod]
    public void ReadFromTestInvalidStageDictionary()
    {
        var allScoreData = TestUtils.Create<AllScoreData>([
            // .. TestUtils.MakeByteArray((int)SQOT.Table),
            .. SquirrelHelper.MakeByteArray("stage", 123),
            .. TestUtils.MakeByteArray((int)SQOT.Null),
        ]);

        allScoreData.StageDictionary.ShouldBeEmpty();
    }

    [TestMethod]
    public void ReadFromTestInvalidStageKey()
    {
        var allScoreData = TestUtils.Create<AllScoreData>([
            // .. TestUtils.MakeByteArray((int)SQOT.Table),
            .. SquirrelHelper.MakeByteArray("stage", new Dictionary<string, int> { { "abc", 123 } }),
            .. TestUtils.MakeByteArray((int)SQOT.Null),
        ]);

        allScoreData.StageDictionary.ShouldBeEmpty();
    }

    [TestMethod]
    public void ReadFromTestInvalidStageValue()
    {
        var allScoreData = TestUtils.Create<AllScoreData>([
            // .. TestUtils.MakeByteArray((int)SQOT.Table),
            .. SquirrelHelper.MakeByteArray("stage", new Dictionary<int, string> { { 123, "abc" } }),
            .. TestUtils.MakeByteArray((int)SQOT.Null),
        ]);

        allScoreData.StageDictionary.ShouldBeEmpty();
    }

    [TestMethod]
    public void ReadFromTestInvalidVersion()
    {
        var allScoreData = TestUtils.Create<AllScoreData>([
            // .. TestUtils.MakeByteArray((int)SQOT.Table),
            .. SquirrelHelper.MakeByteArray("version", 123f),
            .. TestUtils.MakeByteArray((int)SQOT.Null),
        ]);

        allScoreData.Version.ShouldBe(default);
    }
}

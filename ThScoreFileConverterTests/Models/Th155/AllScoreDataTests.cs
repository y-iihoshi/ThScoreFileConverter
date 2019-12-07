using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th155;
using ThScoreFileConverterTests.Extensions;

namespace ThScoreFileConverterTests.Models.Th155
{
    using SQOT = ThScoreFileConverter.Squirrel.SQObjectType;

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
        };

        internal static Properties GetValidProperties()
        {
            return new Properties()
            {
                storyDictionary = Utils.GetEnumerator<StoryChara>().ToDictionary(
                    chara => chara,
                    chara => new AllScoreData.Story
                    {
                        Stage = 1,
                        Ed = LevelFlags.Normal,
                        Available = true,
                        OverDrive = 2,
                        StageOverDrive = 3
                    }),
                characterDictionary = new Dictionary<string, int>()
                {
                    { "reimu", 1 },
                    { "marisa", 2 }
                },
                bgmDictionary = new Dictionary<int, bool>()
                {
                    { 101, true },
                    { 102, false }
                },
                endingDictionary = new Dictionary<string, int>()
                {
                    { "ed1", 3 },
                    { "ed2", 4 }
                },
                stageDictionary = new Dictionary<int, int>()
                {
                    { 201, 5 },
                    { 202, 6 }
                },
                version = 7
            };
        }

        internal static string ToString(StoryChara chara)
        {
            var table = new Dictionary<StoryChara, string>()
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
                { StoryChara.JoonShion,          "jyoon" }
            };
            return table[chara];
        }

        internal static IEnumerable<byte> MakeSQByteArray(in AllScoreData.Story story)
            => TestUtils.MakeByteArray((int)SQOT.Table)
                .Concat(TestUtils.MakeSQByteArray(
                    "stage", story.Stage,
                    "ed", (int)story.Ed,
                    "available", story.Available,
                    "overdrive", story.OverDrive,
                    "stage_overdrive", story.StageOverDrive))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null));

        internal static byte[] MakeByteArray(in Properties properties)
        {
            var sqTable = TestUtils.MakeByteArray((int)SQOT.Table);
            var sqNull = TestUtils.MakeByteArray((int)SQOT.Null);

            return new byte[0]
                // .Concat(sqTable)
                .Concat(TestUtils.MakeSQByteArray("story"))
                .Concat(sqTable)
                .Concat(properties.storyDictionary.SelectMany(
                    pair => TestUtils.MakeSQByteArray(ToString(pair.Key)).Concat(MakeSQByteArray(pair.Value))))
                .Concat(sqNull)
                .Concat(TestUtils.MakeSQByteArray(
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
        public void AllScoreDataTest() => TestUtils.Wrap(() =>
        {
            var allScoreData = new AllScoreData();

            Assert.IsNull(allScoreData.StoryDictionary);
            Assert.IsNull(allScoreData.CharacterDictionary);
            Assert.IsNull(allScoreData.BgmDictionary);
            Assert.IsNull(allScoreData.EndingDictionary);
            Assert.IsNull(allScoreData.StageDictionary);
            Assert.AreEqual(default, allScoreData.Version);
        });

        [TestMethod]
        public void ReadFromTest() => TestUtils.Wrap(() =>
        {
            var properties = GetValidProperties();

            var allScoreData = TestUtils.Create<AllScoreData>(MakeByteArray(properties));

            Validate(properties, allScoreData);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var allScoreData = new AllScoreData();
            allScoreData.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestEmpty() => TestUtils.Wrap(() =>
        {
            _ = TestUtils.Create<AllScoreData>(new byte[0]);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void ReadFromTestNoKey() => TestUtils.Wrap(() =>
        {
            var allScoreData = TestUtils.Create<AllScoreData>(TestUtils.MakeByteArray((int)SQOT.Null));

            Assert.IsNull(allScoreData.StoryDictionary);
            Assert.IsNull(allScoreData.CharacterDictionary);
            Assert.IsNull(allScoreData.BgmDictionary);
            Assert.IsNull(allScoreData.EndingDictionary);
            Assert.IsNull(allScoreData.StageDictionary);
            Assert.AreEqual(default, allScoreData.Version);
        });

        [TestMethod]
        public void ReadFromTestNoTables() => TestUtils.Wrap(() =>
        {
            var version = 1;

            var allScoreData = TestUtils.Create<AllScoreData>(TestUtils.MakeByteArray(
                // (int)SQOT.Table,
                TestUtils.MakeSQByteArray("version", version).ToArray(),
                (int)SQOT.Null));

            Assert.IsNull(allScoreData.StoryDictionary);
            Assert.IsNull(allScoreData.CharacterDictionary);
            Assert.IsNull(allScoreData.BgmDictionary);
            Assert.IsNull(allScoreData.EndingDictionary);
            Assert.IsNull(allScoreData.StageDictionary);
            Assert.AreEqual(version, allScoreData.Version);
        });

        [TestMethod]
        public void ReadFromTestInvalidStoryDictionary() => TestUtils.Wrap(() =>
        {
            var allScoreData = TestUtils.Create<AllScoreData>(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
                .Concat(TestUtils.MakeSQByteArray("story", 123))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNull(allScoreData.StoryDictionary);
        });

        [TestMethod]
        public void ReadFromTestInvalidStoryKey() => TestUtils.Wrap(() =>
        {
            var allScoreData = TestUtils.Create<AllScoreData>(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
                .Concat(TestUtils.MakeSQByteArray("story", new Dictionary<int, int>() { { 123, 456 } }))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNotNull(allScoreData.StoryDictionary);
            Assert.AreEqual(0, allScoreData.StoryDictionary.Count);
        });

        [TestMethod]
        public void ReadFromTestInvalidStoryChara() => TestUtils.Wrap(() =>
        {
            var allScoreData = TestUtils.Create<AllScoreData>(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
                .Concat(TestUtils.MakeSQByteArray("story", new Dictionary<string, int>() { { "abc", 123 } }))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNotNull(allScoreData.StoryDictionary);
            Assert.AreEqual(0, allScoreData.StoryDictionary.Count);
        });

        [TestMethod]
        public void ReadFromTestInvalidStory() => TestUtils.Wrap(() =>
        {
            var allScoreData = TestUtils.Create<AllScoreData>(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
                .Concat(TestUtils.MakeSQByteArray(
                    "story",
                    new Dictionary<string, int>()
                    {
                        { ToString(StoryChara.ReimuKasen), 123 }
                    }))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNotNull(allScoreData.StoryDictionary);
            Assert.AreEqual(1, allScoreData.StoryDictionary.Count);
            var story = allScoreData.StoryDictionary[StoryChara.ReimuKasen];
            Assert.AreEqual(default, story.Stage);
            Assert.AreEqual(default, story.Ed);
            Assert.AreEqual(default, story.Available);
            Assert.AreEqual(default, story.OverDrive);
            Assert.AreEqual(default, story.StageOverDrive);
        });

        [TestMethod]
        public void ReadFromTestInvalidStoryFieldName() => TestUtils.Wrap(() =>
        {
            var allScoreData = TestUtils.Create<AllScoreData>(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
                .Concat(TestUtils.MakeSQByteArray(
                    "story",
                    new Dictionary<string, Dictionary<int, int>>()
                    {
                        {
                            ToString(StoryChara.ReimuKasen),
                            new Dictionary<int, int>() { { 123, 456 } }
                        }
                    }))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNotNull(allScoreData.StoryDictionary);
            Assert.AreEqual(1, allScoreData.StoryDictionary.Count);
            var story = allScoreData.StoryDictionary[StoryChara.ReimuKasen];
            Assert.AreEqual(default, story.Stage);
            Assert.AreEqual(default, story.Ed);
            Assert.AreEqual(default, story.Available);
            Assert.AreEqual(default, story.OverDrive);
            Assert.AreEqual(default, story.StageOverDrive);
        });

        [TestMethod]
        public void ReadFromTestInvalidStoryFieldValue() => TestUtils.Wrap(() =>
        {
            var allScoreData = TestUtils.Create<AllScoreData>(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
                .Concat(TestUtils.MakeSQByteArray(
                    "story",
                    new Dictionary<string, Dictionary<string, float>>()
                    {
                        {
                            ToString(StoryChara.ReimuKasen),
                            new Dictionary<string, float>()
                            {
                                { "stage", 12f },
                                { "ed", 34f },
                                { "available", 56f },
                                { "overdrive", 78f },
                                { "stage_overdrive", 90f }
                            }
                        }
                    }))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNotNull(allScoreData.StoryDictionary);
            Assert.AreEqual(1, allScoreData.StoryDictionary.Count);
            var story = allScoreData.StoryDictionary[StoryChara.ReimuKasen];
            Assert.AreEqual(default, story.Stage);
            Assert.AreEqual(default, story.Ed);
            Assert.AreEqual(default, story.Available);
            Assert.AreEqual(default, story.OverDrive);
            Assert.AreEqual(default, story.StageOverDrive);
        });

        [TestMethod]
        public void ReadFromTestInvalidCharacterDictionary() => TestUtils.Wrap(() =>
        {
            var allScoreData = TestUtils.Create<AllScoreData>(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
                .Concat(TestUtils.MakeSQByteArray("character", 123))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNull(allScoreData.CharacterDictionary);
        });

        [TestMethod]
        public void ReadFromTestInvalidCharacterKey() => TestUtils.Wrap(() =>
        {
            var allScoreData = TestUtils.Create<AllScoreData>(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
                .Concat(TestUtils.MakeSQByteArray("character", new Dictionary<int, int>() { { 123, 456 } }))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNotNull(allScoreData.CharacterDictionary);
            Assert.AreEqual(0, allScoreData.CharacterDictionary.Count);
        });

        [TestMethod]
        public void ReadFromTestInvalidCharacterValue() => TestUtils.Wrap(() =>
        {
            var allScoreData = TestUtils.Create<AllScoreData>(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
                .Concat(TestUtils.MakeSQByteArray("character", new Dictionary<string, string>() { { "abc", "def" } }))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNotNull(allScoreData.CharacterDictionary);
            Assert.AreEqual(0, allScoreData.CharacterDictionary.Count);
        });

        [TestMethod]
        public void ReadFromTestInvalidBgmDictionary() => TestUtils.Wrap(() =>
        {
            var allScoreData = TestUtils.Create<AllScoreData>(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
                .Concat(TestUtils.MakeSQByteArray("bgm", 123))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNull(allScoreData.BgmDictionary);
        });

        [TestMethod]
        public void ReadFromTestInvalidBgmKey() => TestUtils.Wrap(() =>
        {
            var allScoreData = TestUtils.Create<AllScoreData>(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
                .Concat(TestUtils.MakeSQByteArray("bgm", new Dictionary<int, int>() { { 123, 456 } }))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNotNull(allScoreData.BgmDictionary);
            Assert.AreEqual(0, allScoreData.BgmDictionary.Count);
        });

        [TestMethod]
        public void ReadFromTestInvalidBgmValue() => TestUtils.Wrap(() =>
        {
            var allScoreData = TestUtils.Create<AllScoreData>(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
                .Concat(TestUtils.MakeSQByteArray("bgm", new Dictionary<bool, bool>() { { true, false } }))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNotNull(allScoreData.BgmDictionary);
            Assert.AreEqual(0, allScoreData.BgmDictionary.Count);
        });

        [TestMethod]
        public void ReadFromTestInvalidEndingDictionary() => TestUtils.Wrap(() =>
        {
            var allScoreData = TestUtils.Create<AllScoreData>(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
                .Concat(TestUtils.MakeSQByteArray("ed", 123))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNull(allScoreData.EndingDictionary);
        });

        [TestMethod]
        public void ReadFromTestInvalidEndingKey() => TestUtils.Wrap(() =>
        {
            var allScoreData = TestUtils.Create<AllScoreData>(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
                .Concat(TestUtils.MakeSQByteArray("ed", new Dictionary<int, int>() { { 123, 456 } }))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNotNull(allScoreData.EndingDictionary);
            Assert.AreEqual(0, allScoreData.EndingDictionary.Count);
        });

        [TestMethod]
        public void ReadFromTestInvalidEndingValue() => TestUtils.Wrap(() =>
        {
            var allScoreData = TestUtils.Create<AllScoreData>(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
                .Concat(TestUtils.MakeSQByteArray("ed", new Dictionary<string, string>() { { "abc", "def" } }))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNotNull(allScoreData.EndingDictionary);
            Assert.AreEqual(0, allScoreData.EndingDictionary.Count);
        });

        [TestMethod]
        public void ReadFromTestInvalidStageDictionary() => TestUtils.Wrap(() =>
        {
            var allScoreData = TestUtils.Create<AllScoreData>(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
                .Concat(TestUtils.MakeSQByteArray("stage", 123))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNull(allScoreData.StageDictionary);
        });

        [TestMethod]
        public void ReadFromTestInvalidStageKey() => TestUtils.Wrap(() =>
        {
            var allScoreData = TestUtils.Create<AllScoreData>(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
                .Concat(TestUtils.MakeSQByteArray("stage", new Dictionary<string, int>() { { "abc", 123 } }))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNotNull(allScoreData.StageDictionary);
            Assert.AreEqual(0, allScoreData.StageDictionary.Count);
        });

        [TestMethod]
        public void ReadFromTestInvalidStageValue() => TestUtils.Wrap(() =>
        {
            var allScoreData = TestUtils.Create<AllScoreData>(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
                .Concat(TestUtils.MakeSQByteArray("stage", new Dictionary<int, string>() { { 123, "abc" } }))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNotNull(allScoreData.StageDictionary);
            Assert.AreEqual(0, allScoreData.StageDictionary.Count);
        });

        [TestMethod]
        public void ReadFromTestInvalidVersion() => TestUtils.Wrap(() =>
        {
            var allScoreData = TestUtils.Create<AllScoreData>(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
                .Concat(TestUtils.MakeSQByteArray("version", 123f))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.AreEqual(default, allScoreData.Version);
        });
    }
}

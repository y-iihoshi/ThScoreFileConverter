using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    using SQOT = ThScoreFileConverter.Squirrel.SQObjectType;

    [TestClass]
    public class Th155AllScoreDataTests
    {
        internal struct Properties
        {
            public Dictionary<Th155Converter.StoryChara, Th155StoryWrapper> storyDictionary;
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
                storyDictionary = Utils.GetEnumerator<Th155Converter.StoryChara>().ToDictionary(
                    chara => chara,
                    chara => new Th155StoryWrapper()
                    {
                        Stage = 1,
                        Ed = Th155Converter.LevelFlag.Normal,
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

        internal static string ToString(Th155Converter.StoryChara chara)
        {
            var table = new Dictionary<Th155Converter.StoryChara, string>()
            {
                { Th155Converter.StoryChara.ReimuKasen,         "reimu" },
                { Th155Converter.StoryChara.MarisaKoishi,       "marisa" },
                { Th155Converter.StoryChara.NitoriKokoro,       "nitori" },
                { Th155Converter.StoryChara.SumirekoDoremy,     "usami" },
                { Th155Converter.StoryChara.TenshiShinmyoumaru, "tenshi" },
                { Th155Converter.StoryChara.MikoByakuren,       "miko" },
                { Th155Converter.StoryChara.YukariReimu,        "yukari" },
                { Th155Converter.StoryChara.MamizouMokou,       "mamizou" },
                { Th155Converter.StoryChara.ReisenDoremy,       "udonge" },
                { Th155Converter.StoryChara.FutoIchirin,        "futo" },
                { Th155Converter.StoryChara.JoonShion,          "jyoon" }
            };
            return table[chara];
        }

        internal static IEnumerable<byte> MakeSQByteArray(in Th155StoryWrapper story)
            => TestUtils.MakeByteArray((int)SQOT.Table)
                .Concat(TestUtils.MakeSQByteArray(
                    "stage", story.Stage.Value,
                    "ed", (int)story.Ed.Value,
                    "available", story.Available.Value,
                    "overdrive", story.OverDrive.Value,
                    "stage_overdrive", story.StageOverDrive.Value))
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

        internal static void Validate(in Th155AllScoreDataWrapper allScoreData, in Properties properties)
        {
            Assert.AreEqual(properties.storyDictionary.Count, allScoreData.StoryDictionaryCount);

            foreach (var pair in properties.storyDictionary)
            {
                var story = allScoreData.StoryDictionaryItem(pair.Key);
                Assert.AreEqual(pair.Value.Stage, story.Stage);
                Assert.AreEqual(pair.Value.Ed, story.Ed);
                Assert.AreEqual(pair.Value.Available, story.Available);
                Assert.AreEqual(pair.Value.OverDrive, story.OverDrive);
                Assert.AreEqual(pair.Value.StageOverDrive, story.StageOverDrive);
            }

            CollectionAssert.That.AreEqual(properties.characterDictionary.Keys, allScoreData.CharacterDictionary.Keys);
            CollectionAssert.That.AreEqual(
                properties.characterDictionary.Values, allScoreData.CharacterDictionary.Values);
            CollectionAssert.That.AreEqual(properties.bgmDictionary.Keys, allScoreData.BgmDictionary.Keys);
            CollectionAssert.That.AreEqual(properties.bgmDictionary.Values, allScoreData.BgmDictionary.Values);
            CollectionAssert.That.AreEqual(properties.endingDictionary.Keys, allScoreData.EndingDictionary.Keys);
            CollectionAssert.That.AreEqual(properties.endingDictionary.Values, allScoreData.EndingDictionary.Values);
            CollectionAssert.That.AreEqual(properties.stageDictionary.Keys, allScoreData.StageDictionary.Keys);
            CollectionAssert.That.AreEqual(properties.stageDictionary.Values, allScoreData.StageDictionary.Values);
            Assert.AreEqual(properties.version, allScoreData.Version);
        }

        [TestMethod]
        public void Th155AllScoreDataTest() => TestUtils.Wrap(() =>
        {
            var allScoreData = new Th155AllScoreDataWrapper();

            Assert.IsNull(allScoreData.StoryDictionary);
            Assert.IsNull(allScoreData.CharacterDictionary);
            Assert.IsNull(allScoreData.BgmDictionary);
            Assert.IsNull(allScoreData.EndingDictionary);
            Assert.IsNull(allScoreData.StageDictionary);
            Assert.AreEqual(default, allScoreData.Version.Value);
        });

        [TestMethod]
        public void Th155AllScoreDataReadFromTest() => TestUtils.Wrap(() =>
        {
            var properties = GetValidProperties();

            var allScoreData = Th155AllScoreDataWrapper.Create(MakeByteArray(properties));

            Validate(allScoreData, properties);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th155AllScoreDataReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var allScoreData = new Th155AllScoreDataWrapper();
            allScoreData.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th155AllScoreDataReadFromTestEmpty() => TestUtils.Wrap(() =>
        {
            Th155AllScoreDataWrapper.Create(new byte[0]);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void Th155AllScoreDataReadFromTestNoKey() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th155AllScoreDataWrapper.Create(TestUtils.MakeByteArray((int)SQOT.Null));

            Assert.IsNull(allScoreData.StoryDictionary);
            Assert.IsNull(allScoreData.CharacterDictionary);
            Assert.IsNull(allScoreData.BgmDictionary);
            Assert.IsNull(allScoreData.EndingDictionary);
            Assert.IsNull(allScoreData.StageDictionary);
            Assert.AreEqual(default, allScoreData.Version.Value);
        });

        [TestMethod]
        public void Th155AllScoreDataReadFromTestNoTables() => TestUtils.Wrap(() =>
        {
            var version = 1;

            var allScoreData = Th155AllScoreDataWrapper.Create(TestUtils.MakeByteArray(
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
        public void Th155AllScoreDataReadFromTestInvalidStoryDictionary() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th155AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
                .Concat(TestUtils.MakeSQByteArray("story", 123))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNull(allScoreData.StoryDictionary);
        });

        [TestMethod]
        public void Th155AllScoreDataReadFromTestInvalidStoryKey() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th155AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
                .Concat(TestUtils.MakeSQByteArray("story", new Dictionary<int, int>() { { 123, 456 } }))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNotNull(allScoreData.StoryDictionary);
            Assert.AreEqual(0, allScoreData.StoryDictionaryCount);
        });

        [TestMethod]
        public void Th155AllScoreDataReadFromTestInvalidStoryChara() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th155AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
                .Concat(TestUtils.MakeSQByteArray("story", new Dictionary<string, int>() { { "abc", 123 } }))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNotNull(allScoreData.StoryDictionary);
            Assert.AreEqual(0, allScoreData.StoryDictionaryCount);
        });

        [TestMethod]
        public void Th155AllScoreDataReadFromTestInvalidStory() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th155AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
                .Concat(TestUtils.MakeSQByteArray(
                    "story",
                    new Dictionary<string, int>()
                    {
                        { ToString(Th155Converter.StoryChara.ReimuKasen), 123 }
                    }))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNotNull(allScoreData.StoryDictionary);
            Assert.AreEqual(1, allScoreData.StoryDictionaryCount);
            var story = allScoreData.StoryDictionaryItem(Th155Converter.StoryChara.ReimuKasen);
            Assert.AreEqual(default, story.Stage.Value);
            Assert.AreEqual(default, story.Ed.Value);
            Assert.AreEqual(default, story.Available.Value);
            Assert.AreEqual(default, story.OverDrive.Value);
            Assert.AreEqual(default, story.StageOverDrive.Value);
        });

        [TestMethod]
        public void Th155AllScoreDataReadFromTestInvalidStoryFieldName() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th155AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
                .Concat(TestUtils.MakeSQByteArray(
                    "story",
                    new Dictionary<string, Dictionary<int, int>>()
                    {
                        {
                            ToString(Th155Converter.StoryChara.ReimuKasen),
                            new Dictionary<int, int>() { { 123, 456 } }
                        }
                    }))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNotNull(allScoreData.StoryDictionary);
            Assert.AreEqual(1, allScoreData.StoryDictionaryCount);
            var story = allScoreData.StoryDictionaryItem(Th155Converter.StoryChara.ReimuKasen);
            Assert.AreEqual(default, story.Stage.Value);
            Assert.AreEqual(default, story.Ed.Value);
            Assert.AreEqual(default, story.Available.Value);
            Assert.AreEqual(default, story.OverDrive.Value);
            Assert.AreEqual(default, story.StageOverDrive.Value);
        });

        [TestMethod]
        public void Th155AllScoreDataReadFromTestInvalidStoryFieldValue() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th155AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
                .Concat(TestUtils.MakeSQByteArray(
                    "story",
                    new Dictionary<string, Dictionary<string, float>>()
                    {
                        {
                            ToString(Th155Converter.StoryChara.ReimuKasen),
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
            Assert.AreEqual(1, allScoreData.StoryDictionaryCount);
            var story = allScoreData.StoryDictionaryItem(Th155Converter.StoryChara.ReimuKasen);
            Assert.AreEqual(default, story.Stage.Value);
            Assert.AreEqual(default, story.Ed.Value);
            Assert.AreEqual(default, story.Available.Value);
            Assert.AreEqual(default, story.OverDrive.Value);
            Assert.AreEqual(default, story.StageOverDrive.Value);
        });

        [TestMethod]
        public void Th155AllScoreDataReadFromTestInvalidCharacterDictionary() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th155AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
                .Concat(TestUtils.MakeSQByteArray("character", 123))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNull(allScoreData.CharacterDictionary);
        });

        [TestMethod]
        public void Th155AllScoreDataReadFromTestInvalidCharacterKey() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th155AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
                .Concat(TestUtils.MakeSQByteArray("character", new Dictionary<int, int>() { { 123, 456 } }))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNotNull(allScoreData.CharacterDictionary);
            Assert.AreEqual(0, allScoreData.CharacterDictionary.Count);
        });

        [TestMethod]
        public void Th155AllScoreDataReadFromTestInvalidCharacterValue() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th155AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
                .Concat(TestUtils.MakeSQByteArray("character", new Dictionary<string, string>() { { "abc", "def" } }))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNotNull(allScoreData.CharacterDictionary);
            Assert.AreEqual(0, allScoreData.CharacterDictionary.Count);
        });

        [TestMethod]
        public void Th155AllScoreDataReadFromTestInvalidBgmDictionary() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th155AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
                .Concat(TestUtils.MakeSQByteArray("bgm", 123))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNull(allScoreData.BgmDictionary);
        });

        [TestMethod]
        public void Th155AllScoreDataReadFromTestInvalidBgmKey() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th155AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
                .Concat(TestUtils.MakeSQByteArray("bgm", new Dictionary<int, int>() { { 123, 456 } }))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNotNull(allScoreData.BgmDictionary);
            Assert.AreEqual(0, allScoreData.BgmDictionary.Count);
        });

        [TestMethod]
        public void Th155AllScoreDataReadFromTestInvalidBgmValue() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th155AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
                .Concat(TestUtils.MakeSQByteArray("bgm", new Dictionary<bool, bool>() { { true, false } }))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNotNull(allScoreData.BgmDictionary);
            Assert.AreEqual(0, allScoreData.BgmDictionary.Count);
        });

        [TestMethod]
        public void Th155AllScoreDataReadFromTestInvalidEndingDictionary() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th155AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
                .Concat(TestUtils.MakeSQByteArray("ed", 123))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNull(allScoreData.EndingDictionary);
        });

        [TestMethod]
        public void Th155AllScoreDataReadFromTestInvalidEndingKey() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th155AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
                .Concat(TestUtils.MakeSQByteArray("ed", new Dictionary<int, int>() { { 123, 456 } }))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNotNull(allScoreData.EndingDictionary);
            Assert.AreEqual(0, allScoreData.EndingDictionary.Count);
        });

        [TestMethod]
        public void Th155AllScoreDataReadFromTestInvalidEndingValue() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th155AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
                .Concat(TestUtils.MakeSQByteArray("ed", new Dictionary<string, string>() { { "abc", "def" } }))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNotNull(allScoreData.EndingDictionary);
            Assert.AreEqual(0, allScoreData.EndingDictionary.Count);
        });

        [TestMethod]
        public void Th155AllScoreDataReadFromTestInvalidStageDictionary() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th155AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
                .Concat(TestUtils.MakeSQByteArray("stage", 123))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNull(allScoreData.StageDictionary);
        });

        [TestMethod]
        public void Th155AllScoreDataReadFromTestInvalidStageKey() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th155AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
                .Concat(TestUtils.MakeSQByteArray("stage", new Dictionary<string, int>() { { "abc", 123 } }))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNotNull(allScoreData.StageDictionary);
            Assert.AreEqual(0, allScoreData.StageDictionary.Count);
        });

        [TestMethod]
        public void Th155AllScoreDataReadFromTestInvalidStageValue() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th155AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
                .Concat(TestUtils.MakeSQByteArray("stage", new Dictionary<int, string>() { { 123, "abc" } }))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNotNull(allScoreData.StageDictionary);
            Assert.AreEqual(0, allScoreData.StageDictionary.Count);
        });

        [TestMethod]
        public void Th155AllScoreDataReadFromTestInvalidVersion() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th155AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
                .Concat(TestUtils.MakeSQByteArray("version", 123f))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.AreEqual(default, allScoreData.Version.Value);
        });
    }
}

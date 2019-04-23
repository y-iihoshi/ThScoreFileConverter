using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    using SQ = Squirrel;

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
            => TestUtils.MakeByteArray((int)SQ.OTTable)
                .Concat(TestUtils.MakeSQByteArray(
                    "stage", story.Stage.Value,
                    "ed", (int)story.Ed.Value,
                    "available", story.Available.Value,
                    "overdrive", story.OverDrive.Value,
                    "stage_overdrive", story.StageOverDrive.Value))
                .Concat(TestUtils.MakeByteArray((int)SQ.OTNull));

        internal static byte[] MakeByteArray(in Properties properties)
        {
            var sqTable = TestUtils.MakeByteArray((int)SQ.OTTable);
            var sqNull = TestUtils.MakeByteArray((int)SQ.OTNull);

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

            CollectionAssert.AreEqual(
                properties.characterDictionary.Keys, allScoreData.CharacterDictionary.Keys.ToArray());
            CollectionAssert.AreEqual(
                properties.characterDictionary.Values, allScoreData.CharacterDictionary.Values.ToArray());
            CollectionAssert.AreEqual(properties.bgmDictionary.Keys, allScoreData.BgmDictionary.Keys.ToArray());
            CollectionAssert.AreEqual(properties.bgmDictionary.Values, allScoreData.BgmDictionary.Values.ToArray());
            CollectionAssert.AreEqual(
                properties.endingDictionary.Keys, allScoreData.EndingDictionary.Keys.ToArray());
            CollectionAssert.AreEqual(
                properties.endingDictionary.Values, allScoreData.EndingDictionary.Values.ToArray());
            CollectionAssert.AreEqual(properties.stageDictionary.Keys, allScoreData.StageDictionary.Keys.ToArray());
            CollectionAssert.AreEqual(properties.stageDictionary.Values, allScoreData.StageDictionary.Values.ToArray());
            Assert.AreEqual(properties.version, allScoreData.Version);
        }

        internal static bool Th155AllScoreDataReadObjectHelper(byte[] array, out object obj)
        {
            var result = false;

            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(array);
                using (var reader = new BinaryReader(stream))
                {
                    stream = null;
                    result = Th155AllScoreDataWrapper.ReadObject(reader, out obj);
                }
            }
            finally
            {
                stream?.Dispose();
            }

            return result;
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
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th155AllScoreDataReadObjectTestNull() => TestUtils.Wrap(() =>
        {
            Th155AllScoreDataWrapper.ReadObject(null, out object obj);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th155AllScoreDataReadObjectTestEmpty() => TestUtils.Wrap(() =>
        {
            Th155AllScoreDataReadObjectHelper(new byte[0], out object obj);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void Th155AllScoreDataReadObjectTestOTNull() => TestUtils.Wrap(() =>
        {
            var result = Th155AllScoreDataReadObjectHelper(TestUtils.MakeByteArray((int)SQ.OTNull), out object obj);

            Assert.IsTrue(result);
            Assert.IsNotNull(obj);  // Hmm...
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(-1)]
        public void Th155AllScoreDataReadObjectTestOTInteger(int value) => TestUtils.Wrap(() =>
        {
            var result = Th155AllScoreDataReadObjectHelper(TestUtils.MakeSQByteArray(value).ToArray(), out object obj);

            Assert.IsTrue(result);
            Assert.IsTrue(obj is int);
            Assert.AreEqual(value, (int)obj);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th155AllScoreDataReadObjectTestOTIntegerInvalid() => TestUtils.Wrap(() =>
        {
            Th155AllScoreDataReadObjectHelper(TestUtils.MakeByteArray((int)SQ.OTInteger, new byte[3]), out object obj);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0f)]
        [DataRow(1f)]
        [DataRow(-1f)]
        [DataRow(0.25f)]
        [DataRow(0.1f)]
        public void Th155AllScoreDataReadObjectTestOTFloat(float value) => TestUtils.Wrap(() =>
        {
            var result = Th155AllScoreDataReadObjectHelper(TestUtils.MakeSQByteArray(value).ToArray(), out object obj);

            Assert.IsTrue(result);
            Assert.IsTrue(obj is float);
            Assert.AreEqual(value, (float)obj);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th155AllScoreDataReadObjectTestOTFloatInvalid() => TestUtils.Wrap(() =>
        {
            Th155AllScoreDataReadObjectHelper(TestUtils.MakeByteArray((int)SQ.OTFloat, new byte[3]), out object obj);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow((byte)0x00, false)]
        [DataRow((byte)0x01, true)]
        [DataRow((byte)0x02, true)]
        public void Th155AllScoreDataReadObjectTestOTBool(byte value, bool expected) => TestUtils.Wrap(() =>
        {
            var result = Th155AllScoreDataReadObjectHelper(
                TestUtils.MakeByteArray((int)SQ.OTBool, value), out object obj);

            Assert.IsTrue(result);
            Assert.IsTrue(obj is bool);
            Assert.AreEqual(expected, (bool)obj);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th155AllScoreDataReadObjectTestOTBoolInvalid() => TestUtils.Wrap(() =>
        {
            Th155AllScoreDataReadObjectHelper(TestUtils.MakeByteArray((int)SQ.OTBool), out object obj);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, "", "")]
        [DataRow(0, "abc", "")]
        [DataRow(0, null, "")]
        [DataRow(-1, "", "")]
        [DataRow(-1, "abc", "")]
        [DataRow(-1, null, "")]
        public void Th155AllScoreDataReadObjectTestOTStringEmpty(int size, string value, string expected)
            => TestUtils.Wrap(() =>
            {
                var bytes = (value != null) ? Encoding.Default.GetBytes(value) : new byte[0];
                var result = Th155AllScoreDataReadObjectHelper(
                    TestUtils.MakeByteArray((int)SQ.OTString, size, bytes), out object obj);
                var str = obj as string;

                Assert.IsTrue(result);
                Assert.IsNotNull(str);
                Assert.AreEqual(expected, str);
            });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("abc")]
        [DataRow("博麗 霊夢")]
        [DataRow("")]
        [DataRow("\0")]
        public void Th155AllScoreDataReadObjectTestOTString(string value) => TestUtils.Wrap(() =>
        {
            var result = Th155AllScoreDataReadObjectHelper(TestUtils.MakeSQByteArray(value).ToArray(), out object obj);
            var str = obj as string;

            Assert.IsTrue(result);
            Assert.IsNotNull(str);
            Assert.AreEqual(value, str, false, CultureInfo.InvariantCulture);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("abc")]
        [DataRow("博麗 霊夢")]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th155AllScoreDataReadObjectTestOTStringShortened(string value) => TestUtils.Wrap(() =>
        {
            var bytes = Encoding.Default.GetBytes(value);
            Th155AllScoreDataReadObjectHelper(
                TestUtils.MakeByteArray((int)SQ.OTString, bytes.Length + 1, bytes), out object obj);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("abc")]
        [DataRow("博麗 霊夢")]
        public void Th155AllScoreDataReadObjectTestOTStringExceeded(string value) => TestUtils.Wrap(() =>
        {
            var bytes = Encoding.Default.GetBytes(value).Concat(new byte[1] { 1 }).ToArray();
            var result = Th155AllScoreDataReadObjectHelper(
                TestUtils.MakeByteArray((int)SQ.OTString, bytes.Length - 1, bytes), out object obj);
            var str = obj as string;

            Assert.IsTrue(result);
            Assert.IsNotNull(str);
            Assert.AreEqual(value, str, false, CultureInfo.InvariantCulture);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(
            new int[6] { (int)SQ.OTTable, (int)SQ.OTInteger, 123, (int)SQ.OTInteger, 456, (int)SQ.OTNull },
            new int[1] { 123 },
            new int[1] { 456 },
            DisplayName = "one pair")]
        [DataRow(
            new int[10] {
                (int)SQ.OTTable,
                (int)SQ.OTInteger, 123, (int)SQ.OTInteger, 456,
                (int)SQ.OTInteger, 78, (int)SQ.OTInteger, 90,
                (int)SQ.OTNull },
            new int[2] { 123, 78 },
            new int[2] { 456, 90 },
            DisplayName = "two pairs")]
        public void Th155AllScoreDataReadObjectTestOTTable(int[] array, int[] expectedKeys, int[] expectedValues)
            => TestUtils.Wrap(() =>
            {
                var result = Th155AllScoreDataReadObjectHelper(TestUtils.MakeByteArray(array), out object obj);
                var dict = obj as Dictionary<object, object>;

                Assert.IsTrue(result);
                Assert.IsNotNull(dict);
                Assert.AreNotEqual(0, dict.Count);
                CollectionAssert.AreEqual(expectedKeys, dict.Keys.ToArray());
                CollectionAssert.AreEqual(expectedValues, dict.Values.ToArray());
            });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(new int[2] { (int)SQ.OTTable, (int)SQ.OTNull },
            DisplayName = "empty")]
        public void Th155AllScoreDataReadObjectTestOTTableEmpty(int[] array) => TestUtils.Wrap(() =>
        {
            var result = Th155AllScoreDataReadObjectHelper(TestUtils.MakeByteArray(array), out object obj);
            var dict = obj as Dictionary<object, object>;

            Assert.IsTrue(result);
            Assert.IsNotNull(dict);
            Assert.AreEqual(0, dict.Count);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(new int[5] { (int)SQ.OTTable, (int)SQ.OTInteger, 123, (int)SQ.OTInteger, (int)SQ.OTNull },
            DisplayName = "missing value data")]
        [DataRow(new int[5] { (int)SQ.OTTable, (int)SQ.OTInteger, 123, (int)SQ.OTInteger, 456 },
            DisplayName = "missing sentinel")]
        [DataRow(new int[4] { (int)SQ.OTTable, (int)SQ.OTInteger, 123, (int)SQ.OTInteger },
            DisplayName = "missing value data and sentinel")]
        [DataRow(new int[4] { (int)SQ.OTTable, (int)SQ.OTInteger, 123, (int)SQ.OTNull },
            DisplayName = "missing key or value")]
        [DataRow(new int[1] { (int)SQ.OTTable },
            DisplayName = "empty and missing sentinel")]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th155AllScoreDataReadObjectTestOTTableShortened(int[] array) => TestUtils.Wrap(() =>
        {
            Th155AllScoreDataReadObjectHelper(TestUtils.MakeByteArray(array), out object obj);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(new int[6] { (int)SQ.OTTable, 999, 123, (int)SQ.OTInteger, 456, (int)SQ.OTNull },
            DisplayName = "invalid key type")]
        [DataRow(new int[6] { (int)SQ.OTTable, (int)SQ.OTInteger, 123, 999, 456, (int)SQ.OTNull },
            DisplayName = "invalid value type")]
        [DataRow(new int[6] { (int)SQ.OTTable, (int)SQ.OTInteger, 123, (int)SQ.OTInteger, 456, 999 },
            DisplayName = "invalid sentinel")]
        [DataRow(new int[5] { (int)SQ.OTTable, 123, (int)SQ.OTInteger, 456, (int)SQ.OTNull },
            DisplayName = "missing key type")]
        [DataRow(new int[5] { (int)SQ.OTTable, (int)SQ.OTInteger, (int)SQ.OTInteger, 456, (int)SQ.OTNull },
            DisplayName = "missing key data")]
        [DataRow(new int[5] { (int)SQ.OTTable, (int)SQ.OTInteger, 123, 456, (int)SQ.OTNull },
            DisplayName = "missing value type")]
        [DataRow(new int[4] { (int)SQ.OTTable, 123, (int)SQ.OTInteger, 456 },
            DisplayName = "missing key type and sentinel")]
        [DataRow(new int[4] { (int)SQ.OTTable, 123, (int)SQ.OTInteger, (int)SQ.OTNull },
            DisplayName = "missing key type and value data")]
        [DataRow(new int[4] { (int)SQ.OTTable, (int)SQ.OTInteger, (int)SQ.OTInteger, 456 },
            DisplayName = "missing key data and sentinel")]
        [DataRow(new int[4] { (int)SQ.OTTable, (int)SQ.OTInteger, 123, 456 },
            DisplayName = "missing value type and sentinel")]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th155AllScoreDataReadObjectTestOTTableInvalid(int[] array) => TestUtils.Wrap(() =>
        {
            Th155AllScoreDataReadObjectHelper(TestUtils.MakeByteArray(array), out object obj);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(
            new int[7] { (int)SQ.OTArray, 1, (int)SQ.OTInteger, 0, (int)SQ.OTInteger, 123, (int)SQ.OTNull },
            new int[1] { 123 },
            DisplayName = "one element")]
        [DataRow(
            new int[11] {
                (int)SQ.OTArray, 2,
                (int)SQ.OTInteger, 0, (int)SQ.OTInteger, 123,
                (int)SQ.OTInteger, 1, (int)SQ.OTInteger, 456,
                (int)SQ.OTNull },
            new int[2] { 123, 456 },
            DisplayName = "two elements")]
        public void Th155AllScoreDataReadObjectTestOTArray(int[] array, int[] expected) => TestUtils.Wrap(() =>
        {
            var result = Th155AllScoreDataReadObjectHelper(TestUtils.MakeByteArray(array), out object obj);
            var resultArray = obj as object[];

            Assert.IsTrue(result);
            Assert.IsNotNull(resultArray);
            Assert.AreNotEqual(0, resultArray.Length);
            CollectionAssert.AreEqual(expected, resultArray);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(new int[3] { (int)SQ.OTArray, 0, (int)SQ.OTNull },
            DisplayName = "empty")]
        public void Th155AllScoreDataReadObjectTestOTArrayEmpty(int[] array) => TestUtils.Wrap(() =>
        {
            var result = Th155AllScoreDataReadObjectHelper(TestUtils.MakeByteArray(array), out object obj);
            var resultArray = obj as object[];

            Assert.IsTrue(result);
            Assert.IsNotNull(resultArray);
            Assert.AreEqual(0, resultArray.Length);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(new int[7] { (int)SQ.OTArray, 999, (int)SQ.OTInteger, 0, (int)SQ.OTInteger, 123, (int)SQ.OTNull },
            DisplayName = "invalid size")]
        [DataRow(new int[6] { (int)SQ.OTArray, 1, (int)SQ.OTInteger, 0, (int)SQ.OTInteger, (int)SQ.OTNull },
            DisplayName = "missing value data")]
        [DataRow(new int[6] { (int)SQ.OTArray, 1, (int)SQ.OTInteger, 0, (int)SQ.OTInteger, 123 },
            DisplayName = "missing sentinel")]
        [DataRow(new int[5] { (int)SQ.OTArray, 1, (int)SQ.OTInteger, 0, (int)SQ.OTInteger },
            DisplayName = "missing value data and sentinel")]
        [DataRow(new int[5] { (int)SQ.OTArray, 1, (int)SQ.OTInteger, 0, (int)SQ.OTNull },
            DisplayName = "missing value")]
        [DataRow(new int[3] { (int)SQ.OTArray, 999, (int)SQ.OTNull },
            DisplayName = "empty and invalid number of elements")]
        [DataRow(new int[2] { (int)SQ.OTArray, (int)SQ.OTNull },
            DisplayName = "empty and missing number of elements")]
        [DataRow(new int[2] { (int)SQ.OTArray, 0 },
            DisplayName = "empty and missing sentinel")]
        [DataRow(new int[1] { (int)SQ.OTArray },
            DisplayName = "empty and only array type")]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th155AllScoreDataReadObjectTestOTArrayShortened(int[] array) => TestUtils.Wrap(() =>
        {
            Th155AllScoreDataReadObjectHelper(TestUtils.MakeByteArray(array), out object obj);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(new int[7] { (int)SQ.OTArray, 0, (int)SQ.OTInteger, 0, (int)SQ.OTInteger, 123, (int)SQ.OTNull },
            DisplayName = "zero size and one element")]
        [DataRow(new int[7] { (int)SQ.OTArray, -1, (int)SQ.OTInteger, 0, (int)SQ.OTInteger, 123, (int)SQ.OTNull },
            DisplayName = "negative size")]
        [DataRow(new int[7] { (int)SQ.OTArray, 1, 999, 0, (int)SQ.OTInteger, 123, (int)SQ.OTNull },
            DisplayName = "invalid index type")]
        [DataRow(new int[7] { (int)SQ.OTArray, 1, (int)SQ.OTInteger, 999, (int)SQ.OTInteger, 123, (int)SQ.OTNull },
            DisplayName = "invalid index data")]
        [DataRow(new int[7] { (int)SQ.OTArray, 1, (int)SQ.OTInteger, 0, 999, 123, (int)SQ.OTNull },
            DisplayName = "invalid value type")]
        [DataRow(new int[7] { (int)SQ.OTArray, 1, (int)SQ.OTInteger, 0, (int)SQ.OTInteger, 123, 999 },
            DisplayName = "invalid sentinel")]
        [DataRow(new int[6] { (int)SQ.OTArray, (int)SQ.OTInteger, 0, (int)SQ.OTInteger, 123, (int)SQ.OTNull },
            DisplayName = "missing number of elements")]
        [DataRow(new int[6] { (int)SQ.OTArray, 1, 0, (int)SQ.OTInteger, 123, (int)SQ.OTNull },
            DisplayName = "missing index type")]
        [DataRow(new int[6] { (int)SQ.OTArray, 1, (int)SQ.OTInteger, (int)SQ.OTInteger, 123, (int)SQ.OTNull },
            DisplayName = "missing index data")]
        [DataRow(new int[6] { (int)SQ.OTArray, 1, (int)SQ.OTInteger, 0, 123, (int)SQ.OTNull },
            DisplayName = "missing value type")]
        [DataRow(new int[5] { (int)SQ.OTArray, 0, (int)SQ.OTInteger, 123, (int)SQ.OTNull },
            DisplayName = "missing number of elements and index type")]
        [DataRow(new int[5] { (int)SQ.OTArray, (int)SQ.OTInteger, 0, (int)SQ.OTInteger, 123 },
            DisplayName = "missing number of elements and sentinel")]
        [DataRow(new int[5] { (int)SQ.OTArray, 1, 0, (int)SQ.OTInteger, (int)SQ.OTNull },
            DisplayName = "missing index type and value data")]
        [DataRow(new int[5] { (int)SQ.OTArray, 1, 0, (int)SQ.OTInteger, 123 },
            DisplayName = "missing index type and sentinel")]
        [DataRow(new int[5] { (int)SQ.OTArray, 1, (int)SQ.OTInteger, (int)SQ.OTInteger, 123 },
            DisplayName = "missing index data and sentinel")]
        [DataRow(new int[5] { (int)SQ.OTArray, 1, (int)SQ.OTInteger, 0, 123 },
            DisplayName = "missing value type and sentinel")]
        [DataRow(new int[5] { (int)SQ.OTArray, 1, (int)SQ.OTInteger, 123, (int)SQ.OTNull },
            DisplayName = "missing index")]
        [DataRow(new int[3] { (int)SQ.OTArray, 0, 999 },
            DisplayName = "empty and invalid sentinel")]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th155AllScoreDataReadObjectTestOTArrayInvalid(int[] array) => TestUtils.Wrap(() =>
        {
            Th155AllScoreDataReadObjectHelper(TestUtils.MakeByteArray(array), out object obj);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void Th155AllScoreDataReadObjectTestOTInstance() => TestUtils.Wrap(() =>
        {
            var result = Th155AllScoreDataReadObjectHelper(TestUtils.MakeByteArray((int)SQ.OTInstance), out object obj);

            Assert.IsTrue(result);
            Assert.IsNotNull(obj);
        });

        [TestMethod]
        public void Th155AllScoreDataReadObjectTestOTClosure() => TestUtils.Wrap(() =>
        {
            var result = Th155AllScoreDataReadObjectHelper(TestUtils.MakeByteArray((int)SQ.OTClosure), out object obj);

            Assert.IsTrue(result);
            Assert.IsNotNull(obj);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(SQ.OTUserData)]
        [DataRow(SQ.OTNativeClosure)]
        [DataRow(SQ.OTGenerator)]
        [DataRow(SQ.OTUserPointer)]
        [DataRow(SQ.OTThread)]
        [DataRow(SQ.OTFuncProto)]
        [DataRow(SQ.OTClass)]
        [DataRow(SQ.OTWeakRef)]
        [DataRow(SQ.OTOuter)]
        [DataRow((SQ.SQObjectType)999)]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th155AllScoreDataReadObjectTestUnsupported(SQ.SQObjectType type) => TestUtils.Wrap(() =>
        {
            Th155AllScoreDataReadObjectHelper(TestUtils.MakeByteArray((int)type), out object obj);

            Assert.Fail(TestUtils.Unreachable);
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
            var allScoreData = Th155AllScoreDataWrapper.Create(TestUtils.MakeByteArray((int)SQ.OTNull));

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
                // (int)SQ.OTTable,
                TestUtils.MakeSQByteArray("version", version).ToArray(),
                (int)SQ.OTNull));

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
                // .Concat(TestUtils.MakeByteArray((int)SQ.OTTable))
                .Concat(TestUtils.MakeSQByteArray("story", 123))
                .Concat(TestUtils.MakeByteArray((int)SQ.OTNull))
                .ToArray());

            Assert.IsNull(allScoreData.StoryDictionary);
        });

        [TestMethod]
        public void Th155AllScoreDataReadFromTestInvalidStoryKey() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th155AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQ.OTTable))
                .Concat(TestUtils.MakeSQByteArray("story", new Dictionary<int, int>() { { 123, 456 } }))
                .Concat(TestUtils.MakeByteArray((int)SQ.OTNull))
                .ToArray());

            Assert.IsNotNull(allScoreData.StoryDictionary);
            Assert.AreEqual(0, allScoreData.StoryDictionaryCount);
        });

        [TestMethod]
        public void Th155AllScoreDataReadFromTestInvalidStoryChara() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th155AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQ.OTTable))
                .Concat(TestUtils.MakeSQByteArray("story", new Dictionary<string, int>() { { "abc", 123 } }))
                .Concat(TestUtils.MakeByteArray((int)SQ.OTNull))
                .ToArray());

            Assert.IsNotNull(allScoreData.StoryDictionary);
            Assert.AreEqual(0, allScoreData.StoryDictionaryCount);
        });

        [TestMethod]
        public void Th155AllScoreDataReadFromTestInvalidStory() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th155AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQ.OTTable))
                .Concat(TestUtils.MakeSQByteArray(
                    "story",
                    new Dictionary<string, int>()
                    {
                        { ToString(Th155Converter.StoryChara.ReimuKasen), 123 }
                    }))
                .Concat(TestUtils.MakeByteArray((int)SQ.OTNull))
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
                // .Concat(TestUtils.MakeByteArray((int)SQ.OTTable))
                .Concat(TestUtils.MakeSQByteArray(
                    "story",
                    new Dictionary<string, Dictionary<int, int>>()
                    {
                        {
                            ToString(Th155Converter.StoryChara.ReimuKasen),
                            new Dictionary<int, int>() { { 123, 456 } }
                        }
                    }))
                .Concat(TestUtils.MakeByteArray((int)SQ.OTNull))
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
                // .Concat(TestUtils.MakeByteArray((int)SQ.OTTable))
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
                .Concat(TestUtils.MakeByteArray((int)SQ.OTNull))
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
                // .Concat(TestUtils.MakeByteArray((int)SQ.OTTable))
                .Concat(TestUtils.MakeSQByteArray("character", 123))
                .Concat(TestUtils.MakeByteArray((int)SQ.OTNull))
                .ToArray());

            Assert.IsNull(allScoreData.CharacterDictionary);
        });

        [TestMethod]
        public void Th155AllScoreDataReadFromTestInvalidCharacterKey() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th155AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQ.OTTable))
                .Concat(TestUtils.MakeSQByteArray("character", new Dictionary<int, int>() { { 123, 456 } }))
                .Concat(TestUtils.MakeByteArray((int)SQ.OTNull))
                .ToArray());

            Assert.IsNotNull(allScoreData.CharacterDictionary);
            Assert.AreEqual(0, allScoreData.CharacterDictionary.Count);
        });

        [TestMethod]
        public void Th155AllScoreDataReadFromTestInvalidCharacterValue() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th155AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQ.OTTable))
                .Concat(TestUtils.MakeSQByteArray("character", new Dictionary<string, string>() { { "abc", "def" } }))
                .Concat(TestUtils.MakeByteArray((int)SQ.OTNull))
                .ToArray());

            Assert.IsNotNull(allScoreData.CharacterDictionary);
            Assert.AreEqual(0, allScoreData.CharacterDictionary.Count);
        });

        [TestMethod]
        public void Th155AllScoreDataReadFromTestInvalidBgmDictionary() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th155AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQ.OTTable))
                .Concat(TestUtils.MakeSQByteArray("bgm", 123))
                .Concat(TestUtils.MakeByteArray((int)SQ.OTNull))
                .ToArray());

            Assert.IsNull(allScoreData.BgmDictionary);
        });

        [TestMethod]
        public void Th155AllScoreDataReadFromTestInvalidBgmKey() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th155AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQ.OTTable))
                .Concat(TestUtils.MakeSQByteArray("bgm", new Dictionary<int, int>() { { 123, 456 } }))
                .Concat(TestUtils.MakeByteArray((int)SQ.OTNull))
                .ToArray());

            Assert.IsNotNull(allScoreData.BgmDictionary);
            Assert.AreEqual(0, allScoreData.BgmDictionary.Count);
        });

        [TestMethod]
        public void Th155AllScoreDataReadFromTestInvalidBgmValue() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th155AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQ.OTTable))
                .Concat(TestUtils.MakeSQByteArray("bgm", new Dictionary<bool, bool>() { { true, false } }))
                .Concat(TestUtils.MakeByteArray((int)SQ.OTNull))
                .ToArray());

            Assert.IsNotNull(allScoreData.BgmDictionary);
            Assert.AreEqual(0, allScoreData.BgmDictionary.Count);
        });

        [TestMethod]
        public void Th155AllScoreDataReadFromTestInvalidEndingDictionary() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th155AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQ.OTTable))
                .Concat(TestUtils.MakeSQByteArray("ed", 123))
                .Concat(TestUtils.MakeByteArray((int)SQ.OTNull))
                .ToArray());

            Assert.IsNull(allScoreData.EndingDictionary);
        });

        [TestMethod]
        public void Th155AllScoreDataReadFromTestInvalidEndingKey() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th155AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQ.OTTable))
                .Concat(TestUtils.MakeSQByteArray("ed", new Dictionary<int, int>() { { 123, 456 } }))
                .Concat(TestUtils.MakeByteArray((int)SQ.OTNull))
                .ToArray());

            Assert.IsNotNull(allScoreData.EndingDictionary);
            Assert.AreEqual(0, allScoreData.EndingDictionary.Count);
        });

        [TestMethod]
        public void Th155AllScoreDataReadFromTestInvalidEndingValue() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th155AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQ.OTTable))
                .Concat(TestUtils.MakeSQByteArray("ed", new Dictionary<string, string>() { { "abc", "def" } }))
                .Concat(TestUtils.MakeByteArray((int)SQ.OTNull))
                .ToArray());

            Assert.IsNotNull(allScoreData.EndingDictionary);
            Assert.AreEqual(0, allScoreData.EndingDictionary.Count);
        });

        [TestMethod]
        public void Th155AllScoreDataReadFromTestInvalidStageDictionary() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th155AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQ.OTTable))
                .Concat(TestUtils.MakeSQByteArray("stage", 123))
                .Concat(TestUtils.MakeByteArray((int)SQ.OTNull))
                .ToArray());

            Assert.IsNull(allScoreData.StageDictionary);
        });

        [TestMethod]
        public void Th155AllScoreDataReadFromTestInvalidStageKey() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th155AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQ.OTTable))
                .Concat(TestUtils.MakeSQByteArray("stage", new Dictionary<string, int>() { { "abc", 123 } }))
                .Concat(TestUtils.MakeByteArray((int)SQ.OTNull))
                .ToArray());

            Assert.IsNotNull(allScoreData.StageDictionary);
            Assert.AreEqual(0, allScoreData.StageDictionary.Count);
        });

        [TestMethod]
        public void Th155AllScoreDataReadFromTestInvalidStageValue() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th155AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQ.OTTable))
                .Concat(TestUtils.MakeSQByteArray("stage", new Dictionary<int, string>() { { 123, "abc" } }))
                .Concat(TestUtils.MakeByteArray((int)SQ.OTNull))
                .ToArray());

            Assert.IsNotNull(allScoreData.StageDictionary);
            Assert.AreEqual(0, allScoreData.StageDictionary.Count);
        });

        [TestMethod]
        public void Th155AllScoreDataReadFromTestInvalidVersion() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th155AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQ.OTTable))
                .Concat(TestUtils.MakeSQByteArray("version", 123f))
                .Concat(TestUtils.MakeByteArray((int)SQ.OTNull))
                .ToArray());

            Assert.AreEqual(default, allScoreData.Version.Value);
        });
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    using SQ = Squirrel;

    [TestClass]
    public class Th135AllScoreDataTests
    {
        internal struct Properties
        {
            public int storyProgress;
            public Dictionary<Th135Converter.Chara, Th135Converter.LevelFlag> storyClearFlags;
            public int endingCount;
            public int ending2Count;
            public bool isEnabledStageTanuki1;
            public bool isEnabledStageTanuki2;
            public bool isEnabledStageKokoro;
            public bool isPlayableMamizou;
            public bool isPlayableKokoro;
            public Dictionary<int, bool> bgmFlags;
        };

        internal static Properties GetValidProperties() => new Properties()
        {
            storyProgress = 1,
            storyClearFlags = Utils.GetEnumerator<Th135Converter.Chara>()
                .ToDictionary(chara => chara, chara => TestUtils.Cast<Th135Converter.LevelFlag>(chara)),
            endingCount = 2,
            ending2Count = 3,
            isEnabledStageTanuki1 = true,
            isEnabledStageTanuki2 = true,
            isEnabledStageKokoro = false,
            isPlayableMamizou = true,
            isPlayableKokoro = false,
            bgmFlags = Enumerable.Range(1, 10).ToDictionary(id => id, id => id % 2 == 0)
        };

        internal static byte[] MakeByteArray(in Properties properties)
        {
            byte toByte(bool value) => (byte)(value ? 0x01 : 0x00);

            var storyProgressKey = Encoding.Default.GetBytes("story_progress");
            var storyClearKey = Encoding.Default.GetBytes("story_clear");
            var storyClearValue = properties.storyClearFlags.SelectMany(
                pair => TestUtils.MakeByteArray(
                    (int)SQ.OTInteger, TestUtils.Cast<int>(pair.Key),
                    (int)SQ.OTInteger, TestUtils.Cast<int>(pair.Value))).ToArray();
            var edCountKey = Encoding.Default.GetBytes("ed_count");
            var ed2CountKey = Encoding.Default.GetBytes("ed2_count");
            var enableStageTanuki1Key = Encoding.Default.GetBytes("enable_stage_tanuki1");
            var enableStageTanuki2Key = Encoding.Default.GetBytes("enable_stage_tanuki2");
            var enableStageKokoroKey = Encoding.Default.GetBytes("enable_stage_kokoro");
            var enableMamizouKey = Encoding.Default.GetBytes("enable_mamizou");
            var enableKokoroKey = Encoding.Default.GetBytes("enable_kokoro");
            var enableBgmKey = Encoding.Default.GetBytes("enable_bgm");
            var enableBgmValue = properties.bgmFlags.SelectMany(
                pair => TestUtils.MakeByteArray(
                    (int)SQ.OTInteger, pair.Key,
                    (int)SQ.OTBool, toByte(pair.Value))).ToArray();

            return TestUtils.MakeByteArray(
                // (int)SQ.OTTable,
                (int)SQ.OTString, storyProgressKey.Length, storyProgressKey,
                (int)SQ.OTInteger, properties.storyProgress,
                (int)SQ.OTString, storyClearKey.Length, storyClearKey,
                (int)SQ.OTArray, properties.storyClearFlags.Count, storyClearValue, (int)SQ.OTNull,
                (int)SQ.OTString, edCountKey.Length, edCountKey,
                (int)SQ.OTInteger, properties.endingCount,
                (int)SQ.OTString, ed2CountKey.Length, ed2CountKey,
                (int)SQ.OTInteger, properties.ending2Count,
                (int)SQ.OTString, enableStageTanuki1Key.Length, enableStageTanuki1Key,
                (int)SQ.OTBool, toByte(properties.isEnabledStageTanuki1),
                (int)SQ.OTString, enableStageTanuki2Key.Length, enableStageTanuki2Key,
                (int)SQ.OTBool, toByte(properties.isEnabledStageTanuki2),
                (int)SQ.OTString, enableStageKokoroKey.Length, enableStageKokoroKey,
                (int)SQ.OTBool, toByte(properties.isEnabledStageKokoro),
                (int)SQ.OTString, enableMamizouKey.Length, enableMamizouKey,
                (int)SQ.OTBool, toByte(properties.isPlayableMamizou),
                (int)SQ.OTString, enableKokoroKey.Length, enableKokoroKey,
                (int)SQ.OTBool, toByte(properties.isPlayableKokoro),
                (int)SQ.OTString, enableBgmKey.Length, enableBgmKey,
                (int)SQ.OTTable, enableBgmValue, (int)SQ.OTNull,
                (int)SQ.OTNull);
        }

        internal static void Validate(in Th135AllScoreDataWrapper allScoreData, in Properties properties)
        {
            Assert.AreEqual(properties.storyProgress, allScoreData.StoryProgress);
            CollectionAssert.AreEqual(properties.storyClearFlags.Keys, allScoreData.StoryClearFlags.Keys.ToArray());
            CollectionAssert.AreEqual(properties.storyClearFlags.Values, allScoreData.StoryClearFlags.Values.ToArray());
            Assert.AreEqual(properties.endingCount, allScoreData.EndingCount);
            Assert.AreEqual(properties.ending2Count, allScoreData.Ending2Count);
            Assert.AreEqual(properties.isEnabledStageTanuki1, allScoreData.IsEnabledStageTanuki1);
            Assert.AreEqual(properties.isEnabledStageTanuki2, allScoreData.IsEnabledStageTanuki2);
            Assert.AreEqual(properties.isEnabledStageKokoro, allScoreData.IsEnabledStageKokoro);
            Assert.AreEqual(properties.isPlayableMamizou, allScoreData.IsPlayableMamizou);
            Assert.AreEqual(properties.isPlayableKokoro, allScoreData.IsPlayableKokoro);
            CollectionAssert.AreEqual(properties.bgmFlags.Keys, allScoreData.BgmFlags.Keys.ToArray());
            CollectionAssert.AreEqual(properties.bgmFlags.Values, allScoreData.BgmFlags.Values.ToArray());
        }

        internal static bool Th135AllScoreDataReadObjectHelper(byte[] array, out object obj)
        {
            var result = false;

            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(array);
                using (var reader = new BinaryReader(stream))
                {
                    stream = null;
                    result = Th135AllScoreDataWrapper.ReadObject(reader, out obj);
                }
            }
            finally
            {
                stream?.Dispose();
            }

            return result;
        }

        [TestMethod]
        public void Th135AllScoreDataTest() => TestUtils.Wrap(() =>
        {
            var allScoreData = new Th135AllScoreDataWrapper();

            Assert.AreEqual(default, allScoreData.StoryProgress.Value);
            Assert.IsNull(allScoreData.StoryClearFlags);
            Assert.AreEqual(default, allScoreData.EndingCount.Value);
            Assert.AreEqual(default, allScoreData.Ending2Count.Value);
            Assert.AreEqual(default, allScoreData.IsEnabledStageTanuki1.Value);
            Assert.AreEqual(default, allScoreData.IsEnabledStageTanuki2.Value);
            Assert.AreEqual(default, allScoreData.IsEnabledStageKokoro.Value);
            Assert.AreEqual(default, allScoreData.IsPlayableMamizou.Value);
            Assert.AreEqual(default, allScoreData.IsPlayableKokoro.Value);
            Assert.IsNull(allScoreData.BgmFlags);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th135AllScoreDataReadObjectTestNull() => TestUtils.Wrap(() =>
        {
            Th135AllScoreDataWrapper.ReadObject(null, out object obj);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th135AllScoreDataReadObjectTestEmpty() => TestUtils.Wrap(() =>
        {
            Th135AllScoreDataReadObjectHelper(new byte[0], out object obj);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void Th135AllScoreDataReadObjectTestOTNull() => TestUtils.Wrap(() =>
        {
            var result = Th135AllScoreDataReadObjectHelper(TestUtils.MakeByteArray((int)SQ.OTNull), out object obj);

            Assert.IsTrue(result);
            Assert.IsNotNull(obj);  // Hmm...
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(-1)]
        public void Th135AllScoreDataReadObjectTestOTInteger(int value) => TestUtils.Wrap(() =>
        {
            var result = Th135AllScoreDataReadObjectHelper(
                TestUtils.MakeByteArray((int)SQ.OTInteger, value), out object obj);

            Assert.IsTrue(result);
            Assert.IsTrue(obj is int);
            Assert.AreEqual(value, (int)obj);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th135AllScoreDataReadObjectTestOTIntegerInvalid() => TestUtils.Wrap(() =>
        {
            Th135AllScoreDataReadObjectHelper(TestUtils.MakeByteArray((int)SQ.OTInteger, new byte[3]), out object obj);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0f)]
        [DataRow(1f)]
        [DataRow(-1f)]
        [DataRow(0.25f)]
        [DataRow(0.1f)]
        public void Th135AllScoreDataReadObjectTestOTFloat(float value) => TestUtils.Wrap(() =>
        {
            var result = Th135AllScoreDataReadObjectHelper(
                TestUtils.MakeByteArray((int)SQ.OTFloat, value), out object obj);

            Assert.IsTrue(result);
            Assert.IsTrue(obj is float);
            Assert.AreEqual(value, (float)obj);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th135AllScoreDataReadObjectTestOTFloatInvalid() => TestUtils.Wrap(() =>
        {
            Th135AllScoreDataReadObjectHelper(TestUtils.MakeByteArray((int)SQ.OTFloat, new byte[3]), out object obj);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow((byte)0x00, false)]
        [DataRow((byte)0x01, true)]
        [DataRow((byte)0x02, true)]
        public void Th135AllScoreDataReadObjectTestOTBool(byte value, bool expected) => TestUtils.Wrap(() =>
        {
            var result = Th135AllScoreDataReadObjectHelper(
                TestUtils.MakeByteArray((int)SQ.OTBool, value), out object obj);

            Assert.IsTrue(result);
            Assert.IsTrue(obj is bool);
            Assert.AreEqual(expected, (bool)obj);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th135AllScoreDataReadObjectTestOTBoolInvalid() => TestUtils.Wrap(() =>
        {
            Th135AllScoreDataReadObjectHelper(TestUtils.MakeByteArray((int)SQ.OTBool), out object obj);

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
        public void Th135AllScoreDataReadObjectTestOTStringEmpty(int size, string value, string expected)
            => TestUtils.Wrap(() =>
            {
                var bytes = (value != null) ? Encoding.Default.GetBytes(value) : new byte[0];
                var result = Th135AllScoreDataReadObjectHelper(
                    TestUtils.MakeByteArray((int)SQ.OTString, size, bytes), out object obj);
                var str = obj as string;

                Assert.IsTrue(result);
                Assert.IsNotNull(str);
                Assert.AreEqual(expected, str);
            });

        [DataTestMethod]
        [DataRow("abc")]
        [DataRow("博麗 霊夢")]
        [DataRow("")]
        [DataRow("\0")]
        public void Th135AllScoreDataReadObjectTestOTString(string value)
            => Th135AllScoreDataReadObjectTestOTStringEmpty(Encoding.Default.GetByteCount(value), value, value);

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("abc")]
        [DataRow("博麗 霊夢")]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th135AllScoreDataReadObjectTestOTStringShortened(string value) => TestUtils.Wrap(() =>
        {
            var bytes = Encoding.Default.GetBytes(value);
            Th135AllScoreDataReadObjectHelper(
                TestUtils.MakeByteArray((int)SQ.OTString, bytes.Length + 1, bytes), out object obj);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("abc")]
        [DataRow("博麗 霊夢")]
        public void Th135AllScoreDataReadObjectTestOTStringExceeded(string value) => TestUtils.Wrap(() =>
        {
            var bytes = Encoding.Default.GetBytes(value).Concat(new byte[1] { 1 }).ToArray();
            var result = Th135AllScoreDataReadObjectHelper(
                TestUtils.MakeByteArray((int)SQ.OTString, bytes.Length - 1, bytes), out object obj);
            var str = obj as string;

            Assert.IsTrue(result);
            Assert.IsNotNull(str);
            Assert.AreEqual(value, str);
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
        [DataRow(
            new int[6] { (int)SQ.OTTable, (int)SQ.OTInteger, 123, (int)SQ.OTInteger, 456, 999 },
            new int[1] { 123 },
            new int[1] { 456 },
            DisplayName = "invalid sentinel")]
        public void Th135AllScoreDataReadObjectTestOTTable(int[] array, int[] expectedKeys, int[] expectedValues)
            => TestUtils.Wrap(() =>
            {
                var result = Th135AllScoreDataReadObjectHelper(TestUtils.MakeByteArray(array), out object obj);
                var dict = obj as Dictionary<object, object>;

                Assert.IsTrue(result);
                Assert.IsNotNull(dict);
                Assert.AreNotEqual(0, dict.Count);
                CollectionAssert.AreEqual(expectedKeys, dict.Keys.ToArray());
                CollectionAssert.AreEqual(expectedValues, dict.Values.ToArray());
            });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(new int[6] { (int)SQ.OTTable, 999, 123, (int)SQ.OTInteger, 456, (int)SQ.OTNull },
            DisplayName = "invalid key type")]
        [DataRow(new int[6] { (int)SQ.OTTable, (int)SQ.OTInteger, 123, 999, 456, (int)SQ.OTNull },
            DisplayName = "invalid value type")]
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
        [DataRow(new int[2] { (int)SQ.OTTable, (int)SQ.OTNull },
            DisplayName = "empty")]
        public void Th135AllScoreDataReadObjectTestOTTableEmpty(int[] array) => TestUtils.Wrap(() =>
        {
            var result = Th135AllScoreDataReadObjectHelper(TestUtils.MakeByteArray(array), out object obj);
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
        [DataRow(new int[4] { (int)SQ.OTTable, (int)SQ.OTInteger, (int)SQ.OTInteger, 456 },
            DisplayName = "missing key data and sentinel")]
        [DataRow(new int[4] { (int)SQ.OTTable, (int)SQ.OTInteger, 123, 456 },
            DisplayName = "missing value type and sentinel")]
        [DataRow(new int[4] { (int)SQ.OTTable, (int)SQ.OTInteger, 123, (int)SQ.OTInteger },
            DisplayName = "missing value data and sentinel")]
        [DataRow(new int[4] { (int)SQ.OTTable, (int)SQ.OTInteger, 123, (int)SQ.OTNull },
            DisplayName = "missing key or value")]
        [DataRow(new int[1] { (int)SQ.OTTable },
            DisplayName = "empty and missing sentinel")]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th135AllScoreDataReadObjectTestOTTableInvalid(int[] array) => TestUtils.Wrap(() =>
        {
            Th135AllScoreDataReadObjectHelper(TestUtils.MakeByteArray(array), out object obj);

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
        public void Th135AllScoreDataReadObjectTestOTArray(int[] array, int[] expected) => TestUtils.Wrap(() =>
        {
            var result = Th135AllScoreDataReadObjectHelper(TestUtils.MakeByteArray(array), out object obj);
            var resultArray = obj as object[];

            Assert.IsTrue(result);
            Assert.IsNotNull(resultArray);
            Assert.AreNotEqual(0, resultArray.Length);
            CollectionAssert.AreEqual(expected, resultArray);
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
        [DataRow(new int[6] { (int)SQ.OTArray, 1, 0, (int)SQ.OTInteger, 123, (int)SQ.OTNull },
            DisplayName = "missing index type")]
        [DataRow(new int[6] { (int)SQ.OTArray, 1, (int)SQ.OTInteger, (int)SQ.OTInteger, 123, (int)SQ.OTNull },
            DisplayName = "missing index data")]
        [DataRow(new int[6] { (int)SQ.OTArray, 1, (int)SQ.OTInteger, 0, 123, (int)SQ.OTNull },
            DisplayName = "missing value type")]
        [DataRow(new int[5] { (int)SQ.OTArray, 1, 0, (int)SQ.OTInteger, 123 },
            DisplayName = "missing index type and sentinel")]
        [DataRow(new int[5] { (int)SQ.OTArray, 0, (int)SQ.OTInteger, 123, (int)SQ.OTNull },
            DisplayName = "missing number of elements and index type")]
        [DataRow(new int[5] { (int)SQ.OTArray, 1, 0, (int)SQ.OTInteger, (int)SQ.OTNull },
            DisplayName = "missing index type and value data")]
        [DataRow(new int[3] { (int)SQ.OTArray, 0, (int)SQ.OTNull },
            DisplayName = "empty")]
        public void Th135AllScoreDataReadObjectTestOTArrayEmpty(int[] array) => TestUtils.Wrap(() =>
        {
            var result = Th135AllScoreDataReadObjectHelper(TestUtils.MakeByteArray(array), out object obj);
            var resultArray = obj as object[];

            Assert.IsTrue(result);
            Assert.IsNotNull(resultArray);
            Assert.AreEqual(0, resultArray.Length);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(new int[7] { (int)SQ.OTArray, 999, (int)SQ.OTInteger, 0, (int)SQ.OTInteger, 123, (int)SQ.OTNull },
            DisplayName = "invalid size")]
        [DataRow(new int[6] { (int)SQ.OTArray, (int)SQ.OTInteger, 0, (int)SQ.OTInteger, 123, (int)SQ.OTNull },
            DisplayName = "missing number of elements")]
        [DataRow(new int[6] { (int)SQ.OTArray, 1, (int)SQ.OTInteger, 0, (int)SQ.OTInteger, (int)SQ.OTNull },
            DisplayName = "missing value data")]
        [DataRow(new int[6] { (int)SQ.OTArray, 1, (int)SQ.OTInteger, 0, (int)SQ.OTInteger, 123 },
            DisplayName = "missing sentinel")]
        [DataRow(new int[5] { (int)SQ.OTArray, (int)SQ.OTInteger, 0, (int)SQ.OTInteger, 123 },
            DisplayName = "missing number of elements and sentinel")]
        [DataRow(new int[5] { (int)SQ.OTArray, 1, (int)SQ.OTInteger, (int)SQ.OTInteger, 123 },
            DisplayName = "missing index data and sentinel")]
        [DataRow(new int[5] { (int)SQ.OTArray, 1, (int)SQ.OTInteger, 0, 123 },
            DisplayName = "missing value type and sentinel")]
        [DataRow(new int[5] { (int)SQ.OTArray, 1, (int)SQ.OTInteger, 0, (int)SQ.OTInteger },
            DisplayName = "missing value data and sentinel")]
        [DataRow(new int[5] { (int)SQ.OTArray, 1, (int)SQ.OTInteger, 123, (int)SQ.OTNull },
            DisplayName = "missing index")]
        [DataRow(new int[5] { (int)SQ.OTArray, 1, (int)SQ.OTInteger, 0, (int)SQ.OTNull },
            DisplayName = "missing value")]
        [DataRow(new int[3] { (int)SQ.OTArray, 999, (int)SQ.OTNull },
            DisplayName = "empty and invalid number of elements")]
        [DataRow(new int[3] { (int)SQ.OTArray, 0, 999 },
            DisplayName = "empty and invalid sentinel")]
        [DataRow(new int[2] { (int)SQ.OTArray, (int)SQ.OTNull },
            DisplayName = "empty and missing number of elements")]
        [DataRow(new int[2] { (int)SQ.OTArray, 0 },
            DisplayName = "empty and missing sentinel")]
        [DataRow(new int[1] { (int)SQ.OTArray },
            DisplayName = "empty and only array type")]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th135AllScoreDataReadObjectTestOTArrayInvalid(int[] array) => TestUtils.Wrap(() =>
        {
            Th135AllScoreDataReadObjectHelper(TestUtils.MakeByteArray(array), out object obj);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void Th135AllScoreDataReadObjectTestOTInstance() => TestUtils.Wrap(() =>
        {
            var result = Th135AllScoreDataReadObjectHelper(TestUtils.MakeByteArray((int)SQ.OTInstance), out object obj);

            Assert.IsTrue(result);
            Assert.IsNotNull(obj);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(SQ.OTUserData)]
        [DataRow(SQ.OTClosure)]
        [DataRow(SQ.OTNativeClosure)]
        [DataRow(SQ.OTGenerator)]
        [DataRow(SQ.OTUserPointer)]
        [DataRow(SQ.OTThread)]
        [DataRow(SQ.OTFuncProto)]
        [DataRow(SQ.OTClass)]
        [DataRow(SQ.OTWeakRef)]
        [DataRow(SQ.OTOuter)]
        [DataRow((SQ.SQObjectType)999)]
        public void Th135AllScoreDataReadObjectTestUnsupported(SQ.SQObjectType type) => TestUtils.Wrap(() =>
        {
            var result = Th135AllScoreDataReadObjectHelper(TestUtils.MakeByteArray((int)type), out object obj);

            Assert.IsFalse(result);
            Assert.IsNull(obj);
        });

        [TestMethod]
        public void Th135AllScoreDataReadFromTest() => TestUtils.Wrap(() =>
        {
            var properties = GetValidProperties();

            var allScoreData = Th135AllScoreDataWrapper.Create(MakeByteArray(properties));

            Validate(allScoreData, properties);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th135AllScoreDataReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var allScoreData = new Th135AllScoreDataWrapper();
            allScoreData.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th135AllScoreDataReadFromTestEmpty() => TestUtils.Wrap(() =>
        {
            Th135AllScoreDataWrapper.Create(new byte[0]);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void Th135AllScoreDataReadFromTestNoKey() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th135AllScoreDataWrapper.Create(TestUtils.MakeByteArray((int)SQ.OTNull));

            Assert.AreEqual(default, allScoreData.StoryProgress.Value);
            Assert.IsNull(allScoreData.StoryClearFlags);
            Assert.AreEqual(default, allScoreData.EndingCount.Value);
            Assert.AreEqual(default, allScoreData.Ending2Count.Value);
            Assert.AreEqual(default, allScoreData.IsEnabledStageTanuki1.Value);
            Assert.AreEqual(default, allScoreData.IsEnabledStageTanuki2.Value);
            Assert.AreEqual(default, allScoreData.IsEnabledStageKokoro.Value);
            Assert.AreEqual(default, allScoreData.IsPlayableMamizou.Value);
            Assert.AreEqual(default, allScoreData.IsPlayableKokoro.Value);
            Assert.IsNull(allScoreData.BgmFlags);
        });

        [TestMethod]
        public void Th135AllScoreDataReadFromTestNoTables() => TestUtils.Wrap(() =>
        {
            var stageProgressKey = Encoding.Default.GetBytes("story_progress");
            var stageProgressValue = 1;

            var allScoreData = Th135AllScoreDataWrapper.Create(TestUtils.MakeByteArray(
                // (int)SQ.OTTable,
                (int)SQ.OTString, stageProgressKey.Length, stageProgressKey,
                (int)SQ.OTInteger, stageProgressValue,
                (int)SQ.OTNull));

            Assert.AreEqual(stageProgressValue, allScoreData.StoryProgress);
            Assert.IsNull(allScoreData.StoryClearFlags);
            Assert.IsNull(allScoreData.BgmFlags);
        });

        [TestMethod]
        public void Th135AllScoreDataReadFromTestInvalidStoryClear() => TestUtils.Wrap(() =>
        {
            var stageClearKey = Encoding.Default.GetBytes("story_clear");
            var stageClearValue = 1;

            var allScoreData = Th135AllScoreDataWrapper.Create(TestUtils.MakeByteArray(
                // (int)SQ.OTTable,
                (int)SQ.OTString, stageClearKey.Length, stageClearKey,
                (int)SQ.OTInteger, stageClearValue,
                (int)SQ.OTNull));

            Assert.IsNull(allScoreData.StoryClearFlags);
        });

        [TestMethod]
        public void Th135AllScoreDataReadFromTestInvalidStoryClearValue() => TestUtils.Wrap(() =>
        {
            var stageClearKey = Encoding.Default.GetBytes("story_clear");
            var stageClearValue = TestUtils.MakeByteArray((int)SQ.OTInteger, 0, (int)SQ.OTFloat, 123f);

            var allScoreData = Th135AllScoreDataWrapper.Create(TestUtils.MakeByteArray(
                // (int)SQ.OTTable,
                (int)SQ.OTString, stageClearKey.Length, stageClearKey,
                (int)SQ.OTArray, 1, stageClearValue, (int)SQ.OTNull,
                (int)SQ.OTNull));

            Assert.IsNotNull(allScoreData.StoryClearFlags);
            Assert.AreEqual(0, allScoreData.StoryClearFlags.Count);
        });

        [TestMethod]
        public void Th135AllScoreDataReadFromTestInvalidEnableBgm() => TestUtils.Wrap(() =>
        {
            var enableBgmKey = Encoding.Default.GetBytes("enable_bgm");
            var enableBgmValue = 1;

            var allScoreData = Th135AllScoreDataWrapper.Create(TestUtils.MakeByteArray(
                // (int)SQ.OTTable,
                (int)SQ.OTString, enableBgmKey.Length, enableBgmKey,
                (int)SQ.OTInteger, enableBgmValue,
                (int)SQ.OTNull));

            Assert.IsNull(allScoreData.BgmFlags);
        });
    }
}

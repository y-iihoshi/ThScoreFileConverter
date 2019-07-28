using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    using SQOT = ThScoreFileConverter.Squirrel.SQObjectType;

    [TestClass]
    public class Th145AllScoreDataTests
    {
        internal struct Properties
        {
            public int storyProgress;
            public Dictionary<Th145Converter.Chara, Th145Converter.LevelFlag> storyClearFlags;
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
            public Dictionary<Th145Converter.Level, Dictionary<Th145Converter.Chara, int>> clearRanks;
            public Dictionary<Th145Converter.Level, Dictionary<Th145Converter.Chara, int>> clearTimes;
        };

        internal static Properties GetValidProperties()
        {
            var charas = Utils.GetEnumerator<Th145Converter.Chara>();
            var levels = Utils.GetEnumerator<Th145Converter.Level>();

            return new Properties()
            {
                storyProgress = 1,
                storyClearFlags = charas.ToDictionary(
                    chara => chara, chara => TestUtils.Cast<Th145Converter.LevelFlag>(30 - (int)chara)),
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
                        chara => chara, chara => (int)level * 100 + (int)chara)),
                clearTimes = levels.ToDictionary(
                    level => level, level => charas.ToDictionary(
                        chara => chara, chara => (int)chara * 100 + (int)level))
            };
        }

        internal static byte[] MakeByteArray(in Properties properties)
            => new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
                .Concat(TestUtils.MakeSQByteArray(
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
                            perCharaPair => perCharaPair.Value).ToArray()).ToArray()))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray();

        internal static void Validate(in Th145AllScoreDataWrapper allScoreData, in Properties properties)
        {
            Assert.AreEqual(properties.storyProgress, allScoreData.StoryProgress);
            CollectionAssert.AreEqual(properties.storyClearFlags.Keys, allScoreData.StoryClearFlags.Keys.ToArray());
            CollectionAssert.AreEqual(properties.storyClearFlags.Values, allScoreData.StoryClearFlags.Values.ToArray());
            Assert.AreEqual(properties.endingCount, allScoreData.EndingCount);
            Assert.AreEqual(properties.ending2Count, allScoreData.Ending2Count);
            Assert.AreEqual(properties.isEnabledStageTanuki1, allScoreData.IsEnabledStageTanuki1);
            Assert.AreEqual(properties.isEnabledStageTanuki2, allScoreData.IsEnabledStageTanuki2);
            Assert.AreEqual(properties.isEnabledStageKokoro, allScoreData.IsEnabledStageKokoro);
            Assert.AreEqual(properties.isEnabledSt27, allScoreData.IsEnabledSt27);
            Assert.AreEqual(properties.isEnabledSt28, allScoreData.IsEnabledSt28);
            Assert.AreEqual(properties.isPlayableMamizou, allScoreData.IsPlayableMamizou);
            Assert.AreEqual(properties.isPlayableKokoro, allScoreData.IsPlayableKokoro);
            CollectionAssert.AreEqual(properties.bgmFlags.Keys, allScoreData.BgmFlags.Keys.ToArray());
            CollectionAssert.AreEqual(properties.bgmFlags.Values, allScoreData.BgmFlags.Values.ToArray());
            CollectionAssert.AreEqual(properties.clearRanks.Keys, allScoreData.ClearRanks.Keys.ToArray());

            foreach (var pair in properties.clearRanks)
            {
                CollectionAssert.AreEqual(pair.Value.Keys, allScoreData.ClearRanks[pair.Key].Keys);
                CollectionAssert.AreEqual(pair.Value.Values, allScoreData.ClearRanks[pair.Key].Values);
            }

            CollectionAssert.AreEqual(properties.clearTimes.Keys, allScoreData.ClearTimes.Keys.ToArray());

            foreach (var pair in properties.clearTimes)
            {
                CollectionAssert.AreEqual(pair.Value.Keys, allScoreData.ClearTimes[pair.Key].Keys);
                CollectionAssert.AreEqual(pair.Value.Values, allScoreData.ClearTimes[pair.Key].Values);
            }
        }

        [TestMethod]
        public void Th145AllScoreDataTest() => TestUtils.Wrap(() =>
        {
            var allScoreData = new Th145AllScoreDataWrapper();

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
        public void Th145AllScoreDataReadFromTest() => TestUtils.Wrap(() =>
        {
            var properties = GetValidProperties();

            var allScoreData = Th145AllScoreDataWrapper.Create(MakeByteArray(properties));

            Validate(allScoreData, properties);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th145AllScoreDataReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var allScoreData = new Th145AllScoreDataWrapper();
            allScoreData.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th145AllScoreDataReadFromTestEmpty() => TestUtils.Wrap(() =>
        {
            Th145AllScoreDataWrapper.Create(new byte[0]);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void Th145AllScoreDataReadFromTestNoKey() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th145AllScoreDataWrapper.Create(TestUtils.MakeByteArray((int)SQOT.Null));

            Assert.AreEqual(default, allScoreData.StoryProgress.Value);
            Assert.IsNull(allScoreData.StoryClearFlags);
            Assert.AreEqual(default, allScoreData.EndingCount.Value);
            Assert.AreEqual(default, allScoreData.Ending2Count.Value);
            Assert.AreEqual(default, allScoreData.IsEnabledStageTanuki1.Value);
            Assert.AreEqual(default, allScoreData.IsEnabledStageTanuki2.Value);
            Assert.AreEqual(default, allScoreData.IsEnabledStageKokoro.Value);
            Assert.AreEqual(default, allScoreData.IsEnabledSt27.Value);
            Assert.AreEqual(default, allScoreData.IsEnabledSt28.Value);
            Assert.AreEqual(default, allScoreData.IsPlayableMamizou.Value);
            Assert.AreEqual(default, allScoreData.IsPlayableKokoro.Value);
            Assert.IsNull(allScoreData.BgmFlags);
            Assert.IsNull(allScoreData.ClearRanks);
            Assert.IsNull(allScoreData.ClearTimes);
        });

        [TestMethod]
        public void Th145AllScoreDataReadFromTestNoTables() => TestUtils.Wrap(() =>
        {
            var storyProgressValue = 1;

            var allScoreData = Th145AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table)
                .Concat(TestUtils.MakeSQByteArray("story_progress", storyProgressValue))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.AreEqual(storyProgressValue, allScoreData.StoryProgress);
            Assert.IsNull(allScoreData.StoryClearFlags);
            Assert.IsNull(allScoreData.BgmFlags);
            Assert.IsNull(allScoreData.ClearRanks);
            Assert.IsNull(allScoreData.ClearTimes);
        });

        [TestMethod]
        public void Th145AllScoreDataReadFromTestInvalidStoryClear() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th145AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table)
                .Concat(TestUtils.MakeSQByteArray("story_clear", 1))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNull(allScoreData.StoryClearFlags);
        });

        [TestMethod]
        public void Th145AllScoreDataReadFromTestInvalidStoryClearValue() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th145AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table)
                .Concat(TestUtils.MakeSQByteArray("story_clear", new float[] { 123f }))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNotNull(allScoreData.StoryClearFlags);
            Assert.AreEqual(0, allScoreData.StoryClearFlags.Count);
        });

        [TestMethod]
        public void Th145AllScoreDataReadFromTestInvalidEnableBgm() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th145AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table)
                .Concat(TestUtils.MakeSQByteArray("enable_bgm", 1))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNull(allScoreData.BgmFlags);
        });

        [TestMethod]
        public void Th145AllScoreDataReadFromTestInvalidClearRank() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th145AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table)
                .Concat(TestUtils.MakeSQByteArray("clear_rank", 1))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNull(allScoreData.ClearRanks);
        });

        [TestMethod]
        public void Th145AllScoreDataReadFromTestInvalidClearRankValuePerLevel() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th145AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table)
                .Concat(TestUtils.MakeSQByteArray("clear_rank", new float[] { 123f }))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNotNull(allScoreData.ClearRanks);
            Assert.AreEqual(0, allScoreData.ClearRanks.Count);
        });

        [TestMethod]
        public void Th145AllScoreDataReadFromTestInvalidClearRankValuePerChara() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th145AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table)
                .Concat(TestUtils.MakeSQByteArray("clear_rank", new float[][] { new float[] { 123f } }))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNotNull(allScoreData.ClearRanks);
            Assert.AreEqual(1, allScoreData.ClearRanks.Count);
            Assert.IsNotNull(allScoreData.ClearRanks.First().Value);
            Assert.AreEqual(0, allScoreData.ClearRanks.First().Value.Count);
        });

        [TestMethod]
        public void Th145AllScoreDataReadFromTestInvalidClearTime() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th145AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table)
                .Concat(TestUtils.MakeSQByteArray("clear_time", 1))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNull(allScoreData.ClearTimes);
        });

        [TestMethod]
        public void Th145AllScoreDataReadFromTestInvalidClearTimeValuePerLevel() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th145AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table)
                .Concat(TestUtils.MakeSQByteArray("clear_time", new float[] { 123f }))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNotNull(allScoreData.ClearTimes);
            Assert.AreEqual(0, allScoreData.ClearTimes.Count);
        });

        [TestMethod]
        public void Th145AllScoreDataReadFromTestInvalidClearTimeValuePerChara() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th145AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table)
                .Concat(TestUtils.MakeSQByteArray("clear_time", new float[][] { new float[] { 123f } }))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNotNull(allScoreData.ClearTimes);
            Assert.AreEqual(1, allScoreData.ClearTimes.Count);
            Assert.IsNotNull(allScoreData.ClearTimes.First().Value);
            Assert.AreEqual(0, allScoreData.ClearTimes.First().Value.Count);
        });
    }
}

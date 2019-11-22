using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th135;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    using SQOT = ThScoreFileConverter.Squirrel.SQObjectType;

    [TestClass]
    public class Th135AllScoreDataTests
    {
        internal struct Properties
        {
            public int storyProgress;
            public Dictionary<Chara, LevelFlags> storyClearFlags;
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
            storyClearFlags = Utils.GetEnumerator<Chara>().ToDictionary(
                chara => chara, chara => TestUtils.Cast<LevelFlags>(30 - (int)chara)),
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
                    "enable_mamizou", properties.isPlayableMamizou,
                    "enable_kokoro", properties.isPlayableKokoro,
                    "enable_bgm", properties.bgmFlags))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray();

        internal static void Validate(in Th135AllScoreDataWrapper allScoreData, in Properties properties)
        {
            Assert.AreEqual(properties.storyProgress, allScoreData.StoryProgress);
            CollectionAssert.That.AreEqual(properties.storyClearFlags.Keys, allScoreData.StoryClearFlags.Keys);
            CollectionAssert.That.AreEqual(properties.storyClearFlags.Values, allScoreData.StoryClearFlags.Values);
            Assert.AreEqual(properties.endingCount, allScoreData.EndingCount);
            Assert.AreEqual(properties.ending2Count, allScoreData.Ending2Count);
            Assert.AreEqual(properties.isEnabledStageTanuki1, allScoreData.IsEnabledStageTanuki1);
            Assert.AreEqual(properties.isEnabledStageTanuki2, allScoreData.IsEnabledStageTanuki2);
            Assert.AreEqual(properties.isEnabledStageKokoro, allScoreData.IsEnabledStageKokoro);
            Assert.AreEqual(properties.isPlayableMamizou, allScoreData.IsPlayableMamizou);
            Assert.AreEqual(properties.isPlayableKokoro, allScoreData.IsPlayableKokoro);
            CollectionAssert.That.AreEqual(properties.bgmFlags.Keys, allScoreData.BgmFlags.Keys);
            CollectionAssert.That.AreEqual(properties.bgmFlags.Values, allScoreData.BgmFlags.Values);
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
            var allScoreData = Th135AllScoreDataWrapper.Create(TestUtils.MakeByteArray((int)SQOT.Null));

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
            var storyProgressValue = 1;

            var allScoreData = Th135AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table)
                .Concat(TestUtils.MakeSQByteArray("story_progress", storyProgressValue))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.AreEqual(storyProgressValue, allScoreData.StoryProgress);
            Assert.IsNull(allScoreData.StoryClearFlags);
            Assert.IsNull(allScoreData.BgmFlags);
        });

        [TestMethod]
        public void Th135AllScoreDataReadFromTestInvalidStoryClear() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th135AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table)
                .Concat(TestUtils.MakeSQByteArray("story_clear", 1))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNull(allScoreData.StoryClearFlags);
        });

        [TestMethod]
        public void Th135AllScoreDataReadFromTestInvalidStoryClearValue() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th135AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table)
                .Concat(TestUtils.MakeSQByteArray("story_clear", new float[] { 123f }))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNotNull(allScoreData.StoryClearFlags);
            Assert.AreEqual(0, allScoreData.StoryClearFlags.Count);
        });

        [TestMethod]
        public void Th135AllScoreDataReadFromTestInvalidEnableBgm() => TestUtils.Wrap(() =>
        {
            var allScoreData = Th135AllScoreDataWrapper.Create(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table)
                .Concat(TestUtils.MakeSQByteArray("enable_bgm", 1))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNull(allScoreData.BgmFlags);
        });
    }
}

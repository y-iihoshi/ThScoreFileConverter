using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th09;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th06.Wrappers;
using ThScoreFileConverterTests.Models.Th09.Stubs;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th09PlayStatusTests
    {
        internal struct Properties
        {
            public string signature;
            public short size1;
            public short size2;
            public Time totalRunningTime;
            public Time totalPlayTime;
            public byte[] bgmFlags;
            public Dictionary<Th09Converter.Chara, byte> matchFlags;
            public Dictionary<Th09Converter.Chara, byte> storyFlags;
            public Dictionary<Th09Converter.Chara, byte> extraFlags;
            public Dictionary<Th09Converter.Chara, IClearCount> clearCounts;
        };

        internal static Properties ValidProperties => new Properties()
        {
            signature = "PLST",
            size1 = 0x1FC,
            size2 = 0x1FC,
            totalRunningTime = new Time(12, 34, 56, 789, false),
            totalPlayTime = new Time(23, 45, 19, 876, false),
            bgmFlags = TestUtils.MakeRandomArray<byte>(19),
            matchFlags = Utils.GetEnumerator<Th09Converter.Chara>()
                .Select((chara, index) => new { chara, index })
                .ToDictionary(pair => pair.chara, pair => (byte)pair.index),
            storyFlags = Utils.GetEnumerator<Th09Converter.Chara>()
                .Select((chara, index) => new { chara, index })
                .ToDictionary(pair => pair.chara, pair => (byte)(20 + pair.index)),
            extraFlags = Utils.GetEnumerator<Th09Converter.Chara>()
                .Select((chara, index) => new { chara, index })
                .ToDictionary(pair => pair.chara, pair => (byte)(40 + pair.index)),
            clearCounts = Utils.GetEnumerator<Th09Converter.Chara>()
                .ToDictionary(
                    level => level,
                    level => new ClearCountStub(Th09ClearCountTests.ValidStub) as IClearCount)
        };

        internal static byte[] MakeData(in Properties properties)
            => TestUtils.MakeByteArray(
                0u,
                (int)properties.totalRunningTime.Hours,
                properties.totalRunningTime.Minutes,
                properties.totalRunningTime.Seconds,
                properties.totalRunningTime.Milliseconds,
                (int)properties.totalPlayTime.Hours,
                properties.totalPlayTime.Minutes,
                properties.totalPlayTime.Seconds,
                properties.totalPlayTime.Milliseconds,
                properties.bgmFlags,
                new byte[13],
                properties.matchFlags.Values.ToArray(),
                properties.storyFlags.Values.ToArray(),
                properties.extraFlags.Values.ToArray(),
                properties.clearCounts.SelectMany(pair => Th09ClearCountTests.MakeByteArray(pair.Value)).ToArray());

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(), properties.size1, properties.size2, MakeData(properties));

        internal static void Validate(in Th09PlayStatusWrapper playStatus, in Properties properties)
        {
            var data = MakeData(properties);

            Assert.AreEqual(properties.signature, playStatus.Signature);
            Assert.AreEqual(properties.size1, playStatus.Size1);
            Assert.AreEqual(properties.size2, playStatus.Size2);
            CollectionAssert.That.AreEqual(data, playStatus.Data);
            Assert.AreEqual(data[0], playStatus.FirstByteOfData);
            Assert.AreEqual(properties.totalRunningTime.Hours, playStatus.TotalRunningTime.Hours);
            Assert.AreEqual(properties.totalRunningTime.Minutes, playStatus.TotalRunningTime.Minutes);
            Assert.AreEqual(properties.totalRunningTime.Seconds, playStatus.TotalRunningTime.Seconds);
            Assert.AreEqual(properties.totalRunningTime.Milliseconds, playStatus.TotalRunningTime.Milliseconds);
            Assert.IsFalse(playStatus.TotalRunningTime.IsFrames);
            Assert.AreEqual(properties.totalPlayTime.Hours, playStatus.TotalPlayTime.Hours);
            Assert.AreEqual(properties.totalPlayTime.Minutes, playStatus.TotalPlayTime.Minutes);
            Assert.AreEqual(properties.totalPlayTime.Seconds, playStatus.TotalPlayTime.Seconds);
            Assert.AreEqual(properties.totalPlayTime.Milliseconds, playStatus.TotalPlayTime.Milliseconds);
            Assert.IsFalse(playStatus.TotalPlayTime.IsFrames);
            CollectionAssert.That.AreEqual(properties.bgmFlags, playStatus.BgmFlags);
            CollectionAssert.That.AreEqual(properties.matchFlags.Values, playStatus.MatchFlags.Values);
            CollectionAssert.That.AreEqual(properties.storyFlags.Values, playStatus.StoryFlags.Values);
            CollectionAssert.That.AreEqual(properties.extraFlags.Values, playStatus.ExtraFlags.Values);

            foreach (var key in properties.clearCounts.Keys)
            {
                Th09ClearCountTests.Validate(properties.clearCounts[key], playStatus.ClearCountsItem(key));
            }
        }

        [TestMethod]
        public void Th09PlayStatusTestChapter() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var playStatus = new Th09PlayStatusWrapper(chapter);

            Validate(playStatus, properties);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "playStatus")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th09PlayStatusTestNullChapter() => TestUtils.Wrap(() =>
        {
            var playStatus = new Th09PlayStatusWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "playStatus")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th09PlayStatusTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = properties.signature.ToLowerInvariant();

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var playStatus = new Th09PlayStatusWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "playStatus")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th09PlayStatusTestInvalidSize1() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            --properties.size1;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var playStatus = new Th09PlayStatusWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}

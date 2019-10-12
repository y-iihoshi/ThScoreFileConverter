using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th07;
using ThScoreFileConverterTests.Models.Th06.Wrappers;
using LevelWithTotal = ThScoreFileConverter.Models.Th07.LevelWithTotal;

namespace ThScoreFileConverterTests.Models.Th07
{
    [TestClass]
    public class PlayStatusTests
    {
        internal struct Properties
        {
            public string signature;
            public short size1;
            public short size2;
            public Time totalRunningTime;
            public Time totalPlayTime;
            public Dictionary<LevelWithTotal, PlayCountTests.Properties> playCounts;
        };

        internal static Properties ValidProperties { get; } = new Properties()
        {
            signature = "PLST",
            size1 = 0x160,
            size2 = 0x160,
            totalRunningTime = new Time(12, 34, 56, 789, false),
            totalPlayTime = new Time(23, 45, 19, 876, false),
            playCounts = Utils.GetEnumerator<LevelWithTotal>()
                .ToDictionary(
                    level => level,
                    level => new PlayCountTests.Properties(PlayCountTests.ValidProperties))
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
                properties.playCounts.SelectMany(pair => PlayCountTests.MakeByteArray(pair.Value)).ToArray());

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(), properties.size1, properties.size2, MakeData(properties));

        internal static void Validate(in PlayStatus playStatus, in Properties properties)
        {
            var data = MakeData(properties);

            Assert.AreEqual(properties.signature, playStatus.Signature);
            Assert.AreEqual(properties.size1, playStatus.Size1);
            Assert.AreEqual(properties.size2, playStatus.Size2);
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

            foreach (var key in properties.playCounts.Keys)
            {
                PlayCountTests.Validate(playStatus.PlayCounts[key], properties.playCounts[key]);
            }
        }

        [TestMethod]
        public void PlayStatusTestChapter() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var playStatus = new PlayStatus(chapter.Target);

            Validate(playStatus, properties);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PlayStatusTestNullChapter() => TestUtils.Wrap(() =>
        {
            _ = new PlayStatus(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void PlayStatusTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = properties.signature.ToLowerInvariant();

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            _ = new PlayStatus(chapter.Target);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void PlayStatusTestInvalidSize1() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            --properties.size1;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            _ = new PlayStatus(chapter.Target);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th07;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;
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
        }

        internal static Properties ValidProperties { get; } = new Properties()
        {
            signature = "PLST",
            size1 = 0x160,
            size2 = 0x160,
            totalRunningTime = new Time(12, 34, 56, 789, false),
            totalPlayTime = new Time(23, 45, 19, 876, false),
            playCounts = EnumHelper.GetEnumerable<LevelWithTotal>()
                .ToDictionary(
                    level => level,
                    level => new PlayCountTests.Properties(PlayCountTests.ValidProperties)),
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

        internal static void Validate(in Properties expected, in PlayStatus actual)
        {
            var data = MakeData(expected);

            Assert.AreEqual(expected.signature, actual.Signature);
            Assert.AreEqual(expected.size1, actual.Size1);
            Assert.AreEqual(expected.size2, actual.Size2);
            Assert.AreEqual(data[0], actual.FirstByteOfData);
            Assert.AreEqual(expected.totalRunningTime.Hours, actual.TotalRunningTime.Hours);
            Assert.AreEqual(expected.totalRunningTime.Minutes, actual.TotalRunningTime.Minutes);
            Assert.AreEqual(expected.totalRunningTime.Seconds, actual.TotalRunningTime.Seconds);
            Assert.AreEqual(expected.totalRunningTime.Milliseconds, actual.TotalRunningTime.Milliseconds);
            Assert.IsFalse(actual.TotalRunningTime.IsFrames);
            Assert.AreEqual(expected.totalPlayTime.Hours, actual.TotalPlayTime.Hours);
            Assert.AreEqual(expected.totalPlayTime.Minutes, actual.TotalPlayTime.Minutes);
            Assert.AreEqual(expected.totalPlayTime.Seconds, actual.TotalPlayTime.Seconds);
            Assert.AreEqual(expected.totalPlayTime.Milliseconds, actual.TotalPlayTime.Milliseconds);
            Assert.IsFalse(actual.TotalPlayTime.IsFrames);

            foreach (var key in expected.playCounts.Keys)
            {
                PlayCountTests.Validate(actual.PlayCounts[key], expected.playCounts[key]);
            }
        }

        [TestMethod]
        public void PlayStatusTestChapter()
        {
            var properties = ValidProperties;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(properties));
            var playStatus = new PlayStatus(chapter);

            Validate(properties, playStatus);
        }

        [TestMethod]
        public void PlayStatusTestNullChapter()
            => _ = Assert.ThrowsException<ArgumentNullException>(() => _ = new PlayStatus(null!));

        [TestMethod]
        public void PlayStatusTestInvalidSignature()
        {
            var properties = ValidProperties;
            properties.signature = properties.signature.ToLowerInvariant();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(properties));
            _ = Assert.ThrowsException<InvalidDataException>(() => _ = new PlayStatus(chapter));
        }

        [TestMethod]
        public void PlayStatusTestInvalidSize1()
        {
            var properties = ValidProperties;
            --properties.size1;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(properties));
            _ = Assert.ThrowsException<InvalidDataException>(() => _ = new PlayStatus(chapter));
        }
    }
}

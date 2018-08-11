using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th07PlayStatusTests
    {
        internal struct Properties
        {
            public string signature;
            public short size1;
            public short size2;
            public Time totalRunningTime;
            public Time totalPlayTime;
            public Dictionary<Th07Converter.LevelWithTotal, Th07PlayCountTests.Properties> playCounts;
        };

        internal static Properties ValidProperties => new Properties()
        {
            signature = "PLST",
            size1 = 0x160,
            size2 = 0x160,
            totalRunningTime = new Time(12, 34, 56, 789, false),
            totalPlayTime = new Time(23, 45, 19, 876, false),
            playCounts = Utils.GetEnumerator<Th07Converter.LevelWithTotal>()
                .ToDictionary(
                    level => level,
                    level => new Th07PlayCountTests.Properties(Th07PlayCountTests.ValidProperties))
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
                properties.playCounts.SelectMany(pair => Th07PlayCountTests.MakeByteArray(pair.Value)).ToArray());

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(), properties.size1, properties.size2, MakeData(properties));

        internal static void Validate(in Th07PlayStatusWrapper playStatus, in Properties properties)
        {
            var data = MakeData(properties);

            Assert.AreEqual(properties.signature, playStatus.Signature);
            Assert.AreEqual(properties.size1, playStatus.Size1);
            Assert.AreEqual(properties.size2, playStatus.Size2);
            CollectionAssert.AreEqual(data, playStatus.Data.ToArray());
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
                Th07PlayCountTests.Validate(playStatus.PlayCountsItem(key), properties.playCounts[key]);
            }
        }

        [TestMethod]
        public void Th07PlayStatusTestChapter() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var chapter = Th06ChapterWrapper<Th07Converter>.Create(MakeByteArray(properties));
            var playStatus = new Th07PlayStatusWrapper(chapter);

            Validate(playStatus, properties);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "playStatus")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th07PlayStatusTestNullChapter() => TestUtils.Wrap(() =>
        {
            var playStatus = new Th07PlayStatusWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "playStatus")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th07PlayStatusTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = properties.signature.ToLowerInvariant();

            var chapter = Th06ChapterWrapper<Th07Converter>.Create(MakeByteArray(properties));
            var playStatus = new Th07PlayStatusWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "playStatus")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th07PlayStatusTestInvalidSize1() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            ++properties.size1;

            var chapter = Th06ChapterWrapper<Th07Converter>.Create(MakeByteArray(properties));
            var playStatus = new Th07PlayStatusWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}

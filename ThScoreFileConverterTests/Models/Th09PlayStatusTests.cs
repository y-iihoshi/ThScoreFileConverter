using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass()]
    public class Th09PlayStatusTests
    {
        [TestMethod()]
        public void Th09PlayStatusTestChapter() => TestUtils.Wrap(() =>
        {
            var signature = "PLST";
            short size1 = 0x1FC;
            short size2 = 0x1FC;
            var unknown1 = 1u;
            var totalRunningTime = new Time(12, 34, 56, 789, false);
            var totalPlayTime = new Time(23, 45, 19, 876, false);
            var bgmFlags = TestUtils.MakeRandomArray<byte>(19);
            var unknown2 = TestUtils.MakeRandomArray<byte>(13);
            var matchFlags = TestUtils.MakeRandomArray<byte>(16);
            var storyFlags = TestUtils.MakeRandomArray<byte>(16);
            var extraFlags = TestUtils.MakeRandomArray<byte>(16);
            var countsArray = new int[16][];
            var unknown3s = new uint[16];
            var clearCounts = new List<byte>();

            foreach (var index in Enumerable.Range(0, 16))
            {
                countsArray[index] = TestUtils.MakeRandomArray<int>(5);
                unknown3s[index] = 1u + (uint)index;
                clearCounts.AddRange(TestUtils.MakeByteArray(countsArray[index], unknown3s[index]));
            }

            var data = TestUtils.MakeByteArray(
                unknown1,
                (int)totalRunningTime.Hours,
                totalRunningTime.Minutes,
                totalRunningTime.Seconds,
                totalRunningTime.Milliseconds,
                (int)totalPlayTime.Hours,
                totalPlayTime.Minutes,
                totalPlayTime.Seconds,
                totalPlayTime.Milliseconds,
                bgmFlags,
                unknown2,
                matchFlags,
                storyFlags,
                extraFlags,
                clearCounts.ToArray());

            var chapter = Th06ChapterWrapper<Th09Converter>.Create(
                TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
            var playStatus = new Th09PlayStatusWrapper(chapter);

            Assert.AreEqual(signature, playStatus.Signature);
            Assert.AreEqual(size1, playStatus.Size1);
            Assert.AreEqual(size2, playStatus.Size2);
            CollectionAssert.AreEqual(data, playStatus.Data.ToArray());
            Assert.AreEqual(data[0], playStatus.FirstByteOfData);
            Assert.AreEqual(totalRunningTime.Hours, playStatus.TotalRunningTime.Hours);
            Assert.AreEqual(totalRunningTime.Minutes, playStatus.TotalRunningTime.Minutes);
            Assert.AreEqual(totalRunningTime.Seconds, playStatus.TotalRunningTime.Seconds);
            Assert.AreEqual(totalRunningTime.Milliseconds, playStatus.TotalRunningTime.Milliseconds);
            Assert.IsFalse(playStatus.TotalRunningTime.IsFrames);
            Assert.AreEqual(totalPlayTime.Hours, playStatus.TotalPlayTime.Hours);
            Assert.AreEqual(totalPlayTime.Minutes, playStatus.TotalPlayTime.Minutes);
            Assert.AreEqual(totalPlayTime.Seconds, playStatus.TotalPlayTime.Seconds);
            Assert.AreEqual(totalPlayTime.Milliseconds, playStatus.TotalPlayTime.Milliseconds);
            Assert.IsFalse(playStatus.TotalPlayTime.IsFrames);
            CollectionAssert.AreEqual(bgmFlags, playStatus.BgmFlags.ToArray());
            CollectionAssert.AreEqual(matchFlags, playStatus.MatchFlags.Values.ToArray());
            CollectionAssert.AreEqual(storyFlags, playStatus.StoryFlags.Values.ToArray());
            CollectionAssert.AreEqual(extraFlags, playStatus.ExtraFlags.Values.ToArray());

            foreach (var index in Enumerable.Range(0, 16))
            {
                var clearCount = playStatus.ClearCountsItem((Th09Converter.Chara)index);
                CollectionAssert.AreEqual(countsArray[index], clearCount.Counts.Values.ToArray());
            }
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "playStatus")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th09PlayStatusTestNullChapter() => TestUtils.Wrap(() =>
        {
            var playStatus = new Th09PlayStatusWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "playStatus")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th09PlayStatusTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var signature = "plst";
            short size1 = 0x1FC;
            short size2 = 0x1FC;
            var unknown1 = 1u;
            var totalRunningTime = new Time(12, 34, 56, 789, false);
            var totalPlayTime = new Time(23, 45, 19, 876, false);
            var bgmFlags = TestUtils.MakeRandomArray<byte>(19);
            var unknown2 = TestUtils.MakeRandomArray<byte>(13);
            var matchFlags = TestUtils.MakeRandomArray<byte>(16);
            var storyFlags = TestUtils.MakeRandomArray<byte>(16);
            var extraFlags = TestUtils.MakeRandomArray<byte>(16);
            var countsArray = new int[16][];
            var unknown3s = new uint[16];
            var clearCounts = new List<byte>();

            foreach (var index in Enumerable.Range(0, 16))
            {
                countsArray[index] = TestUtils.MakeRandomArray<int>(5);
                unknown3s[index] = 1u + (uint)index;
                clearCounts.AddRange(TestUtils.MakeByteArray(countsArray[index], unknown3s[index]));
            }

            var data = TestUtils.MakeByteArray(
                unknown1,
                (int)totalRunningTime.Hours,
                totalRunningTime.Minutes,
                totalRunningTime.Seconds,
                totalRunningTime.Milliseconds,
                (int)totalPlayTime.Hours,
                totalPlayTime.Minutes,
                totalPlayTime.Seconds,
                totalPlayTime.Milliseconds,
                bgmFlags,
                unknown2,
                matchFlags,
                storyFlags,
                extraFlags,
                clearCounts.ToArray());

            var chapter = Th06ChapterWrapper<Th09Converter>.Create(
                TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
            var playStatus = new Th09PlayStatusWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "playStatus")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th09PlayStatusTestInvalidSize1() => TestUtils.Wrap(() =>
        {
            var signature = "PLST";
            short size1 = 0x1FD;
            short size2 = 0x1FC;
            var unknown1 = 1u;
            var totalRunningTime = new Time(12, 34, 56, 789, false);
            var totalPlayTime = new Time(23, 45, 19, 876, false);
            var bgmFlags = TestUtils.MakeRandomArray<byte>(19);
            var unknown2 = TestUtils.MakeRandomArray<byte>(13);
            var matchFlags = TestUtils.MakeRandomArray<byte>(16);
            var storyFlags = TestUtils.MakeRandomArray<byte>(16);
            var extraFlags = TestUtils.MakeRandomArray<byte>(16);
            var countsArray = new int[16][];
            var unknown3s = new uint[16];
            var clearCounts = new List<byte>();

            foreach (var index in Enumerable.Range(0, 16))
            {
                countsArray[index] = TestUtils.MakeRandomArray<int>(5);
                unknown3s[index] = 1u + (uint)index;
                clearCounts.AddRange(TestUtils.MakeByteArray(countsArray[index], unknown3s[index]));
            }

            var data = TestUtils.MakeByteArray(
                unknown1,
                (int)totalRunningTime.Hours,
                totalRunningTime.Minutes,
                totalRunningTime.Seconds,
                totalRunningTime.Milliseconds,
                (int)totalPlayTime.Hours,
                totalPlayTime.Minutes,
                totalPlayTime.Seconds,
                totalPlayTime.Milliseconds,
                bgmFlags,
                unknown2,
                matchFlags,
                storyFlags,
                extraFlags,
                clearCounts.ToArray());

            var chapter = Th06ChapterWrapper<Th09Converter>.Create(
                TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
            var playStatus = new Th09PlayStatusWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}

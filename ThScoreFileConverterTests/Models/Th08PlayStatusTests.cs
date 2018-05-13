using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace ThScoreFileConverter.Models.Tests
{
    [TestClass()]
    public class Th08PlayStatusTests
    {
        [TestMethod()]
        public void Th08PlayStatusTestChapter()
        {
            try
            {
                var signature = "PLST";
                short size1 = 0x228;
                short size2 = 0x228;
                var unknown1 = 1u;
                var totalRunningTime = new Time(12, 34, 56, 789, false);
                var totalPlayTime = new Time(23, 45, 19, 876, false);
                var totalTrials = new int[7];
                var trialsArray = new int[7][];
                var unknown2s = new uint[7];
                var totalClears = new int[7];
                var totalContinues = new int[7];
                var totalPractices = new int[7];
                var playCounts = new List<byte>();

                foreach (var index in Enumerable.Range(0, 7))
                {
                    totalTrials[index] = 1 + index;
                    trialsArray[index] = TestUtils.MakeRandomArray<int>(12);
                    unknown2s[index] = 10u + (uint)index;
                    totalClears[index] = 100 + index;
                    totalContinues[index] = 1000 + index;
                    totalPractices[index] = 10000 + index;
                    playCounts.AddRange(
                        TestUtils.MakeByteArray(
                            totalTrials[index],
                            trialsArray[index],
                            unknown2s[index],
                            totalClears[index],
                            totalContinues[index],
                            totalPractices[index]));
                }

                var bgmFlags = TestUtils.MakeRandomArray<byte>(21);
                var unknown3 = TestUtils.MakeRandomArray<byte>(11);
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
                    playCounts.ToArray(),
                    bgmFlags,
                    unknown3);

                var chapter = Th06ChapterWrapper<Th08Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var playStatus = new Th08PlayStatusWrapper(chapter);

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

                foreach (var index in Enumerable.Range(0, 5))
                {
                    var playCount = playStatus.PlayCountsItem((ThConverter.Level)index);
                    Assert.AreEqual(totalTrials[index], playCount.TotalTrial.Value);
                    CollectionAssert.AreEqual(trialsArray[index], playCount.Trials.Values.ToArray());
                    Assert.AreEqual(totalClears[index], playCount.TotalClear.Value);
                    Assert.AreEqual(totalContinues[index], playCount.TotalContinue.Value);
                    Assert.AreEqual(totalPractices[index], playCount.TotalPractice.Value);
                }

                Assert.AreEqual(totalTrials[6], playStatus.TotalPlayCount.TotalTrial.Value);
                CollectionAssert.AreEqual(trialsArray[6], playStatus.TotalPlayCount.Trials.Values.ToArray());
                Assert.AreEqual(totalClears[6], playStatus.TotalPlayCount.TotalClear.Value);
                Assert.AreEqual(totalContinues[6], playStatus.TotalPlayCount.TotalContinue.Value);
                Assert.AreEqual(totalPractices[6], playStatus.TotalPlayCount.TotalPractice.Value);
                CollectionAssert.AreEqual(bgmFlags, playStatus.BgmFlags.ToArray());
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "playStatus")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th08PlayStatusTestNullChapter()
        {
            try
            {
                var playStatus = new Th08PlayStatusWrapper(null);

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "playStatus")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08PlayStatusTestInvalidSignature()
        {
            try
            {
                var signature = "plst";
                short size1 = 0x228;
                short size2 = 0x228;
                var unknown1 = 1u;
                var totalRunningTime = new Time(12, 34, 56, 789, false);
                var totalPlayTime = new Time(23, 45, 19, 876, false);
                var totalTrials = new int[7];
                var trialsArray = new int[7][];
                var unknown2s = new uint[7];
                var totalClears = new int[7];
                var totalContinues = new int[7];
                var totalPractices = new int[7];
                var playCounts = new List<byte>();

                foreach (var index in Enumerable.Range(0, 7))
                {
                    totalTrials[index] = 1 + index;
                    trialsArray[index] = TestUtils.MakeRandomArray<int>(6);
                    unknown2s[index] = 10u + (uint)index;
                    totalClears[index] = 100 + index;
                    totalContinues[index] = 1000 + index;
                    totalPractices[index] = 10000 + index;
                    playCounts.AddRange(
                        TestUtils.MakeByteArray(
                            totalTrials[index],
                            trialsArray[index],
                            unknown2s[index],
                            totalClears[index],
                            totalContinues[index],
                            totalPractices[index]));
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
                    playCounts.ToArray());

                var chapter = Th06ChapterWrapper<Th08Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var playStatus = new Th08PlayStatusWrapper(chapter);

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "playStatus")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08PlayStatusTestInvalidSize1()
        {
            try
            {
                var signature = "PLST";
                short size1 = 0x229;
                short size2 = 0x228;
                var unknown1 = 1u;
                var totalRunningTime = new Time(12, 34, 56, 789, false);
                var totalPlayTime = new Time(23, 45, 19, 876, false);
                var totalTrials = new int[7];
                var trialsArray = new int[7][];
                var unknown2s = new uint[7];
                var totalClears = new int[7];
                var totalContinues = new int[7];
                var totalPractices = new int[7];
                var playCounts = new List<byte>();

                foreach (var index in Enumerable.Range(0, 7))
                {
                    totalTrials[index] = 1 + index;
                    trialsArray[index] = TestUtils.MakeRandomArray<int>(6);
                    unknown2s[index] = 10u + (uint)index;
                    totalClears[index] = 100 + index;
                    totalContinues[index] = 1000 + index;
                    totalPractices[index] = 10000 + index;
                    playCounts.AddRange(
                        TestUtils.MakeByteArray(
                            totalTrials[index],
                            trialsArray[index],
                            unknown2s[index],
                            totalClears[index],
                            totalContinues[index],
                            totalPractices[index]));
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
                    playCounts.ToArray());

                var chapter = Th06ChapterWrapper<Th08Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var playStatus = new Th08PlayStatusWrapper(chapter);

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }
    }
}

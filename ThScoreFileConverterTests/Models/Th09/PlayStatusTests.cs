using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th09;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th09.Stubs;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;

namespace ThScoreFileConverterTests.Models.Th09
{
    [TestClass]
    public class PlayStatusTests
    {
        internal static PlayStatusStub ValidStub { get; } = new PlayStatusStub()
        {
            Signature = "PLST",
            Size1 = 0x1FC,
            Size2 = 0x1FC,
            TotalRunningTime = new Time(12, 34, 56, 789, false),
            TotalPlayTime = new Time(23, 45, 19, 876, false),
            BgmFlags = TestUtils.MakeRandomArray<byte>(19),
            MatchFlags = Utils.GetEnumerator<Chara>()
                .Select((chara, index) => new { chara, index })
                .ToDictionary(pair => pair.chara, pair => (byte)pair.index),
            StoryFlags = Utils.GetEnumerator<Chara>()
                .Select((chara, index) => new { chara, index })
                .ToDictionary(pair => pair.chara, pair => (byte)(20 + pair.index)),
            ExtraFlags = Utils.GetEnumerator<Chara>()
                .Select((chara, index) => new { chara, index })
                .ToDictionary(pair => pair.chara, pair => (byte)(40 + pair.index)),
            ClearCounts = Utils.GetEnumerator<Chara>()
                .ToDictionary(
                    level => level,
                    level => new ClearCountStub(ClearCountTests.ValidStub) as IClearCount)
        };

        internal static byte[] MakeByteArray(IPlayStatus playStatus)
            => TestUtils.MakeByteArray(
                playStatus.Signature.ToCharArray(),
                playStatus.Size1,
                playStatus.Size2,
                0u,
                (int)playStatus.TotalRunningTime.Hours,
                playStatus.TotalRunningTime.Minutes,
                playStatus.TotalRunningTime.Seconds,
                playStatus.TotalRunningTime.Milliseconds,
                (int)playStatus.TotalPlayTime.Hours,
                playStatus.TotalPlayTime.Minutes,
                playStatus.TotalPlayTime.Seconds,
                playStatus.TotalPlayTime.Milliseconds,
                playStatus.BgmFlags,
                new byte[13],
                playStatus.MatchFlags.Values.ToArray(),
                playStatus.StoryFlags.Values.ToArray(),
                playStatus.ExtraFlags.Values.ToArray(),
                playStatus.ClearCounts.SelectMany(pair => ClearCountTests.MakeByteArray(pair.Value)).ToArray());

        internal static void Validate(IPlayStatus expected, IPlayStatus actual)
        {
            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Size1, actual.Size1);
            Assert.AreEqual(expected.Size2, actual.Size2);
            Assert.AreEqual(expected.FirstByteOfData, actual.FirstByteOfData);
            Assert.AreEqual(expected.TotalRunningTime.Hours, actual.TotalRunningTime.Hours);
            Assert.AreEqual(expected.TotalRunningTime.Minutes, actual.TotalRunningTime.Minutes);
            Assert.AreEqual(expected.TotalRunningTime.Seconds, actual.TotalRunningTime.Seconds);
            Assert.AreEqual(expected.TotalRunningTime.Milliseconds, actual.TotalRunningTime.Milliseconds);
            Assert.IsFalse(actual.TotalRunningTime.IsFrames);
            Assert.AreEqual(expected.TotalPlayTime.Hours, actual.TotalPlayTime.Hours);
            Assert.AreEqual(expected.TotalPlayTime.Minutes, actual.TotalPlayTime.Minutes);
            Assert.AreEqual(expected.TotalPlayTime.Seconds, actual.TotalPlayTime.Seconds);
            Assert.AreEqual(expected.TotalPlayTime.Milliseconds, actual.TotalPlayTime.Milliseconds);
            Assert.IsFalse(actual.TotalPlayTime.IsFrames);
            CollectionAssert.That.AreEqual(expected.BgmFlags, actual.BgmFlags);
            CollectionAssert.That.AreEqual(expected.MatchFlags.Values, actual.MatchFlags.Values);
            CollectionAssert.That.AreEqual(expected.StoryFlags.Values, actual.StoryFlags.Values);
            CollectionAssert.That.AreEqual(expected.ExtraFlags.Values, actual.ExtraFlags.Values);

            foreach (var key in expected.ClearCounts.Keys)
            {
                ClearCountTests.Validate(expected.ClearCounts[key], actual.ClearCounts[key]);
            }
        }

        [TestMethod]
        public void PlayStatusTestChapter()
        {
            var stub = ValidStub;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            var playStatus = new PlayStatus(chapter);

            Validate(stub, playStatus);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PlayStatusTestNullChapter()
        {
            _ = new PlayStatus(null!);

            Assert.Fail(TestUtils.Unreachable);
        }

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void PlayStatusTestInvalidSignature()
        {
            var stub = new PlayStatusStub(ValidStub);
            stub.Signature = stub.Signature.ToLowerInvariant();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new PlayStatus(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void PlayStatusTestInvalidSize1()
        {
            var stub = new PlayStatusStub(ValidStub);
            --stub.Size1;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new PlayStatus(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }
    }
}

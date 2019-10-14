using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
        internal static PlayStatusStub ValidStub => new PlayStatusStub()
        {
            Signature = "PLST",
            Size1 = 0x1FC,
            Size2 = 0x1FC,
            TotalRunningTime = new Time(12, 34, 56, 789, false),
            TotalPlayTime = new Time(23, 45, 19, 876, false),
            BgmFlags = TestUtils.MakeRandomArray<byte>(19),
            MatchFlags = Utils.GetEnumerator<Th09Converter.Chara>()
                .Select((chara, index) => new { chara, index })
                .ToDictionary(pair => pair.chara, pair => (byte)pair.index),
            StoryFlags = Utils.GetEnumerator<Th09Converter.Chara>()
                .Select((chara, index) => new { chara, index })
                .ToDictionary(pair => pair.chara, pair => (byte)(20 + pair.index)),
            ExtraFlags = Utils.GetEnumerator<Th09Converter.Chara>()
                .Select((chara, index) => new { chara, index })
                .ToDictionary(pair => pair.chara, pair => (byte)(40 + pair.index)),
            ClearCounts = Utils.GetEnumerator<Th09Converter.Chara>()
                .ToDictionary(
                    level => level,
                    level => new ClearCountStub(Th09ClearCountTests.ValidStub) as IClearCount)
        };

        internal static byte[] MakeData(IPlayStatus playStatus)
            => TestUtils.MakeByteArray(
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
                playStatus.ClearCounts.SelectMany(pair => Th09ClearCountTests.MakeByteArray(pair.Value)).ToArray());

        internal static byte[] MakeByteArray(IPlayStatus playStatus)
            => TestUtils.MakeByteArray(
                playStatus.Signature.ToCharArray(), playStatus.Size1, playStatus.Size2, MakeData(playStatus));

        internal static void Validate(IPlayStatus expected, in Th09PlayStatusWrapper actual)
        {
            var data = MakeData(expected);

            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Size1, actual.Size1);
            Assert.AreEqual(expected.Size2, actual.Size2);
            CollectionAssert.That.AreEqual(data, actual.Data);
            Assert.AreEqual(data[0], actual.FirstByteOfData);
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
                Th09ClearCountTests.Validate(expected.ClearCounts[key], actual.ClearCountsItem(key));
            }
        }

        [TestMethod]
        public void Th09PlayStatusTestChapter() => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            var playStatus = new Th09PlayStatusWrapper(chapter);

            Validate(stub, playStatus);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th09PlayStatusTestNullChapter() => TestUtils.Wrap(() =>
        {
            _ = new Th09PlayStatusWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th09PlayStatusTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var stub = new PlayStatusStub(ValidStub);
            stub.Signature = stub.Signature.ToLowerInvariant();

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th09PlayStatusWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th09PlayStatusTestInvalidSize1() => TestUtils.Wrap(() =>
        {
            var stub = new PlayStatusStub(ValidStub);
            --stub.Size1;

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th09PlayStatusWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}

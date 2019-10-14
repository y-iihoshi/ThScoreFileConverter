using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th08;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th08.Stubs;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;

namespace ThScoreFileConverterTests.Models.Th08
{
    [TestClass]
    public class PlayStatusTests
    {
        internal static PlayStatusStub ValidStub { get; } = new PlayStatusStub()
        {
            Signature = "PLST",
            Size1 = 0x228,
            Size2 = 0x228,
            TotalRunningTime = new Time(12, 34, 56, 789, false),
            TotalPlayTime = new Time(23, 45, 19, 876, false),
            PlayCounts = Utils.GetEnumerator<Level>()
                .ToDictionary(
                    level => level,
                    level => new PlayCountStub(PlayCountTests.ValidStub) as IPlayCount),
            TotalPlayCount = new PlayCountStub(PlayCountTests.ValidStub),
            BgmFlags = TestUtils.MakeRandomArray<byte>(21)
        };

        internal static byte[] MakeByteArray(IPlayStatus status)
            => TestUtils.MakeByteArray(
                status.Signature.ToCharArray(),
                status.Size1,
                status.Size2,
                0u,
                (int)status.TotalRunningTime.Hours,
                status.TotalRunningTime.Minutes,
                status.TotalRunningTime.Seconds,
                status.TotalRunningTime.Milliseconds,
                (int)status.TotalPlayTime.Hours,
                status.TotalPlayTime.Minutes,
                status.TotalPlayTime.Seconds,
                status.TotalPlayTime.Milliseconds,
                status.PlayCounts.SelectMany(pair => PlayCountTests.MakeByteArray(pair.Value)).ToArray(),
                PlayCountTests.MakeByteArray(PlayCountTests.ValidStub),
                PlayCountTests.MakeByteArray(status.TotalPlayCount),
                status.BgmFlags,
                new byte[11]);

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

            foreach (var key in expected.PlayCounts.Keys)
            {
                PlayCountTests.Validate(expected.PlayCounts[key], actual.PlayCounts[key]);
            }

            PlayCountTests.Validate(expected.TotalPlayCount, actual.TotalPlayCount);
            CollectionAssert.That.AreEqual(expected.BgmFlags, actual.BgmFlags);
        }

        [TestMethod]
        public void PlayStatusTestChapter() => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            var playStatus = new PlayStatus(chapter);

            Validate(stub, playStatus);
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
            var stub = new PlayStatusStub(ValidStub);
            stub.Signature = stub.Signature.ToLowerInvariant();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new PlayStatus(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void PlayStatusTestInvalidSize1() => TestUtils.Wrap(() =>
        {
            var stub = new PlayStatusStub(ValidStub);
            --stub.Size1;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new PlayStatus(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}

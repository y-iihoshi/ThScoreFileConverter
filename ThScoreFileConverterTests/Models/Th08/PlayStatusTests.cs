using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th08;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th06.Wrappers;
using ThScoreFileConverterTests.Models.Th08;
using ThScoreFileConverterTests.Models.Th08.Stubs;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th08PlayStatusTests
    {
        internal static PlayStatusStub ValidStub => new PlayStatusStub()
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

        internal static byte[] MakeData(IPlayStatus status)
            => TestUtils.MakeByteArray(
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

        internal static byte[] MakeByteArray(IPlayStatus status)
            => TestUtils.MakeByteArray(
                status.Signature.ToCharArray(), status.Size1, status.Size2, MakeData(status));

        internal static void Validate(IPlayStatus expected, in Th08PlayStatusWrapper actual)
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

            foreach (var key in expected.PlayCounts.Keys)
            {
                PlayCountTests.Validate(expected.PlayCounts[key], actual.PlayCounts[key]);
            }

            PlayCountTests.Validate(expected.TotalPlayCount, actual.TotalPlayCount);
            CollectionAssert.That.AreEqual(expected.BgmFlags, actual.BgmFlags);
        }

        [TestMethod]
        public void Th08PlayStatusTestChapter() => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            var playStatus = new Th08PlayStatusWrapper(chapter);

            Validate(stub, playStatus);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th08PlayStatusTestNullChapter() => TestUtils.Wrap(() =>
        {
            _ = new Th08PlayStatusWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08PlayStatusTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var stub = new PlayStatusStub(ValidStub);
            stub.Signature = stub.Signature.ToLowerInvariant();

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th08PlayStatusWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08PlayStatusTestInvalidSize1() => TestUtils.Wrap(() =>
        {
            var stub = new PlayStatusStub(ValidStub);
            --stub.Size1;

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th08PlayStatusWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}

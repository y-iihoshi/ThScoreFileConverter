using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th16ScoreDataTests
    {
        internal struct Properties
        {
            public uint score;
            public Th16Converter.StageProgress stageProgress;
            public byte continueCount;
            public byte[] name;
            public uint dateTime;
            public float slowRate;
            public Th16Converter.Season season;
        };

        internal static Properties ValidProperties => new Properties()
        {
            score = 12u,
            stageProgress = Th16Converter.StageProgress.St3,
            continueCount = 4,
            name = TestUtils.MakeRandomArray<byte>(10),
            dateTime = 567u,
            slowRate = 8.9f,
            season = Th16Converter.Season.Full
        };

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.score,
                (byte)properties.stageProgress,
                properties.continueCount,
                properties.name,
                properties.dateTime,
                0u,
                properties.slowRate,
                (int)properties.season);

        internal static void Validate(in Th16ScoreDataWrapper scoreData, in Properties properties)
        {
            Assert.AreEqual(properties.score, scoreData.Score);
            Assert.AreEqual(properties.stageProgress, scoreData.StageProgress);
            Assert.AreEqual(properties.continueCount, scoreData.ContinueCount);
            CollectionAssert.AreEqual(properties.name, scoreData.Name?.ToArray());
            Assert.AreEqual(properties.dateTime, scoreData.DateTime);
            Assert.AreEqual(properties.slowRate, scoreData.SlowRate);
            Assert.AreEqual(properties.season, scoreData.Season);
        }

        [TestMethod]
        public void Th16ScoreDataTest() => TestUtils.Wrap(() =>
        {
            var properties = new Properties();
            var scoreData = new Th16ScoreDataWrapper();

            Validate(scoreData, properties);
        });

        [TestMethod]
        public void Th16ScoreDataReadFromTest() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var scoreData = Th16ScoreDataWrapper.Create(MakeByteArray(properties));

            Validate(scoreData, properties);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th16ScoreDataReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var scoreData = new Th16ScoreDataWrapper();
            scoreData.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidStageProgresses
            => TestUtils.GetInvalidEnumerators(typeof(Th16Converter.StageProgress));

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "scoreData")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidStageProgresses))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th16ScoreDataReadFromTestInvalidStageProgress(int stageProgress) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.stageProgress = TestUtils.Cast<Th16Converter.StageProgress>(stageProgress);

            var scoreData = Th16ScoreDataWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "scoreData")]
        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th16ScoreDataReadFromTestShortenedName() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.name = properties.name.Take(properties.name.Length - 1).ToArray();

            var scoreData = Th16ScoreDataWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "scoreData")]
        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th16ScoreDataReadFromTestExceededName() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.name = properties.name.Concat(TestUtils.MakeRandomArray<byte>(1)).ToArray();

            var scoreData = Th16ScoreDataWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidSeasons
            => TestUtils.GetInvalidEnumerators(typeof(Th16Converter.Season));

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "scoreData")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidSeasons))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th16ScoreDataReadFromTestInvalidSeason(int season) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.season = TestUtils.Cast<Th16Converter.Season>(season);

            var scoreData = Th16ScoreDataWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}

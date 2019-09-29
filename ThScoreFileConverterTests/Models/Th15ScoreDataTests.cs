using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th15ScoreDataTests
    {
        internal struct Properties
        {
            public uint score;
            public Th15Converter.StageProgress stageProgress;
            public byte continueCount;
            public byte[] name;
            public uint dateTime;
            public float slowRate;
            public uint retryCount;
        };

        internal static Properties ValidProperties => new Properties()
        {
            score = 12u,
            stageProgress = Th15Converter.StageProgress.St3,
            continueCount = 4,
            name = TestUtils.MakeRandomArray<byte>(10),
            dateTime = 56u,
            slowRate = 7.8f,
            retryCount = 9u
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
                properties.retryCount);

        internal static void Validate(in Th15ScoreDataWrapper scoreData, in Properties properties)
        {
            Assert.AreEqual(properties.score, scoreData.Score);
            Assert.AreEqual(properties.stageProgress, scoreData.StageProgress);
            Assert.AreEqual(properties.continueCount, scoreData.ContinueCount);
            CollectionAssert.That.AreEqual(properties.name, scoreData.Name);
            Assert.AreEqual(properties.dateTime, scoreData.DateTime);
            Assert.AreEqual(properties.slowRate, scoreData.SlowRate);
            Assert.AreEqual(properties.retryCount, scoreData.RetryCount);
        }

        [TestMethod]
        public void Th15ScoreDataTest() => TestUtils.Wrap(() =>
        {
            var properties = new Properties();
            var scoreData = new Th15ScoreDataWrapper();

            Validate(scoreData, properties);
        });

        [TestMethod]
        public void Th15ScoreDataReadFromTest() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var scoreData = Th15ScoreDataWrapper.Create(MakeByteArray(properties));

            Validate(scoreData, properties);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th15ScoreDataReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var scoreData = new Th15ScoreDataWrapper();
            scoreData.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidStageProgresses
            => TestUtils.GetInvalidEnumerators(typeof(Th15Converter.StageProgress));

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "scoreData")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidStageProgresses))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th15ScoreDataReadFromTestInvalidStageProgress(int stageProgress) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.stageProgress = TestUtils.Cast<Th15Converter.StageProgress>(stageProgress);

            var scoreData = Th15ScoreDataWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "scoreData")]
        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th15ScoreDataReadFromTestShortenedName() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.name = properties.name.Take(properties.name.Length - 1).ToArray();

            var scoreData = Th15ScoreDataWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void Th15ScoreDataReadFromTestExceededName() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            var validNameLength = properties.name.Length;
            properties.name = properties.name.Concat(TestUtils.MakeRandomArray<byte>(1)).ToArray();

            var scoreData = Th15ScoreDataWrapper.Create(MakeByteArray(properties));

            Assert.AreEqual(properties.score, scoreData.Score);
            Assert.AreEqual(properties.stageProgress, scoreData.StageProgress);
            Assert.AreEqual(properties.continueCount, scoreData.ContinueCount);
            CollectionAssert.That.AreNotEqual(properties.name, scoreData.Name);
            CollectionAssert.That.AreEqual(properties.name.Take(validNameLength), scoreData.Name);
            Assert.AreNotEqual(properties.dateTime, scoreData.DateTime);
            Assert.AreNotEqual(properties.slowRate, scoreData.SlowRate);
            Assert.AreNotEqual(properties.retryCount, scoreData.RetryCount);
        });
    }
}

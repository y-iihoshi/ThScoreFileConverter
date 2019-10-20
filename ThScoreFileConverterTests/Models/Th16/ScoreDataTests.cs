using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th16;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th16.Stubs;

namespace ThScoreFileConverterTests.Models.Th16
{
    [TestClass]
    public class ScoreDataTests
    {
        internal static ScoreDataStub ValidStub { get; } = new ScoreDataStub()
        {
            Score = 12u,
            StageProgress = Th16Converter.StageProgress.St3,
            ContinueCount = 4,
            Name = TestUtils.MakeRandomArray<byte>(10),
            DateTime = 567u,
            SlowRate = 8.9f,
            Season = Th16Converter.Season.Full
        };

        internal static byte[] MakeByteArray(IScoreData scoreData)
            => TestUtils.MakeByteArray(
                scoreData.Score,
                (byte)scoreData.StageProgress,
                scoreData.ContinueCount,
                scoreData.Name.ToArray(),
                scoreData.DateTime,
                0u,
                scoreData.SlowRate,
                (int)scoreData.Season);

        internal static void Validate(IScoreData expected, IScoreData actual)
        {
            Assert.AreEqual(expected.Score, actual.Score);
            Assert.AreEqual(expected.StageProgress, actual.StageProgress);
            Assert.AreEqual(expected.ContinueCount, actual.ContinueCount);
            CollectionAssert.That.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.DateTime, actual.DateTime);
            Assert.AreEqual(expected.SlowRate, actual.SlowRate);
            Assert.AreEqual(expected.Season, actual.Season);
        }

        [TestMethod]
        public void ScoreDataTest() => TestUtils.Wrap(() =>
        {
            var stub = new ScoreDataStub();
            var scoreData = new ScoreData();

            Validate(stub, scoreData);
        });

        [TestMethod]
        public void ReadFromTest() => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;

            var scoreData = TestUtils.Create<ScoreData>(MakeByteArray(stub));

            Validate(stub, scoreData);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var scoreData = new ScoreData();
            scoreData.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidStageProgresses
            => TestUtils.GetInvalidEnumerators(typeof(Th16Converter.StageProgress));

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidStageProgresses))]
        [ExpectedException(typeof(InvalidCastException))]
        public void ReadFromTestInvalidStageProgress(int stageProgress) => TestUtils.Wrap(() =>
        {
            var stub = new ScoreDataStub(ValidStub)
            {
                StageProgress = TestUtils.Cast<Th16Converter.StageProgress>(stageProgress),
            };

            _ = TestUtils.Create<ScoreData>(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestShortenedName() => TestUtils.Wrap(() =>
        {
            var stub = new ScoreDataStub(ValidStub);
            stub.Name = stub.Name.SkipLast(1).ToArray();

            _ = TestUtils.Create<ScoreData>(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void ReadFromTestExceededName() => TestUtils.Wrap(() =>
        {
            var stub = new ScoreDataStub(ValidStub);
            stub.Name = stub.Name.Concat(TestUtils.MakeRandomArray<byte>(1)).ToArray();

            _ = TestUtils.Create<ScoreData>(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidSeasons
            => TestUtils.GetInvalidEnumerators(typeof(Th16Converter.Season));

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidSeasons))]
        [ExpectedException(typeof(InvalidCastException))]
        public void ReadFromTestInvalidSeason(int season) => TestUtils.Wrap(() =>
        {
            var stub = new ScoreDataStub(ValidStub)
            {
                Season = TestUtils.Cast<Th16Converter.Season>(season),
            };

            _ = TestUtils.Create<ScoreData>(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}

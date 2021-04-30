using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models.Th16;
using ThScoreFileConverterTests.UnitTesting;
using StageProgress = ThScoreFileConverter.Models.Th13.StageProgress;

namespace ThScoreFileConverterTests.Models.Th16
{
    [TestClass]
    public class ScoreDataTests
    {
        internal static Mock<IScoreData> MockScoreData()
        {
            var mock = new Mock<IScoreData>();
            _ = mock.SetupGet(m => m.Score).Returns(12u);
            _ = mock.SetupGet(m => m.StageProgress).Returns(StageProgress.Three);
            _ = mock.SetupGet(m => m.ContinueCount).Returns(4);
            _ = mock.SetupGet(m => m.Name).Returns(TestUtils.MakeRandomArray<byte>(10));
            _ = mock.SetupGet(m => m.DateTime).Returns(567u);
            _ = mock.SetupGet(m => m.SlowRate).Returns(8.9f);
            _ = mock.SetupGet(m => m.Season).Returns(Season.Full);
            return mock;
        }

        internal static byte[] MakeByteArray(IScoreData scoreData)
        {
            return TestUtils.MakeByteArray(
                scoreData.Score,
                (byte)scoreData.StageProgress,
                scoreData.ContinueCount,
                scoreData.Name,
                scoreData.DateTime,
                0u,
                scoreData.SlowRate,
                (int)scoreData.Season);
        }

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
        public void ScoreDataTest()
        {
            var mock = new Mock<IScoreData>();
            var scoreData = new ScoreData();

            Validate(mock.Object, scoreData);
        }

        [TestMethod]
        public void ReadFromTest()
        {
            var mock = MockScoreData();
            var scoreData = TestUtils.Create<ScoreData>(MakeByteArray(mock.Object));

            Validate(mock.Object, scoreData);
        }

        public static IEnumerable<object[]> InvalidStageProgresses
            => TestUtils.GetInvalidEnumerators(typeof(StageProgress));

        [DataTestMethod]
        [DynamicData(nameof(InvalidStageProgresses))]
        public void ReadFromTestInvalidStageProgress(int stageProgress)
        {
            var mock = MockScoreData();
            _ = mock.SetupGet(m => m.StageProgress).Returns(TestUtils.Cast<StageProgress>(stageProgress));

            _ = Assert.ThrowsException<InvalidCastException>(
                () => _ = TestUtils.Create<ScoreData>(MakeByteArray(mock.Object)));
        }

        [TestMethod]
        public void ReadFromTestShortenedName()
        {
            var mock = MockScoreData();
            var name = mock.Object.Name;
            _ = mock.SetupGet(m => m.Name).Returns(name.SkipLast(1).ToArray());

            _ = Assert.ThrowsException<EndOfStreamException>(
                () => _ = TestUtils.Create<ScoreData>(MakeByteArray(mock.Object)));
        }

        [TestMethod]
        public void ReadFromTestExceededName()
        {
            var mock = MockScoreData();
            var name = mock.Object.Name;
            _ = mock.SetupGet(m => m.Name).Returns(name.Concat(TestUtils.MakeRandomArray<byte>(1)).ToArray());

            _ = Assert.ThrowsException<InvalidCastException>(
                () => _ = TestUtils.Create<ScoreData>(MakeByteArray(mock.Object)));
        }

        public static IEnumerable<object[]> InvalidSeasons
            => TestUtils.GetInvalidEnumerators(typeof(Season));

        [DataTestMethod]
        [DynamicData(nameof(InvalidSeasons))]
        public void ReadFromTestInvalidSeason(int season)
        {
            var mock = MockScoreData();
            _ = mock.SetupGet(m => m.Season).Returns(TestUtils.Cast<Season>(season));

            _ = Assert.ThrowsException<InvalidCastException>(
                () => _ = TestUtils.Create<ScoreData>(MakeByteArray(mock.Object)));
        }
    }
}

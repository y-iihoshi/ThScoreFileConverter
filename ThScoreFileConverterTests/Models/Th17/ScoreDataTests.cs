using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models.Th17;
using ThScoreFileConverterTests.Extensions;
using StageProgress = ThScoreFileConverter.Models.Th13.StageProgress;

namespace ThScoreFileConverterTests.Models.Th17
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
            return mock;
        }

        internal static byte[] MakeByteArray(in IScoreData scoreData)
            => TestUtils.MakeByteArray(
                scoreData.Score,
                (byte)scoreData.StageProgress,
                scoreData.ContinueCount,
                scoreData.Name,
                scoreData.DateTime,
                0u,
                scoreData.SlowRate,
                0u);

        internal static void Validate(in IScoreData expected, in IScoreData actual)
        {
            Assert.AreEqual(expected.Score, actual.Score);
            Assert.AreEqual(expected.StageProgress, actual.StageProgress);
            Assert.AreEqual(expected.ContinueCount, actual.ContinueCount);
            CollectionAssert.That.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.DateTime, actual.DateTime);
            Assert.AreEqual(expected.SlowRate, actual.SlowRate);
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

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull()
        {
            var scoreData = new ScoreData();

            scoreData.ReadFrom(null!);

            Assert.Fail(TestUtils.Unreachable);
        }

        public static IEnumerable<object[]> InvalidStageProgresses
            => TestUtils.GetInvalidEnumerators(typeof(StageProgress));

        [DataTestMethod]
        [DynamicData(nameof(InvalidStageProgresses))]
        [ExpectedException(typeof(InvalidCastException))]
        public void ReadFromTestInvalidStageProgress(int stageProgress)
        {
            var mock = MockScoreData();
            _ = mock.SetupGet(m => m.StageProgress).Returns(TestUtils.Cast<StageProgress>(stageProgress));

            _ = TestUtils.Create<ScoreData>(MakeByteArray(mock.Object));

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestShortenedName()
        {
            var mock = MockScoreData();
            var name = mock.Object.Name;
            _ = mock.SetupGet(m => m.Name).Returns(name.SkipLast(1).ToArray());

            _ = TestUtils.Create<ScoreData>(MakeByteArray(mock.Object));

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void ReadFromTestExceededName()
        {
            var mock = MockScoreData();
            var name = mock.Object.Name;
            var validNameLength = name.Count();
            _ = mock.SetupGet(m => m.Name).Returns(name.Concat(TestUtils.MakeRandomArray<byte>(1)).ToArray());

            var scoreData = TestUtils.Create<ScoreData>(MakeByteArray(mock.Object));

            Assert.AreEqual(mock.Object.Score, scoreData.Score);
            Assert.AreEqual(mock.Object.StageProgress, scoreData.StageProgress);
            Assert.AreEqual(mock.Object.ContinueCount, scoreData.ContinueCount);
            CollectionAssert.That.AreNotEqual(mock.Object.Name, scoreData.Name);
            CollectionAssert.That.AreEqual(mock.Object.Name.Take(validNameLength), scoreData.Name);
            Assert.AreNotEqual(mock.Object.DateTime, scoreData.DateTime);
            Assert.AreNotEqual(mock.Object.SlowRate, scoreData.SlowRate);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models.Th10;
using ThScoreFileConverterTests.Extensions;

namespace ThScoreFileConverterTests.Models.Th10
{
    [TestClass]
    public class ScoreDataTests
    {
        internal static Mock<IScoreData<TStageProgress>> MockScoreData<TStageProgress>()
            where TStageProgress : struct, Enum
        {
            var mock = new Mock<IScoreData<TStageProgress>>();
            _ = mock.SetupGet(m => m.Score).Returns(12u);
            _ = mock.SetupGet(m => m.StageProgress).Returns(TestUtils.Cast<TStageProgress>(3));
            _ = mock.SetupGet(m => m.ContinueCount).Returns(4);
            _ = mock.SetupGet(m => m.Name).Returns(TestUtils.MakeRandomArray<byte>(10));
            _ = mock.SetupGet(m => m.DateTime).Returns(567u);
            _ = mock.SetupGet(m => m.SlowRate).Returns(8.9f);
            return mock;
        }

        internal static byte[] MakeByteArray<TStageProgress>(IScoreData<TStageProgress> scoreData, int unknownSize)
            where TStageProgress : struct, Enum
            => TestUtils.MakeByteArray(
                scoreData.Score,
                (byte)TestUtils.Cast<int>(scoreData.StageProgress),
                scoreData.ContinueCount,
                scoreData.Name.ToArray(),
                scoreData.DateTime,
                scoreData.SlowRate,
                new byte[unknownSize]);

        internal static byte[] MakeByteArray(IScoreData<StageProgress> scoreData) => MakeByteArray(scoreData, 0);

        internal static void Validate<TStageProgress>(
            IScoreData<TStageProgress> expected, IScoreData<TStageProgress> actual)
            where TStageProgress : struct, Enum
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
            var mock = new Mock<IScoreData<StageProgress>>();
            var scoreData = new ScoreData();

            Validate(mock.Object, scoreData);
        }

        [TestMethod]
        public void ReadFromTest()
        {
            var mock = MockScoreData<StageProgress>();

            var scoreData = TestUtils.Create<ScoreData>(MakeByteArray(mock.Object));

            Validate(mock.Object, scoreData);
        }

        [TestMethod]
        public void ReadFromTestNull()
        {
            var scoreData = new ScoreData();
            _ = Assert.ThrowsException<ArgumentNullException>(() => scoreData.ReadFrom(null!));
        }

        [TestMethod]
        public void ReadFromTestShortenedName()
        {
            var mock = MockScoreData<StageProgress>();
            var name = mock.Object.Name;
            _ = mock.SetupGet(m => m.Name).Returns(name.SkipLast(1).ToArray());

            _ = Assert.ThrowsException<EndOfStreamException>(
                () => _ = TestUtils.Create<ScoreData>(MakeByteArray(mock.Object)));
        }

        [TestMethod]
        public void ReadFromTestExceededName()
        {
            var mock = MockScoreData<StageProgress>();
            var name = mock.Object.Name;
            _ = mock.SetupGet(m => m.Name).Returns(name.Concat(TestUtils.MakeRandomArray<byte>(1)).ToArray());

            var scoreData = TestUtils.Create<ScoreData>(MakeByteArray(mock.Object));

            Assert.AreEqual(mock.Object.Score, scoreData.Score);
            Assert.AreEqual(mock.Object.StageProgress, scoreData.StageProgress);
            Assert.AreEqual(mock.Object.ContinueCount, scoreData.ContinueCount);
            CollectionAssert.That.AreNotEqual(mock.Object.Name, scoreData.Name);
            CollectionAssert.That.AreEqual(mock.Object.Name.SkipLast(1), scoreData.Name);
            Assert.AreNotEqual(mock.Object.DateTime, scoreData.DateTime);
            Assert.AreNotEqual(mock.Object.SlowRate, scoreData.SlowRate);
        }

        public static IEnumerable<object[]> InvalidStageProgresses
            => TestUtils.GetInvalidEnumerators(typeof(StageProgress));

        [DataTestMethod]
        [DynamicData(nameof(InvalidStageProgresses))]
        public void ReadFromTestInvalidStageProgress(int stageProgress)
        {
            var mock = MockScoreData<StageProgress>();
            _ = mock.SetupGet(m => m.StageProgress).Returns(TestUtils.Cast<StageProgress>(stageProgress));

            _ = Assert.ThrowsException<InvalidCastException>(
                () => _ = TestUtils.Create<ScoreData>(MakeByteArray(mock.Object)));
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models.Th17;
using ThScoreFileConverterTests.UnitTesting;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>;
using StageProgress = ThScoreFileConverter.Models.Th13.StageProgress;

namespace ThScoreFileConverterTests.Models.Th17
{
    [TestClass]
    public class ScoreDataTests
    {
        internal static byte[] MakeByteArray(in IScoreData scoreData)
        {
            return TestUtils.MakeByteArray(
                scoreData.Score,
                (byte)scoreData.StageProgress,
                scoreData.ContinueCount,
                scoreData.Name,
                scoreData.DateTime,
                0u,
                scoreData.SlowRate,
                0u);
        }

        [TestMethod]
        public void ReadFromTest()
        {
            var mock = Th10.ScoreDataTests.MockScoreData<StageProgress>();
            var scoreData = TestUtils.Create<ScoreData>(MakeByteArray(mock.Object));

            Th10.ScoreDataTests.Validate(mock.Object, scoreData);
        }

        public static IEnumerable<object[]> InvalidStageProgresses
            => TestUtils.GetInvalidEnumerators(typeof(StageProgress));

        [DataTestMethod]
        [DynamicData(nameof(InvalidStageProgresses))]
        public void ReadFromTestInvalidStageProgress(int stageProgress)
        {
            var mock = Th10.ScoreDataTests.MockScoreData<StageProgress>();
            _ = mock.SetupGet(m => m.StageProgress).Returns(TestUtils.Cast<StageProgress>(stageProgress));

            _ = Assert.ThrowsException<InvalidCastException>(
                () => _ = TestUtils.Create<ScoreData>(MakeByteArray(mock.Object)));
        }

        [TestMethod]
        public void ReadFromTestShortenedName()
        {
            var mock = Th10.ScoreDataTests.MockScoreData<StageProgress>();
            var name = mock.Object.Name;
            _ = mock.SetupGet(m => m.Name).Returns(name.SkipLast(1).ToArray());

            _ = Assert.ThrowsException<EndOfStreamException>(
                () => _ = TestUtils.Create<ScoreData>(MakeByteArray(mock.Object)));
        }

        [TestMethod]
        public void ReadFromTestExceededName()
        {
            var mock = Th10.ScoreDataTests.MockScoreData<StageProgress>();
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

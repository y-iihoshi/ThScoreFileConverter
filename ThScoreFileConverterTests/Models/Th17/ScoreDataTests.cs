using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models.Th17;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th17.Stubs;

namespace ThScoreFileConverterTests.Models.Th17
{
    [TestClass]
    public class ScoreDataTests
    {
        internal static ScoreDataStub ValidStub { get; } = new ScoreDataStub()
        {
            Score = 12u,
            StageProgress = StageProgress.Three,
            ContinueCount = 4,
            Name = TestUtils.MakeRandomArray<byte>(10),
            DateTime = 567u,
            SlowRate = 8.9f,
        };

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
            var stub = new ScoreDataStub();
            var scoreData = new ScoreData();

            Validate(stub, scoreData);
        }

        [TestMethod]
        public void ReadFromTest()
        {
            var stub = ValidStub;

            var scoreData = TestUtils.Create<ScoreData>(MakeByteArray(stub));

            Validate(stub, scoreData);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull()
        {
            var scoreData = new ScoreData();

            scoreData.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        }

        public static IEnumerable<object[]> InvalidStageProgresses
            => TestUtils.GetInvalidEnumerators(typeof(StageProgress));

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidStageProgresses))]
        [ExpectedException(typeof(InvalidCastException))]
        public void ReadFromTestInvalidStageProgress(int stageProgress)
        {
            var stub = new ScoreDataStub(ValidStub)
            {
                StageProgress = TestUtils.Cast<StageProgress>(stageProgress),
            };

            _ = TestUtils.Create<ScoreData>(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestShortenedName()
        {
            var stub = new ScoreDataStub(ValidStub);
            stub.Name = stub.Name.SkipLast(1).ToArray();

            _ = TestUtils.Create<ScoreData>(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void ReadFromTestExceededName()
        {
            var stub = new ScoreDataStub(ValidStub);
            var validNameLength = stub.Name.Count();
            stub.Name = stub.Name.Concat(TestUtils.MakeRandomArray<byte>(1)).ToArray();

            var scoreData = TestUtils.Create<ScoreData>(MakeByteArray(stub));

            Assert.AreEqual(stub.Score, scoreData.Score);
            Assert.AreEqual(stub.StageProgress, scoreData.StageProgress);
            Assert.AreEqual(stub.ContinueCount, scoreData.ContinueCount);
            CollectionAssert.That.AreNotEqual(stub.Name, scoreData.Name);
            CollectionAssert.That.AreEqual(stub.Name.Take(validNameLength), scoreData.Name);
            Assert.AreNotEqual(stub.DateTime, scoreData.DateTime);
            Assert.AreNotEqual(stub.SlowRate, scoreData.SlowRate);
        }
    }
}

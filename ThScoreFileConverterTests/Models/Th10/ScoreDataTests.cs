using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th10;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th10.Stubs;

namespace ThScoreFileConverterTests.Models.Th10
{
    [TestClass]
    public class ScoreDataTests
    {
        internal static ScoreDataStub<StageProgress> ValidStub { get; } = MakeValidStub<StageProgress>();

        internal static ScoreDataStub<TStageProgress> MakeValidStub<TStageProgress>()
            where TStageProgress : struct, Enum
            => new ScoreDataStub<TStageProgress>()
            {
                Score = 12u,
                StageProgress = TestUtils.Cast<TStageProgress>(3),
                ContinueCount = 4,
                Name = TestUtils.MakeRandomArray<byte>(10),
                DateTime = 567u,
                SlowRate = 8.9f,
            };

        internal static byte[] MakeByteArray<TParent, TStageProgress>(IScoreData<TStageProgress> scoreData)
            where TParent : ThConverter
            where TStageProgress : struct, Enum
        {
            var unknownSize = 0;

            var type = typeof(TParent);
            if (type == typeof(Th10Converter))
            {
                // Do nothing
            }
            else if (type == typeof(Th128Converter))
            {
                unknownSize = 8;
            }
            else
            {
                unknownSize = 4;
            }

            return MakeByteArray(scoreData, unknownSize);
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
            var stub = new ScoreDataStub<StageProgress>();
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
            scoreData.ReadFrom(null!);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestShortenedName()
        {
            var stub = new ScoreDataStub<StageProgress>(ValidStub);
            stub.Name = stub.Name.SkipLast(1).ToArray();

            _ = TestUtils.Create<ScoreData>(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void ReadFromTestExceededName()
        {
            var stub = new ScoreDataStub<StageProgress>(ValidStub);
            stub.Name = stub.Name.Concat(TestUtils.MakeRandomArray<byte>(1)).ToArray();

            var scoreData = TestUtils.Create<ScoreData>(MakeByteArray(stub));

            Assert.AreEqual(stub.Score, scoreData.Score);
            Assert.AreEqual(stub.StageProgress, scoreData.StageProgress);
            Assert.AreEqual(stub.ContinueCount, scoreData.ContinueCount);
            CollectionAssert.That.AreNotEqual(stub.Name, scoreData.Name);
            CollectionAssert.That.AreEqual(stub.Name.SkipLast(1), scoreData.Name);
            Assert.AreNotEqual(stub.DateTime, scoreData.DateTime);
            Assert.AreNotEqual(stub.SlowRate, scoreData.SlowRate);
        }

        public static IEnumerable<object[]> InvalidStageProgresses
            => TestUtils.GetInvalidEnumerators(typeof(StageProgress));

        [DataTestMethod]
        [DynamicData(nameof(InvalidStageProgresses))]
        [ExpectedException(typeof(InvalidCastException))]
        public void ReadFromTestInvalidStageProgress(int stageProgress)
        {
            var stub = new ScoreDataStub<StageProgress>(ValidStub)
            {
                StageProgress = TestUtils.Cast<StageProgress>(stageProgress),
            };

            _ = TestUtils.Create<ScoreData>(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        }
    }
}

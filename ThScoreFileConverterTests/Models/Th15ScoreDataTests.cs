using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th15;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th15.Stubs;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th15ScoreDataTests
    {
        internal static ScoreDataStub ValidStub { get; } = new ScoreDataStub()
        {
            Score = 12u,
            StageProgress = Th15Converter.StageProgress.St3,
            ContinueCount = 4,
            Name = TestUtils.MakeRandomArray<byte>(10),
            DateTime = 56u,
            SlowRate = 7.8f,
            RetryCount = 9u
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
                scoreData.RetryCount);

        internal static void Validate(IScoreData expected, in Th15ScoreDataWrapper actual)
        {
            Assert.AreEqual(expected.Score, actual.Score);
            Assert.AreEqual(expected.StageProgress, actual.StageProgress);
            Assert.AreEqual(expected.ContinueCount, actual.ContinueCount);
            CollectionAssert.That.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.DateTime, actual.DateTime);
            Assert.AreEqual(expected.SlowRate, actual.SlowRate);
            Assert.AreEqual(expected.RetryCount, actual.RetryCount);
        }

        [TestMethod]
        public void Th15ScoreDataTest() => TestUtils.Wrap(() =>
        {
            var stub = new ScoreDataStub();
            var scoreData = new Th15ScoreDataWrapper();

            Validate(stub, scoreData);
        });

        [TestMethod]
        public void Th15ScoreDataReadFromTest() => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;

            var scoreData = Th15ScoreDataWrapper.Create(MakeByteArray(stub));

            Validate(stub, scoreData);
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

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidStageProgresses))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th15ScoreDataReadFromTestInvalidStageProgress(int stageProgress) => TestUtils.Wrap(() =>
        {
            var stub = new ScoreDataStub(ValidStub)
            {
                StageProgress = TestUtils.Cast<Th15Converter.StageProgress>(stageProgress),
            };

            _ = Th15ScoreDataWrapper.Create(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th15ScoreDataReadFromTestShortenedName() => TestUtils.Wrap(() =>
        {
            var stub = new ScoreDataStub(ValidStub);
            stub.Name = stub.Name.SkipLast(1).ToArray();

            _ = Th15ScoreDataWrapper.Create(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void Th15ScoreDataReadFromTestExceededName() => TestUtils.Wrap(() =>
        {
            var stub = new ScoreDataStub(ValidStub);
            var validNameLength = stub.Name.Count();
            stub.Name = stub.Name.Concat(TestUtils.MakeRandomArray<byte>(1)).ToArray();

            var scoreData = Th15ScoreDataWrapper.Create(MakeByteArray(stub));

            Assert.AreEqual(stub.Score, scoreData.Score);
            Assert.AreEqual(stub.StageProgress, scoreData.StageProgress);
            Assert.AreEqual(stub.ContinueCount, scoreData.ContinueCount);
            CollectionAssert.That.AreNotEqual(stub.Name, scoreData.Name);
            CollectionAssert.That.AreEqual(stub.Name.Take(validNameLength), scoreData.Name);
            Assert.AreNotEqual(stub.DateTime, scoreData.DateTime);
            Assert.AreNotEqual(stub.SlowRate, scoreData.SlowRate);
            Assert.AreNotEqual(stub.RetryCount, scoreData.RetryCount);
        });
    }
}

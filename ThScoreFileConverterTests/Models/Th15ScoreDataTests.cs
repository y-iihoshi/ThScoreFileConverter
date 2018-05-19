using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace ThScoreFileConverter.Models.Tests
{
    [TestClass()]
    public class Th15ScoreDataTests
    {
        [TestMethod()]
        public void Th15ScoreDataTest()
        {
            try
            {
                var scoreData = new Th15ScoreDataWrapper();

                Assert.AreEqual(default, scoreData.Score.Value);
                Assert.AreEqual(default, scoreData.StageProgress.Value);
                Assert.AreEqual(default, scoreData.ContinueCount.Value);
                Assert.IsNull(scoreData.Name);
                Assert.AreEqual(default, scoreData.DateTime.Value);
                Assert.AreEqual(default, scoreData.SlowRate.Value);
                Assert.AreEqual(default, scoreData.RetryCount.Value);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [TestMethod()]
        public void Th15ScoreDataReadFromTest()
        {
            try
            {
                var score = 12u;
                var stageProgress = Th15Converter.StageProgress.St3;
                var continueCount = (byte)4;
                var name = TestUtils.MakeRandomArray<byte>(10);
                var dateTime = 56u;
                var unknown = 0u;
                var slowRate = 7.8f;
                var retryCount = 9u;

                var scoreData = Th15ScoreDataWrapper.Create(
                    TestUtils.MakeByteArray(
                        score,
                        TestUtils.Cast<byte>(stageProgress),
                        continueCount,
                        name,
                        dateTime,
                        unknown,
                        slowRate,
                        retryCount));

                Assert.AreEqual(score, scoreData.Score);
                Assert.AreEqual(stageProgress, scoreData.StageProgress);
                Assert.AreEqual(continueCount, scoreData.ContinueCount);
                CollectionAssert.AreEqual(name, scoreData.Name.ToArray());
                Assert.AreEqual(dateTime, scoreData.DateTime);
                Assert.AreEqual(slowRate, scoreData.SlowRate);
                Assert.AreEqual(retryCount, scoreData.RetryCount);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th15ScoreDataReadFromTestNull()
        {
            try
            {
                var scoreData = new Th15ScoreDataWrapper();
                scoreData.ReadFrom(null);
                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "scoreData")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th15ScoreDataReadFromTestInvalidStageProgress()
        {
            try
            {
                var score = 12u;
                var stageProgress = (Th15Converter.StageProgress)byte.MaxValue;
                var continueCount = (byte)4;
                var name = TestUtils.MakeRandomArray<byte>(10);
                var dateTime = 56u;
                var unknown = 0u;
                var slowRate = 7.8f;
                var retryCount = 9u;

                var scoreData = Th15ScoreDataWrapper.Create(
                    TestUtils.MakeByteArray(
                        score,
                        TestUtils.Cast<byte>(stageProgress),
                        continueCount,
                        name,
                        dateTime,
                        unknown,
                        slowRate,
                        retryCount));

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "scoreData")]
        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th15ScoreDataReadFromTestShortenedName()
        {
            try
            {
                var score = 12u;
                var stageProgress = Th15Converter.StageProgress.St3;
                var continueCount = (byte)4;
                var name = TestUtils.MakeRandomArray<byte>(9);
                var dateTime = 56u;
                var unknown = 0u;
                var slowRate = 7.8f;
                var retryCount = 9u;

                var scoreData = Th15ScoreDataWrapper.Create(
                    TestUtils.MakeByteArray(
                        score,
                        TestUtils.Cast<byte>(stageProgress),
                        continueCount,
                        name,
                        dateTime,
                        unknown,
                        slowRate,
                        retryCount));

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "scoreData")]
        [TestMethod()]
        public void Th15ScoreDataReadFromTestExceededName()
        {
            try
            {
                var score = 12u;
                var stageProgress = Th15Converter.StageProgress.St3;
                var continueCount = (byte)4;
                var name = TestUtils.MakeRandomArray<byte>(11);
                var dateTime = 56u;
                var unknown = 0u;
                var slowRate = 7.8f;
                var retryCount = 9u;

                var scoreData = Th15ScoreDataWrapper.Create(
                    TestUtils.MakeByteArray(
                        score,
                        TestUtils.Cast<byte>(stageProgress),
                        continueCount,
                        name,
                        dateTime,
                        unknown,
                        slowRate,
                        retryCount));

                Assert.AreEqual(score, scoreData.Score);
                Assert.AreEqual(stageProgress, scoreData.StageProgress);
                Assert.AreEqual(continueCount, scoreData.ContinueCount);
                CollectionAssert.AreNotEqual(name, scoreData.Name.ToArray());
                CollectionAssert.AreEqual(name.Take(10).ToArray(), scoreData.Name.ToArray());
                Assert.AreNotEqual(dateTime, scoreData.DateTime);
                Assert.AreNotEqual(slowRate, scoreData.SlowRate);
                Assert.AreNotEqual(retryCount, scoreData.RetryCount);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }
    }
}

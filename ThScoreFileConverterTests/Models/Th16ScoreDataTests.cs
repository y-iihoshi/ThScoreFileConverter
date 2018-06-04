using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass()]
    public class Th16ScoreDataTests
    {
        [TestMethod()]
        public void Th16ScoreDataTest()
        {
            try
            {
                var scoreData = new Th16ScoreDataWrapper();

                Assert.AreEqual(default, scoreData.Score.Value);
                Assert.AreEqual(default, scoreData.StageProgress.Value);
                Assert.AreEqual(default, scoreData.ContinueCount.Value);
                Assert.IsNull(scoreData.Name);
                Assert.AreEqual(default, scoreData.DateTime.Value);
                Assert.AreEqual(default, scoreData.SlowRate.Value);
                Assert.AreEqual(default, scoreData.Season.Value);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [TestMethod()]
        public void Th16ScoreDataReadFromTest()
        {
            try
            {
                var score = 12u;
                var stageProgress = Th16Converter.StageProgress.St3;
                var continueCount = (byte)4;
                var name = TestUtils.MakeRandomArray<byte>(10);
                var dateTime = 567u;
                var unknown = 0u;
                var slowRate = 8.9f;
                var season = Th16Converter.Season.Full;

                var scoreData = Th16ScoreDataWrapper.Create(
                    TestUtils.MakeByteArray(
                        score,
                        TestUtils.Cast<byte>(stageProgress),
                        continueCount,
                        name,
                        dateTime,
                        unknown,
                        slowRate,
                        TestUtils.Cast<uint>(season)));

                Assert.AreEqual(score, scoreData.Score);
                Assert.AreEqual(stageProgress, scoreData.StageProgress);
                Assert.AreEqual(continueCount, scoreData.ContinueCount);
                CollectionAssert.AreEqual(name, scoreData.Name.ToArray());
                Assert.AreEqual(dateTime, scoreData.DateTime);
                Assert.AreEqual(slowRate, scoreData.SlowRate);
                Assert.AreEqual(season, scoreData.Season);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th16ScoreDataReadFromTestNull()
        {
            try
            {
                var scoreData = new Th16ScoreDataWrapper();
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
        public void Th16ScoreDataReadFromTestInvalidStageProgress()
        {
            try
            {
                var score = 12u;
                var stageProgress = (Th16Converter.StageProgress)byte.MaxValue;
                var continueCount = (byte)4;
                var name = TestUtils.MakeRandomArray<byte>(10);
                var dateTime = 567u;
                var unknown = 0u;
                var slowRate = 8.9f;
                var season = Th16Converter.Season.Full;

                var scoreData = Th16ScoreDataWrapper.Create(
                    TestUtils.MakeByteArray(
                        score,
                        TestUtils.Cast<byte>(stageProgress),
                        continueCount,
                        name,
                        dateTime,
                        unknown,
                        slowRate,
                        TestUtils.Cast<uint>(season)));

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
        public void Th16ScoreDataReadFromTestShortenedName()
        {
            try
            {
                var score = 12u;
                var stageProgress = Th16Converter.StageProgress.St3;
                var continueCount = (byte)4;
                var name = TestUtils.MakeRandomArray<byte>(9);
                var dateTime = 567u;
                var unknown = 0u;
                var slowRate = 8.9f;
                var season = Th16Converter.Season.Full;

                var scoreData = Th16ScoreDataWrapper.Create(
                    TestUtils.MakeByteArray(
                        score,
                        TestUtils.Cast<byte>(stageProgress),
                        continueCount,
                        name,
                        dateTime,
                        unknown,
                        slowRate,
                        TestUtils.Cast<uint>(season)));

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
        public void Th16ScoreDataReadFromTestExceededName()
        {
            try
            {
                var score = 12u;
                var stageProgress = Th16Converter.StageProgress.St3;
                var continueCount = (byte)4;
                var name = TestUtils.MakeRandomArray<byte>(11);
                var dateTime = 567u;
                var unknown = 0u;
                var slowRate = 8.9f;
                var season = Th16Converter.Season.Full;

                var scoreData = Th16ScoreDataWrapper.Create(
                    TestUtils.MakeByteArray(
                        score,
                        TestUtils.Cast<byte>(stageProgress),
                        continueCount,
                        name,
                        dateTime,
                        unknown,
                        slowRate,
                        TestUtils.Cast<uint>(season)));

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
        public void Th16ScoreDataReadFromTestInvalidSeason()
        {
            try
            {
                var score = 12u;
                var stageProgress = Th16Converter.StageProgress.St3;
                var continueCount = (byte)4;
                var name = TestUtils.MakeRandomArray<byte>(10);
                var dateTime = 567u;
                var unknown = 0u;
                var slowRate = 8.9f;
                var season = TestUtils.Cast<Th16Converter.Season>(int.MaxValue);

                var scoreData = Th16ScoreDataWrapper.Create(
                    TestUtils.MakeByteArray(
                        score,
                        TestUtils.Cast<byte>(stageProgress),
                        continueCount,
                        name,
                        dateTime,
                        unknown,
                        slowRate,
                        TestUtils.Cast<uint>(season)));

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }
    }
}

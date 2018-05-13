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
    public class Th07PlayCountTests
    {
        [TestMethod()]
        public void Th07PlayCountTest()
        {
            try
            {
                var playCount = new Th07PlayCountWrapper();

                Assert.AreEqual(0, playCount.TotalTrial.Value);
                Assert.AreEqual(0, playCount.Trials.Count);
                Assert.AreEqual(0, playCount.TotalRetry.Value);
                Assert.AreEqual(0, playCount.TotalClear.Value);
                Assert.AreEqual(0, playCount.TotalContinue.Value);
                Assert.AreEqual(0, playCount.TotalPractice.Value);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [TestMethod()]
        public void Th07PlayCountReadFromTest()
        {
            try
            {
                var totalTrial = 1;
                var trials = TestUtils.MakeRandomArray<int>(6);
                var totalRetry = 2;
                var totalClear = 3;
                var totalContinue = 4;
                var totalPractice = 5;

                var playCount = Th07PlayCountWrapper.Create(
                    TestUtils.MakeByteArray(totalTrial, trials, totalRetry, totalClear, totalContinue, totalPractice));

                Assert.AreEqual(totalTrial, playCount.TotalTrial.Value);
                CollectionAssert.AreEqual(trials, playCount.Trials.Values.ToArray());
                Assert.AreEqual(totalRetry, playCount.TotalRetry.Value);
                Assert.AreEqual(totalClear, playCount.TotalClear.Value);
                Assert.AreEqual(totalContinue, playCount.TotalContinue.Value);
                Assert.AreEqual(totalPractice, playCount.TotalPractice.Value);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th07PlayCountReadFromTestNull()
        {
            try
            {
                var playCount = new Th07PlayCountWrapper();
                playCount.ReadFrom(null);
                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "playCount")]
        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th07PlayCountReadFromTestShortenedTrials()
        {
            try
            {
                var totalTrial = 1;
                var trials = TestUtils.MakeRandomArray<int>(5);
                var totalRetry = 2;
                var totalClear = 3;
                var totalContinue = 4;
                var totalPractice = 5;

                var playCount = Th07PlayCountWrapper.Create(
                    TestUtils.MakeByteArray(totalTrial, trials, totalRetry, totalClear, totalContinue, totalPractice));

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [TestMethod()]
        public void Th07PlayCountReadFromTestExceededTrials()
        {
            try
            {
                var totalTrial = 1;
                var trials = TestUtils.MakeRandomArray<int>(7);
                var totalRetry = 2;
                var totalClear = 3;
                var totalContinue = 4;
                var totalPractice = 5;

                var playCount = Th07PlayCountWrapper.Create(
                    TestUtils.MakeByteArray(totalTrial, trials, totalRetry, totalClear, totalContinue, totalPractice));

                Assert.AreEqual(totalTrial, playCount.TotalTrial.Value);
                CollectionAssert.AreNotEqual(trials, playCount.Trials.Values.ToArray());
                CollectionAssert.AreEqual(trials.Take(6).ToArray(), playCount.Trials.Values.ToArray());
                Assert.AreNotEqual(totalRetry, playCount.TotalRetry.Value);
                Assert.AreNotEqual(totalClear, playCount.TotalClear.Value);
                Assert.AreNotEqual(totalContinue, playCount.TotalContinue.Value);
                Assert.AreNotEqual(totalPractice, playCount.TotalPractice.Value);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }
    }
}

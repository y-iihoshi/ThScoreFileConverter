using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace ThScoreFileConverterTests.Models
{
    [TestClass()]
    public class Th08PlayCountTests
    {
        [TestMethod()]
        public void Th08PlayCountTest()
        {
            try
            {
                var playCount = new Th08PlayCountWrapper();

                Assert.AreEqual(default, playCount.TotalTrial.Value);
                Assert.AreEqual(0, playCount.Trials.Count);
                Assert.AreEqual(default, playCount.TotalClear.Value);
                Assert.AreEqual(default, playCount.TotalContinue.Value);
                Assert.AreEqual(default, playCount.TotalPractice.Value);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [TestMethod()]
        public void Th08PlayCountReadFromTest()
        {
            try
            {
                var totalTrial = 1;
                var trials = TestUtils.MakeRandomArray<int>(12);
                var unknown = 2u;
                var totalClear = 3;
                var totalContinue = 4;
                var totalPractice = 5;

                var playCount = Th08PlayCountWrapper.Create(
                    TestUtils.MakeByteArray(totalTrial, trials, unknown, totalClear, totalContinue, totalPractice));

                Assert.AreEqual(totalTrial, playCount.TotalTrial.Value);
                CollectionAssert.AreEqual(trials, playCount.Trials.Values.ToArray());
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
        public void Th08PlayCountReadFromTestNull()
        {
            try
            {
                var playCount = new Th08PlayCountWrapper();
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
        public void Th08PlayCountReadFromTestShortenedTrials()
        {
            try
            {
                var totalTrial = 1;
                var trials = TestUtils.MakeRandomArray<int>(11);
                var unknown = 2u;
                var totalClear = 3;
                var totalContinue = 4;
                var totalPractice = 5;

                var playCount = Th08PlayCountWrapper.Create(
                    TestUtils.MakeByteArray(totalTrial, trials, unknown, totalClear, totalContinue, totalPractice));

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [TestMethod()]
        public void Th08PlayCountReadFromTestExceededTrials()
        {
            try
            {
                var totalTrial = 1;
                var trials = TestUtils.MakeRandomArray<int>(13);
                var unknown = 2u;
                var totalClear = 3;
                var totalContinue = 4;
                var totalPractice = 5;

                var playCount = Th08PlayCountWrapper.Create(
                    TestUtils.MakeByteArray(totalTrial, trials, unknown, totalClear, totalContinue, totalPractice));

                Assert.AreEqual(totalTrial, playCount.TotalTrial.Value);
                CollectionAssert.AreNotEqual(trials, playCount.Trials.Values.ToArray());
                CollectionAssert.AreEqual(trials.Take(12).ToArray(), playCount.Trials.Values.ToArray());
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

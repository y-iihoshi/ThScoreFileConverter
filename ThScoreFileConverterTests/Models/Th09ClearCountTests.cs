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
    public class Th09ClearCountTests
    {
        [TestMethod()]
        public void Th09ClearCountTest()
        {
            try
            {
                var clearCount = new Th09ClearCountWrapper();

                Assert.AreEqual(0, clearCount.Counts.Count);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [TestMethod()]
        public void Th09ClearCountReadFromTest()
        {
            try
            {
                var counts = TestUtils.MakeRandomArray<int>(5);
                var unknown = 1u;

                var clearCount = Th09ClearCountWrapper.Create(TestUtils.MakeByteArray(counts, unknown));

                CollectionAssert.AreEqual(counts, clearCount.Counts.Values.ToArray());
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th09ClearCountReadFromTestNull()
        {
            try
            {
                var clearCount = new Th09ClearCountWrapper();
                clearCount.ReadFrom(null);
                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "clearCount")]
        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th09ClearCountReadFromTestShortenedTrials()
        {
            try
            {
                var counts = TestUtils.MakeRandomArray<int>(4);
                var unknown = 1u;

                var clearCount = Th09ClearCountWrapper.Create(TestUtils.MakeByteArray(counts, unknown));

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [TestMethod()]
        public void Th09ClearCountReadFromTestExceededTrials()
        {
            try
            {
                var counts = TestUtils.MakeRandomArray<int>(6);
                var unknown = 1u;

                var clearCount = Th09ClearCountWrapper.Create(
                    TestUtils.MakeByteArray(counts, unknown));

                CollectionAssert.AreNotEqual(counts, clearCount.Counts.Values.ToArray());
                CollectionAssert.AreEqual(counts.Take(5).ToArray(), clearCount.Counts.Values.ToArray());
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }
    }
}

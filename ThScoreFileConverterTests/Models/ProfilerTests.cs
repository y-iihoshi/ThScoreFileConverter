using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ThScoreFileConverter.Models.Tests
{
    [TestClass()]
    public class ProfilerTests
    {
        [TestMethod()]
        public void ProfilerTest()
        {
            var value = 0L;
            void preprocess() => ++value;
            void postprocess(TimeSpan span) => value += span.Ticks;

            using (var profiler = new Profiler(preprocess, postprocess))
            {
                Assert.AreEqual(1L, value);
            }

            Assert.IsTrue(value > 1L);
        }

        [TestMethod()]
        public void ProfilerTestNull()
        {
            using (var profiler = new Profiler(null, null))
            {
            }
        }

        [TestMethod()]
        public void ProfilerTestPreprocess()
        {
            var value = 0;
            void preprocess() => ++value;

            using (var profiler = new Profiler(preprocess, null))
            {
                Assert.AreEqual(1, value);
            }

            Assert.AreEqual(1, value);
        }

        [TestMethod()]
        public void ProfilerTestPostprocess()
        {
            var value = 0L;
            void postprocess(TimeSpan span) => value += span.Ticks;

            using (var profiler = new Profiler(null, postprocess))
            {
                Assert.AreEqual(0L, value);
            }

            Assert.IsTrue(value > 0L);
        }

        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "ThScoreFileConverter.Models.Profiler.#ctor(System.String)")]
        [TestMethod()]
        public void ProfilerTestMessage()
        {
            using (var profiler = new Profiler(nameof(ProfilerTestMessage)))
            {
            }
        }

        [TestMethod()]
        public void ProfilerTestNullMessage()
        {
            using (var profiler = new Profiler(null))
            {
            }
        }

        [TestMethod()]
        public void ProfilerTestEmptyMessage()
        {
            using (var profiler = new Profiler(string.Empty))
            {
            }
        }

        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "ThScoreFileConverter.Models.Profiler.#ctor(System.String)")]
        [TestMethod()]
        public void DisposeTest()
        {
            var profiler = new Profiler(nameof(DisposeTest));
            profiler.Dispose();
        }

        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "ThScoreFileConverter.Models.Profiler.#ctor(System.String)")]
        [SuppressMessage("Microsoft.Usage", "CA2202:DoNotDisposeObjectsMultipleTimes")]
        [TestMethod()]
        public void DisposeTestTwice()
        {
            using (var profiler = new Profiler(nameof(DisposeTest)))
            {
                profiler.Dispose();
            }
        }
    }
}

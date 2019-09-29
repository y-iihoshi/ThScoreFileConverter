using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Extensions;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th09ClearCountTests
    {
        internal struct Properties
        {
            public Dictionary<Level, int> counts;

            public Properties(in Properties properties)
                => this.counts = new Dictionary<Level, int>(properties.counts);
        };

        internal static Properties ValidProperties => new Properties()
        {
            counts = Utils.GetEnumerator<Level>()
                .Select((level, index) => new { level, index })
                .ToDictionary(pair => pair.level, pair => pair.index)
        };

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(properties.counts.Values.ToArray(), 0u);

        internal static void Validate(in Th09ClearCountWrapper clearCount, in Properties properties)
            => CollectionAssert.That.AreEqual(properties.counts.Values, clearCount.Counts.Values);

        [TestMethod]
        public void Th09ClearCountTest() => TestUtils.Wrap(() =>
        {
            var clearCount = new Th09ClearCountWrapper();

            Assert.AreEqual(0, clearCount.Counts.Count);
        });

        [TestMethod]
        public void Th09ClearCountReadFromTest() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var clearCount = Th09ClearCountWrapper.Create(MakeByteArray(properties));

            Validate(clearCount, properties);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th09ClearCountReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var clearCount = new Th09ClearCountWrapper();
            clearCount.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "clearCount")]
        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th09ClearCountReadFromTestShortenedTrials() => TestUtils.Wrap(() =>
        {
            var properties = new Properties(ValidProperties);
            properties.counts.Remove(Level.Extra);

            var clearCount = Th09ClearCountWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void Th09ClearCountReadFromTestExceededTrials() => TestUtils.Wrap(() =>
        {
            var properties = new Properties(ValidProperties);
            properties.counts.Add(TestUtils.Cast<Level>(99), 99);

            var clearCount = Th09ClearCountWrapper.Create(MakeByteArray(properties));

            CollectionAssert.That.AreNotEqual(properties.counts.Values, clearCount.Counts.Values);
            CollectionAssert.That.AreEqual(properties.counts.Values.SkipLast(1), clearCount.Counts.Values);
        });
    }
}

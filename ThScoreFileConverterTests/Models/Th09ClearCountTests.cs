using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th09ClearCountTests
    {
        internal struct Properties
        {
            public Dictionary<ThConverter.Level, int> counts;

            public Properties(in Properties properties)
                => this.counts = new Dictionary<ThConverter.Level, int>(properties.counts);
        };

        internal static Properties ValidProperties => new Properties()
        {
            counts = Utils.GetEnumerator<ThConverter.Level>()
                .Select((level, index) => new { level, index })
                .ToDictionary(pair => pair.level, pair => pair.index)
        };

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(properties.counts.Values.ToArray(), 0u);

        internal static void Validate(in Th09ClearCountWrapper clearCount, in Properties properties)
            => CollectionAssert.AreEqual(properties.counts.Values, clearCount.Counts.Values.ToArray());

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
            properties.counts.Remove(ThConverter.Level.Extra);

            var clearCount = Th09ClearCountWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void Th09ClearCountReadFromTestExceededTrials() => TestUtils.Wrap(() =>
        {
            var properties = new Properties(ValidProperties);
            properties.counts.Add(TestUtils.Cast<ThConverter.Level>(99), 99);

            var clearCount = Th09ClearCountWrapper.Create(MakeByteArray(properties));

            CollectionAssert.AreNotEqual(properties.counts.Values, clearCount.Counts.Values.ToArray());
            CollectionAssert.AreEqual(
                properties.counts.Values.Take(properties.counts.Count - 1).ToArray(),
                clearCount.Counts.Values.ToArray());
        });
    }
}

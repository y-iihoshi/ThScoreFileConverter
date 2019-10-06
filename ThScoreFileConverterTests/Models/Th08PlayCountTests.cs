using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th08PlayCountTests
    {
        internal struct Properties
        {
            public int totalTrial;
            public Dictionary<Th08Converter.Chara, int> trials;
            public int totalClear;
            public int totalContinue;
            public int totalPractice;

            public Properties(in Properties properties)
            {
                this.totalTrial = properties.totalTrial;
                this.trials = new Dictionary<Th08Converter.Chara, int>(properties.trials);
                this.totalClear = properties.totalClear;
                this.totalContinue = properties.totalContinue;
                this.totalPractice = properties.totalPractice;
            }
        };

        internal static Properties ValidProperties => new Properties()
        {
            totalTrial = 1,
            trials = Utils.GetEnumerator<Th08Converter.Chara>()
                .Select((chara, index) => new { chara, index })
                .ToDictionary(pair => pair.chara, pair => pair.index),
            totalClear = 3,
            totalContinue = 4,
            totalPractice = 5,
        };

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.totalTrial,
                properties.trials.Values.ToArray(),
                0u,
                properties.totalClear,
                properties.totalContinue,
                properties.totalPractice);

        internal static void Validate(in Th08PlayCountWrapper playCount, in Properties properties)
        {
            Assert.AreEqual(properties.totalTrial, playCount.TotalTrial.Value);
            CollectionAssert.That.AreEqual(properties.trials.Values, playCount.Trials.Values);
            Assert.AreEqual(properties.totalClear, playCount.TotalClear.Value);
            Assert.AreEqual(properties.totalContinue, playCount.TotalContinue.Value);
            Assert.AreEqual(properties.totalPractice, playCount.TotalPractice.Value);
        }

        [TestMethod]
        public void Th08PlayCountTest() => TestUtils.Wrap(() =>
        {
            var playCount = new Th08PlayCountWrapper();

            Assert.AreEqual(default, playCount.TotalTrial.Value);
            Assert.AreEqual(0, playCount.Trials.Count);
            Assert.AreEqual(default, playCount.TotalClear.Value);
            Assert.AreEqual(default, playCount.TotalContinue.Value);
            Assert.AreEqual(default, playCount.TotalPractice.Value);
        });

        [TestMethod]
        public void Th08PlayCountReadFromTest() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var playCount = Th08PlayCountWrapper.Create(MakeByteArray(properties));

            Validate(playCount, properties);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th08PlayCountReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var playCount = new Th08PlayCountWrapper();
            playCount.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "playCount")]
        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th08PlayCountReadFromTestShortenedTrials() => TestUtils.Wrap(() =>
        {
            var properties = new Properties(ValidProperties);
            properties.trials.Remove(Th08Converter.Chara.Yuyuko);

            var playCount = Th08PlayCountWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void Th08PlayCountReadFromTestExceededTrials() => TestUtils.Wrap(() =>
        {
            var properties = new Properties(ValidProperties);
            properties.trials.Add(TestUtils.Cast<Th08Converter.Chara>(99), 99);

            var playCount = Th08PlayCountWrapper.Create(MakeByteArray(properties));

            Assert.AreEqual(properties.totalTrial, playCount.TotalTrial.Value);
            CollectionAssert.That.AreNotEqual(properties.trials.Values, playCount.Trials.Values);
            CollectionAssert.That.AreEqual(properties.trials.Values.SkipLast(1), playCount.Trials.Values);
            Assert.AreNotEqual(properties.totalClear, playCount.TotalClear.Value);
            Assert.AreNotEqual(properties.totalContinue, playCount.TotalContinue.Value);
            Assert.AreNotEqual(properties.totalPractice, playCount.TotalPractice.Value);
        });
    }
}

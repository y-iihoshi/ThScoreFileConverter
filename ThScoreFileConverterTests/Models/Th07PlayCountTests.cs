using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass()]
    public class Th07PlayCountTests
    {
        internal struct Properties
        {
            public int totalTrial;
            public Dictionary<Th07Converter.Chara, int> trials;
            public int totalRetry;
            public int totalClear;
            public int totalContinue;
            public int totalPractice;

            public Properties(in Properties properties)
            {
                this.totalTrial = properties.totalTrial;
                this.trials = new Dictionary<Th07Converter.Chara, int>(properties.trials);
                this.totalRetry = properties.totalRetry;
                this.totalClear = properties.totalClear;
                this.totalContinue = properties.totalContinue;
                this.totalPractice = properties.totalPractice;
            }
        };

        internal static Properties ValidProperties => new Properties()
        {
            totalTrial = 1,
            trials = Utils.GetEnumerator<Th07Converter.Chara>()
                .Select((chara, index) => new { chara, index })
                .ToDictionary(pair => pair.chara, pair => pair.index),
            totalRetry = 2,
            totalClear = 3,
            totalContinue = 4,
            totalPractice = 5
        };

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.totalTrial,
                properties.trials.Values.ToArray(),
                properties.totalRetry,
                properties.totalClear,
                properties.totalContinue,
                properties.totalPractice);

        internal static void Validate(in Th07PlayCountWrapper playCount, in Properties properties)
        {
            Assert.AreEqual(properties.totalTrial, playCount.TotalTrial.Value);
            CollectionAssert.AreEqual(properties.trials.Values, playCount.Trials.Values.ToArray());
            Assert.AreEqual(properties.totalRetry, playCount.TotalRetry.Value);
            Assert.AreEqual(properties.totalClear, playCount.TotalClear.Value);
            Assert.AreEqual(properties.totalContinue, playCount.TotalContinue.Value);
            Assert.AreEqual(properties.totalPractice, playCount.TotalPractice.Value);
        }

        [TestMethod()]
        public void Th07PlayCountTest() => TestUtils.Wrap(() =>
        {
            var playCount = new Th07PlayCountWrapper();

            Assert.AreEqual(default, playCount.TotalTrial.Value);
            Assert.AreEqual(0, playCount.Trials.Count);
            Assert.AreEqual(default, playCount.TotalRetry.Value);
            Assert.AreEqual(default, playCount.TotalClear.Value);
            Assert.AreEqual(default, playCount.TotalContinue.Value);
            Assert.AreEqual(default, playCount.TotalPractice.Value);
        });

        [TestMethod()]
        public void Th07PlayCountReadFromTest() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var playCount = Th07PlayCountWrapper.Create(MakeByteArray(properties));

            Validate(playCount, properties);
        });

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th07PlayCountReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var playCount = new Th07PlayCountWrapper();
            playCount.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "playCount")]
        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th07PlayCountReadFromTestShortenedTrials() => TestUtils.Wrap(() =>
        {
            var properties = new Properties(ValidProperties);
            properties.trials.Remove(Th07Converter.Chara.SakuyaB);

            var playCount = Th07PlayCountWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod()]
        public void Th07PlayCountReadFromTestExceededTrials() => TestUtils.Wrap(() =>
        {
            var properties = new Properties(ValidProperties);
            properties.trials.Add(TestUtils.Cast<Th07Converter.Chara>(99), 99);

            var playCount = Th07PlayCountWrapper.Create(MakeByteArray(properties));

            Assert.AreEqual(properties.totalTrial, playCount.TotalTrial.Value);
            CollectionAssert.AreNotEqual(properties.trials.Values, playCount.Trials.Values.ToArray());
            CollectionAssert.AreEqual(
                properties.trials.Values.Take(properties.trials.Count - 1).ToArray(),
                playCount.Trials.Values.ToArray());
            Assert.AreNotEqual(properties.totalRetry, playCount.TotalRetry.Value);
            Assert.AreNotEqual(properties.totalClear, playCount.TotalClear.Value);
            Assert.AreNotEqual(properties.totalContinue, playCount.TotalContinue.Value);
            Assert.AreNotEqual(properties.totalPractice, playCount.TotalPractice.Value);
        });
    }
}

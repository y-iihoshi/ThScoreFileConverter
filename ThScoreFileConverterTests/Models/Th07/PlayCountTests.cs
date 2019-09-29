using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th07;
using ThScoreFileConverterTests.Extensions;

namespace ThScoreFileConverterTests.Models.Th07
{
    [TestClass]
    public class PlayCountTests
    {
        internal struct Properties
        {
            public int totalTrial;
            public Dictionary<Chara, int> trials;
            public int totalRetry;
            public int totalClear;
            public int totalContinue;
            public int totalPractice;

            public Properties(in Properties properties)
            {
                this.totalTrial = properties.totalTrial;
                this.trials = new Dictionary<Chara, int>(properties.trials);
                this.totalRetry = properties.totalRetry;
                this.totalClear = properties.totalClear;
                this.totalContinue = properties.totalContinue;
                this.totalPractice = properties.totalPractice;
            }
        };

        internal static Properties ValidProperties => new Properties()
        {
            totalTrial = 1,
            trials = Utils.GetEnumerator<Chara>()
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

        internal static PlayCount Create(byte[] array)
        {
            var playCount = new PlayCount();

            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(array);
                using (var reader = new BinaryReader(stream))
                {
                    stream = null;
                    playCount.ReadFrom(reader);
                }
            }
            finally
            {
                stream?.Dispose();
            }

            return playCount;
        }

        internal static void Validate(in PlayCount playCount, in Properties properties)
        {
            Assert.AreEqual(properties.totalTrial, playCount.TotalTrial);
            CollectionAssert.That.AreEqual(properties.trials.Values, playCount.Trials.Values);
            Assert.AreEqual(properties.totalRetry, playCount.TotalRetry);
            Assert.AreEqual(properties.totalClear, playCount.TotalClear);
            Assert.AreEqual(properties.totalContinue, playCount.TotalContinue);
            Assert.AreEqual(properties.totalPractice, playCount.TotalPractice);
        }

        [TestMethod]
        public void PlayCountTest() => TestUtils.Wrap(() =>
        {
            var playCount = new PlayCount();

            Assert.AreEqual(default, playCount.TotalTrial);
            Assert.AreEqual(0, playCount.Trials.Count);
            Assert.AreEqual(default, playCount.TotalRetry);
            Assert.AreEqual(default, playCount.TotalClear);
            Assert.AreEqual(default, playCount.TotalContinue);
            Assert.AreEqual(default, playCount.TotalPractice);
        });

        [TestMethod]
        public void ReadFromTest() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var playCount = Create(MakeByteArray(properties));

            Validate(playCount, properties);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var playCount = new PlayCount();
            playCount.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestShortenedTrials() => TestUtils.Wrap(() =>
        {
            var properties = new Properties(ValidProperties);
            properties.trials.Remove(Chara.SakuyaB);

            _ = Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void ReadFromTestExceededTrials() => TestUtils.Wrap(() =>
        {
            var properties = new Properties(ValidProperties);
            properties.trials.Add(TestUtils.Cast<Chara>(99), 99);

            var playCount = Create(MakeByteArray(properties));

            Assert.AreEqual(properties.totalTrial, playCount.TotalTrial);
            CollectionAssert.That.AreNotEqual(properties.trials.Values, playCount.Trials.Values);
            CollectionAssert.That.AreEqual(properties.trials.Values.SkipLast(1), playCount.Trials.Values);
            Assert.AreNotEqual(properties.totalRetry, playCount.TotalRetry);
            Assert.AreNotEqual(properties.totalClear, playCount.TotalClear);
            Assert.AreNotEqual(properties.totalContinue, playCount.TotalContinue);
            Assert.AreNotEqual(properties.totalPractice, playCount.TotalPractice);
        });
    }
}

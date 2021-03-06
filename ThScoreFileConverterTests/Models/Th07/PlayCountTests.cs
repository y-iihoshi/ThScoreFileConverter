﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models.Th07;
using ThScoreFileConverterTests.UnitTesting;

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
        }

        internal static Properties ValidProperties { get; } = new Properties()
        {
            totalTrial = 1,
            trials = EnumHelper<Chara>.Enumerable.Select((chara, index) => (chara, index)).ToDictionary(),
            totalRetry = 2,
            totalClear = 3,
            totalContinue = 4,
            totalPractice = 5,
        };

        internal static byte[] MakeByteArray(in Properties properties)
        {
            return TestUtils.MakeByteArray(
                properties.totalTrial,
                properties.trials.Values,
                properties.totalRetry,
                properties.totalClear,
                properties.totalContinue,
                properties.totalPractice);
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
        public void PlayCountTest()
        {
            var playCount = new PlayCount();

            Assert.AreEqual(default, playCount.TotalTrial);
            Assert.AreEqual(0, playCount.Trials.Count);
            Assert.AreEqual(default, playCount.TotalRetry);
            Assert.AreEqual(default, playCount.TotalClear);
            Assert.AreEqual(default, playCount.TotalContinue);
            Assert.AreEqual(default, playCount.TotalPractice);
        }

        [TestMethod]
        public void ReadFromTest()
        {
            var properties = ValidProperties;

            var playCount = TestUtils.Create<PlayCount>(MakeByteArray(properties));

            Validate(playCount, properties);
        }

        [TestMethod]
        public void ReadFromTestShortenedTrials()
        {
            var properties = new Properties(ValidProperties);
            _ = properties.trials.Remove(Chara.SakuyaB);

            _ = Assert.ThrowsException<EndOfStreamException>(
                () => TestUtils.Create<PlayCount>(MakeByteArray(properties)));
        }

        [TestMethod]
        public void ReadFromTestExceededTrials()
        {
            var properties = new Properties(ValidProperties);
            properties.trials.Add(TestUtils.Cast<Chara>(99), 99);

            var playCount = TestUtils.Create<PlayCount>(MakeByteArray(properties));

            Assert.AreEqual(properties.totalTrial, playCount.TotalTrial);
            CollectionAssert.That.AreNotEqual(properties.trials.Values, playCount.Trials.Values);
            CollectionAssert.That.AreEqual(properties.trials.Values.SkipLast(1), playCount.Trials.Values);
            Assert.AreNotEqual(properties.totalRetry, playCount.TotalRetry);
            Assert.AreNotEqual(properties.totalClear, playCount.TotalClear);
            Assert.AreNotEqual(properties.totalContinue, playCount.TotalContinue);
            Assert.AreNotEqual(properties.totalPractice, playCount.TotalPractice);
        }
    }
}

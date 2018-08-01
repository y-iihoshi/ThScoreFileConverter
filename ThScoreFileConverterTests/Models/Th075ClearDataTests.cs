using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass()]
    public class Th075ClearDataTests
    {
        internal struct Properties
        {
            public int useCount;
            public int clearCount;
            public int maxCombo;
            public int maxDamage;
            public List<int> maxBonuses;
            public List<short> cardGotCount;
            public List<short> cardTrialCount;
            public List<byte> cardTrulyGot;
            public List<Th075HighScoreTests.Properties> ranking;
        };

        internal static Properties DefaultProperties => new Properties()
        {
            useCount = default,
            clearCount = default,
            maxCombo = default,
            maxDamage = default,
            maxBonuses = new List<int>(),
            cardGotCount = new List<short>(),
            cardTrialCount = new List<short>(),
            cardTrulyGot = new List<byte>(),
            ranking = new List<Th075HighScoreTests.Properties>()
        };

        internal static Properties ValidProperties => new Properties()
        {
            useCount = 1234,
            clearCount = 2345,
            maxCombo = 3456,
            maxDamage = 4567,
            maxBonuses = TestUtils.MakeRandomArray<int>(100).ToList(),
            cardGotCount = TestUtils.MakeRandomArray<short>(100).ToList(),
            cardTrialCount = TestUtils.MakeRandomArray<short>(100).ToList(),
            cardTrulyGot = TestUtils.MakeRandomArray<byte>(100).ToList(),
            ranking = Enumerable.Range(0, 10)
                .Select(index => new Th075HighScoreTests.Properties()
                {
                    encodedName = new byte[] { 15, 37, 26, 50, 30, 43, (byte)(52 + index), 103 },
                    decodedName = Utils.Format("Player{0} ", index),
                    month = (byte)(1 + index),
                    day = (byte)(10 + index),
                    score = 1234567 + index
                }).ToList()
        };

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.useCount,
                properties.clearCount,
                properties.maxCombo,
                properties.maxDamage,
                properties.maxBonuses.ToArray(),
                new byte[0xC8],
                properties.cardGotCount.ToArray(),
                new byte[0x64],
                properties.cardTrialCount.ToArray(),
                new byte[0x64],
                properties.cardTrulyGot.ToArray(),
                new byte[0x38],
                properties.ranking.SelectMany(element => Th075HighScoreTests.MakeByteArray(element)).ToArray());

        internal static void Validate(in Th075ClearDataWrapper clearData, in Properties properties)
        {
            Assert.AreEqual(properties.useCount, clearData.UseCount);
            Assert.AreEqual(properties.clearCount, clearData.ClearCount);
            Assert.AreEqual(properties.maxCombo, clearData.MaxCombo);
            Assert.AreEqual(properties.maxDamage, clearData.MaxDamage);
            CollectionAssert.AreEqual(properties.maxBonuses, clearData.MaxBonuses.ToArray());
            CollectionAssert.AreEqual(properties.cardGotCount, clearData.CardGotCount.ToArray());
            CollectionAssert.AreEqual(properties.cardTrialCount, clearData.CardTrialCount.ToArray());
            CollectionAssert.AreEqual(properties.cardTrulyGot, clearData.CardTrulyGot.ToArray());
            Assert.AreEqual(properties.ranking.Count, clearData.RankingCount);
            foreach (var index in Enumerable.Range(0, properties.ranking.Count))
            {
                var propHighScore = properties.ranking[index];
                var highScore = clearData.RankingItem(index);
                Assert.AreEqual(propHighScore.decodedName, highScore.Name);
                Assert.AreEqual(propHighScore.month, highScore.Month);
                Assert.AreEqual(propHighScore.day, highScore.Day);
                Assert.AreEqual(propHighScore.score, highScore.Score);
            }
        }

        [TestMethod()]
        public void Th075ClearDataTest() => TestUtils.Wrap(() =>
        {
            var clearData = new Th075ClearDataWrapper();

            Validate(clearData, DefaultProperties);
        });

        [TestMethod()]
        public void ReadFromTest() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var clearData = Th075ClearDataWrapper.Create(MakeByteArray(properties));

            Validate(clearData, properties);
        });

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var clearData = new Th075ClearDataWrapper();
            clearData.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}

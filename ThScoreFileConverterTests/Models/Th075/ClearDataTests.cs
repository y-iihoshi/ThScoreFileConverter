using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th075;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th075.Stubs;

namespace ThScoreFileConverterTests.Models.Th075
{
    [TestClass]
    public class ClearDataTests
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
            public List<IHighScore> ranking;
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
            ranking = new List<IHighScore>()
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
                .Select(index => new HighScoreStub()
                {
                    EncodedName = new byte[] { 15, 37, 26, 50, 30, 43, (byte)(52 + index), 103 },
                    Name = Utils.Format("Player{0} ", index),
                    Month = (byte)(1 + index),
                    Day = (byte)(10 + index),
                    Score = 1234567 + index
                } as IHighScore).ToList()
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
                properties.ranking.SelectMany(
                    element => HighScoreTests.MakeByteArray(element as HighScoreStub)).ToArray());

        internal static void Validate(in Properties properties, in ClearData clearData)
        {
            Assert.AreEqual(properties.useCount, clearData.UseCount);
            Assert.AreEqual(properties.clearCount, clearData.ClearCount);
            Assert.AreEqual(properties.maxCombo, clearData.MaxCombo);
            Assert.AreEqual(properties.maxDamage, clearData.MaxDamage);
            CollectionAssert.That.AreEqual(properties.maxBonuses, clearData.MaxBonuses);
            CollectionAssert.That.AreEqual(properties.cardGotCount, clearData.CardGotCount);
            CollectionAssert.That.AreEqual(properties.cardTrialCount, clearData.CardTrialCount);
            CollectionAssert.That.AreEqual(properties.cardTrulyGot, clearData.CardTrulyGot);
            Assert.AreEqual(properties.ranking.Count, clearData.Ranking.Count);
            foreach (var index in Enumerable.Range(0, properties.ranking.Count))
            {
                var highScoreStub = properties.ranking[index];
                var highScore = clearData.Ranking[index];
                Assert.AreEqual(highScoreStub.Name, highScore.Name);
                Assert.AreEqual(highScoreStub.Month, highScore.Month);
                Assert.AreEqual(highScoreStub.Day, highScore.Day);
                Assert.AreEqual(highScoreStub.Score, highScore.Score);
            }
        }

        [TestMethod]
        public void ClearDataTest() => TestUtils.Wrap(() =>
        {
            var clearData = new ClearData();

            Validate(DefaultProperties, clearData);
        });

        [TestMethod]
        public void ReadFromTest() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var clearData = TestUtils.Create<ClearData>(MakeByteArray(properties));

            Validate(properties, clearData);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var clearData = new ClearData();
            clearData.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Th13;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th15ClearDataPerGameModeTests
    {
        internal struct Properties
        {
            public Dictionary<ThConverter.LevelWithTotal, Th15ScoreDataTests.Properties[]> rankings;
            public int totalPlayCount;
            public int playTime;
            public Dictionary<ThConverter.LevelWithTotal, int> clearCounts;
            public Dictionary<ThConverter.LevelWithTotal, int> clearFlags;
            public Dictionary<int, SpellCardTests.Properties<Level>> cards;
        };

        internal static Properties GetValidProperties()
        {
            var levelsWithTotal = Utils.GetEnumerator<ThConverter.LevelWithTotal>();

            return new Properties()
            {
                rankings = levelsWithTotal.ToDictionary(
                    level => level,
                    level => Enumerable.Range(0, 10).Select(
                        index => new Th15ScoreDataTests.Properties()
                        {
                            score = 12345670u - (uint)index * 1000u,
                            stageProgress = Th15Converter.StageProgress.St6,
                            continueCount = (byte)index,
                            name = TestUtils.MakeRandomArray<byte>(10),
                            dateTime = 34567890u,
                            slowRate = 1.2f,
                            retryCount = (uint)index / 4u
                        }).ToArray()),
                totalPlayCount = 23,
                playTime = 4567890,
                clearCounts = levelsWithTotal.ToDictionary(level => level, level => 100 - TestUtils.Cast<int>(level)),
                clearFlags = levelsWithTotal.ToDictionary(level => level, level => TestUtils.Cast<int>(level) % 2),
                cards = Enumerable.Range(1, 119).ToDictionary(
                    index => index,
                    index => new SpellCardTests.Properties<Level>()
                    {
                        name = TestUtils.MakeRandomArray<byte>(0x80),
                        clearCount = 12 + index,
                        practiceClearCount = 34 + index,
                        trialCount = 56 + index,
                        practiceTrialCount = 78 + index,
                        id = index,
                        level = Level.Hard,
                        practiceScore = 90123
                    })
            };
        }

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.rankings.Values.SelectMany(
                    ranking => ranking.SelectMany(
                        scoreData => Th15ScoreDataTests.MakeByteArray(scoreData))).ToArray(),
                new byte[0x140],
                properties.cards.Values.SelectMany(
                    card => SpellCardTests.MakeByteArray(card)).ToArray(),
                properties.totalPlayCount,
                properties.playTime,
                0u,
                properties.clearCounts.Values.ToArray(),
                0u,
                properties.clearFlags.Values.ToArray(),
                0u);

        internal static void Validate(in Th15ClearDataPerGameModeWrapper clearData, in Properties properties)
        {
            foreach (var pair in properties.rankings)
            {
                for (var index = 0; index < pair.Value.Length; ++index)
                {
                    Th15ScoreDataTests.Validate(clearData.RankingItem(pair.Key, index), pair.Value[index]);
                }
            }

            Assert.AreEqual(properties.totalPlayCount, clearData.TotalPlayCount);
            Assert.AreEqual(properties.playTime, clearData.PlayTime);
            CollectionAssert.AreEqual(properties.clearCounts.Values, clearData.ClearCounts.Values.ToArray());
            CollectionAssert.AreEqual(properties.clearFlags.Values, clearData.ClearFlags.Values.ToArray());

            foreach (var pair in properties.cards)
            {
                SpellCardTests.Validate(clearData.CardsItem(pair.Key), pair.Value);
            }
        }

        [TestMethod]
        public void Th15ClearDataPerGameModeTest() => TestUtils.Wrap(() =>
        {
            var clearData = new Th15ClearDataPerGameModeWrapper();

            Assert.IsNull(clearData.Rankings);
            Assert.AreEqual(default, clearData.TotalPlayCount.Value);
            Assert.AreEqual(default, clearData.PlayTime.Value);
            Assert.IsNull(clearData.ClearCounts);
            Assert.IsNull(clearData.ClearFlags);
            Assert.IsNull(clearData.Cards);
        });

        [TestMethod]
        public void Th15ClearDataPerGameModeReadFromTest() => TestUtils.Wrap(() =>
        {
            var properties = GetValidProperties();

            var clearData = Th15ClearDataPerGameModeWrapper.Create(MakeByteArray(properties));

            Validate(clearData, properties);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th15ClearDataPerGameModeReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var clearData = new Th15ClearDataPerGameModeWrapper();
            clearData.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "clearData")]
        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th15ClearDataPerGameModeReadFromTestShortened() => TestUtils.Wrap(() =>
        {
            var properties = GetValidProperties();
            var array = MakeByteArray(properties);
            array = array.Take(array.Length - 1).ToArray();

            var clearData = Th15ClearDataPerGameModeWrapper.Create(array);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void Th15ClearDataPerGameModeReadFromTestExceeded() => TestUtils.Wrap(() =>
        {
            var properties = GetValidProperties();
            var array = MakeByteArray(properties).Concat(new byte[1] { 1 }).ToArray();

            var clearData = Th15ClearDataPerGameModeWrapper.Create(array);

            Validate(clearData, properties);
        });
    }
}

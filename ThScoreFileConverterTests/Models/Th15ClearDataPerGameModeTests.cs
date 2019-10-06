using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th13;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th13;
using ThScoreFileConverterTests.Models.Th13.Stubs;
using ThScoreFileConverterTests.Models.Th15.Stubs;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th15ClearDataPerGameModeTests
    {
        internal struct Properties
        {
            public Dictionary<LevelWithTotal, ScoreDataStub[]> rankings;
            public int totalPlayCount;
            public int playTime;
            public Dictionary<LevelWithTotal, int> clearCounts;
            public Dictionary<LevelWithTotal, int> clearFlags;
            public Dictionary<int, ISpellCard<Level>> cards;
        };

        internal static Properties GetValidProperties()
        {
            var levelsWithTotal = Utils.GetEnumerator<LevelWithTotal>();

            return new Properties()
            {
                rankings = levelsWithTotal.ToDictionary(
                    level => level,
                    level => Enumerable.Range(0, 10).Select(
                        index => new ScoreDataStub()
                        {
                            Score = 12345670u - (uint)index * 1000u,
                            StageProgress = Th15Converter.StageProgress.St6,
                            ContinueCount = (byte)index,
                            Name = TestUtils.MakeRandomArray<byte>(10),
                            DateTime = 34567890u,
                            SlowRate = 1.2f,
                            RetryCount = (uint)index / 4u
                        }).ToArray()),
                totalPlayCount = 23,
                playTime = 4567890,
                clearCounts = levelsWithTotal.ToDictionary(level => level, level => 100 - TestUtils.Cast<int>(level)),
                clearFlags = levelsWithTotal.ToDictionary(level => level, level => TestUtils.Cast<int>(level) % 2),
                cards = Enumerable.Range(1, 119).ToDictionary(
                    index => index,
                    index => new SpellCardStub<Level>()
                    {
                        Name = TestUtils.MakeRandomArray<byte>(0x80),
                        ClearCount = 12 + index,
                        PracticeClearCount = 34 + index,
                        TrialCount = 56 + index,
                        PracticeTrialCount = 78 + index,
                        Id = index,
                        Level = Level.Hard,
                        PracticeScore = 90123
                    } as ISpellCard<Level>)
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
                    Th15ScoreDataTests.Validate(pair.Value[index], clearData.RankingItem(pair.Key, index));
                }
            }

            Assert.AreEqual(properties.totalPlayCount, clearData.TotalPlayCount);
            Assert.AreEqual(properties.playTime, clearData.PlayTime);
            CollectionAssert.That.AreEqual(properties.clearCounts.Values, clearData.ClearCounts.Values);
            CollectionAssert.That.AreEqual(properties.clearFlags.Values, clearData.ClearFlags.Values);

            foreach (var pair in properties.cards)
            {
                SpellCardTests.Validate(pair.Value, clearData.CardsItem(pair.Key));
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

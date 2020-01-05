using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th15;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th15.Stubs;
using ISpellCard = ThScoreFileConverter.Models.Th13.ISpellCard<ThScoreFileConverter.Models.Level>;
using SpellCardStub = ThScoreFileConverterTests.Models.Th13.Stubs.SpellCardStub<ThScoreFileConverter.Models.Level>;
using StageProgress = ThScoreFileConverter.Models.Th13.StageProgress;

namespace ThScoreFileConverterTests.Models.Th15
{
    [TestClass]
    public class ClearDataPerGameModeTests
    {
        internal static ClearDataPerGameModeStub MakeValidStub()
        {
            var levelsWithTotal = Utils.GetEnumerator<LevelWithTotal>();

            return new ClearDataPerGameModeStub
            {
                Rankings = levelsWithTotal.ToDictionary(
                    level => level,
                    level => Enumerable.Range(0, 10).Select(
                        index => new ScoreDataStub
                        {
                            Score = 12345670u - (uint)index * 1000u,
                            StageProgress = StageProgress.Five,
                            ContinueCount = (byte)index,
                            Name = TestUtils.CP932Encoding.GetBytes($"Player{index}\0\0\0"),
                            DateTime = 34567890u,
                            SlowRate = 1.2f,
                            RetryCount = (uint)index % 4u
                        }).ToList() as IReadOnlyList<IScoreData>),
                TotalPlayCount = 23,
                PlayTime = 4567890,
                ClearCounts = levelsWithTotal.ToDictionary(level => level, level => 100 - TestUtils.Cast<int>(level)),
                ClearFlags = levelsWithTotal.ToDictionary(level => level, level => TestUtils.Cast<int>(level) % 2),
                Cards = Enumerable.Range(1, 119).ToDictionary(
                    index => index,
                    index => new SpellCardStub
                    {
                        Name = TestUtils.MakeRandomArray<byte>(0x80),
                        ClearCount = 12 + index,
                        PracticeClearCount = 34 + index,
                        TrialCount = 56 + index,
                        PracticeTrialCount = 78 + index,
                        Id = index,
                        Level = Level.Hard,
                        PracticeScore = 90123
                    } as ISpellCard)
            };
        }

        internal static byte[] MakeByteArray(IClearDataPerGameMode clearData)
            => TestUtils.MakeByteArray(
                clearData.Rankings.Values.SelectMany(
                    ranking => ranking.SelectMany(scoreData => ScoreDataTests.MakeByteArray(scoreData))).ToArray(),
                new byte[0x140],
                clearData.Cards.Values.SelectMany(card => SpellCardTests.MakeByteArray(card)).ToArray(),
                clearData.TotalPlayCount,
                clearData.PlayTime,
                0u,
                clearData.ClearCounts.Values.ToArray(),
                0u,
                clearData.ClearFlags.Values.ToArray(),
                0u);

        internal static void Validate(IClearDataPerGameMode expected, IClearDataPerGameMode actual)
        {
            foreach (var pair in expected.Rankings)
            {
                for (var index = 0; index < pair.Value.Count(); ++index)
                {
                    ScoreDataTests.Validate(pair.Value[index], actual.Rankings[pair.Key][index]);
                }
            }

            Assert.AreEqual(expected.TotalPlayCount, actual.TotalPlayCount);
            Assert.AreEqual(expected.PlayTime, actual.PlayTime);
            CollectionAssert.That.AreEqual(expected.ClearCounts.Values, actual.ClearCounts.Values);
            CollectionAssert.That.AreEqual(expected.ClearFlags.Values, actual.ClearFlags.Values);

            foreach (var pair in expected.Cards)
            {
                SpellCardTests.Validate(pair.Value, actual.Cards[pair.Key]);
            }
        }

        [TestMethod]
        public void ClearDataPerGameModeTest() => TestUtils.Wrap(() =>
        {
            var stub = new ClearDataPerGameModeStub();

            var clearData = new ClearDataPerGameMode();

            Validate(stub, clearData);
        });

        [TestMethod]
        public void ReadFromTest() => TestUtils.Wrap(() =>
        {
            var stub = MakeValidStub();

            var clearData = TestUtils.Create<ClearDataPerGameMode>(MakeByteArray(stub));

            Validate(stub, clearData);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var clearData = new ClearDataPerGameMode();
            clearData.ReadFrom(null!);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestShortened() => TestUtils.Wrap(() =>
        {
            var stub = MakeValidStub();
            var array = MakeByteArray(stub).SkipLast(1).ToArray();

            _ = TestUtils.Create<ClearDataPerGameMode>(array);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void ReadFromTestExceeded() => TestUtils.Wrap(() =>
        {
            var stub = MakeValidStub();
            var array = MakeByteArray(stub).Concat(new byte[1] { 1 }).ToArray();

            var clearData = TestUtils.Create<ClearDataPerGameMode>(array);

            Validate(stub, clearData);
        });
    }
}

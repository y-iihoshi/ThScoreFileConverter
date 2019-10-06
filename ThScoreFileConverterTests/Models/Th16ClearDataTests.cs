using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th13;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th10.Wrappers;
using ThScoreFileConverterTests.Models.Th13;
using ThScoreFileConverterTests.Models.Th13.Stubs;
using ThScoreFileConverterTests.Models.Th16.Stubs;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th16ClearDataTests
    {
        internal struct Properties
        {
            public string signature;
            public ushort version;
            public uint checksum;
            public int size;
            public Th16Converter.CharaWithTotal chara;
            public Dictionary<LevelWithTotal, ScoreDataStub[]> rankings;
            public int totalPlayCount;
            public int playTime;
            public Dictionary<LevelWithTotal, int> clearCounts;
            public Dictionary<LevelWithTotal, int> clearFlags;
            public Dictionary<(Level, Th16Converter.StagePractice), IPractice> practices;
            public Dictionary<int, ISpellCard<Level>> cards;
        };

        internal static Properties GetValidProperties()
        {
            var levels = Utils.GetEnumerator<Level>();
            var levelsWithTotal = Utils.GetEnumerator<LevelWithTotal>();
            var stages = Utils.GetEnumerator<Th16Converter.StagePractice>();

            return new Properties()
            {
                signature = "CR",
                version = 1,
                checksum = 0u,
                size = 0x5318,
                chara = Th16Converter.CharaWithTotal.Aya,
                rankings = levelsWithTotal.ToDictionary(
                    level => level,
                    level => Enumerable.Range(0, 10).Select(
                        index => new ScoreDataStub()
                        {
                            Score = 12345670u - (uint)index * 1000u,
                            StageProgress = Th16Converter.StageProgress.St6,
                            ContinueCount = (byte)index,
                            Name = TestUtils.MakeRandomArray<byte>(10),
                            DateTime = 34567890u,
                            SlowRate = 1.2f,
                            Season = Th16Converter.Season.Autumn
                        }).ToArray()),
                totalPlayCount = 23,
                playTime = 4567890,
                clearCounts = levelsWithTotal.ToDictionary(level => level, level => 100 - TestUtils.Cast<int>(level)),
                clearFlags = levelsWithTotal.ToDictionary(level => level, level => TestUtils.Cast<int>(level) % 2),
                practices = levels
                    .SelectMany(level => stages.Select(stage => (level, stage)))
                    .ToDictionary(
                        pair => pair,
                        pair => new PracticeStub()
                        {
                            Score = 123456u - TestUtils.Cast<uint>(pair.level) * 10u,
                            ClearFlag = (byte)(TestUtils.Cast<int>(pair.stage) % 2),
                            EnableFlag = (byte)(TestUtils.Cast<int>(pair.level) % 2)
                        } as IPractice),
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

        internal static byte[] MakeData(in Properties properties)
            => TestUtils.MakeByteArray(
                (int)properties.chara,
                properties.rankings.Values.SelectMany(
                    ranking => ranking.SelectMany(
                        scoreData => Th16ScoreDataTests.MakeByteArray(scoreData))).ToArray(),
                new byte[0x140],
                properties.cards.Values.SelectMany(
                    card => SpellCardTests.MakeByteArray(card)).ToArray(),
                properties.totalPlayCount,
                properties.playTime,
                0u,
                properties.clearCounts.Values.ToArray(),
                0u,
                properties.clearFlags.Values.ToArray(),
                0u,
                properties.practices.Values.SelectMany(
                    practice => PracticeTests.MakeByteArray(practice)).ToArray(),
                new byte[0x40]);

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(),
                properties.version,
                properties.checksum,
                properties.size,
                MakeData(properties));

        internal static void Validate(in Th16ClearDataWrapper clearData, in Properties properties)
        {
            var data = MakeData(properties);

            Assert.AreEqual(properties.signature, clearData.Signature);
            Assert.AreEqual(properties.version, clearData.Version);
            Assert.AreEqual(properties.checksum, clearData.Checksum);
            Assert.AreEqual(properties.size, clearData.Size);
            CollectionAssert.That.AreEqual(data, clearData.Data);
            Assert.AreEqual(properties.chara, clearData.Chara);

            foreach (var pair in properties.rankings)
            {
                for (var index = 0; index < pair.Value.Length; ++index)
                {
                    Th16ScoreDataTests.Validate(pair.Value[index], clearData.RankingItem(pair.Key, index));
                }
            }

            Assert.AreEqual(properties.totalPlayCount, clearData.TotalPlayCount);
            Assert.AreEqual(properties.playTime, clearData.PlayTime);
            CollectionAssert.That.AreEqual(properties.clearCounts.Values, clearData.ClearCounts.Values);
            CollectionAssert.That.AreEqual(properties.clearFlags.Values, clearData.ClearFlags.Values);

            foreach (var pair in properties.practices)
            {
                PracticeTests.Validate(pair.Value, clearData.Practices[pair.Key]);
            }

            foreach (var pair in properties.cards)
            {
                SpellCardTests.Validate(pair.Value, clearData.CardsItem(pair.Key));
            }
        }

        [TestMethod]
        public void Th16ClearDataTestChapter() => TestUtils.Wrap(() =>
        {
            var properties = GetValidProperties();

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var clearData = new Th16ClearDataWrapper(chapter);

            Validate(clearData, properties);
            Assert.IsFalse(clearData.IsValid.Value);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "clearData")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th16ClearDataTestNullChapter() => TestUtils.Wrap(() =>
        {
            var clearData = new Th16ClearDataWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "clearData")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th16ClearDataTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var properties = GetValidProperties();
            properties.signature = properties.signature.ToLowerInvariant();

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var clearData = new Th16ClearDataWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "clearData")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th16ClearDataTestInvalidVersion() => TestUtils.Wrap(() =>
        {
            var properties = GetValidProperties();
            ++properties.version;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var clearData = new Th16ClearDataWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "clearData")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th16ClearDataTestInvalidSize() => TestUtils.Wrap(() =>
        {
            var properties = GetValidProperties();
            --properties.size;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var clearData = new Th16ClearDataWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("CR", (ushort)1, 0x5318, true)]
        [DataRow("cr", (ushort)1, 0x5318, false)]
        [DataRow("CR", (ushort)0, 0x5318, false)]
        [DataRow("CR", (ushort)1, 0x5319, false)]
        public void Th16ClearDataCanInitializeTest(string signature, ushort version, int size, bool expected)
            => TestUtils.Wrap(() =>
            {
                var checksum = 0u;
                var data = new byte[size];

                var chapter = ChapterWrapper.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

                Assert.AreEqual(expected, Th16ClearDataWrapper.CanInitialize(chapter));
            });
    }
}

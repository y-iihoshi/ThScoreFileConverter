using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th17;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th17.Stubs;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;
using IPractice = ThScoreFileConverter.Models.Th13.IPractice;
using ISpellCard = ThScoreFileConverter.Models.Th13.ISpellCard<ThScoreFileConverter.Models.Level>;
using StageProgress = ThScoreFileConverter.Models.Th13.StageProgress;

namespace ThScoreFileConverterTests.Models.Th17
{
    [TestClass]
    public class ClearDataTests
    {
        internal static ClearDataStub MakeValidStub()
        {
            var levels = Utils.GetEnumerable<Level>();
            var levelsWithTotal = Utils.GetEnumerable<LevelWithTotal>();
            var stages = Utils.GetEnumerable<StagePractice>();

            return new ClearDataStub()
            {
                Signature = "CR",
                Version = 2,
                Checksum = 0u,
                Size = 0x4820,
                Chara = CharaWithTotal.MarisaB,
                Rankings = levelsWithTotal.ToDictionary(
                    level => level,
                    level => Enumerable.Range(0, 10).Select(
                        index => new ScoreDataStub()
                        {
                            Score = 12345670u - ((uint)index * 1000u),
                            StageProgress = (StageProgress)index,
                            ContinueCount = (byte)index,
                            Name = TestUtils.MakeByteArray($"Player{index}\0\0\0").Skip(1).ToArray(),   // skip length
                            DateTime = 34567890u,
                            SlowRate = 1.2f,
                        }).ToList() as IReadOnlyList<IScoreData>),
                TotalPlayCount = 23,
                PlayTime = 4567890,
                ClearCounts = levelsWithTotal.ToDictionary(level => level, level => 100 - TestUtils.Cast<int>(level)),
                ClearFlags = levelsWithTotal.ToDictionary(level => level, level => TestUtils.Cast<int>(level) % 2),
                Practices = levels
                    .SelectMany(level => stages.Select(stage => (level, stage)))
                    .ToDictionary(
                        pair => pair,
                        pair => Mock.Of<IPractice>(
                            m => (m.Score == 123456u - (TestUtils.Cast<uint>(pair.level) * 10u))
                                 && (m.ClearFlag == (byte)(TestUtils.Cast<int>(pair.stage) % 2))
                                 && (m.EnableFlag == (byte)(TestUtils.Cast<int>(pair.level) % 2)))),
                Cards = Definitions.CardTable.ToDictionary(
                    pair => pair.Key,
                    pair => Mock.Of<ISpellCard>(
                        m => (m.Name == TestUtils.MakeRandomArray<byte>(0x80))
                             && (m.ClearCount == ((pair.Key % 2 == 0) ? 0 : 12 + pair.Key))
                             && (m.PracticeClearCount == ((pair.Key % 3 == 0) ? 0 : 34 + pair.Key))
                             && (m.TrialCount == ((pair.Key % 4 == 0) ? 0 : 56 + pair.Key))
                             && (m.PracticeTrialCount == ((pair.Key % 5 == 0) ? 0 : 78 + pair.Key))
                             && (m.Id == pair.Value.Id)
                             && (m.Level == pair.Value.Level)
                             && (m.PracticeScore == 90123))),
            };
        }

        internal static byte[] MakeByteArray(IClearData clearData)
            => TestUtils.MakeByteArray(
                clearData.Signature.ToCharArray(),
                clearData.Version,
                clearData.Checksum,
                clearData.Size,
                (int)clearData.Chara,
                clearData.Rankings.Values.SelectMany(
                    ranking => ranking.SelectMany(scoreData => ScoreDataTests.MakeByteArray(scoreData))).ToArray(),
                new byte[0x140],
                clearData.Cards.Values.SelectMany(card => Th13.SpellCardTests.MakeByteArray(card)).ToArray(),
                clearData.TotalPlayCount,
                clearData.PlayTime,
                0u,
                clearData.ClearCounts.Values.ToArray(),
                0u,
                clearData.ClearFlags.Values.ToArray(),
                0u,
                clearData.Practices.Values.SelectMany(practice => Th13.PracticeTests.MakeByteArray(practice)).ToArray(),
                new byte[0x40]);

        internal static void Validate(IClearData expected, IClearData actual)
        {
            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Version, actual.Version);
            Assert.AreEqual(expected.Checksum, actual.Checksum);
            Assert.AreEqual(expected.Size, actual.Size);
            Assert.AreEqual(expected.Chara, actual.Chara);

            foreach (var pair in expected.Rankings)
            {
                for (var index = 0; index < pair.Value.Count; ++index)
                {
                    ScoreDataTests.Validate(actual.Rankings[pair.Key][index], pair.Value[index]);
                }
            }

            Assert.AreEqual(expected.TotalPlayCount, actual.TotalPlayCount);
            Assert.AreEqual(expected.PlayTime, actual.PlayTime);
            CollectionAssert.That.AreEqual(expected.ClearCounts.Values, actual.ClearCounts.Values);
            CollectionAssert.That.AreEqual(expected.ClearFlags.Values, actual.ClearFlags.Values);

            foreach (var pair in expected.Practices)
            {
                Th13.PracticeTests.Validate(pair.Value, actual.Practices[pair.Key]);
            }

            foreach (var pair in expected.Cards)
            {
                Th13.SpellCardTests.Validate(pair.Value, actual.Cards[pair.Key]);
            }
        }

        [TestMethod]
        public void ClearDataTestChapter()
        {
            var stub = MakeValidStub();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            var clearData = new ClearData(chapter);

            Validate(stub, clearData);
            Assert.IsFalse(clearData.IsValid);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ClearDataTestNullChapter()
        {
            _ = new ClearData(null!);

            Assert.Fail(TestUtils.Unreachable);
        }

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ClearDataTestInvalidSignature()
        {
            var stub = MakeValidStub();
            stub.Signature = stub.Signature.ToLowerInvariant();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new ClearData(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ClearDataTestInvalidVersion()
        {
            var stub = MakeValidStub();
            ++stub.Version;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new ClearData(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ClearDataTestInvalidSize()
        {
            var stub = MakeValidStub();
            --stub.Size;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new ClearData(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        [DataTestMethod]
        [DataRow("CR", (ushort)2, 0x4820, true)]
        [DataRow("cr", (ushort)2, 0x4820, false)]
        [DataRow("CR", (ushort)1, 0x4820, false)]
        [DataRow("CR", (ushort)2, 0x4821, false)]
        public void ClearDataCanInitializeTest(string signature, ushort version, int size, bool expected)
        {
            var checksum = 0u;
            var data = new byte[size];

            var chapter = TestUtils.Create<Chapter>(
                TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

            Assert.AreEqual(expected, ClearData.CanInitialize(chapter));
        }
    }
}

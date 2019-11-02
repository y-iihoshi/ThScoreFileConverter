using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th10;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th10.Stubs;
using ThScoreFileConverterTests.Models.Th10.Wrappers;

namespace ThScoreFileConverterTests.Models.Th10
{
    [TestClass]
    public class ClearDataTests
    {
        internal static ClearDataStub<CharaWithTotal, StageProgress> MakeValidStub()
        {
            var levels = Utils.GetEnumerator<Level>();
            var levelsExceptExtra = levels.Where(level => level != Level.Extra);
            var stages = Utils.GetEnumerator<Stage>();
            var stagesExceptExtra = stages.Where(stage => stage != Stage.Extra);

            return new ClearDataStub<CharaWithTotal, StageProgress>()
            {
                Signature = "CR",
                Version = 0x0000,
                Checksum = 0u,
                Size = 0x437C,
                Chara = CharaWithTotal.ReimuB,
                Rankings = levels.ToDictionary(
                    level => level,
                    level => Enumerable.Range(0, 10).Select(
                        index => new ScoreDataStub<StageProgress>()
                        {
                            Score = 12345670u - (uint)index * 1000u,
                            StageProgress = StageProgress.Five,
                            ContinueCount = (byte)index,
                            Name = TestUtils.CP932Encoding.GetBytes($"Player{index}\0\0\0"),
                            DateTime = 34567890u,
                            SlowRate = 1.2f
                        }).ToList() as IReadOnlyList<IScoreData<StageProgress>>),
                TotalPlayCount = 23,
                PlayTime = 4567890,
                ClearCounts = levels.ToDictionary(level => level, level => 100 - (int)level),
                Practices = levelsExceptExtra
                    .SelectMany(level => stagesExceptExtra.Select(stage => (level, stage)))
                    .ToDictionary(
                        pair => pair,
                        pair => new PracticeStub()
                        {
                            Score = 123456u - (uint)pair.level * 10u,
                            StageFlag = (uint)pair.stage % 2u
                        } as IPractice),
                Cards = Enumerable.Range(1, 110).ToDictionary(
                    index => index,
                    index => new SpellCardStub()
                    {
                        Name = TestUtils.MakeRandomArray<byte>(0x80),
                        ClearCount = 123 + index,
                        TrialCount = 456 + index,
                        Id = index,
                        Level = Level.Hard
                    } as ISpellCard<Level>)
            };
        }

        internal static byte[] MakeData<TCharaWithTotal, TStageProgress>(
            IClearData<TCharaWithTotal, TStageProgress> clearData, int scoreDataUnknownSize)
            where TCharaWithTotal : struct, Enum
            where TStageProgress : struct, Enum
            => TestUtils.MakeByteArray(
                TestUtils.Cast<int>(clearData.Chara),
                clearData.Rankings.Values.SelectMany(
                    ranking => ranking.SelectMany(
                        scoreData => ScoreDataTests.MakeByteArray(scoreData, scoreDataUnknownSize))).ToArray(),
                clearData.TotalPlayCount,
                clearData.PlayTime,
                clearData.ClearCounts.Values.ToArray(),
                clearData.Practices.Values.SelectMany(
                    practice => PracticeTests.MakeByteArray(practice)).ToArray(),
                clearData.Cards.Values.SelectMany(
                    card => SpellCardTests.MakeByteArray(card)).ToArray());

        internal static byte[] MakeByteArray<TCharaWithTotal, TStageProgress>(
            IClearData<TCharaWithTotal, TStageProgress> clearData, int scoreDataUnknownSize)
            where TCharaWithTotal : struct, Enum
            where TStageProgress : struct, Enum
            => TestUtils.MakeByteArray(
                clearData.Signature.ToCharArray(),
                clearData.Version,
                clearData.Checksum,
                clearData.Size,
                MakeData(clearData, scoreDataUnknownSize));

        internal static byte[] MakeByteArray(IClearData<CharaWithTotal, StageProgress> clearData)
            => MakeByteArray(clearData, 0);

        internal static void Validate<TCharaWithTotal, TStageProgress>(
            IClearData<TCharaWithTotal, TStageProgress> expected, IClearData<TCharaWithTotal, TStageProgress> actual)
            where TCharaWithTotal : struct, Enum
            where TStageProgress : struct, Enum
        {
            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Version, actual.Version);
            Assert.AreEqual(expected.Checksum, actual.Checksum);
            Assert.AreEqual(expected.Size, actual.Size);
            Assert.AreEqual(expected.Chara, actual.Chara);

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

            foreach (var pair in expected.Practices)
            {
                PracticeTests.Validate(pair.Value, actual.Practices[pair.Key]);
            }

            foreach (var pair in expected.Cards)
            {
                SpellCardTests.Validate(pair.Value, actual.Cards[pair.Key]);
            }
        }

        [TestMethod]
        public void ClearDataTestChapter() => TestUtils.Wrap(() =>
        {
            var stub = MakeValidStub();

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            var clearData = new ClearData(chapter.Target);

            Validate(stub, clearData);
            Assert.IsFalse(clearData.IsValid);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ClearDataTestNullChapter() => TestUtils.Wrap(() =>
        {
            _ = new ClearData(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ClearDataTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var stub = MakeValidStub();
            stub.Signature = stub.Signature.ToLowerInvariant();

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new ClearData(chapter.Target);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ClearDataTestInvalidVersion() => TestUtils.Wrap(() =>
        {
            var stub = MakeValidStub();
            ++stub.Version;

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new ClearData(chapter.Target);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ClearDataTestInvalidSize() => TestUtils.Wrap(() =>
        {
            var stub = MakeValidStub();
            --stub.Size;

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new ClearData(chapter.Target);

            Assert.Fail(TestUtils.Unreachable);
        });

        [DataTestMethod]
        [DataRow("CR", (ushort)0, 0x437C, true)]
        [DataRow("cr", (ushort)0, 0x437C, false)]
        [DataRow("CR", (ushort)1, 0x437C, false)]
        [DataRow("CR", (ushort)0, 0x437D, false)]
        public void CanInitializeTest(string signature, ushort version, int size, bool expected) => TestUtils.Wrap(() =>
        {
            var checksum = 0u;
            var data = new byte[size];

            var chapter = ChapterWrapper.Create(
                TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

            Assert.AreEqual(expected, ClearData.CanInitialize(chapter.Target));
        });
    }
}

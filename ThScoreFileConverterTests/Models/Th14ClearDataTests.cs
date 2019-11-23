using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th13;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th10.Stubs;
using ThScoreFileConverterTests.Models.Th10.Wrappers;
using ThScoreFileConverterTests.Models.Th13;
using ThScoreFileConverterTests.Models.Th13.Stubs;
using ThScoreFileConverterTests.Models.Wrappers;
using CharaWithTotal = ThScoreFileConverter.Models.Th14.CharaWithTotal;
using LevelPractice = ThScoreFileConverter.Models.Th14.LevelPractice;
using LevelPracticeWithTotal = ThScoreFileConverter.Models.Th14.LevelPracticeWithTotal;
using PracticeStub = ThScoreFileConverterTests.Models.Th13.Stubs.PracticeStub;
using ScoreDataTests = ThScoreFileConverterTests.Models.Th10.ScoreDataTests;
using StagePractice = ThScoreFileConverter.Models.Th14.StagePractice;
using StageProgress = ThScoreFileConverter.Models.Th13.StageProgress;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th14ClearDataTests
    {
        internal static ClearDataStub<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>
            MakeValidStub<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>()
            where TChWithT : struct, Enum       // TCharaWithTotal
            where TLv : struct, Enum            // TLevel
            where TLvPrac : struct, Enum        // TLevelPractice
            where TLvPracWithT : struct, Enum   // TLevelPracticeWithTotal
            where TStPrac : struct, Enum        // TStagePractice
        {
            var levels = Utils.GetEnumerator<TLvPrac>();
            var levelsWithTotal = Utils.GetEnumerator<TLvPracWithT>();
            var stages = Utils.GetEnumerator<TStPrac>();

            return new ClearDataStub<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>
            {
                Signature = "CR",
                Version = 0x0001,
                Checksum = 0u,
                Size = 0x5298,
                Chara = TestUtils.Cast<TChWithT>(1),
                Rankings = levelsWithTotal.ToDictionary(
                    level => level,
                    level => Enumerable.Range(0, 10).Select(
                        index => new ScoreDataStub<StageProgress>()
                        {
                            Score = 12345670u - (uint)index * 1000u,
                            StageProgress = StageProgress.Five,
                            ContinueCount = (byte)index,
                            Name = TestUtils.MakeRandomArray<byte>(10),
                            DateTime = 34567890u,
                            SlowRate = 1.2f
                        }).ToList() as IReadOnlyList<ThScoreFileConverter.Models.Th10.IScoreData<StageProgress>>),
                TotalPlayCount = 23,
                PlayTime = 4567890,
                ClearCounts = levelsWithTotal.ToDictionary(level => level, level => 100 - TestUtils.Cast<int>(level)),
                ClearFlags = levelsWithTotal.ToDictionary(level => level, level => TestUtils.Cast<int>(level) % 2),
                Practices = levels
                    .SelectMany(level => stages.Select(stage => (level, stage)))
                    .ToDictionary(
                        pair => pair,
                        pair => new PracticeStub()
                        {
                            Score = 123456u - TestUtils.Cast<uint>(pair.level) * 10u,
                            ClearFlag = (byte)(TestUtils.Cast<int>(pair.stage) % 2),
                            EnableFlag = (byte)(TestUtils.Cast<int>(pair.level) % 2)
                        } as IPractice),
                Cards = Enumerable.Range(1, 120).ToDictionary(
                    index => index,
                    index => new SpellCardStub<TLv>()
                    {
                        Name = TestUtils.MakeRandomArray<byte>(0x80),
                        ClearCount = 12 + index,
                        PracticeClearCount = 34 + index,
                        TrialCount = 56 + index,
                        PracticeTrialCount = 78 + index,
                        Id = index,
                        Level = TestUtils.Cast<TLv>(2),
                        PracticeScore = 90123
                    } as ISpellCard<TLv>)
            };
        }

        internal static byte[] MakeData<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>(
            IClearData<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac> clearData)
            where TParent : ThConverter
            where TChWithT : struct, Enum
            where TLv : struct, Enum
            where TLvPrac : struct, Enum
            where TLvPracWithT : struct, Enum
            where TStPrac : struct, Enum
            => TestUtils.MakeByteArray(
                TestUtils.Cast<int>(clearData.Chara),
                clearData.Rankings.Values.SelectMany(
                    ranking => ranking.SelectMany(
                        scoreData => ScoreDataTests.MakeByteArray<TParent, StageProgress>(scoreData))).ToArray(),
                clearData.TotalPlayCount,
                clearData.PlayTime,
                clearData.ClearCounts.Values.ToArray(),
                clearData.ClearFlags.Values.ToArray(),
                clearData.Practices.Values.SelectMany(practice => PracticeTests.MakeByteArray(practice)).ToArray(),
                clearData.Cards.Values.SelectMany(card => SpellCardTests.MakeByteArray(card)).ToArray());

        internal static byte[] MakeByteArray<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>(
            IClearData<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac> clearData)
            where TParent : ThConverter
            where TChWithT : struct, Enum
            where TLv : struct, Enum
            where TLvPrac : struct, Enum
            where TLvPracWithT : struct, Enum
            where TStPrac : struct, Enum
            => TestUtils.MakeByteArray(
                clearData.Signature.ToCharArray(),
                clearData.Version,
                clearData.Checksum,
                clearData.Size,
                MakeData<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>(clearData));

        internal static void Validate<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>(
            IClearData<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac> expected,
            in Th13ClearDataWrapper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac> actual)
            where TParent : ThConverter
            where TChWithT : struct, Enum
            where TLv : struct, Enum
            where TLvPrac : struct, Enum
            where TLvPracWithT : struct, Enum
            where TStPrac : struct, Enum
        {
            var data = MakeData<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>(expected);

            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Version, actual.Version);
            Assert.AreEqual(expected.Checksum, actual.Checksum);
            Assert.AreEqual(expected.Size, actual.Size);
            CollectionAssert.That.AreEqual(data, actual.Data);
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
            CollectionAssert.That.AreEqual(expected.ClearFlags.Values, actual.ClearFlags.Values);

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
        public void Th14ClearDataTestChapter() => TestUtils.Wrap(() =>
        {
            var stub = MakeValidStub<CharaWithTotal, Level, LevelPractice, LevelPracticeWithTotal, StagePractice>();

            var chapter = ChapterWrapper.Create(MakeByteArray<
                Th14Converter, CharaWithTotal, Level, LevelPractice, LevelPracticeWithTotal, StagePractice>(stub));
            var clearData = new Th13ClearDataWrapper<
                Th14Converter, CharaWithTotal, Level, LevelPractice, LevelPracticeWithTotal, StagePractice>(chapter);

            Validate(stub, clearData);
            Assert.IsFalse(clearData.IsValid.Value);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th14ClearDataTestNullChapter() => TestUtils.Wrap(() =>
        {
            _ = new Th13ClearDataWrapper<
                Th14Converter, CharaWithTotal, Level, LevelPractice, LevelPracticeWithTotal, StagePractice>(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th14ClearDataTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var stub = MakeValidStub<CharaWithTotal, Level, LevelPractice, LevelPracticeWithTotal, StagePractice>();
            stub.Signature = stub.Signature.ToLowerInvariant();

            var chapter = ChapterWrapper.Create(MakeByteArray<
                Th14Converter, CharaWithTotal, Level, LevelPractice, LevelPracticeWithTotal, StagePractice>(stub));
            _ = new Th13ClearDataWrapper<
                Th14Converter, CharaWithTotal, Level, LevelPractice, LevelPracticeWithTotal, StagePractice>(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th14ClearDataTestInvalidVersion() => TestUtils.Wrap(() =>
        {
            var stub = MakeValidStub<CharaWithTotal, Level, LevelPractice, LevelPracticeWithTotal, StagePractice>();
            ++stub.Version;

            var chapter = ChapterWrapper.Create(MakeByteArray<
                Th14Converter, CharaWithTotal, Level, LevelPractice, LevelPracticeWithTotal, StagePractice>(stub));
            _ = new Th13ClearDataWrapper<
                Th14Converter, CharaWithTotal, Level, LevelPractice, LevelPracticeWithTotal, StagePractice>(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th14ClearDataTestInvalidSize() => TestUtils.Wrap(() =>
        {
            var stub = MakeValidStub<CharaWithTotal, Level, LevelPractice, LevelPracticeWithTotal, StagePractice>();
            --stub.Size;

            var chapter = ChapterWrapper.Create(MakeByteArray<
                Th14Converter, CharaWithTotal, Level, LevelPractice, LevelPracticeWithTotal, StagePractice>(stub));
            _ = new Th13ClearDataWrapper<
                Th14Converter, CharaWithTotal, Level, LevelPractice, LevelPracticeWithTotal, StagePractice>(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [DataTestMethod]
        [DataRow("CR", (ushort)1, 0x5298, true)]
        [DataRow("cr", (ushort)1, 0x5298, false)]
        [DataRow("CR", (ushort)0, 0x5298, false)]
        [DataRow("CR", (ushort)1, 0x5299, false)]
        public void Th14ClearDataCanInitializeTest(string signature, ushort version, int size, bool expected)
            => TestUtils.Wrap(() =>
            {
                var checksum = 0u;
                var data = new byte[size];

                var chapter = ChapterWrapper.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

                Assert.AreEqual(expected, Th13ClearDataWrapper<
                    Th14Converter, CharaWithTotal, Level, LevelPractice, LevelPracticeWithTotal, StagePractice>
                    .CanInitialize(chapter));
            });
    }
}

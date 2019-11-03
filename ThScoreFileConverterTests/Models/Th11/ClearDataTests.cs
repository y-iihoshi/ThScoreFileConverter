using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th10;
using ThScoreFileConverterTests.Models.Th10.Stubs;
using ThScoreFileConverterTests.Models.Th10.Wrappers;
using CharaWithTotal = ThScoreFileConverter.Models.Th11.CharaWithTotal;
using ClearData = ThScoreFileConverter.Models.Th11.ClearData;
using StageProgress = ThScoreFileConverter.Models.Th11.StageProgress;

namespace ThScoreFileConverterTests.Models.Th11
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
                Size = 0x68D4,
                Chara = CharaWithTotal.ReimuSuika,
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
                Cards = Enumerable.Range(1, 175).ToDictionary(
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

        internal static byte[] MakeByteArray(IClearData<CharaWithTotal, StageProgress> clearData)
            => Th10.ClearDataTests.MakeByteArray(clearData, 4);

        [TestMethod]
        public void ClearDataTestChapter() => TestUtils.Wrap(() =>
        {
            var stub = MakeValidStub();

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            var clearData = new ClearData(chapter.Target);

            Th10.ClearDataTests.Validate(stub, clearData);
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
        [DataRow("CR", (ushort)0, 0x68D4, true)]
        [DataRow("cr", (ushort)0, 0x68D4, false)]
        [DataRow("CR", (ushort)1, 0x68D4, false)]
        [DataRow("CR", (ushort)0, 0x68D5, false)]
        public void CanInitializeTest(string signature, ushort version, int size, bool expected)
            => TestUtils.Wrap(() =>
            {
                var checksum = 0u;
                var data = new byte[size];

                var chapter = ChapterWrapper.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

                Assert.AreEqual(expected, ClearData.CanInitialize(chapter.Target));
            });
    }
}

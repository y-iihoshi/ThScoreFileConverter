using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th11;
using ThScoreFileConverterTests.Models.Th10.Stubs;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;
using IClearData = ThScoreFileConverter.Models.Th10.IClearData<
    ThScoreFileConverter.Models.Th11.CharaWithTotal, ThScoreFileConverter.Models.Th10.StageProgress>;
using IPractice = ThScoreFileConverter.Models.Th10.IPractice;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th10.StageProgress>;
using ISpellCard = ThScoreFileConverter.Models.Th10.ISpellCard<ThScoreFileConverter.Models.Level>;
using StageProgress = ThScoreFileConverter.Models.Th10.StageProgress;

namespace ThScoreFileConverterTests.Models.Th11
{
    [TestClass]
    public class ClearDataTests
    {
        internal static ClearDataStub<CharaWithTotal, StageProgress> MakeValidStub()
        {
            var levels = Utils.GetEnumerable<Level>();
            var levelsExceptExtra = levels.Where(level => level != Level.Extra);
            var stages = Utils.GetEnumerable<Stage>();
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
                        index => Mock.Of<IScoreData>(
                            m => (m.Score == 12345670u - ((uint)index * 1000u))
                                 && (m.StageProgress == StageProgress.Five)
                                 && (m.ContinueCount == (byte)index)
                                 && (m.Name == TestUtils.CP932Encoding.GetBytes($"Player{index}\0\0\0"))
                                 && (m.DateTime == 34567890u)
                                 && (m.SlowRate == 1.2f))).ToList() as IReadOnlyList<IScoreData>),
                TotalPlayCount = 23,
                PlayTime = 4567890,
                ClearCounts = levels.ToDictionary(level => level, level => 100 - (int)level),
                Practices = levelsExceptExtra
                    .SelectMany(level => stagesExceptExtra.Select(stage => (level, stage)))
                    .ToDictionary(
                        pair => pair,
                        pair => Mock.Of<IPractice>(
                            m => (m.Score == 123456u - ((uint)pair.level * 10u))
                                 && (m.StageFlag == (uint)pair.stage % 2u))),
                Cards = Enumerable.Range(1, 175).ToDictionary(
                    index => index,
                    index => new SpellCardStub()
                    {
                        Name = TestUtils.MakeRandomArray<byte>(0x80),
                        ClearCount = 123 + index,
                        TrialCount = 456 + index,
                        Id = index,
                        Level = Level.Hard,
                    } as ISpellCard),
            };
        }

        internal static byte[] MakeByteArray(IClearData clearData)
            => Th10.ClearDataTests.MakeByteArray(clearData, 4);

        [TestMethod]
        public void ClearDataTestChapter()
        {
            var stub = MakeValidStub();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            var clearData = new ClearData(chapter);

            Th10.ClearDataTests.Validate(stub, clearData);
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
        [DataRow("CR", (ushort)0, 0x68D4, true)]
        [DataRow("cr", (ushort)0, 0x68D4, false)]
        [DataRow("CR", (ushort)1, 0x68D4, false)]
        [DataRow("CR", (ushort)0, 0x68D5, false)]
        public void CanInitializeTest(string signature, ushort version, int size, bool expected)
        {
            var checksum = 0u;
            var data = new byte[size];

            var chapter = TestUtils.Create<Chapter>(
                TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

            Assert.AreEqual(expected, ClearData.CanInitialize(chapter));
        }
    }
}

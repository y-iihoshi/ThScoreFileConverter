﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th14;
using ThScoreFileConverterTests.Extensions;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Models.Th14.CharaWithTotal,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Th14.LevelPractice,
    ThScoreFileConverter.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Models.Th14.StagePractice>;
using IPractice = ThScoreFileConverter.Models.Th13.IPractice;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>;
using ISpellCard = ThScoreFileConverter.Models.Th13.ISpellCard<ThScoreFileConverter.Models.Level>;
using StageProgress = ThScoreFileConverter.Models.Th13.StageProgress;

namespace ThScoreFileConverterTests.Models.Th14
{
    [TestClass]
    public class ClearDataTests
    {
        internal static Mock<IClearData> MockClearData()
        {
            var levels = Utils.GetEnumerable<LevelPractice>();
            var levelsWithTotal = Utils.GetEnumerable<LevelPracticeWithTotal>();
            var stages = Utils.GetEnumerable<StagePractice>();

            var mock = new Mock<IClearData>();
            _ = mock.SetupGet(m => m.Signature).Returns("CR");
            _ = mock.SetupGet(m => m.Version).Returns(0x0001);
            _ = mock.SetupGet(m => m.Checksum).Returns(0u);
            _ = mock.SetupGet(m => m.Size).Returns(0x5298);
            _ = mock.SetupGet(m => m.Chara).Returns(CharaWithTotal.ReimuB);
            _ = mock.SetupGet(m => m.Rankings).Returns(
                levelsWithTotal.ToDictionary(
                    level => level,
                    level => Enumerable.Range(0, 10).Select(
                        index => Mock.Of<IScoreData>(
                            m => (m.Score == 12345670u - ((uint)index * 1000u))
                                 && (m.StageProgress == StageProgress.Five)
                                 && (m.ContinueCount == (byte)index)
                                 && (m.Name == TestUtils.CP932Encoding.GetBytes($"Player{index}\0\0\0"))
                                 && (m.DateTime == 34567890u)
                                 && (m.SlowRate == 1.2f))).ToList() as IReadOnlyList<IScoreData>));
            _ = mock.SetupGet(m => m.TotalPlayCount).Returns(23);
            _ = mock.SetupGet(m => m.PlayTime).Returns(4567890);
            _ = mock.SetupGet(m => m.ClearCounts).Returns(
                levelsWithTotal.ToDictionary(level => level, level => 100 - TestUtils.Cast<int>(level)));
            _ = mock.SetupGet(m => m.ClearFlags).Returns(
                levelsWithTotal.ToDictionary(level => level, level => TestUtils.Cast<int>(level) % 2));
            _ = mock.SetupGet(m => m.Practices).Returns(
                levels
                    .SelectMany(level => stages.Select(stage => (level, stage)))
                    .ToDictionary(
                        pair => pair,
                        pair => Mock.Of<IPractice>(
                            m => (m.Score == 123456u - (TestUtils.Cast<uint>(pair.level) * 10u))
                                 && (m.ClearFlag == (byte)(TestUtils.Cast<int>(pair.stage) % 2))
                                 && (m.EnableFlag == (byte)(TestUtils.Cast<int>(pair.level) % 2)))));
            _ = mock.SetupGet(m => m.Cards).Returns(
                Enumerable.Range(1, 120).ToDictionary(
                    index => index,
                    index => Mock.Of<ISpellCard>(
                        m => (m.Name == TestUtils.MakeRandomArray<byte>(0x80))
                             && (m.ClearCount == 12 + index)
                             && (m.PracticeClearCount == 34 + index)
                             && (m.TrialCount == 56 + index)
                             && (m.PracticeTrialCount == 78 + index)
                             && (m.Id == index)
                             && (m.Level == Level.Hard)
                             && (m.PracticeScore == 90123))));
            return mock;
        }

        internal static byte[] MakeData(IClearData clearData)
            => TestUtils.MakeByteArray(
                TestUtils.Cast<int>(clearData.Chara),
                clearData.Rankings.Values.SelectMany(
                    ranking => ranking.SelectMany(
                        scoreData => Th10.ScoreDataTests.MakeByteArray(scoreData, 4))).ToArray(),
                clearData.TotalPlayCount,
                clearData.PlayTime,
                clearData.ClearCounts.Values.ToArray(),
                clearData.ClearFlags.Values.ToArray(),
                clearData.Practices.Values.SelectMany(practice => Th13.PracticeTests.MakeByteArray(practice)).ToArray(),
                clearData.Cards.Values.SelectMany(card => Th13.SpellCardTests.MakeByteArray(card)).ToArray());

        internal static byte[] MakeByteArray(IClearData clearData)
            => TestUtils.MakeByteArray(
                clearData.Signature.ToCharArray(),
                clearData.Version,
                clearData.Checksum,
                clearData.Size,
                MakeData(clearData));

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
                    Th10.ScoreDataTests.Validate(pair.Value[index], actual.Rankings[pair.Key][index]);
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
            var mock = MockClearData();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            var clearData = new ClearData(chapter);

            Validate(mock.Object, clearData);
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
            var mock = MockClearData();
            var signature = mock.Object.Signature;
            _ = mock.SetupGet(m => m.Signature).Returns(signature.ToLowerInvariant());

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = new ClearData(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ClearDataTestInvalidVersion()
        {
            var mock = MockClearData();
            var version = mock.Object.Version;
            _ = mock.SetupGet(m => m.Version).Returns(++version);

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = new ClearData(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ClearDataTestInvalidSize()
        {
            var mock = MockClearData();
            var size = mock.Object.Size;
            _ = mock.SetupGet(m => m.Size).Returns(--size);

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = new ClearData(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        [DataTestMethod]
        [DataRow("CR", (ushort)1, 0x5298, true)]
        [DataRow("cr", (ushort)1, 0x5298, false)]
        [DataRow("CR", (ushort)0, 0x5298, false)]
        [DataRow("CR", (ushort)1, 0x5299, false)]
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

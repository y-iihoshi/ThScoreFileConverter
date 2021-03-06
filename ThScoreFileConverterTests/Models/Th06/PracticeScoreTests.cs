﻿using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th06;
using ThScoreFileConverterTests.UnitTesting;

namespace ThScoreFileConverterTests.Models.Th06
{
    [TestClass]
    public class PracticeScoreTests
    {
        internal static Mock<IPracticeScore> MockPracticeScore()
        {
            var mock = new Mock<IPracticeScore>();
            _ = mock.SetupGet(m => m.Signature).Returns("PSCR");
            _ = mock.SetupGet(m => m.Size1).Returns(0x14);
            _ = mock.SetupGet(m => m.Size2).Returns(0x14);
            _ = mock.SetupGet(m => m.HighScore).Returns(123456);
            _ = mock.SetupGet(m => m.Chara).Returns(Chara.ReimuB);
            _ = mock.SetupGet(m => m.Level).Returns(Level.Hard);
            _ = mock.SetupGet(m => m.Stage).Returns(Stage.Six);
            return mock;
        }

        internal static byte[] MakeByteArray(IPracticeScore score)
        {
            return TestUtils.MakeByteArray(
                score.Signature.ToCharArray(),
                score.Size1,
                score.Size2,
                0u,
                score.HighScore,
                (byte)score.Chara,
                (byte)score.Level,
                (byte)score.Stage,
                (byte)0);
        }

        internal static void Validate(IPracticeScore expected, IPracticeScore actual)
        {
            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Size1, actual.Size1);
            Assert.AreEqual(expected.Size2, actual.Size2);
            Assert.AreEqual(expected.FirstByteOfData, actual.FirstByteOfData);
            Assert.AreEqual(expected.HighScore, actual.HighScore);
            Assert.AreEqual(expected.Chara, actual.Chara);
            Assert.AreEqual(expected.Level, actual.Level);
            Assert.AreEqual(expected.Stage, actual.Stage);
        }

        [TestMethod]
        public void PracticeScoreTestChapter()
        {
            var mock = MockPracticeScore();
            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            var score = new PracticeScore(chapter);

            Validate(mock.Object, score);
        }

        [TestMethod]
        public void PracticeScoreTestInvalidSignature()
        {
            var mock = MockPracticeScore();
            var signature = mock.Object.Signature;
            _ = mock.SetupGet(m => m.Signature).Returns(signature.ToLowerInvariant());

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidDataException>(() => new PracticeScore(chapter));
        }

        [TestMethod]
        public void PracticeScoreTestInvalidSize1()
        {
            var mock = MockPracticeScore();
            var size = mock.Object.Size1;
            _ = mock.SetupGet(m => m.Size1).Returns(--size);

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidDataException>(() => new PracticeScore(chapter));
        }

        public static IEnumerable<object[]> InvalidCharacters
            => TestUtils.GetInvalidEnumerators(typeof(Chara));

        [DataTestMethod]
        [DynamicData(nameof(InvalidCharacters))]
        public void PracticeScoreTestInvalidChara(int chara)
        {
            var mock = MockPracticeScore();
            _ = mock.SetupGet(m => m.Chara).Returns(TestUtils.Cast<Chara>(chara));

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidCastException>(() => new PracticeScore(chapter));
        }

        public static IEnumerable<object[]> InvalidLevels
            => TestUtils.GetInvalidEnumerators(typeof(Level));

        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        public void PracticeScoreTestInvalidLevel(int level)
        {
            var mock = MockPracticeScore();
            _ = mock.SetupGet(m => m.Level).Returns(TestUtils.Cast<Level>(level));

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidCastException>(() => new PracticeScore(chapter));
        }

        public static IEnumerable<object[]> InvalidStages
            => TestUtils.GetInvalidEnumerators(typeof(Stage));

        [DataTestMethod]
        [DynamicData(nameof(InvalidStages))]
        public void PracticeScoreTestInvalidStage(int stage)
        {
            var mock = MockPracticeScore();
            _ = mock.SetupGet(m => m.Stage).Returns(TestUtils.Cast<Stage>(stage));

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidCastException>(() => new PracticeScore(chapter));
        }
    }
}

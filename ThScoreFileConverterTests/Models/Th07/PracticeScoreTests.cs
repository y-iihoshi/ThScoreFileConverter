using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models.Th07;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;

namespace ThScoreFileConverterTests.Models.Th07
{
    [TestClass]
    public class PracticeScoreTests
    {
        internal static Mock<IPracticeScore> MockPracticeScore()
        {
            var mock = new Mock<IPracticeScore>();
            _ = mock.SetupGet(m => m.Signature).Returns("PSCR");
            _ = mock.SetupGet(m => m.Size1).Returns(0x18);
            _ = mock.SetupGet(m => m.Size2).Returns(0x18);
            _ = mock.SetupGet(m => m.TrialCount).Returns(987);
            _ = mock.SetupGet(m => m.HighScore).Returns(123456);
            _ = mock.SetupGet(m => m.Chara).Returns(Chara.ReimuB);
            _ = mock.SetupGet(m => m.Level).Returns(Level.Hard);
            _ = mock.SetupGet(m => m.Stage).Returns(Stage.Six);
            return mock;
        }

        internal static byte[] MakeByteArray(IPracticeScore score)
            => TestUtils.MakeByteArray(
                score.Signature.ToCharArray(),
                score.Size1,
                score.Size2,
                0u,
                score.TrialCount,
                score.HighScore,
                (byte)score.Chara,
                (byte)score.Level,
                (byte)score.Stage,
                (byte)0);

        internal static void Validate(IPracticeScore expected, IPracticeScore actual)
        {
            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Size1, actual.Size1);
            Assert.AreEqual(expected.Size2, actual.Size2);
            Assert.AreEqual(expected.FirstByteOfData, actual.FirstByteOfData);
            Assert.AreEqual(expected.TrialCount, actual.TrialCount);
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
        [ExpectedException(typeof(ArgumentNullException))]
        public void PracticeScoreTestNullChapter()
        {
            _ = new PracticeScore(null!);

            Assert.Fail(TestUtils.Unreachable);
        }

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void PracticeScoreTestInvalidSignature()
        {
            var mock = MockPracticeScore();
            var signature = mock.Object.Signature;
            _ = mock.SetupGet(m => m.Signature).Returns(signature.ToLowerInvariant());

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = new PracticeScore(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void PracticeScoreTestInvalidSize1()
        {
            var mock = MockPracticeScore();
            var size = mock.Object.Size1;
            _ = mock.SetupGet(m => m.Size1).Returns(--size);

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = new PracticeScore(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        public static IEnumerable<object[]> InvalidCharacters
            => TestUtils.GetInvalidEnumerators(typeof(Chara));

        [DataTestMethod]
        [DynamicData(nameof(InvalidCharacters))]
        [ExpectedException(typeof(InvalidCastException))]
        public void PracticeScoreTestInvalidChara(int chara)
        {
            var mock = MockPracticeScore();
            _ = mock.SetupGet(m => m.Chara).Returns(TestUtils.Cast<Chara>(chara));

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = new PracticeScore(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        public static IEnumerable<object[]> InvalidLevels
            => TestUtils.GetInvalidEnumerators(typeof(Level));

        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidCastException))]
        public void PracticeScoreTestInvalidLevel(int level)
        {
            var mock = MockPracticeScore();
            _ = mock.SetupGet(m => m.Level).Returns(TestUtils.Cast<Level>(level));

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = new PracticeScore(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        public static IEnumerable<object[]> InvalidStages
            => TestUtils.GetInvalidEnumerators(typeof(Stage));

        [DataTestMethod]
        [DynamicData(nameof(InvalidStages))]
        [ExpectedException(typeof(InvalidCastException))]
        public void PracticeScoreTestInvalidStage(int stage)
        {
            var mock = MockPracticeScore();
            _ = mock.SetupGet(m => m.Stage).Returns(TestUtils.Cast<Stage>(stage));

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = new PracticeScore(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }
    }
}

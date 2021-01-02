using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th09;
using ThScoreFileConverterTests.Extensions;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;

namespace ThScoreFileConverterTests.Models.Th09
{
    [TestClass]
    public class HighScoreTests
    {
        internal static Mock<IHighScore> MockHighScore()
        {
            var mock = new Mock<IHighScore>();
            _ = mock.SetupGet(m => m.Signature).Returns("HSCR");
            _ = mock.SetupGet(m => m.Size1).Returns(0x2C);
            _ = mock.SetupGet(m => m.Size2).Returns(0x2C);
            _ = mock.SetupGet(m => m.Score).Returns(1234567u);
            _ = mock.SetupGet(m => m.Chara).Returns(Chara.Marisa);
            _ = mock.SetupGet(m => m.Level).Returns(Level.Hard);
            _ = mock.SetupGet(m => m.Rank).Returns(987);
            _ = mock.SetupGet(m => m.Name).Returns(TestUtils.CP932Encoding.GetBytes("Player1\0\0"));
            _ = mock.SetupGet(m => m.Date).Returns(TestUtils.CP932Encoding.GetBytes("06/01/23\0"));
            _ = mock.SetupGet(m => m.ContinueCount).Returns(2);
            return mock;
        }

        internal static byte[] MakeByteArray(IHighScore highScore)
            => TestUtils.MakeByteArray(
                highScore.Signature.ToCharArray(),
                highScore.Size1,
                highScore.Size2,
                0u,
                highScore.Score,
                0u,
                (byte)highScore.Chara,
                (byte)highScore.Level,
                highScore.Rank,
                highScore.Name,
                highScore.Date,
                (byte)0,
                highScore.ContinueCount);

        internal static void Validate(IHighScore expected, IHighScore actual)
        {
            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Size1, actual.Size1);
            Assert.AreEqual(expected.Size2, actual.Size2);
            Assert.AreEqual(expected.FirstByteOfData, actual.FirstByteOfData);
            Assert.AreEqual(expected.Score, actual.Score);
            Assert.AreEqual(expected.Chara, actual.Chara);
            Assert.AreEqual(expected.Level, actual.Level);
            Assert.AreEqual(expected.Rank, actual.Rank);
            CollectionAssert.That.AreEqual(expected.Name, actual.Name);
            CollectionAssert.That.AreEqual(expected.Date, actual.Date);
            Assert.AreEqual(expected.ContinueCount, actual.ContinueCount);
        }

        [TestMethod]
        public void HighScoreTestChapter()
        {
            var mock = MockHighScore();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            var highScore = new HighScore(chapter);

            Validate(mock.Object, highScore);
        }

        [TestMethod]
        public void HighScoreTestInvalidSignature()
        {
            var mock = MockHighScore();
            var signature = mock.Object.Signature;
            _ = mock.SetupGet(m => m.Signature).Returns(signature.ToLowerInvariant());

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidDataException>(() => _ = new HighScore(chapter));
        }

        [TestMethod]
        public void HighScoreTestInvalidSize()
        {
            var mock = MockHighScore();
            var size = mock.Object.Size1;
            _ = mock.SetupGet(m => m.Size1).Returns(--size);

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidDataException>(() => _ = new HighScore(chapter));
        }

        public static IEnumerable<object[]> InvalidCharacters
            => TestUtils.GetInvalidEnumerators(typeof(Chara));

        [DataTestMethod]
        [DynamicData(nameof(InvalidCharacters))]
        public void HighScoreTestInvalidChara(int chara)
        {
            var mock = MockHighScore();
            _ = mock.SetupGet(m => m.Chara).Returns(TestUtils.Cast<Chara>(chara));

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidCastException>(() => _ = new HighScore(chapter));
        }

        public static IEnumerable<object[]> InvalidLevels
            => TestUtils.GetInvalidEnumerators(typeof(Level));

        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        public void HighScoreTestInvalidLevel(int level)
        {
            var mock = MockHighScore();
            _ = mock.SetupGet(m => m.Level).Returns(TestUtils.Cast<Level>(level));

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidCastException>(() => _ = new HighScore(chapter));
        }
    }
}

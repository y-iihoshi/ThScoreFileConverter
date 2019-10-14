using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th09;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th09.Stubs;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;

namespace ThScoreFileConverterTests.Models.Th09
{
    [TestClass]
    public class HighScoreTests
    {
        internal static HighScoreStub ValidStub { get; } = new HighScoreStub()
        {
            Signature = "HSCR",
            Size1 = 0x2C,
            Size2 = 0x2C,
            Score = 1234567u,
            Chara = Chara.Marisa,
            Level = Level.Hard,
            Rank = 987,
            Name = TestUtils.CP932Encoding.GetBytes("Player1\0\0"),
            Date = TestUtils.CP932Encoding.GetBytes("06/01/23\0"),
            ContinueCount = 2,
        };

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
        public void HighScoreTestChapter() => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            var highScore = new HighScore(chapter);

            Validate(stub, highScore);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HighScoreTestNullChapter() => TestUtils.Wrap(() =>
        {
            _ = new HighScore(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void HighScoreTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var stub = new HighScoreStub(ValidStub);
            stub.Signature = stub.Signature.ToLowerInvariant();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new HighScore(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void HighScoreTestInvalidSize() => TestUtils.Wrap(() =>
        {
            var stub = new HighScoreStub(ValidStub);
            --stub.Size1;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new HighScore(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidCharacters
            => TestUtils.GetInvalidEnumerators(typeof(Chara));

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidCharacters))]
        [ExpectedException(typeof(InvalidCastException))]
        public void HighScoreTestInvalidChara(int chara) => TestUtils.Wrap(() =>
        {
            var stub = new HighScoreStub(ValidStub)
            {
                Chara = TestUtils.Cast<Chara>(chara),
            };

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new HighScore(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidLevels
            => TestUtils.GetInvalidEnumerators(typeof(Level));

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidCastException))]
        public void HighScoreTestInvalidLevel(int level) => TestUtils.Wrap(() =>
        {
            var stub = new HighScoreStub(ValidStub)
            {
                Level = TestUtils.Cast<Level>(level),
            };

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new HighScore(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}

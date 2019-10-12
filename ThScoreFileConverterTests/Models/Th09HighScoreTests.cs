using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th09;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th06.Wrappers;
using ThScoreFileConverterTests.Models.Th09.Stubs;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th09HighScoreTests
    {
        internal static HighScoreStub ValidStub { get; } = new HighScoreStub()
        {
            Signature = "HSCR",
            Size1 = 0x2C,
            Size2 = 0x2C,
            Score = 1234567u,
            Chara = Th09Converter.Chara.Marisa,
            Level = Level.Hard,
            Rank = 987,
            Name = TestUtils.CP932Encoding.GetBytes("Player1\0\0"),
            Date = TestUtils.CP932Encoding.GetBytes("06/01/23\0"),
            ContinueCount = 2,
        };

        internal static byte[] MakeData(IHighScore highScore)
            => TestUtils.MakeByteArray(
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

        internal static byte[] MakeByteArray(IHighScore highScore)
            => TestUtils.MakeByteArray(
                highScore.Signature.ToCharArray(), highScore.Size1, highScore.Size2, MakeData(highScore));

        internal static void Validate(IHighScore expected, in Th09HighScoreWrapper actual)
        {
            var data = MakeData(expected);

            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Size1, actual.Size1);
            Assert.AreEqual(expected.Size2, actual.Size2);
            CollectionAssert.That.AreEqual(data, actual.Data);
            Assert.AreEqual(data[0], actual.FirstByteOfData);
            Assert.AreEqual(expected.Score, actual.Score);
            Assert.AreEqual(expected.Chara, actual.Chara);
            Assert.AreEqual(expected.Level, actual.Level);
            Assert.AreEqual(expected.Rank, actual.Rank);
            CollectionAssert.That.AreEqual(expected.Name, actual.Name);
            CollectionAssert.That.AreEqual(expected.Date, actual.Date);
            Assert.AreEqual(expected.ContinueCount, actual.ContinueCount);
        }

        [TestMethod]
        public void Th09HighScoreTestChapter() => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            var highScore = new Th09HighScoreWrapper(chapter);

            Validate(stub, highScore);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th09HighScoreTestNullChapter() => TestUtils.Wrap(() =>
        {
            _ = new Th09HighScoreWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th09HighScoreTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var stub = new HighScoreStub(ValidStub);
            stub.Signature = stub.Signature.ToLowerInvariant();

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th09HighScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th09HighScoreTestInvalidSize() => TestUtils.Wrap(() =>
        {
            var stub = new HighScoreStub(ValidStub);
            --stub.Size1;

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th09HighScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidCharacters
            => TestUtils.GetInvalidEnumerators(typeof(Th09Converter.Chara));

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidCharacters))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th09HighScoreTestInvalidChara(int chara) => TestUtils.Wrap(() =>
        {
            var stub = new HighScoreStub(ValidStub)
            {
                Chara = TestUtils.Cast<Th09Converter.Chara>(chara),
            };

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th09HighScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidLevels
            => TestUtils.GetInvalidEnumerators(typeof(Level));

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th09HighScoreTestInvalidLevel(int level) => TestUtils.Wrap(() =>
        {
            var stub = new HighScoreStub(ValidStub)
            {
                Level = TestUtils.Cast<Level>(level),
            };

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th09HighScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}

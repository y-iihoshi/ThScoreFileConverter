using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th09HighScoreTests
    {
        internal struct Properties
        {
            public string signature;
            public short size1;
            public short size2;
            public uint score;
            public Th09Converter.Chara chara;
            public ThConverter.Level level;
            public short rank;
            public byte[] name;
            public byte[] date;
            public byte continueCount;
        };

        internal static Properties ValidProperties => new Properties()
        {
            signature = "HSCR",
            size1 = 0x2C,
            size2 = 0x2C,
            score = 1234567u,
            chara = Th09Converter.Chara.Marisa,
            level = ThConverter.Level.Hard,
            rank = 987,
            name = TestUtils.CP932Encoding.GetBytes("Player1\0\0"),
            date = TestUtils.CP932Encoding.GetBytes("06/01/23\0"),
            continueCount = 2,
        };

        internal static byte[] MakeData(in Properties properties)
            => TestUtils.MakeByteArray(
                0u,
                properties.score,
                0u,
                (byte)properties.chara,
                (byte)properties.level,
                properties.rank,
                properties.name,
                properties.date,
                (byte)0,
                properties.continueCount);

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(), properties.size1, properties.size2, MakeData(properties));

        internal static void Validate(in Th09HighScoreWrapper highScore, in Properties properties)
        {
            var data = MakeData(properties);

            Assert.AreEqual(properties.signature, highScore.Signature);
            Assert.AreEqual(properties.size1, highScore.Size1);
            Assert.AreEqual(properties.size2, highScore.Size2);
            CollectionAssert.AreEqual(data, highScore.Data.ToArray());
            Assert.AreEqual(data[0], highScore.FirstByteOfData);
            Assert.AreEqual(properties.score, highScore.Score);
            Assert.AreEqual(properties.chara, highScore.Chara);
            Assert.AreEqual(properties.level, highScore.Level);
            Assert.AreEqual(properties.rank, highScore.Rank);
            CollectionAssert.AreEqual(properties.name, highScore.Name.ToArray());
            CollectionAssert.AreEqual(properties.date, highScore.Date.ToArray());
            Assert.AreEqual(properties.continueCount, highScore.ContinueCount);
        }

        [TestMethod]
        public void Th09HighScoreTestChapter() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var chapter = Th06ChapterWrapper<Th09Converter>.Create(MakeByteArray(properties));
            var highScore = new Th09HighScoreWrapper(chapter);

            Validate(highScore, properties);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "highScore")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th09HighScoreTestNullChapter() => TestUtils.Wrap(() =>
        {
            var highScore = new Th09HighScoreWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "highScore")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th09HighScoreTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = properties.signature.ToLowerInvariant();

            var chapter = Th06ChapterWrapper<Th09Converter>.Create(MakeByteArray(properties));
            var highScore = new Th09HighScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "highScore")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th09HighScoreTestInvalidSize() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            --properties.size1;

            var chapter = Th06ChapterWrapper<Th09Converter>.Create(MakeByteArray(properties));
            var highScore = new Th09HighScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidCharacters
            => TestUtils.GetInvalidEnumerators(typeof(Th09Converter.Chara));

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "highScore")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidCharacters))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th09HighScoreTestInvalidChara(int chara) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.chara = TestUtils.Cast<Th09Converter.Chara>(chara);

            var chapter = Th06ChapterWrapper<Th09Converter>.Create(MakeByteArray(properties));
            var highScore = new Th09HighScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidLevels
            => TestUtils.GetInvalidEnumerators(typeof(ThConverter.Level));

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "highScore")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th09HighScoreTestInvalidLevel(int level) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.level = TestUtils.Cast<ThConverter.Level>(level);

            var chapter = Th06ChapterWrapper<Th09Converter>.Create(MakeByteArray(properties));
            var highScore = new Th09HighScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}

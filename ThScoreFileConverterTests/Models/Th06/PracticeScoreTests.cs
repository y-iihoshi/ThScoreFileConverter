using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Th06.Wrappers;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th06PracticeScoreTests
    {
        internal struct Properties
        {
            public string signature;
            public short size1;
            public short size2;
            public int highScore;
            public Th06Converter.Chara chara;
            public ThConverter.Level level;
            public ThConverter.Stage stage;
        };

        internal static Properties ValidProperties => new Properties()
        {
            signature = "PSCR",
            size1 = 0x14,
            size2 = 0x14,
            highScore = 123456,
            chara = Th06Converter.Chara.ReimuB,
            level = ThConverter.Level.Hard,
            stage = ThConverter.Stage.Extra
        };

        internal static byte[] MakeData(in Properties properties)
            => TestUtils.MakeByteArray(
                0u,
                properties.highScore,
                (byte)properties.chara,
                (byte)properties.level,
                (byte)properties.stage,
                (byte)0);

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(), properties.size1, properties.size2, MakeData(properties));

        internal static void Validate(in Th06PracticeScoreWrapper score, in Properties properties)
        {
            var data = MakeData(properties);

            Assert.AreEqual(properties.signature, score.Signature);
            Assert.AreEqual(properties.size1, score.Size1);
            Assert.AreEqual(properties.size2, score.Size2);
            CollectionAssert.AreEqual(data, score.Data.ToArray());
            Assert.AreEqual(data[0], score.FirstByteOfData);
            Assert.AreEqual(properties.highScore, score.HighScore.Value);
            Assert.AreEqual(properties.chara, score.Chara.Value);
            Assert.AreEqual(properties.level, score.Level.Value);
            Assert.AreEqual(properties.stage, score.Stage.Value);
        }

        [TestMethod]
        public void Th06PracticeScoreTestChapter() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var score = new Th06PracticeScoreWrapper(chapter);

            Validate(score, properties);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th06PracticeScoreTestNullChapter() => TestUtils.Wrap(() =>
        {
            var score = new Th06PracticeScoreWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th06PracticeScoreTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = properties.signature.ToLowerInvariant();

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var score = new Th06PracticeScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th06PracticeScoreTestInvalidSize1() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            --properties.size1;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var score = new Th06PracticeScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidCharacters
            => TestUtils.GetInvalidEnumerators(typeof(Th06Converter.Chara));

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidCharacters))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th06PracticeScoreTestInvalidChara(int chara) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.chara = TestUtils.Cast<Th06Converter.Chara>(chara);

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var score = new Th06PracticeScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidLevels
            => TestUtils.GetInvalidEnumerators(typeof(ThConverter.Level));

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th06PracticeScoreTestInvalidLevel(int level) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.level = TestUtils.Cast<ThConverter.Level>(level);

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var score = new Th06PracticeScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidStages
            => TestUtils.GetInvalidEnumerators(typeof(ThConverter.Stage));

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidStages))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th06PracticeScoreTestInvalidStage(int stage) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.stage = TestUtils.Cast<ThConverter.Stage>(stage);

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var score = new Th06PracticeScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}

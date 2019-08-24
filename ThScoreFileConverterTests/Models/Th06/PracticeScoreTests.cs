using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th06;
using ThScoreFileConverterTests.Models.Th06.Wrappers;

namespace ThScoreFileConverterTests.Models.Th06
{
    [TestClass]
    public class PracticeScoreTests
    {
        internal struct Properties
        {
            public string signature;
            public short size1;
            public short size2;
            public int highScore;
            public Chara chara;
            public Level level;
            public Stage stage;
        };

        internal static Properties ValidProperties => new Properties()
        {
            signature = "PSCR",
            size1 = 0x14,
            size2 = 0x14,
            highScore = 123456,
            chara = Chara.ReimuB,
            level = Level.Hard,
            stage = Stage.Extra
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

        internal static void Validate(in PracticeScore score, in Properties properties)
        {
            var data = MakeData(properties);

            Assert.AreEqual(properties.signature, score.Signature);
            Assert.AreEqual(properties.size1, score.Size1);
            Assert.AreEqual(properties.size2, score.Size2);
            Assert.AreEqual(data[0], score.FirstByteOfData);
            Assert.AreEqual(properties.highScore, score.HighScore);
            Assert.AreEqual(properties.chara, score.Chara);
            Assert.AreEqual(properties.level, score.Level);
            Assert.AreEqual(properties.stage, score.Stage);
        }

        [TestMethod]
        public void Th06PracticeScoreTestChapter() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var score = new PracticeScore(chapter.Target as Chapter);

            Validate(score, properties);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th06PracticeScoreTestNullChapter() => TestUtils.Wrap(() =>
        {
            var score = new PracticeScore(null);

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
            var score = new PracticeScore(chapter.Target as Chapter);

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
            var score = new PracticeScore(chapter.Target as Chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidCharacters
            => TestUtils.GetInvalidEnumerators(typeof(Chara));

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidCharacters))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th06PracticeScoreTestInvalidChara(int chara) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.chara = TestUtils.Cast<Chara>(chara);

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var score = new PracticeScore(chapter.Target as Chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidLevels
            => TestUtils.GetInvalidEnumerators(typeof(Level));

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th06PracticeScoreTestInvalidLevel(int level) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.level = TestUtils.Cast<Level>(level);

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var score = new PracticeScore(chapter.Target as Chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidStages
            => TestUtils.GetInvalidEnumerators(typeof(Stage));

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidStages))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th06PracticeScoreTestInvalidStage(int stage) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.stage = TestUtils.Cast<Stage>(stage);

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var score = new PracticeScore(chapter.Target as Chapter);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}

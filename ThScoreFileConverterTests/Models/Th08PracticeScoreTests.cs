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
    public class Th08PracticeScoreTests
    {
        internal struct Properties
        {
            public string signature;
            public short size1;
            public short size2;
            public Dictionary<Th08StageLevelPairTests.Properties, int> playCounts;
            public Dictionary<Th08StageLevelPairTests.Properties, int> highScores;
            public Th08Converter.Chara chara;
        };

        internal static Properties ValidProperties => new Properties()
        {
            signature = "PSCR",
            size1 = 0x178,
            size2 = 0x178,
            playCounts = Utils.GetEnumerator<Th08Converter.Stage>()
                .SelectMany(stage => Utils.GetEnumerator<ThConverter.Level>().Select(level => new { stage, level }))
                .ToDictionary(
                    pair => new Th08StageLevelPairTests.Properties() { stage = pair.stage, level = pair.level },
                    pair => (int)pair.stage * 10 + (int)pair.level),
            highScores = Utils.GetEnumerator<Th08Converter.Stage>()
                .SelectMany(stage => Utils.GetEnumerator<ThConverter.Level>().Select(level => new { stage, level }))
                .ToDictionary(
                    pair => new Th08StageLevelPairTests.Properties() { stage = pair.stage, level = pair.level },
                    pair => (int)pair.level * 10 + (int)pair.stage),
            chara = Th08Converter.Chara.MarisaAlice
        };

        internal static byte[] MakeData(in Properties properties)
            => TestUtils.MakeByteArray(
                0u,
                properties.playCounts.Values.ToArray(),
                properties.highScores.Values.ToArray(),
                (byte)properties.chara,
                new byte[3]);

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(), properties.size1, properties.size2, MakeData(properties));

        internal static void Validate(in Th08PracticeScoreWrapper score, in Properties properties)
        {
            var data = MakeData(properties);

            Assert.AreEqual(properties.signature, score.Signature);
            Assert.AreEqual(properties.size1, score.Size1);
            Assert.AreEqual(properties.size2, score.Size2);
            CollectionAssert.AreEqual(data, score.Data.ToArray());
            Assert.AreEqual(data[0], score.FirstByteOfData);
            CollectionAssert.AreEqual(properties.playCounts.Values, score.PlayCountsValues.ToArray());
            CollectionAssert.AreEqual(properties.highScores.Values, score.HighScoresValues.ToArray());
            Assert.AreEqual(properties.chara, score.Chara);
        }

        [TestMethod]
        public void Th08PracticeScoreTestChapter() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var chapter = Th06ChapterWrapper<Th08Converter>.Create(MakeByteArray(properties));
            var score = new Th08PracticeScoreWrapper(chapter);

            Validate(score, properties);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th08PracticeScoreTestNullChapter() => TestUtils.Wrap(() =>
        {
            var score = new Th08PracticeScoreWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08PracticeScoreTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = properties.signature.ToLowerInvariant();

            var chapter = Th06ChapterWrapper<Th08Converter>.Create(MakeByteArray(properties));
            var score = new Th08PracticeScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08PracticeScoreTestInvalidSize1() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            --properties.size1;

            var chapter = Th06ChapterWrapper<Th08Converter>.Create(MakeByteArray(properties));
            var score = new Th08PracticeScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidCharacters
            => TestUtils.GetInvalidEnumerators(typeof(Th08Converter.Chara));

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidCharacters))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th08PracticeScoreTestInvalidChara(int chara) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.chara = TestUtils.Cast<Th08Converter.Chara>(chara);

            var chapter = Th06ChapterWrapper<Th08Converter>.Create(MakeByteArray(properties));
            var score = new Th08PracticeScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}

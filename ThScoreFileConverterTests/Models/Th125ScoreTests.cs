using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th125ScoreTests
    {
        internal struct Properties
        {
            public string signature;
            public ushort version;
            public int size;
            public uint checksum;
            public Th095LevelScenePairTests.Properties<Th125Converter.Level> levelScene;
            public int highScore;
            public Th125Converter.Chara chara;
            public int trialCount;
            public int firstSuccess;
            public uint dateTime;
            public int bestshotScore;
        };

        internal static Properties ValidProperties => new Properties()
        {
            signature = "SC",
            version = 0,
            size = 0x48,
            checksum = 0u,
            levelScene = Th095LevelScenePairTests.GetValidProperties<Th125Converter.Level>(),
            highScore = 1234567,
            chara = Th125Converter.Chara.Hatate,
            trialCount = 9876,
            firstSuccess = 5432,
            dateTime = 34567890,
            bestshotScore = 23456
        };

        internal static byte[] MakeData(in Properties properties)
            => TestUtils.MakeByteArray(
                (int)properties.levelScene.level * 10 + properties.levelScene.scene - 1,
                properties.highScore,
                new byte[4],
                (int)properties.chara,
                new byte[4],
                properties.trialCount,
                properties.firstSuccess,
                0u,
                properties.dateTime,
                new uint[3],
                properties.bestshotScore,
                new byte[8]);

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(),
                properties.version,
                properties.size,
                properties.checksum,
                MakeData(properties));

        internal static void Validate(in Th125ScoreWrapper score, in Properties properties)
        {
            var data = MakeData(properties);

            Assert.AreEqual(properties.signature, score.Signature);
            Assert.AreEqual(properties.version, score.Version);
            Assert.AreEqual(properties.size, score.Size);
            Assert.AreEqual(properties.checksum, score.Checksum);
            CollectionAssert.AreEqual(data, score.Data.ToArray());
            Assert.AreEqual(properties.levelScene.level, score.LevelScene.Level);
            Assert.AreEqual(properties.levelScene.scene, score.LevelScene.Scene);
            Assert.AreEqual(properties.highScore, score.HighScore);
            Assert.AreEqual(properties.chara, score.Chara);
            Assert.AreEqual(properties.trialCount, score.TrialCount);
            Assert.AreEqual(properties.firstSuccess, score.FirstSuccess);
            Assert.AreEqual(properties.dateTime, score.DateTime);
            Assert.AreEqual(properties.bestshotScore, score.BestshotScore);
        }

        [TestMethod]
        public void Th125ScoreTestChapter() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var chapter = Th095ChapterWrapper<Th125Converter>.Create(MakeByteArray(properties));
            var score = new Th125ScoreWrapper(chapter);

            Validate(score, properties);
            Assert.IsFalse(score.IsValid.Value);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th125ScoreTestNullChapter() => TestUtils.Wrap(() =>
        {
            var score = new Th125ScoreWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th125ScoreTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = properties.signature.ToLowerInvariant();

            var chapter = Th095ChapterWrapper<Th125Converter>.Create(MakeByteArray(properties));
            var score = new Th125ScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th125ScoreTestInvalidVersion() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            ++properties.version;

            var chapter = Th095ChapterWrapper<Th125Converter>.Create(MakeByteArray(properties));
            var score = new Th125ScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th125ScoreTestInvalidSize() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            ++properties.size;

            var chapter = Th095ChapterWrapper<Th125Converter>.Create(MakeByteArray(properties));
            var score = new Th125ScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidLevels
            => TestUtils.GetInvalidEnumerators(typeof(Th125Converter.Level));

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th125ScoreTestInvalidLevel(int level) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.levelScene.level = TestUtils.Cast<Th125Converter.Level>(level);

            var chapter = Th095ChapterWrapper<Th125Converter>.Create(MakeByteArray(properties));
            var score = new Th125ScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidCharacters
            => TestUtils.GetInvalidEnumerators(typeof(Th125Converter.Chara));

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidCharacters))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th125ScoreTestInvalidChara(int chara) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.chara = TestUtils.Cast<Th125Converter.Chara>(chara);

            var chapter = Th095ChapterWrapper<Th125Converter>.Create(MakeByteArray(properties));
            var score = new Th125ScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("SC", (ushort)0, 0x48, true)]
        [DataRow("sc", (ushort)0, 0x48, false)]
        [DataRow("SC", (ushort)1, 0x48, false)]
        [DataRow("SC", (ushort)0, 0x49, false)]
        public void Th125ScoreCanInitializeTest(string signature, ushort version, int size, bool expected)
            => TestUtils.Wrap(() =>
            {
                var checksum = 0u;
                var data = new byte[size];

                var chapter = Th095ChapterWrapper<Th125Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, size, checksum, data));

                Assert.AreEqual(expected, Th125ScoreWrapper.CanInitialize(chapter));
            });
    }
}

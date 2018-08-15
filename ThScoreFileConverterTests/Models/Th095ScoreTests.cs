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
    public class Th095ScoreTests
    {
        internal struct Properties
        {
            public string signature;
            public ushort version;
            public int size;
            public uint checksum;
            public Th095LevelScenePairTests.Properties<Th095Converter.Level> levelScene;
            public int highScore;
            public int bestshotScore;
            public uint dateTime;
            public int trialCount;
            public float slowRate1;
            public float slowRate2;
        };

        internal static Properties ValidProperties => new Properties()
        {
            signature = "SC",
            version = 1,
            size = 0x60,
            checksum = 0u,
            levelScene = Th095LevelScenePairTests.GetValidProperties<Th095Converter.Level>(),
            highScore = 1234567,
            bestshotScore = 23456,
            dateTime = 34567890,
            trialCount = 9876,
            slowRate1 = 1.23f,
            slowRate2 = 2.34f
        };

        internal static byte[] MakeData(in Properties properties)
            => TestUtils.MakeByteArray(
                (int)properties.levelScene.level * 10 + properties.levelScene.scene - 1,
                properties.highScore,
                0u,
                properties.bestshotScore,
                new byte[0x20],
                properties.dateTime,
                0u,
                properties.trialCount,
                properties.slowRate1,
                properties.slowRate2,
                new byte[0x10]);

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(),
                properties.version,
                properties.size,
                properties.checksum,
                MakeData(properties));

        internal static void Validate(in Th095ScoreWrapper score, in Properties properties)
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
            Assert.AreEqual(properties.bestshotScore, score.BestshotScore);
            Assert.AreEqual(properties.dateTime, score.DateTime);
            Assert.AreEqual(properties.trialCount, score.TrialCount);
            Assert.AreEqual(properties.slowRate1, score.SlowRate1);
            Assert.AreEqual(properties.slowRate2, score.SlowRate2);
        }

        [TestMethod]
        public void Th095ScoreTestChapter() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var chapter = Th095ChapterWrapper<Th095Converter>.Create(MakeByteArray(properties));
            var score = new Th095ScoreWrapper(chapter);

            Validate(score, properties);
            Assert.IsFalse(score.IsValid.Value);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th095ScoreTestNullChapter() => TestUtils.Wrap(() =>
        {
            var score = new Th095ScoreWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th095ScoreTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = properties.signature.ToLowerInvariant();

            var chapter = Th095ChapterWrapper<Th095Converter>.Create(MakeByteArray(properties));
            var score = new Th095ScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th095ScoreTestInvalidVersion() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            ++properties.version;

            var chapter = Th095ChapterWrapper<Th095Converter>.Create(MakeByteArray(properties));
            var score = new Th095ScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th095ScoreTestInvalidSize() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            ++properties.size;

            var chapter = Th095ChapterWrapper<Th095Converter>.Create(MakeByteArray(properties));
            var score = new Th095ScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidLevels
            => TestUtils.GetInvalidEnumerators(typeof(Th095Converter.Level));

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th095ScoreTestInvalidLevel(int level) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.levelScene.level = TestUtils.Cast<Th095Converter.Level>(level);

            var chapter = Th095ChapterWrapper<Th095Converter>.Create(MakeByteArray(properties));
            var score = new Th095ScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("SC", (ushort)1, 0x60, true)]
        [DataRow("sc", (ushort)1, 0x60, false)]
        [DataRow("SC", (ushort)0, 0x60, false)]
        [DataRow("SC", (ushort)1, 0x61, false)]
        public void Th095ScoreCanInitializeTest(string signature, ushort version, int size, bool expected)
            => TestUtils.Wrap(() =>
            {
                var checksum = 0u;
                var data = new byte[size];

                var chapter = Th095ChapterWrapper<Th095Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, size, checksum, data));

                Assert.AreEqual(expected, Th095ScoreWrapper.CanInitialize(chapter));
            });
    }
}

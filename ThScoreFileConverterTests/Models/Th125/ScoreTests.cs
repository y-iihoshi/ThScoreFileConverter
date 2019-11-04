using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using ThScoreFileConverter.Models.Th125;
using ThScoreFileConverterTests.Models.Th125.Stubs;
using Chapter = ThScoreFileConverter.Models.Th095.Chapter;

namespace ThScoreFileConverterTests.Models.Th125
{
    [TestClass]
    public class ScoreTests
    {
        internal static ScoreStub ValidStub { get; } = new ScoreStub()
        {
            Signature = "SC",
            Version = 0,
            Size = 0x48,
            Checksum = 0u,
            LevelScene = (Level.Nine, 7),
            HighScore = 1234567,
            Chara = Chara.Hatate,
            TrialCount = 9876,
            FirstSuccess = 5432,
            DateTime = 34567890,
            BestshotScore = 23456
        };

        internal static byte[] MakeData(IScore score)
            => TestUtils.MakeByteArray(
                (int)score.LevelScene.Level * 10 + score.LevelScene.Scene - 1,
                score.HighScore,
                new byte[4],
                (int)score.Chara,
                new byte[4],
                score.TrialCount,
                score.FirstSuccess,
                0u,
                score.DateTime,
                new uint[3],
                score.BestshotScore,
                new byte[8]);

        internal static byte[] MakeByteArray(IScore score)
            => TestUtils.MakeByteArray(
                score.Signature.ToCharArray(),
                score.Version,
                score.Size,
                score.Checksum,
                MakeData(score));

        internal static void Validate(IScore expected, IScore actual)
        {
            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Version, actual.Version);
            Assert.AreEqual(expected.Size, actual.Size);
            Assert.AreEqual(expected.Checksum, actual.Checksum);
            Assert.AreEqual(expected.LevelScene, actual.LevelScene);
            Assert.AreEqual(expected.HighScore, actual.HighScore);
            Assert.AreEqual(expected.Chara, actual.Chara);
            Assert.AreEqual(expected.TrialCount, actual.TrialCount);
            Assert.AreEqual(expected.FirstSuccess, actual.FirstSuccess);
            Assert.AreEqual(expected.DateTime, actual.DateTime);
            Assert.AreEqual(expected.BestshotScore, actual.BestshotScore);
        }

        [TestMethod]
        public void ScoreTestChapter() => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            var score = new Score(chapter);

            Validate(stub, score);
            Assert.IsFalse(score.IsValid);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ScoreTestNullChapter() => TestUtils.Wrap(() =>
        {
            _ = new Score(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ScoreTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var stub = new ScoreStub(ValidStub);
            stub.Signature = stub.Signature.ToLowerInvariant();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new Score(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ScoreTestInvalidVersion() => TestUtils.Wrap(() =>
        {
            var stub = new ScoreStub(ValidStub);
            ++stub.Version;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new Score(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ScoreTestInvalidSize() => TestUtils.Wrap(() =>
        {
            var stub = new ScoreStub(ValidStub);
            --stub.Size;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new Score(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidLevels
            => TestUtils.GetInvalidEnumerators(typeof(Level));

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidCastException))]
        public void ScoreTestInvalidLevel(int level) => TestUtils.Wrap(() =>
        {
            var stub = new ScoreStub(ValidStub);
            stub.LevelScene = (TestUtils.Cast<Level>(level), stub.LevelScene.Scene);

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new Score(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidCharacters
            => TestUtils.GetInvalidEnumerators(typeof(Chara));

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidCharacters))]
        [ExpectedException(typeof(InvalidCastException))]
        public void ScoreTestInvalidChara(int chara) => TestUtils.Wrap(() =>
        {
            var stub = new ScoreStub(ValidStub)
            {
                Chara = TestUtils.Cast<Chara>(chara),
            };

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new Score(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("SC", (ushort)0, 0x48, true)]
        [DataRow("sc", (ushort)0, 0x48, false)]
        [DataRow("SC", (ushort)1, 0x48, false)]
        [DataRow("SC", (ushort)0, 0x49, false)]
        public void CanInitializeTest(string signature, ushort version, int size, bool expected)
            => TestUtils.Wrap(() =>
            {
                var checksum = 0u;
                var data = new byte[size];

                var chapter = TestUtils.Create<Chapter>(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, size, checksum, data));

                Assert.AreEqual(expected, Score.CanInitialize(chapter));
            });
    }
}

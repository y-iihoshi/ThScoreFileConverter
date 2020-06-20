using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th07;
using ThScoreFileConverterTests.Models.Th07.Stubs;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;

namespace ThScoreFileConverterTests.Models.Th07
{
    [TestClass]
    public class PracticeScoreTests
    {
        internal static PracticeScoreStub ValidStub { get; } = new PracticeScoreStub()
        {
            Signature = "PSCR",
            Size1 = 0x18,
            Size2 = 0x18,
            TrialCount = 987,
            HighScore = 123456,
            Chara = Chara.ReimuB,
            Level = Level.Hard,
            Stage = Stage.Six,
        };

        internal static byte[] MakeByteArray(IPracticeScore score)
            => TestUtils.MakeByteArray(
                score.Signature.ToCharArray(),
                score.Size1,
                score.Size2,
                0u,
                score.TrialCount,
                score.HighScore,
                (byte)score.Chara,
                (byte)score.Level,
                (byte)score.Stage,
                (byte)0);

        internal static void Validate(IPracticeScore expected, IPracticeScore actual)
        {
            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Size1, actual.Size1);
            Assert.AreEqual(expected.Size2, actual.Size2);
            Assert.AreEqual(expected.FirstByteOfData, actual.FirstByteOfData);
            Assert.AreEqual(expected.TrialCount, actual.TrialCount);
            Assert.AreEqual(expected.HighScore, actual.HighScore);
            Assert.AreEqual(expected.Chara, actual.Chara);
            Assert.AreEqual(expected.Level, actual.Level);
            Assert.AreEqual(expected.Stage, actual.Stage);
        }

        [TestMethod]
        public void PracticeScoreTestChapter()
        {
            var chapter = TestUtils.Create<Chapter>(MakeByteArray(ValidStub));
            var score = new PracticeScore(chapter);

            Validate(ValidStub, score);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PracticeScoreTestNullChapter()
        {
            _ = new PracticeScore(null!);

            Assert.Fail(TestUtils.Unreachable);
        }

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void PracticeScoreTestInvalidSignature()
        {
            var stub = new PracticeScoreStub(ValidStub);
            stub.Signature = stub.Signature.ToLowerInvariant();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new PracticeScore(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void PracticeScoreTestInvalidSize1()
        {
            var stub = new PracticeScoreStub(ValidStub);
            --stub.Size1;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new PracticeScore(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        public static IEnumerable<object[]> InvalidCharacters
            => TestUtils.GetInvalidEnumerators(typeof(Chara));

        [DataTestMethod]
        [DynamicData(nameof(InvalidCharacters))]
        [ExpectedException(typeof(InvalidCastException))]
        public void PracticeScoreTestInvalidChara(int chara)
        {
            var stub = new PracticeScoreStub(ValidStub)
            {
                Chara = TestUtils.Cast<Chara>(chara),
            };

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new PracticeScore(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        public static IEnumerable<object[]> InvalidLevels
            => TestUtils.GetInvalidEnumerators(typeof(Level));

        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidCastException))]
        public void PracticeScoreTestInvalidLevel(int level)
        {
            var stub = new PracticeScoreStub(ValidStub)
            {
                Level = TestUtils.Cast<Level>(level),
            };

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new PracticeScore(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        public static IEnumerable<object[]> InvalidStages
            => TestUtils.GetInvalidEnumerators(typeof(Stage));

        [DataTestMethod]
        [DynamicData(nameof(InvalidStages))]
        [ExpectedException(typeof(InvalidCastException))]
        public void PracticeScoreTestInvalidStage(int stage)
        {
            var stub = new PracticeScoreStub(ValidStub)
            {
                Stage = TestUtils.Cast<Stage>(stage),
            };

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new PracticeScore(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }
    }
}

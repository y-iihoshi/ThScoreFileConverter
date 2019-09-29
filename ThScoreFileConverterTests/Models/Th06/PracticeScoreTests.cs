using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th06;
using ThScoreFileConverterTests.Models.Th06.Stubs;
using ThScoreFileConverterTests.Models.Th06.Wrappers;

namespace ThScoreFileConverterTests.Models.Th06
{
    [TestClass]
    public class PracticeScoreTests
    {
        internal static PracticeScoreStub ValidStub { get; } = new PracticeScoreStub()
        {
            Signature = "PSCR",
            Size1 = 0x14,
            Size2 = 0x14,
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
            Assert.AreEqual(expected.HighScore, actual.HighScore);
            Assert.AreEqual(expected.Chara, actual.Chara);
            Assert.AreEqual(expected.Level, actual.Level);
            Assert.AreEqual(expected.Stage, actual.Stage);
        }

        [TestMethod]
        public void PracticeScoreTestChapter() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(MakeByteArray(ValidStub));
            var score = new PracticeScore(chapter.Target);

            Validate(ValidStub, score);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PracticeScoreTestNullChapter() => TestUtils.Wrap(() =>
        {
            _ = new PracticeScore(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void PracticeScoreTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var stub = new PracticeScoreStub(ValidStub);
            stub.Signature = stub.Signature.ToLowerInvariant();

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new PracticeScore(chapter.Target);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void PracticeScoreTestInvalidSize1() => TestUtils.Wrap(() =>
        {
            var stub = new PracticeScoreStub(ValidStub);
            --stub.Size1;

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new PracticeScore(chapter.Target);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidCharacters
            => TestUtils.GetInvalidEnumerators(typeof(Chara));

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidCharacters))]
        [ExpectedException(typeof(InvalidCastException))]
        public void PracticeScoreTestInvalidChara(int chara) => TestUtils.Wrap(() =>
        {
            var stub = new PracticeScoreStub(ValidStub)
            {
                Chara = TestUtils.Cast<Chara>(chara),
            };

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new PracticeScore(chapter.Target);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidLevels
            => TestUtils.GetInvalidEnumerators(typeof(Level));

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidCastException))]
        public void PracticeScoreTestInvalidLevel(int level) => TestUtils.Wrap(() =>
        {
            var stub = new PracticeScoreStub(ValidStub)
            {
                Level = TestUtils.Cast<Level>(level),
            };

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new PracticeScore(chapter.Target);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidStages
            => TestUtils.GetInvalidEnumerators(typeof(Stage));

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidStages))]
        [ExpectedException(typeof(InvalidCastException))]
        public void PracticeScoreTestInvalidStage(int stage) => TestUtils.Wrap(() =>
        {
            var stub = new PracticeScoreStub(ValidStub)
            {
                Stage = TestUtils.Cast<Stage>(stage),
            };

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new PracticeScore(chapter.Target);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th08;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th08.Stubs;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;
using Stage = ThScoreFileConverter.Models.Th08.Stage;

namespace ThScoreFileConverterTests.Models.Th08
{
    [TestClass]
    public class PracticeScoreTests
    {
        internal static PracticeScoreStub ValidStub { get; } = new PracticeScoreStub()
        {
            Signature = "PSCR",
            Size1 = 0x178,
            Size2 = 0x178,
            PlayCounts = Utils.GetEnumerator<Stage>()
                .SelectMany(stage => Utils.GetEnumerator<Level>().Select(level => (stage, level)))
                .ToDictionary(pair => pair, pair => (int)pair.stage * 10 + (int)pair.level),
            HighScores = Utils.GetEnumerator<Stage>()
                .SelectMany(stage => Utils.GetEnumerator<Level>().Select(level => (stage, level)))
                .ToDictionary(pair => pair, pair => (int)pair.level * 10 + (int)pair.stage),
            Chara = Chara.MarisaAlice
        };

        internal static byte[] MakeByteArray(IPracticeScore score)
            => TestUtils.MakeByteArray(
                score.Signature.ToCharArray(),
                score.Size1,
                score.Size2,
                0u,
                score.PlayCounts.Values.ToArray(),
                score.HighScores.Values.ToArray(),
                (byte)score.Chara,
                new byte[3]);

        internal static void Validate(IPracticeScore expected, IPracticeScore actual)
        {
            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Size1, actual.Size1);
            Assert.AreEqual(expected.Size2, actual.Size2);
            Assert.AreEqual(expected.FirstByteOfData, actual.FirstByteOfData);
            CollectionAssert.That.AreEqual(expected.PlayCounts.Values, actual.PlayCounts.Values);
            CollectionAssert.That.AreEqual(expected.HighScores.Values, actual.HighScores.Values);
            Assert.AreEqual(expected.Chara, actual.Chara);
        }

        [TestMethod]
        public void PracticeScoreTestChapter() => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            var score = new PracticeScore(chapter);

            Validate(stub, score);
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

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new PracticeScore(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void PracticeScoreTestInvalidSize1() => TestUtils.Wrap(() =>
        {
            var stub = new PracticeScoreStub(ValidStub);
            --stub.Size1;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new PracticeScore(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidCharacters
            => TestUtils.GetInvalidEnumerators(typeof(Chara));

        [DataTestMethod]
        [DynamicData(nameof(InvalidCharacters))]
        [ExpectedException(typeof(InvalidCastException))]
        public void PracticeScoreTestInvalidChara(int chara) => TestUtils.Wrap(() =>
        {
            var stub = new PracticeScoreStub(ValidStub)
            {
                Chara = TestUtils.Cast<Chara>(chara),
            };

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new PracticeScore(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}

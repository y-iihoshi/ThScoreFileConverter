using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th08;
using ThScoreFileConverterTests.Extensions;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;
using Stage = ThScoreFileConverter.Models.Th08.Stage;

namespace ThScoreFileConverterTests.Models.Th08
{
    [TestClass]
    public class PracticeScoreTests
    {
        internal static Mock<IPracticeScore> MockPracticeScore()
        {
            var pairs = Utils.GetEnumerable<Stage>()
                .SelectMany(stage => Utils.GetEnumerable<Level>().Select(level => (stage, level)));
            var mock = new Mock<IPracticeScore>();
            _ = mock.SetupGet(m => m.Signature).Returns("PSCR");
            _ = mock.SetupGet(m => m.Size1).Returns(0x178);
            _ = mock.SetupGet(m => m.Size2).Returns(0x178);
            _ = mock.SetupGet(m => m.PlayCounts).Returns(
                pairs.ToDictionary(pair => pair, pair => ((int)pair.stage * 10) + (int)pair.level));
            _ = mock.SetupGet(m => m.HighScores).Returns(
                pairs.ToDictionary(pair => pair, pair => ((int)pair.level * 10) + (int)pair.stage));
            _ = mock.SetupGet(m => m.Chara).Returns(Chara.MarisaAlice);
            return mock;
        }

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
        public void PracticeScoreTestChapter()
        {
            var mock = MockPracticeScore();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            var score = new PracticeScore(chapter);

            Validate(mock.Object, score);
        }

        [TestMethod]
        public void PracticeScoreTestNullChapter()
            => _ = Assert.ThrowsException<ArgumentNullException>(() => _ = new PracticeScore(null!));

        [TestMethod]
        public void PracticeScoreTestInvalidSignature()
        {
            var mock = MockPracticeScore();
            var signature = mock.Object.Signature;
            _ = mock.SetupGet(m => m.Signature).Returns(signature.ToLowerInvariant());

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidDataException>(() => _ = new PracticeScore(chapter));
        }

        [TestMethod]
        public void PracticeScoreTestInvalidSize1()
        {
            var mock = MockPracticeScore();
            var size = mock.Object.Size1;
            _ = mock.SetupGet(m => m.Size1).Returns(--size);

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidDataException>(() => _ = new PracticeScore(chapter));
        }

        public static IEnumerable<object[]> InvalidCharacters
            => TestUtils.GetInvalidEnumerators(typeof(Chara));

        [DataTestMethod]
        [DynamicData(nameof(InvalidCharacters))]
        public void PracticeScoreTestInvalidChara(int chara)
        {
            var mock = MockPracticeScore();
            _ = mock.SetupGet(m => m.Chara).Returns(TestUtils.Cast<Chara>(chara));

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidCastException>(() => _ = new PracticeScore(chapter));
        }
    }
}
